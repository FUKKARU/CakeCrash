using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    [CreateAssetMenu(menuName = "ParamsSO/GameState", fileName = "GameStateParamsSO")]
    public class GameStateParamsSO : ScriptableObject
    {
        #region QOL向上処理
        // ParamsSOが保存してある場所のパス
        public const string PATH = "GameStateParamsSO";

        // ParamsSOの実体
        private static GameStateParamsSO _entity = null;
        public static GameStateParamsSO Entity
        {
            get
            {
                // 初アクセス時にロードする
                if (_entity == null)
                {
                    _entity = Resources.Load<GameStateParamsSO>(PATH);

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

        [Header("解像度(ex. 1920*1080)")] public Vector2Int Resolution;
        [Header("フルスクリーンにする")] public bool IsFullScreen;
        [Header("Vsyncをオンにする")] public bool IsVsyncOn;
        [Header("(Vsyncがオフの時のみ)ターゲットフレームレート")] public int TargetFrameRate;
    }
}