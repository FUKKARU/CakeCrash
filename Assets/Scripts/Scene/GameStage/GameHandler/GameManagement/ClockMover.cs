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
                float timeRatio = GameManager.Instance.TimePassed / GameManager.Instance.ClearTime; // Œo‰ßŠÔ‚ÌŠ„‡
                clockImage.fillAmount = timeRatio; // Œv‚Ì‰ñ“]Šp‚ğ•ÏX
                clockHand.rectTransform.localRotation = Quaternion.Euler(0, 0, timeRatio * -360); // Œv‚Ìj‚ğ‰ñ“]
            }
        }
    }
}