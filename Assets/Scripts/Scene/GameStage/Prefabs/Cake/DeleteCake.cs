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

        IsMissingHandler isMissingHandler;
        Transform creamParent;
        [SerializeField] GameObject creamPrfb;
        Rigidbody rb;
        public bool IsTouchedHammerHead { get; set; } = false; // ハンマーの頭に触れたかどうか
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
            isMissingHandler = GameObject.FindGameObjectWithTag("IsMissingHandler").GetComponent<IsMissingHandler>();
            creamParent = GameObject.FindGameObjectWithTag("CreamParent").transform;
            rb = GetComponent<Rigidbody>();
            StartCoroutine(TimeCount());
        }

        void Update()
        {
            // ケーキがハンマーに触れたらケーキを吹っ飛ばす。
            if (IsTouchedHammerHead && !isSmashed)
            {
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

            #region もしもレーンから外れたら消す。壁にぶつかった場合はクリームを生成してから自身も消す。
            if (cakeSize == SIZE.BIG)
            {
                if (Mathf.Abs(transform.position.z) >= CakeParamsSO.Entity.LaneLimitZList[0])
                {
                    if (isHitWall)
                    {
                        GenerateCream();
                    }
                }
            }
            else if (cakeSize == SIZE.MEDIUM)
            {
                if (Mathf.Abs(transform.position.z) >= CakeParamsSO.Entity.LaneLimitZList[1])
                {
                    if (isHitWall)
                    {
                        GenerateCream();
                    }
                }
            }
            else if (cakeSize == SIZE.SMALL)
            {
                if (Mathf.Abs(transform.position.z) >= CakeParamsSO.Entity.LaneLimitZList[2])
                {
                    if (isHitWall)
                    {
                        GenerateCream();
                    }
                }
            }
            #endregion
        }

        void GenerateCream()
        {
            float thisX = transform.position.x;
            float thisY = transform.position.y;
            Vector3 generatePos_ = new Vector3(thisX, thisY, CreamParamsSO.Entity.CreamGenerateZ);
            Vector3 cameraPos = mainCamera.transform.position;
            Vector3 direction = generatePos_ - cameraPos;
            Vector3 generatePos = cameraPos + (1 - CreamParamsSO.Entity.CreamGenerateOffset / direction.magnitude) * direction;
            if (GameManager.Instance.missCream == null) Instantiate(creamPrfb, generatePos, Quaternion.identity, creamParent);
            Destroy(gameObject);
        }

        IEnumerator TimeCount()
        {
            yield return new WaitForSeconds(audioPlayableTime);
            isSoundPlayable = true;
        }

        // 机または壁に触れた
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Desk"))
            {
                if (isSoundPlayable)
                {
                    audioSourceFall.PlayOneShot(SoundParamsSO.Entity.CakeFallSE);
                }
            }

            if (collision.gameObject.CompareTag("Wall"))
            {
                isHitWall = true;
            }
        }

        // ハンマーの頭に触れた時に呼ばれる
        public void HitHammer()
        {
            IsTouchedHammerHead = true;
            audioSourceSmashed.PlayOneShot(SoundParamsSO.Entity.HammerHitCakeSE);

            if (cakeColor == COLOR.RED)
            {
                if (GameManager.Instance.PushedColor == GameManager.PUSHED_COLOR.RED)
                {
                    GameManager.Instance.Score--;
                    GameManager.Instance.ComboContinuation();
                }
                else
                {
                    isMissingHandler.MissCreamGenerate();
                    GameManager.Instance.ComboEnd();
                    Destroy(gameObject);
                }
            }
            else if (cakeColor == COLOR.GREEN)
            {
                if (GameManager.Instance.PushedColor == GameManager.PUSHED_COLOR.GREEN)
                {
                    GameManager.Instance.Score--;
                    GameManager.Instance.ComboContinuation();
                }
                else
                {
                    isMissingHandler.MissCreamGenerate();
                    GameManager.Instance.ComboEnd();
                    Destroy(gameObject);
                }
            }
            else if (cakeColor == COLOR.BLUE)
            {
                if (GameManager.Instance.PushedColor == GameManager.PUSHED_COLOR.BLUE)
                {
                    GameManager.Instance.Score--;
                    GameManager.Instance.ComboContinuation();
                }
                else
                {
                    isMissingHandler.MissCreamGenerate();
                    GameManager.Instance.ComboEnd();
                    Destroy(gameObject);
                }
            }
        }
    }
}