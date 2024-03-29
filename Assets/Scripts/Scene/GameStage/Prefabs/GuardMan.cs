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

        // 監視中判定になるドアの回転角 (y軸)
        const float isLookingTrigerDoorRotateY = 70;
        // ドアを開けるイベントの変数 (様子見，開ける，閉じる : 目標角度，増分)
        readonly Vector2[] doorOpenList = { new Vector2(30, 6) , new Vector2(120, 3) , new Vector2(0, 6) };
        // ドアを開けるイベントの変数 (様子見=>開ける，開ける=>閉める : 待つ秒数)
        readonly float[] doorOpenWaitTime = { 1, 3 };
        // フェイントのイベントの変数 (様子見，閉じる : 目標角度，増分)
        readonly Vector2[] feintList = { new Vector2(30, 6), new Vector2(0, 3) };
        // フェイントのイベントの変数 (様子見=>閉める : 待つ秒数)
        readonly float[] feintWaitTime = { 1 };

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            // ゲームオーバーになったら，その時の状態で固定する
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
            // ドアが一定以上開いている　→　監視中
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

            // 目的の角度になったら操作可能にする
            if (Mathf.Abs(door.transform.eulerAngles.y - targetRot) <= 1)
                canHandle = true;
        }

        // 目的の角度まで回転させる
        private void DoorMovement()
        {
            Quaternion temp = Quaternion.AngleAxis(targetRot, Vector3.up);
            door.transform.rotation = Quaternion.RotateTowards(door.transform.rotation, temp, rotStep);
        }

        // ドアの角度を操作
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
            // 歩く
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

            // 歩く
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

            // 歩く
            GuardManAnim anim = Instantiate(guardManAnim, enter.position, Quaternion.Euler(0, 0, 0));
            anim.Setup(exit);
            yield return new WaitForSeconds(3f);
            Destroy(anim.gameObject);
        }
    }
}