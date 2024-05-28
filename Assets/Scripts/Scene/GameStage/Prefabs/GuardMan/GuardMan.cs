using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class GuardMan : MonoBehaviour
    {
        [SerializeField] private GameObject door;
        [SerializeField] private GameObject handLight;
        [SerializeField] private GuardManAnim guardManAnim;
        [SerializeField] private Transform enter;
        [SerializeField] private Transform exit;
        AudioSource audioSource;
        bool canHandle = false;
        float targetRot = 0;
        float rotStep = 0;
        float timer = 0;

        // �Ď�������ɂȂ�h�A�̉�]�p (y��)
        const float isLookingTrigerDoorRotateY = 70;
        // �h�A���J����C�x���g�̕ϐ� (�l�q���C�J����C���� : �ڕW�p�x�C����)
        readonly Vector2[] doorOpenList = { new Vector2(30, 6) , new Vector2(120, 3) , new Vector2(0, 6) };
        // �h�A���J����C�x���g�̕ϐ� (�l�q��=>�J����C�J����=>�߂� : �҂b��)
        readonly float[] doorOpenWaitTime = { 1, 3 };
        // �t�F�C���g�̃C�x���g�̕ϐ� (�l�q���C���� : �ڕW�p�x�C����)
        readonly Vector2[] feintList = { new Vector2(30, 6), new Vector2(0, 3) };
        // �t�F�C���g�̃C�x���g�̕ϐ� (�l�q��=>�߂� : �҂b��)
        readonly float[] feintWaitTime = { 1 };

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            // �Q�[���I�[�o�[�ɂȂ�����C���̎��̏�ԂŌŒ肷��
            if (GameManager.Instance.IsGameOver)
            {
                StopAllCoroutines();
                gameObject.GetComponent<GuardMan>().enabled = false;
            }

            if (!HumanParamsSO.Entity.GuardManStop)
            {
                timer += Time.deltaTime;
                if (timer >= HumanParamsSO.Entity.EventSpan)
                {
                    timer = 0;
                    StopAllCoroutines();
                    StartCoroutine(CPU());
                }

                UpdateCheck();
                DoorMovement();
            }
        }

        private void UpdateCheck()
        {
            // �h�A�����ȏ�J���Ă���@���@�Ď���
            if (door.transform.eulerAngles.y >= isLookingTrigerDoorRotateY)
            {
                GameManager.Instance.IsLooking = true;
                handLight.SetActive(true);
            }
            else
            {
                GameManager.Instance.IsLooking = false;
                handLight.SetActive(false);
            }

            // �ړI�̊p�x�ɂȂ����瑀��\�ɂ���
            if (Mathf.Abs(door.transform.eulerAngles.y - targetRot) <= 1)
                canHandle = true;
        }

        // �ړI�̊p�x�܂ŉ�]������
        private void DoorMovement()
        {
            Quaternion temp = Quaternion.AngleAxis(targetRot, Vector3.up);
            door.transform.rotation = Quaternion.RotateTowards(door.transform.rotation, temp, rotStep);
        }

        // �h�A�̊p�x�𑀍�
        private IEnumerator Handling(Vector2 rotInfo)
        {
            float rot = rotInfo.x;
            float step = rotInfo.y;
            yield return new WaitUntil(() => canHandle);
            canHandle = false;
            targetRot = rot;
            rotStep = step;
        }

        private IEnumerator CPU()
        {
            // ����
            GuardManAnim anim = Instantiate(guardManAnim, exit.position, Quaternion.Euler(0, 180f, 0));
            anim.Setup(enter);
            yield return new WaitForSeconds(3f);
            Destroy(anim.gameObject);

            int r = Random.Range(1, HumanParamsSO.Entity.FeintProbability + 1);
            if (r == 1)
            {
                StartCoroutine(Feint());
            }
            else
            {
                StartCoroutine(Open());
            }
        }

        private IEnumerator Open()
        {
            audioSource.PlayOneShot(SoundParamsSO.Entity.DoorHalfOpenSE);
            StartCoroutine(Handling(doorOpenList[0]));
            GameManager.Instance.SquatAnnounceUI.SetActive(true);

            yield return new WaitForSeconds(doorOpenWaitTime[0]);
            audioSource.PlayOneShot(SoundParamsSO.Entity.DoorOpenSE);
            StartCoroutine(Handling(doorOpenList[1]));

            yield return new WaitForSeconds(doorOpenWaitTime[1]);
            audioSource.PlayOneShot(SoundParamsSO.Entity.DoorCloseSE);
            yield return StartCoroutine(Handling(doorOpenList[2]));
            GameManager.Instance.SquatAnnounceUI.SetActive(false);

            // ����
            GuardManAnim anim = Instantiate(guardManAnim, enter.position, Quaternion.Euler(0, 0, 0));
            anim.Setup(exit);
            yield return new WaitForSeconds(3f);
            Destroy(anim.gameObject);
        }

        private IEnumerator Feint()
        {
            audioSource.PlayOneShot(SoundParamsSO.Entity.DoorHalfOpenSE);
            StartCoroutine(Handling(feintList[0]));
            GameManager.Instance.SquatAnnounceUI.SetActive(true);

            yield return new WaitForSeconds(feintWaitTime[0]);
            audioSource.PlayOneShot(SoundParamsSO.Entity.DoorCloseSE);
            yield return StartCoroutine(Handling(feintList[1]));
            GameManager.Instance.SquatAnnounceUI.SetActive(false);

            // ����
            GuardManAnim anim = Instantiate(guardManAnim, enter.position, Quaternion.Euler(0, 0, 0));
            anim.Setup(exit);
            yield return new WaitForSeconds(3f);
            Destroy(anim.gameObject);
        }
    }
}