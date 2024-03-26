using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Main
{
    public class LeftMode : MonoBehaviour
    {
        [SerializeField] Reset resetScript;
        [SerializeField] Toggle toggle;
        bool isSetting = false; // Start�̏������I������܂ŁC�u�l���ύX���ꂽ�v���Ƃ��C���m����Ȃ��悤�ɂ��邽�߂̃t���O

        void Start()
        {
            isSetting = true;

            int dValue = ConfigParamsSO.Entity.DefaultLeftMode;
            // ���[�h
            toggle.isOn = (PlayerPrefs.GetInt("LeftMode", dValue) == 1) ? true : false;

            isSetting = false;
        }

        public void OnValueChanged(bool value)
        {
            if (resetScript.IsConfigReseted || isSetting)
            {
                return;
            }
            else
            {
                // �Z�[�u
                PlayerPrefs.SetInt("LeftMode", (value == true) ? 1 : 0);
                PlayerPrefs.Save();
            }
        }
    }
}
