using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    [CreateAssetMenu(menuName = "ParamsSO/Cake", fileName = "CakeParamsSO")]
    public class CakeParamsSO : ScriptableObject
    {
        #region QOL向上処理
        // CakeParamsSOが保存してある場所のパス
        public const string PATH = "CakeParamsSO";

        // CakeParamsSOの実体
        private static CakeParamsSO _entity = null;
        public static CakeParamsSO Entity
        {
            get
            {
                // 初アクセス時にロードする
                if (_entity == null)
                {
                    _entity = Resources.Load<CakeParamsSO>(PATH);

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

        [Header("ケーキを最初に生成するx座標のリスト(降順)")] public float[] CakeStartGenerateXList = { 40f, 0f, -40f };
        [Header("ケーキが画面から消えるx座標(正)")] public float CakeLimitX = 43f;
        [Header("ケーキ同士の間隔")] public float CakeOfst = 40f;
        [Header("ケーキを生成するy座標(昇順)")] public float[] CakeGenerateYList = { 34f, 39f, 44f };
        [Header("ケーキがレーンからはみ出る境界のz座標(正)\r\n(大，中，小)")] public float[] LaneLimitZList = { 23f, 20.6f, 18.2f };
        [Header("ケーキを消せるようになる境界のx座標(正)\r\n(大，中，小)")] public float[] DeletableXList = { 15.5f, 13.1f, 10.7f };
        [Header("ケーキが中央に「近い」位置にいると\r\n判定する境界のx座標の範囲を、↑の何倍にするか")] public float IsNearCenterXCoef = 1.15f;
        [Header("シーン内にケーキを何セットまで許容するか")] public int MaxCakeSet = 3;
        [Header("ケーキの流れるスピード")] public float CakeSpeed = 7f;
        [Header("ケーキを吹っ飛ばす方向\r\n(大，中，小)")] public Vector3[] SmashVector3 = { new Vector3(-0.75f, 0.25f, 1f), new Vector3(-0.75f, 0.25f, 1f), new Vector3(-0.75f, 0.25f, 1f) };
        [Header("ケーキを吹っ飛ばす力\r\n(大，中，小)")] public float[] SmashPower = { 300f, 300f, 300f };
    }
}