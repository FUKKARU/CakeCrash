using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Main
{
    public class BGMVolume : MonoBehaviour
    {
        [SerializeField] Reset resetScript;
        [SerializeField] Slider slider;
        [SerializeField] TextMeshProUGUI text;
        [SerializeField] AudioMixer audioMixer;
        bool isSetting = false; // Startの処理が終了するまで，「値が変更された」ことを，検知されないようにするためのフラグ

        void Start()
        {
            isSetting = true;

            int dValue = ConfigParamsSO.Entity.DefaultSoundVolume[0];
            // ロード
            int value = PlayerPrefs.GetInt("BGMVolume", dValue);

            slider.value = value;
            text.text = $"BGMの音量（{value}）";
            audioMixer.SetFloat("BGMParam", value);

            isSetting = false;
        }

        public void OnValueChanged(float value_)
        {
            if (!resetScript.IsConfigReseted && !isSetting)
            {
                int value = (int)value_;

                // セーブ
                PlayerPrefs.SetInt("BGMVolume", value);
                PlayerPrefs.Save();

                text.text = $"BGMの音量（{value}）";
                audioMixer.SetFloat("BGMParam", value);
            }
        }
    }
}