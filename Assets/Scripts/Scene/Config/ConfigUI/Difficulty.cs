using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Main
{
    public class Difficulty : MonoBehaviour
    {
        [SerializeField] Reset resetScript;
        [SerializeField] TMP_Dropdown dropDown;
        bool isSetting = false; // Start�̏������I������܂ŁC�u�l���ύX���ꂽ�v���Ƃ��C���m����Ȃ��悤�ɂ��邽�߂̃t���O

        void Start()
        {
            isSetting = true;

            int dValue = ConfigParamsSO.Entity.DefaultDifficulty;
            // ���[�h
            dropDown.value = PlayerPrefs.GetInt("Difficulty", dValue);

            isSetting = false;
        }

        public void OnValueChanged(int value)
        {
            if (!resetScript.IsConfigReseted && !isSetting)
            {
                // �Z�[�u
                PlayerPrefs.SetInt("Difficulty", value);
                PlayerPrefs.Save();
            }
        }
    }
}
