using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    [CreateAssetMenu(menuName = "ParamsSO/Cream", fileName = "CreamParamsSO")]
    public class CreamParamsSO : ScriptableObject
    {
        #region QOL向上処理
        // ParamsSOが保存してある場所のパス
        public const string PATH = "CreamParamsSO";

        // ParamsSOの実体
        private static CreamParamsSO _entity = null;
        public static CreamParamsSO Entity
        {
            get
            {
                // 初アクセス時にロードする
                if (_entity == null)
                {
                    _entity = Resources.Load<CreamParamsSO>(PATH);

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

        [Header("クリームを生成するz座標(めり込み防止のため)")] public float CreamGenerateZ = 115f;
        [Header("クリームを生成した座標からカメラに近づける距離")] public float CreamGenerateOffset = 5f;
        [Header("ミスした時の，クリームを生成する座標")] public Vector3 MissCreamGeneratePos = new Vector3(0.5f, 35f, -27f);
        [Header("クリームが消えるまでの時間(秒)")] public float CreamFadePeriod = 0.5f;
        [Header("ミスした時の，クリームが消えるまでの時間(秒)")] public float MissCreamFadePeriod = 1.5f;
    }
}