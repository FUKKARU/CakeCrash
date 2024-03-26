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
        bool isSetting = false; // Start�̏������I������܂ŁC�u�l���ύX���ꂽ�v���Ƃ��C���m����Ȃ��悤�ɂ��邽�߂̃t���O

        void Start()
        {
            isSetting = true;

            int dValue = ConfigParamsSO.Entity.DefaultBrightness;
            // ���[�h
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
                // �Z�[�u
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