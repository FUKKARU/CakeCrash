using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    [CreateAssetMenu(menuName = "ParamsSO/Other", fileName = "OtherParamsSO")]
    public class OtherParamsSO : ScriptableObject
    {
        #region QOL向上処理
        // ParamsSOが保存してある場所のパス
        public const string PATH = "OtherParamsSO";

        // ParamsSOの実体
        private static OtherParamsSO _entity = null;
        public static OtherParamsSO Entity
        {
            get
            {
                // 初アクセス時にロードする
                if (_entity == null)
                {
                    _entity = Resources.Load<OtherParamsSO>(PATH);

                    //ロード出来なかった場合はエラーログを表示
                    if (_entity == null)
                    {
                        Debug.LogError(PATH + " not found");
                    }
                }

                return _entity;
            }
        }
        #endregion

        [Header("ライトの角度 (開始，終了)")] public Vector2 LightTheta = new Vector2(-10f, -190f);
        [Header("画面がフェードアウトするスピード")] public float FadeOutSpeed = 100f;
        [Header("ケーキが範囲外のUIを消すまでの秒数")] public float CakeOutOfRangeUIHideDuration = 2f;
        [Header("ゲームオーバーの演出時にカメラが向くオイラー回転角x")] public float GameOverCameraRotationX = -30f;
        [Header("Quit(0)ボタンが長押し判定になる秒数")] public float QuitHoldPeriod = 2f;
        [Header("ノルマ達成回数削除\r\n(Ctrl + Shift + Alt + ↑ + ← + ↓ + →)\r\nボタンが長押し判定になる秒数")] public float ClearTimesResetHoldPeriod = 5f;
        [Header("各種ボタンを押してからすぐにシーン遷移する場合，\r\nそれまでの秒数")] public float SceneChangeWaitTime = 0.2f;
    }
}