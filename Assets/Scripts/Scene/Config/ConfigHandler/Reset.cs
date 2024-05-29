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
            // ���̃��\�b�h�̏������I������܂ŁC���Z�b�g�{�^���ɂ���āu�l���ύX���ꂽ�v���Ƃ��C���m����Ȃ��悤�ɂ��邽�߂̃t���O
            IsConfigReseted = true;

            // �Z�[�u�f�[�^���폜
            PlayerPrefs.DeleteKey("Brightness");
            PlayerPrefs.DeleteKey("BGMVolume");
            PlayerPrefs.DeleteKey("SEVolume");
            PlayerPrefs.DeleteKey("SystemVolume");
            PlayerPrefs.DeleteKey("Difficulty");

            // �e��UI�̒l��ύX
            brightness.value = ConfigParamsSO.Entity.DefaultBrightness;
            BGMVolume.value = ConfigParamsSO.Entity.DefaultSoundVolume[0];
            SEVolume.value = ConfigParamsSO.Entity.DefaultSoundVolume[1];
            SystemVolume.value = ConfigParamsSO.Entity.DefaultSoundVolume[2];
            difficulty.value = ConfigParamsSO.Entity.DefaultDifficulty;

            // �q��UI�C�����̖��邳�C���ʂ̒l��ύX
            BGMVolumeText.text = $"BGM�̉��ʁi{ConfigParamsSO.Entity.DefaultSoundVolume[0]}�j";
            SEVolumeText.text = $"�����̉��ʁi{ConfigParamsSO.Entity.DefaultSoundVolume[1]}�j";
            SystemVolumeText.text = $"�V�X�e�����̉��ʁi{ConfigParamsSO.Entity.DefaultSoundVolume[2]}�j";
            RenderSettings.ambientIntensity = ConfigParamsSO.Entity.DefaultBrightnessVars[0];
            audioMixer.SetFloat("BGMParam", ConfigParamsSO.Entity.DefaultSoundVolume[0]);
            audioMixer.SetFloat("SEParam", ConfigParamsSO.Entity.DefaultSoundVolume[1]);
            audioMixer.SetFloat("SystemParam", ConfigParamsSO.Entity.DefaultSoundVolume[2]);

            IsConfigReseted = false;

            audioSource.PlayOneShot(SoundParamsSO.Entity.ButtonClickSystem);
        }
    }
}
