using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class SunMover : MonoBehaviour
    {
        private AudioSource audioSource;
        private float thetaRange;
        new Light light;
        private bool isOwlHootSEPlayed = false;

        void Start()
        {
            light = GetComponent<Light>();
            audioSource = GetComponent<AudioSource>();
            thetaRange = OtherParamsSO.Entity.LightTheta.y - OtherParamsSO.Entity.LightTheta.x; // 動く角度の幅(符号付き)
        }

        void Update()
        {
            // クリアにもゲームオーバーにもなっていないならば
            if (!GameManager.Instance.IsClear && !GameManager.Instance.IsGameOver)
            {
                // 経過時間の割合から現在の角度を計算
                float timeRatio = GameManager.Instance.TimePassed / GameManager.Instance.ClearTime;
                float nowTheta = OtherParamsSO.Entity.LightTheta.x + thetaRange * timeRatio;

                transform.rotation = Quaternion.Euler(nowTheta, -90, 0); // 回転

                // 地平線に隠れている間は非アクティブ
                if (-180 < nowTheta && nowTheta < 0)
                {
                    light.enabled = false;
                }
                else
                {
                    light.enabled = true;
                }
                
                // ゲームが半分進行した段階で、1度フクロウのSEを鳴らす。
                if (!isOwlHootSEPlayed && timeRatio > 0.5f)
                {
                    isOwlHootSEPlayed = true;
                    audioSource.PlayOneShot(SoundParamsSO.Entity.OwlHootSE);
                }
            }
        }
    }
}
