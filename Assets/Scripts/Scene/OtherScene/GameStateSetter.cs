using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class GameStateSetter
    {
        [RuntimeInitializeOnLoadMethod]
        static void RuntimeInitializeOnLoadMethods()
        {
            SetResolutionAndFullScreenMode(); // �𑜓x�ƃt���X�N���[���ɂ��邩�ǂ�����ݒ�
            SetVsyncAndTargetFrameRate(); // Vsync�i�ƃ^�[�Q�b�g�t���[�����[�g�j�̐ݒ�
        }

        #region �ڍ�
        static void SetResolutionAndFullScreenMode()
        {
            Screen.SetResolution(GameStateParamsSO.Entity.Resolution.x, GameStateParamsSO.Entity.Resolution.y, GameStateParamsSO.Entity.IsFullScreen);
        }

        static void SetVsyncAndTargetFrameRate()
        {
            if (GameStateParamsSO.Entity.IsVsyncOn)
            {
                QualitySettings.vSyncCount = 1; // VSync��ON�ɂ���
            }
            else
            {
                QualitySettings.vSyncCount = 0; // VSync��OFF�ɂ���
                Application.targetFrameRate = GameStateParamsSO.Entity.TargetFrameRate; // �^�[�Q�b�g�t���[�����[�g�̐ݒ�
            }
        }
        #endregion
    }
}