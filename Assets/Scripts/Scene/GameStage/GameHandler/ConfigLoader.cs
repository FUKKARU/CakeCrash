using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class ConfigLoader : MonoBehaviour
    {
        //[SerializeField] GameObject slider;

        void Awake()
        {
            LoadLeftMode();
            LoadBrightness();
            LoadDifficulty();
        }

        void LoadLeftMode()
        {
            int dValue = ConfigParamsSO.Entity.DefaultLeftMode;
            bool data = (PlayerPrefs.GetInt("LeftMode", dValue) == 1) ? true : false;
            GameManager.Instance.IsLeftMode = data;
        }

        void LoadBrightness()
        {
            int dValue = ConfigParamsSO.Entity.DefaultBrightness;
            int value = PlayerPrefs.GetInt("Brightness", dValue);
            // I = d * var1^(n/var2)
            float d = ConfigParamsSO.Entity.DefaultBrightnessVars[0];
            float var1 = ConfigParamsSO.Entity.DefaultBrightnessVars[1];
            float var2 = ConfigParamsSO.Entity.DefaultBrightnessVars[2];
            RenderSettings.ambientIntensity = d * Mathf.Pow(var1, value / var2);
        }

        void LoadDifficulty()
        {
            int dValue = ConfigParamsSO.Entity.DefaultDifficulty;
            int difficulty = PlayerPrefs.GetInt("Difficulty", dValue);
        }
    }
}