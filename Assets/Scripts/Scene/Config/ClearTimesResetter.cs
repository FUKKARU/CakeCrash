using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    // ���̃N���X���C���X�^���X�����ꂽ���C�m���}�N���A�񐔂̃Z�[�u�f�[�^��S�č폜����B
    public class ClearTimesResetter : MonoBehaviour
    {
        void Start()
        {
            PlayerPrefs.DeleteKey("EasyClearTimes");
            PlayerPrefs.DeleteKey("NormalClearTimes");
            PlayerPrefs.DeleteKey("HardClearTimes");
            PlayerPrefs.DeleteKey("SuperHardClearTimes");
        }
    }
}