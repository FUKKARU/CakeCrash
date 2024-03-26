using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Main
{
    public class ClockMover : MonoBehaviour
    {
        [SerializeField] Image clockImage;
        [SerializeField] Image clockHand;

        void Update()
        {
            if (!GameManager.Instance.IsClear && !GameManager.Instance.IsGameOver)
            {
                float timeRatio = GameManager.Instance.TimePassed / GameManager.Instance.ClearTime; // �o�ߎ��Ԃ̊���
                clockImage.fillAmount = timeRatio; // ���v�̉�]�p��ύX
                clockHand.rectTransform.localRotation = Quaternion.Euler(0, 0, timeRatio * -360); // ���v�̐j����]
            }
        }
    }
}