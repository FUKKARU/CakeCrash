using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Main
{
    public class ClearTimes : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI clearTimesText;

        readonly int[] dTimes = { 0, 0, 0, 0 }; // �����ݒ�F�m���}�B���� (E,N,H,SH)
        readonly int[] maxTimes = { 999, 999, 999, 999 }; // �m���}�B���񐔂̍ő�l (E,N,H,SH)

        void Start()
        {
            // �m���}�B���񐔂̃��[�h
            int eTimes = PlayerPrefs.GetInt("EasyClearTimes", dTimes[0]);
            int nTimes = PlayerPrefs.GetInt("NormalClearTimes", dTimes[1]);
            int hTimes = PlayerPrefs.GetInt("HardClearTimes", dTimes[2]);
            int shTimes = PlayerPrefs.GetInt("SuperHardClearTimes", dTimes[3]);

            // �ő�l���z���Ă��Ȃ����ǂ����`�F�b�N
            if (eTimes > maxTimes[0]) { eTimes = maxTimes[0]; }
            if (nTimes > maxTimes[1]) { nTimes = maxTimes[1]; }
            if (hTimes > maxTimes[2]) { hTimes = maxTimes[2]; }
            if (shTimes > maxTimes[3]) { shTimes = maxTimes[3]; }

            // �\��
            clearTimesText.text = $"<size=24>�m���}�B���񐔁F</size>{eTimes} - {nTimes} - {hTimes} - {shTimes}";
        }
    }
}