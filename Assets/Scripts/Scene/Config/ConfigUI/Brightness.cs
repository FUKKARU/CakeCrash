using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Main
{
    public class Brightness : MonoBehaviour
    {
        [SerializeField] Reset resetScript;
        [SerializeField] Slider slider;
        bool isSetting = false; // Startの処理が終了するまで，「値が変更された」ことを，検知されないようにするためのフラグ

        void Start()
        {
            isSetting = true;

            int dValue = ConfigParamsSO.Entity.DefaultBrightness;
            // ロード
            int value = PlayerPrefs.GetInt("Brightness", dValue);

            slider.value = value;
            // I = d * var1^(n/var2)
            float d = ConfigParamsSO.Entity.DefaultBrightnessVars[0];
            float var1 = ConfigParamsSO.Entity.DefaultBrightnessVars[1];
            float var2 = ConfigParamsSO.Entity.DefaultBrightnessVars[2];
            RenderSettings.ambientIntensity = d * Mathf.Pow(var1, value / var2);

            isSetting = false;
        }

        public void OnValueChanged(float value)
        {
            if (!resetScript.IsConfigReseted && !isSetting)
            {
                // セーブ
                PlayerPrefs.SetInt("Brightness", (int)value);
                PlayerPrefs.Save();

                // I = d * var1^(n/var2)
                float d = ConfigParamsSO.Entity.DefaultBrightnessVars[0];
                float var1 = ConfigParamsSO.Entity.DefaultBrightnessVars[1];
                float var2 = ConfigParamsSO.Entity.DefaultBrightnessVars[2];
                RenderSettings.ambientIntensity = d * Mathf.Pow(var1, value / var2);
            }
        }
    }
}