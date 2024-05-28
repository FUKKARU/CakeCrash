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
        bool isSetting = false; // Startの処理が終了するまで，「値が変更された」ことを，検知されないようにするためのフラグ

        void Start()
        {
            isSetting = true;

            int dValue = ConfigParamsSO.Entity.DefaultSoundVolume[1];
            // ロード
            int value = PlayerPrefs.GetInt("SEVolume", dValue);

            slider.value = value;
            text.text = $"環境音の音量（{value}）";
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

                // セーブ
                PlayerPrefs.SetInt("SEVolume", value);
                PlayerPrefs.Save();

                text.text = $"環境音の音量（{value}）";
                audioMixer.SetFloat("SEParam", value);
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