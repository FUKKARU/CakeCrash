using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class DeleteCake : MonoBehaviour
    {
        [SerializeField] AudioSource audioSourceFall;
        [SerializeField] AudioSource audioSourceSmashed;
        Camera mainCamera;

        // ケーキの色
        enum COLOR { NULL, RED, GREEN, BLUE }
        [SerializeField] COLOR cakeColor = COLOR.NULL;

        // ケーキのサイズ
        enum SIZE { NULL, BIG, MEDIUM, SMALL }
        [SerializeField] SIZE cakeSize = SIZE.NULL;

        [SerializeField] GameObject creamPrfb;
        Rigidbody rb;
        bool isAtCenter = false;
        bool isNearCenter = false;
        bool isAtBottom = false;
        bool isSmashing = false; // 吹っ飛ばす状態になっているかどうか
        bool isTouchedHammerHead = false; // ハンマーの頭に触れたかどうか
        bool isSmashed = false; // 吹っ飛ばされたかどうか(1回だけ実行するためのただのフラグ)
        bool isHitWall = false;
        bool isSoundPlayable = false; // 落下音を再生できるかどうか
        float time = 0f;

        const float deleteTimeLimit = 15; // ケーキがシーンに何秒以上存在したら消すか
        const float audioPlayableTime = 0.5f; // 生成されてから，落下音を再生可能になるまでの時間(秒)

        void Awake()
        {
            mainCamera = Camera.main;
        }

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            StartCoroutine(TimeCount());
        }

        void Update()
        {
            // ケーキがハンマーに触れた，かつ吹っ飛ばす状態のときのみ，ケーキを吹っ飛ばす。
            if (isTouchedHammerHead && !isSmashed && isSmashing)
            {
                // ミスった場合は吹っ飛ばすのではなく消す。
                if (GameManager.Instance.IsDoingPenalty)
                {
                    Destroy(gameObject);
                }

                isSmashed = true; // 吹っ飛ばされたので，このif文はもう実行されない。

                gameObject.layer = 6; // レイヤーを変える。

                // 静止摩擦力と動摩擦力を0にして吹っ飛ばす。
                // Friction Combine が Minimum なので，上のケーキとの摩擦は0になる。
                GetComponent<BoxCollider>().material.staticFriction = 0;
                GetComponent<BoxCollider>().material.dynamicFriction = 0;
                Vector3 direction = Vector3.zero;
                if (cakeSize == SIZE.BIG)
                {
                    direction = CakeParamsSO.Entity.SmashVector3[0] * CakeParamsSO.Entity.SmashPower[0];
                }
                else if (cakeSize == SIZE.MEDIUM)
                {
                    direction = CakeParamsSO.Entity.SmashVector3[1] * CakeParamsSO.Entity.SmashPower[1];
                }
                else if (cakeSize == SIZE.SMALL)
                {
                    direction = CakeParamsSO.Entity.SmashVector3[2] * CakeParamsSO.Entity.SmashPower[2];
                }
                if (GameManager.Instance.IsLeftMode)
                {
                    direction.x *= -1;
                }
                rb.AddForce(direction, ForceMode.Impulse);
            }

            // もしもシーンに長く残っていたら消す
            time += Time.deltaTime;
            if (time >= deleteTimeLimit)
            {
                Destroy(gameObject);
            }

            #region もしもレーンから外れたら，吹っ飛ばす状態になっていない場合なら消す。壁にぶつかった場合はクリームを生成してから自身も消す。
            if (cakeSize == SIZE.BIG)
            {
                if (Mathf.Abs(transform.position.z) >= CakeParamsSO.Entity.LaneLimitZList[0])
                {
                    if (!isSmashing)
                    {
                        Destroy(gameObject);
                    }
                    else if (isHitWall)
                    {
                        GenerateCream();
                    }
                }
            }
            else if (cakeSize == SIZE.MEDIUM)
            {
                if (Mathf.Abs(transform.position.z) >= CakeParamsSO.Entity.LaneLimitZList[1])
                {
                    if (!isSmashing)
                    {
                        Destroy(gameObject);
                    }
                    else if (isHitWall)
                    {
                        GenerateCream();
                    }
                }
            }
            else if (cakeSize == SIZE.SMALL)
            {
                if (Mathf.Abs(transform.position.z) >= CakeParamsSO.Entity.LaneLimitZList[2])
                {
                    if (!isSmashing)
                    {
                        Destroy(gameObject);
                    }
                    else if (isHitWall)
                    {
                        GenerateCream();
                    }
                }
            }
            #endregion

            # region ケーキが中央に来たかを判定
            if (cakeSize == SIZE.BIG)
            {
                isAtCenter = Mathf.Abs(transform.position.x) <= CakeParamsSO.Entity.DeletableXList[0] ? true : false;
            }
            else if (cakeSize == SIZE.MEDIUM)
            {
                isAtCenter = Mathf.Abs(transform.position.x) <= CakeParamsSO.Entity.DeletableXList[1] ? true : false;
            }
            else if (cakeSize == SIZE.SMALL)
            {
                isAtCenter = Mathf.Abs(transform.position.x) <= CakeParamsSO.Entity.DeletableXList[2] ? true : false;
            }

            float xCoef = CakeParamsSO.Entity.IsNearCenterXCoef;
            if (cakeSize == SIZE.BIG)
            {
                isNearCenter = Mathf.Abs(transform.position.x) <= CakeParamsSO.Entity.DeletableXList[0] *  xCoef? true : false;
            }
            else if (cakeSize == SIZE.MEDIUM)
            {
                isNearCenter = Mathf.Abs(transform.position.x) <= CakeParamsSO.Entity.DeletableXList[1] * xCoef ? true : false;
            }
            else if (cakeSize == SIZE.SMALL)
            {
                isNearCenter = Mathf.Abs(transform.position.x) <= CakeParamsSO.Entity.DeletableXList[2] * xCoef ? true : false;
            }
            #endregion

            // ケーキを吹っ飛ばす状態でない，かつ疲れていない，かつ隠れていない，かつ警備員に見られていない，
            // かつクリア状態でもゲームオーバー状態でもないときにしか入力を受け付けない
            if (!isSmashing && !GameManager.Instance.IsTired && !GameManager.Instance.IsHiding && !GameManager.Instance.IsLooking && !GameManager.Instance.IsClear && !GameManager.Instance.IsGameOver)
            {
                DeleteOrMiss();
            }
        }

        void DeleteOrMiss()
        {
            // さらに，1番底でプレイヤーの正面にあるもののみ，入力を受け付ける（ただしIsDoingPenaltyがオフの場合）
            if (isAtBottom && !GameManager.Instance.IsDoingPenalty)
            {
                if (isAtCenter)
                {
                    // ボタンを押したら必ず，ハンマーを振ることができる合図を送り，ケーキを吹っ飛ばす状態にする。
                    // Aが赤，Sが緑，Dが青に対応。正しく押したらポイントを増やし，間違ったらIsDoingPenaltyをオンにする。
                    #region
                    if (GameManager.Instance.IsRed >= 0.99f)
                    {
                        GameManager.Instance.IsRed = 0;
                        GameManager.Instance.IsHammerGeneratable = true;
                        isSmashing = true;

                        if (cakeColor == COLOR.RED)
                        {
                            GameManager.Instance.Score -= 1;
                        }
                        else
                        {
                            GameManager.Instance.IsDoingPenalty = true;
                            GameManager.Instance.CurrentStamina -= GameManager.Instance.OnMissedStaminaDecreaseAmount;
                        }
                    }
                    else if (GameManager.Instance.IsGreen >= 0.99f)
                    {
                        GameManager.Instance.IsGreen = 0;
                        GameManager.Instance.IsHammerGeneratable = true;
                        isSmashing = true;

                        if (cakeColor == COLOR.GREEN)
                        {
                            GameManager.Instance.Score -= 1;
                        }
                        else
                        {
                            GameManager.Instance.IsDoingPenalty = true;
                            GameManager.Instance.CurrentStamina -= GameManager.Instance.OnMissedStaminaDecreaseAmount;
                        }
                    }
                    else if (GameManager.Instance.IsBlue >= 0.99f)
                    {
                        GameManager.Instance.IsBlue = 0;
                        GameManager.Instance.IsHammerGeneratable = true;
                        isSmashing = true;

                        if (cakeColor == COLOR.BLUE)
                        {
                            GameManager.Instance.Score -= 1;
                        }
                        else
                        {
                            GameManager.Instance.IsDoingPenalty = true;
                            GameManager.Instance.CurrentStamina -= GameManager.Instance.OnMissedStaminaDecreaseAmount;
                        }
                    }
                    #endregion

                    #region ケーキの位置に、矢印を表示
                    // 初期値を代入
                    GameObject hitTutorial = GameManager.Instance.HitTutorial[0];

                    // 表示するべき矢印の種類を決定
                    if (cakeSize == SIZE.BIG) { hitTutorial = GameManager.Instance.HitTutorial[0]; }
                    else if (cakeSize == SIZE.MEDIUM) { hitTutorial = GameManager.Instance.HitTutorial[1]; }
                    else if (cakeSize == SIZE.SMALL) { hitTutorial = GameManager.Instance.HitTutorial[2]; }

                    // 座標変換
                    RectTransform parentUI = hitTutorial.transform.parent.GetComponent<RectTransform>();
                    Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(parentUI, screenPos, null, out Vector2 uiLocalPos);

                    // 調整項
                    uiLocalPos.x *= CakeParamsSO.Entity.CakeSpeed / 7f * 1.07f;
                    uiLocalPos.y = 0f;

                    // 矢印を表示
                    hitTutorial.SetActive(true);
                    hitTutorial.transform.localPosition = uiLocalPos;
                    #endregion
                }
                else if (isNearCenter)
                {
                    if (GameManager.Instance.IsRed > 0.99f || GameManager.Instance.IsGreen >= 0.9f || GameManager.Instance.IsBlue >= 0.9f)
                    {
                        GameManager.Instance.ShowCakeOutOfRangeUI();　// 叩けないUIを表示（表示中でないときのみ）
                    }
                }
            }
        }

        void GenerateCream()
        {
            float thisX = transform.position.x;
            float thisY = transform.position.y;
            Vector3 generatePos_ = new Vector3(thisX, thisY, CreamParamsSO.Entity.CreamGenerateZ);
            Vector3 cameraPos = mainCamera.transform.position;
            Vector3 direction = generatePos_ - cameraPos;
            Vector3 generatePos = cameraPos + (1 - CreamParamsSO.Entity.CreamGenerateOffset / direction.magnitude) * direction;
            Instantiate(creamPrfb, generatePos, Quaternion.identity);
            Destroy(gameObject);
        }

        IEnumerator TimeCount()
        {
            yield return new WaitForSeconds(audioPlayableTime);
            isSoundPlayable = true;
            yield break;
        }

        // 底のケーキになったか，壁に衝突したか，ハンマーの頭に触れたかを判定
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Desk")
            {
                isAtBottom = true;

                if (isSoundPlayable)
                {
                    audioSourceFall.PlayOneShot(SoundParamsSO.Entity.CakeFallSE);
                }
            }

            if (collision.gameObject.tag == "HammerHead")
            {
                if (isSmashing)
                {
                    isTouchedHammerHead = true;

                    if (!GameManager.Instance.IsDoingPenalty)
                    {
                        audioSourceSmashed.PlayOneShot(SoundParamsSO.Entity.HammerHitCakeSE);
                    }
                }
            }

            if (collision.gameObject.tag == "Wall")
            {
                isHitWall = true;
            }
        }
    }
}