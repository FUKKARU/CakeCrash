using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class HammerGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject hammerPrfb;

        void Update()
        {
            // ハンマーが生成可能になったら，ハンマーを一回だけ生成
            if (GameManager.Instance.IsHammerGeneratable)
            {
                GameManager.Instance.IsHammerGeneratable = false;
                GameManager.Instance.IsHammerShakable = true;
                Quaternion rot;
                if (!GameManager.Instance.IsLeftMode)
                {
                    rot = Quaternion.Euler(0, HumanParamsSO.Entity.HammerEulerY.x, 0);
                }
                else
                {
                    rot = Quaternion.Euler(0, 360 - HumanParamsSO.Entity.HammerEulerY.x, 0);
                }
                Instantiate(hammerPrfb, HumanParamsSO.Entity.HammerGeneratePosition, rot);
            }
        }
    }
}
