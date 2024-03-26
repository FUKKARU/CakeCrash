using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Main
{
    public class Difficulty : MonoBehaviour
    {
        [SerializeField] Reset resetScript;
        [SerializeField] TMP_Dropdown dropDown;
        bool isSetting = false; // Startの処理が終了するまで，「値が変更された」ことを，検知されないようにするためのフラグ

        void Start()
        {
            isSetting = true;

            int dValue = ConfigParamsSO.Entity.DefaultDifficulty;
            // ロード
            dropDown.value = PlayerPrefs.GetInt("Difficulty", dValue);

            isSetting = false;
        }

        public void OnValueChanged(int value)
        {
            if (!resetScript.IsConfigReseted && !isSetting)
            {
                // セーブ
                PlayerPrefs.SetInt("Difficulty", value);
                PlayerPrefs.Save();
            }
        }
    }
}
