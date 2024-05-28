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
        bool isSetting = false; // Start�̏������I������܂ŁC�u�l���ύX���ꂽ�v���Ƃ��C���m����Ȃ��悤�ɂ��邽�߂̃t���O

        void Start()
        {
            isSetting = true;

            int dValue = ConfigParamsSO.Entity.DefaultSoundVolume[0];
            // ���[�h
            int value = PlayerPrefs.GetInt("BGMVolume", dValue);

            slider.value = value;
            text.text = $"BGM�̉��ʁi{value}�j";
            audioMixer.SetFloat("BGMParam", value);

            isSetting = false;
        }

        public void OnValueChanged(float value_)
        {
            if (!resetScript.IsConfigReseted && !isSetting)
            {
                int value = (int)value_;

                // �Z�[�u
                PlayerPrefs.SetInt("BGMVolume", value);
                PlayerPrefs.Save();

                text.text = $"BGM�̉��ʁi{value}�j";
                audioMixer.SetFloat("BGMParam", value);
            }
        }
    }
}