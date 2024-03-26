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

        // �P�[�L�̐F
        enum COLOR { NULL, RED, GREEN, BLUE }
        [SerializeField] COLOR cakeColor = COLOR.NULL;

        // �P�[�L�̃T�C�Y
        enum SIZE { NULL, BIG, MEDIUM, SMALL }
        [SerializeField] SIZE cakeSize = SIZE.NULL;

        [SerializeField] GameObject creamPrfb;
        Rigidbody rb;
        bool isAtCenter = false;
        bool isNearCenter = false;
        bool isAtBottom = false;
        bool isSmashing = false; // ������΂���ԂɂȂ��Ă��邩�ǂ���
        bool isTouchedHammerHead = false; // �n���}�[�̓��ɐG�ꂽ���ǂ���
        bool isSmashed = false; // ������΂��ꂽ���ǂ���(1�񂾂����s���邽�߂̂����̃t���O)
        bool isHitWall = false;
        bool isSoundPlayable = false; // ���������Đ��ł��邩�ǂ���
        float time = 0f;

        const float deleteTimeLimit = 15; // �P�[�L���V�[���ɉ��b�ȏ㑶�݂����������
        const float audioPlayableTime = 0.5f; // ��������Ă���C���������Đ��\�ɂȂ�܂ł̎���(�b)

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
            // �P�[�L���n���}�[�ɐG�ꂽ�C��������΂���Ԃ̂Ƃ��̂݁C�P�[�L�𐁂���΂��B
            if (isTouchedHammerHead && !isSmashed && isSmashing)
            {
                // �~�X�����ꍇ�͐�����΂��̂ł͂Ȃ������B
                if (GameManager.Instance.IsDoingPenalty)
                {
                    Destroy(gameObject);
                }

                isSmashed = true; // ������΂��ꂽ�̂ŁC����if���͂������s����Ȃ��B

                gameObject.layer = 6; // ���C���[��ς���B

                // �Î~���C�͂Ɠ����C�͂�0�ɂ��Đ�����΂��B
                // Friction Combine �� Minimum �Ȃ̂ŁC��̃P�[�L�Ƃ̖��C��0�ɂȂ�B
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

            // �������V�[���ɒ����c���Ă��������
            time += Time.deltaTime;
            if (time >= deleteTimeLimit)
            {
                Destroy(gameObject);
            }

            #region ���������[������O�ꂽ��C������΂���ԂɂȂ��Ă��Ȃ��ꍇ�Ȃ�����B�ǂɂԂ������ꍇ�̓N���[���𐶐����Ă��玩�g�������B
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

            # region �P�[�L�������ɗ������𔻒�
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

            // �P�[�L�𐁂���΂���ԂłȂ��C�����Ă��Ȃ��C���B��Ă��Ȃ��C���x�����Ɍ����Ă��Ȃ��C
            // ���N���A��Ԃł��Q�[���I�[�o�[��Ԃł��Ȃ��Ƃ��ɂ������͂��󂯕t���Ȃ�
            if (!isSmashing && !GameManager.Instance.IsTired && !GameManager.Instance.IsHiding && !GameManager.Instance.IsLooking && !GameManager.Instance.IsClear && !GameManager.Instance.IsGameOver)
            {
                DeleteOrMiss();
            }
        }

        void DeleteOrMiss()
        {
            // ����ɁC1�Ԓ�Ńv���C���[�̐��ʂɂ�����̂̂݁C���͂��󂯕t����i������IsDoingPenalty���I�t�̏ꍇ�j
            if (isAtBottom && !GameManager.Instance.IsDoingPenalty)
            {
                if (isAtCenter)
                {
                    // �{�^������������K���C�n���}�[��U�邱�Ƃ��ł��鍇�}�𑗂�C�P�[�L�𐁂���΂���Ԃɂ���B
                    // A���ԁCS���΁CD���ɑΉ��B��������������|�C���g�𑝂₵�C�Ԉ������IsDoingPenalty���I���ɂ���B
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

                    #region �P�[�L�̈ʒu�ɁA����\��
                    // �����l����
                    GameObject hitTutorial = GameManager.Instance.HitTutorial[0];

                    // �\������ׂ����̎�ނ�����
                    if (cakeSize == SIZE.BIG) { hitTutorial = GameManager.Instance.HitTutorial[0]; }
                    else if (cakeSize == SIZE.MEDIUM) { hitTutorial = GameManager.Instance.HitTutorial[1]; }
                    else if (cakeSize == SIZE.SMALL) { hitTutorial = GameManager.Instance.HitTutorial[2]; }

                    // ���W�ϊ�
                    RectTransform parentUI = hitTutorial.transform.parent.GetComponent<RectTransform>();
                    Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(parentUI, screenPos, null, out Vector2 uiLocalPos);

                    // ������
                    uiLocalPos.x *= CakeParamsSO.Entity.CakeSpeed / 7f * 1.07f;
                    uiLocalPos.y = 0f;

                    // ����\��
                    hitTutorial.SetActive(true);
                    hitTutorial.transform.localPosition = uiLocalPos;
                    #endregion
                }
                else if (isNearCenter)
                {
                    if (GameManager.Instance.IsRed > 0.99f || GameManager.Instance.IsGreen >= 0.9f || GameManager.Instance.IsBlue >= 0.9f)
                    {
                        GameManager.Instance.ShowCakeOutOfRangeUI();�@// �@���Ȃ�UI��\���i�\�����łȂ��Ƃ��̂݁j
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

        // ��̃P�[�L�ɂȂ������C�ǂɏՓ˂������C�n���}�[�̓��ɐG�ꂽ���𔻒�
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