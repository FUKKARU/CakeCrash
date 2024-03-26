using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class HammerSmash : MonoBehaviour
    {
        [SerializeField] AudioSource audioSource;

        void Start()
        {
            audioSource.PlayOneShot(SoundParamsSO.Entity.HammerSmashSE);
        }

        void Update()
        {
            if (!GameManager.Instance.IsLeftMode)
            {
                // 回転
                transform.Rotate(Vector3.down * 10 * (HumanParamsSO.Entity.HammerSpeed * Time.deltaTime));

                // 振り終わったら消す。
                if (HumanParamsSO.Entity.HammerEulerY.x < transform.eulerAngles.y && transform.eulerAngles.y < HumanParamsSO.Entity.HammerEulerY.y)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                // 回転
                transform.Rotate(Vector3.up * 10 * (HumanParamsSO.Entity.HammerSpeed * Time.deltaTime));

                // 振り終わったら消す。
                if (360 - HumanParamsSO.Entity.HammerEulerY.y < transform.eulerAngles.y && transform.eulerAngles.y < 360 - HumanParamsSO.Entity.HammerEulerY.x)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}