using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Main
{
    // �m���}�N���A�񐔂̃Z�[�u�f�[�^��S�č폜����B
    public class ClearTimesResetter : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI clearTimesText;
        [SerializeField] AudioSource audioSource;
        float clearTimesResetTime = 0; // �N���A�񐔂��폜����{�^����������Ă��鎞��
        bool isReseted = false; // �폜�������������ǂ���

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

                        clearTimesText.text = "<size=24>�m���}�B���񐔁F</size>0 - 0 - 0 - 0";

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