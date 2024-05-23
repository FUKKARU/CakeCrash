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

        IsMissingHandler isMissingHandler;
        Transform creamParent;
        [SerializeField] GameObject creamPrfb;
        Rigidbody rb;
        public bool IsTouchedHammerHead { get; set; } = false; // �n���}�[�̓��ɐG�ꂽ���ǂ���
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
            isMissingHandler = GameObject.FindGameObjectWithTag("IsMissingHandler").GetComponent<IsMissingHandler>();
            creamParent = GameObject.FindGameObjectWithTag("CreamParent").transform;
            rb = GetComponent<Rigidbody>();
            StartCoroutine(TimeCount());
        }

        void Update()
        {
            // �P�[�L���n���}�[�ɐG�ꂽ��P�[�L�𐁂���΂��B
            if (IsTouchedHammerHead && !isSmashed)
            {
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

            #region ���������[������O�ꂽ������B�ǂɂԂ������ꍇ�̓N���[���𐶐����Ă��玩�g�������B
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

        // ���܂��͕ǂɐG�ꂽ
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

        // �n���}�[�̓��ɐG�ꂽ���ɌĂ΂��
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