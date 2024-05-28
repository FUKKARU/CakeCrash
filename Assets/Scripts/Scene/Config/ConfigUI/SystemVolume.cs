using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Main
{
    public class SystemVolume : MonoBehaviour
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

            int dValue = ConfigParamsSO.Entity.DefaultSoundVolume[2];
            // ���[�h
            int value = PlayerPrefs.GetInt("SystemVolume", dValue);

            slider.value = value;
            text.text = $"�V�X�e�����̉��ʁi{value}�j";
            audioMixer.SetFloat("SystemParam", value);

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
                PlayerPrefs.SetInt("SystemVolume", value);
                PlayerPrefs.Save();

                text.text = $"�V�X�e�����̉��ʁi{value}�j";
                audioMixer.SetFloat("SystemParam", value);
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