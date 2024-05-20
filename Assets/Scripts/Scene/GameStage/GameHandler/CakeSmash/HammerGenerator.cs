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
            // �n���}�[�������\�ɂȂ�����C�n���}�[����񂾂�����
            if (GameManager.Instance.IsHammerGeneratable)
            {
                GameManager.Instance.IsHammerGeneratable = false;
                GameManager.Instance.IsHammerShakable = true;
                Instantiate(hammerPrfb, HumanParamsSO.Entity.HammerGeneratePosition, Quaternion.identity);
            }
        }
    }
}
