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
        [Header("ロードが完了してから何秒後に\nトランジションの演出が入るか")] public float DurAfterLoadCompleted;
        [Header("ロード完了後のトランジションにおいて、\n何秒で画面が覆われる/明けるか")] public float LoadTransisionDur;
        [Header("覆われるトランジションの後、\nかつ明けるトランジションの前に、\n何秒のクールタイムを挟むか")] public float BetweenTransisionDur;
        [Header("ライトの角度 (開始，終了)")] public Vector2 LightTheta;
        [Header("画面がフェードアウトするスピード")] public float FadeOutSpeed;
        [Header("ケーキが範囲外のUIを消すまでの秒数")] public float CakeOutOfRangeUIHideDuration;
        [Header("ゲームオーバーの演出時にカメラが向くオイラー回転角x")] public float GameOverCameraRotationX;
        [Header("各種ボタンを押してからすぐにシーン遷移する場合，\r\nそれまでの秒数")] public float SceneChangeWaitTime;
    }
}