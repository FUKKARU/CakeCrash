using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class SquatMove : MonoBehaviour
    {
        [SerializeField] private GameObject targetObject; // カメラがターゲットするオブジェクト
        [SerializeField] AudioSource audioSourceSE;
        [SerializeField] AudioSource audioSourceBGM;
        private Transform tr;
        private float a_amplitude; // y = asin(wt) の a
        private float y0; // カメラの初期y座標
        private float y; // y = asin(wt) の y
        private float releasedY; // スペースが離されたときのカメラのy座標
        private float time = 0f; // ゲーム開始からの経過時間
        private float timeCheck = 0f; // 動き初めのtimeを保存(y = asin(wt) の t = time - timeCheck)
        private bool isSquatHeld = false; // フラグ1
        private bool isDownMode = true; // フラグ2

        void Start()
        {
            tr = GetComponent<Transform>();
            y0 = tr.position.y;
            a_amplitude = HumanParamsSO.Entity.DoubleA_YRange / 2;
            audioSourceBGM.enabled = false;
        }

        void Update()
        {
            time += Time.deltaTime;
            // 疲れていない，かつゲームオーバー状態でもクリア状態でもないときのみ，入力を受け付ける。
            if (!isSquatHeld && isDownMode && IA.InputGetter.Instance.IsSquat && !GameManager.Instance.IsGameOver)
            {
                GameManager.Instance.IsHiding = true;
                timeCheck = time;
                isSquatHeld = true;

                audioSourceSE.PlayOneShot(SoundParamsSO.Entity.SquatSE);
            }

            // 1.下降 : スペースを放すと上昇開始。底についてスペースをホールドすると位置をキープ
            if (isSquatHeld && isDownMode)
            {
                float t = time - timeCheck; // 下降開始時からのtをカウント
                // 疲れていない，かつゲームオーバー状態でもクリア状態でもないときのみ，入力を受け付ける。
                if (!IA.InputGetter.Instance.IsSquat && !GameManager.Instance.IsGameOver)
                {
                    releasedY = y;
                    isSquatHeld = false;
                    isDownMode = false;
                    timeCheck = time;

                    audioSourceBGM.enabled = false;
                    // 半分以上下降してから立ち上がるときのみ，SEを再生
                    if (releasedY <= y0 - a_amplitude)
                    {
                        audioSourceSE.PlayOneShot(SoundParamsSO.Entity.StandUpSE);
                    }
                }

                if (HumanParamsSO.Entity.W_Speed * t <= Mathf.PI)
                {
                    // y = acoswt - (a-y0)
                    y = a_amplitude * Mathf.Cos(HumanParamsSO.Entity.W_Speed * t) - (a_amplitude - y0);
                    tr.position = new Vector3(tr.position.x, y, tr.position.z);
                }
                else
                {
                    // y = y0 - 2a
                    y = y0 - 2 * a_amplitude;
                    tr.position = new Vector3(tr.position.x, y, tr.position.z);

                    if (!audioSourceBGM.enabled && isSquatHeld && isDownMode)
                    {
                        audioSourceBGM.enabled = true;
                        audioSourceBGM.clip = SoundParamsSO.Entity.HideSE;
                        audioSourceBGM.Play();
                    }

                }
            }

            // 2.上昇 : 開始位置は一定ではないのでaを変更する。途中でスペースを押しても反応しない
            if (!isSquatHeld && !isDownMode)
            {
                float t = time - timeCheck; // 上昇開始時からのtをカウント
                if (HumanParamsSO.Entity.W_Speed * t <= Mathf.PI)
                {
                    // releasedY ~ y0 までサインカーブで移動
                    a_amplitude = (y0 - releasedY) / 2;
                    // y = -acoswt - (a - y0)
                    y = -a_amplitude * Mathf.Cos(HumanParamsSO.Entity.W_Speed * t) - (a_amplitude - y0);
                    tr.position = new Vector3(tr.position.x, y, tr.position.z);
                }
                else
                {
                    // y = y0
                    y = y0;
                    tr.position = new Vector3(tr.position.x, y0, tr.position.z);
                    a_amplitude = HumanParamsSO.Entity.DoubleA_YRange / 2;
                    isDownMode = true;
                    GameManager.Instance.IsHiding = false;
                }
            }
        }
    }
}