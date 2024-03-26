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
        bool isSetting = false; // Startの処理が終了するまで，「値が変更された」ことを，検知されないようにするためのフラグ

        void Start()
        {
            isSetting = true;

            int dValue = ConfigParamsSO.Entity.DefaultSoundVolume[2];
            // ロード
            int value = PlayerPrefs.GetInt("SystemVolume", dValue);

            slider.value = value;
            text.text = $"システム音の音量（{value}）";
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

                // セーブ
                PlayerPrefs.SetInt("SystemVolume", value);
                PlayerPrefs.Save();

                text.text = $"システム音の音量（{value}）";
                audioMixer.SetFloat("SystemParam", value);
            }
        }

        // 音を鳴らす
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