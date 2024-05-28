using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Main
{
    public class SEVolume : MonoBehaviour
    {
        [SerializeField] Reset resetScript;
        [SerializeField] Slider slider;
        [SerializeField] TextMeshProUGUI text;
        [SerializeField] AudioMixer audioMixer;
        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioClip audioClip;
        bool isSetting = false; // Start�̏������I������܂ŁC�u�l���ύX���ꂽ�v���Ƃ��C���m����Ȃ��悤�ɂ��邽�߂̃t���O

        void Start()
        {
            isSetting = true;

            int dValue = ConfigParamsSO.Entity.DefaultSoundVolume[1];
            // ���[�h
            int value = PlayerPrefs.GetInt("SEVolume", dValue);

            slider.value = value;
            text.text = $"�����̉��ʁi{value}�j";
            audioMixer.SetFloat("SEParam", value);

            isSetting = false;
        }

        public void OnValueChanged(float value_)
        {
            if (!resetScript.IsConfigReseted && !isSetting)
            {
                StopAllCoroutines();
                StartCoroutine(WaitForMouseUp());

                int value = (int)value_;

                // �Z�[�u
                PlayerPrefs.SetInt("SEVolume", value);
                PlayerPrefs.Save();

                text.text = $"�����̉��ʁi{value}�j";
                audioMixer.SetFloat("SEParam", value);
            }
        }

        // ����炷
        IEnumerator WaitForMouseUp()
        {
            while (true)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    audioSource.PlayOneShot(audioClip);
                    yield break;
                }

                yield return null;
            }
        }
    }
}