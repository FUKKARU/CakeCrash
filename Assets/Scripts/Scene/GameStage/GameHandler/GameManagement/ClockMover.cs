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
                float timeRatio = GameManager.Instance.TimePassed / GameManager.Instance.ClearTime; // 経過時間の割合
                clockImage.fillAmount = timeRatio; // 時計の回転角を変更
                clockHand.rectTransform.localRotation = Quaternion.Euler(0, 0, timeRatio * -360); // 時計の針を回転
            }
        }
    }
}