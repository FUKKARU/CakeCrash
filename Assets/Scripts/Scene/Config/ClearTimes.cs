using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Main
{
    public class ClearTimes : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI clearTimesText;

        readonly int[] dTimes = { 0, 0, 0, 0 }; // 初期設定：ノルマ達成回数 (E,N,H,SH)
        readonly int[] maxTimes = { 999, 999, 999, 999 }; // ノルマ達成回数の最大値 (E,N,H,SH)

        void Start()
        {
            // ノルマ達成回数のロード
            int eTimes = PlayerPrefs.GetInt("EasyClearTimes", dTimes[0]);
            int nTimes = PlayerPrefs.GetInt("NormalClearTimes", dTimes[1]);
            int hTimes = PlayerPrefs.GetInt("HardClearTimes", dTimes[2]);
            int shTimes = PlayerPrefs.GetInt("SuperHardClearTimes", dTimes[3]);

            // 最大値を越えていないかどうかチェック
            if (eTimes > maxTimes[0]) { eTimes = maxTimes[0]; }
            if (nTimes > maxTimes[1]) { nTimes = maxTimes[1]; }
            if (hTimes > maxTimes[2]) { hTimes = maxTimes[2]; }
            if (shTimes > maxTimes[3]) { shTimes = maxTimes[3]; }

            // 表示
            clearTimesText.text = $"<size=24>ノルマ達成回数：</size>{eTimes} - {nTimes} - {hTimes} - {shTimes}";
        }
    }
}