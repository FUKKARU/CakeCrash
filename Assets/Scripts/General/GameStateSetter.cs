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
            SetResolutionAndFullScreenMode(); // 解像度とフルスクリーンにするかどうかを設定
            SetVsyncAndTargetFrameRate(); // Vsync（とターゲットフレームレート）の設定
        }

        #region 詳細
        static void SetResolutionAndFullScreenMode()
        {
            Screen.SetResolution(GameStateParamsSO.Entity.Resolution.x, GameStateParamsSO.Entity.Resolution.y, GameStateParamsSO.Entity.IsFullScreen);
        }

        static void SetVsyncAndTargetFrameRate()
        {
            if (GameStateParamsSO.Entity.IsVsyncOn)
            {
                QualitySettings.vSyncCount = 1; // VSyncをONにする
            }
            else
            {
                QualitySettings.vSyncCount = 0; // VSyncをOFFにする
                Application.targetFrameRate = GameStateParamsSO.Entity.TargetFrameRate; // ターゲットフレームレートの設定
            }
        }
        #endregion
    }
}