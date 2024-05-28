using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Main
{
    // ノルマクリア回数のセーブデータを全て削除する。
    public class ClearTimesResetter : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI clearTimesText;
        [SerializeField] AudioSource audioSource;
        float clearTimesResetTime = 0; // クリア回数を削除するボタンが押されている時間
        bool isReseted = false; // 削除が完了したかどうか

        void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.RightArrow))
            {
                if (!isReseted)
                {
                    clearTimesResetTime += Time.deltaTime;
                    if (clearTimesResetTime >= OtherParamsSO.Entity.ClearTimesResetHoldPeriod)
                    {
                        isReseted = true;

                        PlayerPrefs.DeleteKey("EasyClearTimes");
                        PlayerPrefs.DeleteKey("NormalClearTimes");
                        PlayerPrefs.DeleteKey("HardClearTimes");
                        PlayerPrefs.DeleteKey("SuperHardClearTimes");

                        clearTimesText.text = "<size=24>ノルマ達成回数：</size>0 - 0 - 0 - 0";

                        audioSource.PlayOneShot(SoundParamsSO.Entity.ButtonClickSystem);
                    }
                }
            }
            else
            {
                clearTimesResetTime = 0;

                isReseted = false;
            }
        }
    }
}