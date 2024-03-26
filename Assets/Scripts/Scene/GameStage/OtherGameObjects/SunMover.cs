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
            thetaRange = OtherParamsSO.Entity.LightTheta.y - OtherParamsSO.Entity.LightTheta.x; // �����p�x�̕�(�����t��)
        }

        void Update()
        {
            // �N���A�ɂ��Q�[���I�[�o�[�ɂ��Ȃ��Ă��Ȃ��Ȃ��
            if (!GameManager.Instance.IsClear && !GameManager.Instance.IsGameOver)
            {
                // �o�ߎ��Ԃ̊������猻�݂̊p�x���v�Z
                float timeRatio = GameManager.Instance.TimePassed / GameManager.Instance.ClearTime;
                float nowTheta = OtherParamsSO.Entity.LightTheta.x + thetaRange * timeRatio;

                transform.rotation = Quaternion.Euler(nowTheta, -90, 0); // ��]

                // �n�����ɉB��Ă���Ԃ͔�A�N�e�B�u
                if (-180 < nowTheta && nowTheta < 0)
                {
                    light.enabled = false;
                }
                else
                {
                    light.enabled = true;
                }
                
                // �Q�[���������i�s�����i�K�ŁA1�x�t�N���E��SE��炷�B
                if (!isOwlHootSEPlayed && timeRatio > 0.5f)
                {
                    isOwlHootSEPlayed = true;
                    audioSource.PlayOneShot(SoundParamsSO.Entity.OwlHootSE);
                }
            }
        }
    }
}
