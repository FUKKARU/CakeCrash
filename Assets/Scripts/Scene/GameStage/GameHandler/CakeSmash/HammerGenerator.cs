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
                Instantiate(hammerPrfb, HumanParamsSO.Entity.HammerGeneratePosition, Quaternion.identity);
            }
        }
    }
}
