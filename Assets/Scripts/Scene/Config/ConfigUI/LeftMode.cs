using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Main
{
    public class LeftMode : MonoBehaviour
    {
        [SerializeField] Reset resetScript;
        [SerializeField] Toggle toggle;
        bool isSetting = false; // Startの処理が終了するまで，「値が変更された」ことを，検知されないようにするためのフラグ

        void Start()
        {
            isSetting = true;

            int dValue = ConfigParamsSO.Entity.DefaultLeftMode;
            // ロード
            toggle.isOn = (PlayerPrefs.GetInt("LeftMode", dValue) == 1) ? true : false;

            isSetting = false;
        }

        public void OnValueChanged(bool value)
        {
            if (resetScript.IsConfigReseted || isSetting)
            {
                return;
            }
            else
            {
                // セーブ
                PlayerPrefs.SetInt("LeftMode", (value == true) ? 1 : 0);
                PlayerPrefs.Save();
            }
        }
    }
}
