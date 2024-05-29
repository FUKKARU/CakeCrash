using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Main
{
    public class Reset : MonoBehaviour
    {
        [SerializeField] Slider brightness;
        [SerializeField] Slider BGMVolume;
        [SerializeField] Slider SEVolume;
        [SerializeField] Slider SystemVolume;
        [SerializeField] TMP_Dropdown difficulty;

        [SerializeField] TextMeshProUGUI BGMVolumeText;
        [SerializeField] TextMeshProUGUI SEVolumeText;
        [SerializeField] TextMeshProUGUI SystemVolumeText;
        [SerializeField] AudioMixer audioMixer;

        [SerializeField] AudioSource audioSource;

        public bool IsConfigReseted { get; set; } = false;

        public void OnClick()
        {
            // このメソッドの処理が終了するまで，リセットボタンによって「値が変更された」ことを，検知されないようにするためのフラグ
            IsConfigReseted = true;

            // セーブデータを削除
            PlayerPrefs.DeleteKey("Brightness");
            PlayerPrefs.DeleteKey("BGMVolume");
            PlayerPrefs.DeleteKey("SEVolume");
            PlayerPrefs.DeleteKey("SystemVolume");
            PlayerPrefs.DeleteKey("Difficulty");

            // 親のUIの値を変更
            brightness.value = ConfigParamsSO.Entity.DefaultBrightness;
            BGMVolume.value = ConfigParamsSO.Entity.DefaultSoundVolume[0];
            SEVolume.value = ConfigParamsSO.Entity.DefaultSoundVolume[1];
            SystemVolume.value = ConfigParamsSO.Entity.DefaultSoundVolume[2];
            difficulty.value = ConfigParamsSO.Entity.DefaultDifficulty;

            // 子のUI，部屋の明るさ，音量の値を変更
            BGMVolumeText.text = $"BGMの音量（{ConfigParamsSO.Entity.DefaultSoundVolume[0]}）";
            SEVolumeText.text = $"環境音の音量（{ConfigParamsSO.Entity.DefaultSoundVolume[1]}）";
            SystemVolumeText.text = $"システム音の音量（{ConfigParamsSO.Entity.DefaultSoundVolume[2]}）";
            RenderSettings.ambientIntensity = ConfigParamsSO.Entity.DefaultBrightnessVars[0];
            audioMixer.SetFloat("BGMParam", ConfigParamsSO.Entity.DefaultSoundVolume[0]);
            audioMixer.SetFloat("SEParam", ConfigParamsSO.Entity.DefaultSoundVolume[1]);
            audioMixer.SetFloat("SystemParam", ConfigParamsSO.Entity.DefaultSoundVolume[2]);

            IsConfigReseted = false;

            audioSource.PlayOneShot(SoundParamsSO.Entity.ButtonClickSystem);
        }
    }
}
