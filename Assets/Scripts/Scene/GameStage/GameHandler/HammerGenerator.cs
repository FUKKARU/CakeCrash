using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class HammerGenerator : MonoBehaviour
    {
        [SerializeField] Transform hammerParent;
        [SerializeField] private GameObject hammerPrfb;

        void Update()
        {
            // �n���}�[�������\�ɂȂ�����C�n���}�[����񂾂�����
            if (GameManager.Instance.IsHammerGeneratable)
            {
                GameManager.Instance.IsHammerGeneratable = false;
                GameManager.Instance.IsHammerShakable = true;
                GameObject hammer = Instantiate(hammerPrfb, HumanParamsSO.Entity.HammerGeneratePosition, Quaternion.identity, hammerParent);
                GameManager.Instance.Hammers.Add(hammer);
            }
        }
    }
}
