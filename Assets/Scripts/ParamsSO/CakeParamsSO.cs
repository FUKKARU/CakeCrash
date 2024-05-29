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

        [Header("ケーキを最初に生成するx座標のリスト(降順)")] public float[] CakeStartGenerateXList;
        [Header("ケーキが画面から消えるx座標(正)")] public float CakeLimitX;
        [Header("ケーキ同士の間隔")] public float CakeOfst;
        [Header("ケーキを生成するy座標(昇順)")] public float[] CakeGenerateYList;
        [Header("ケーキがレーンからはみ出る境界のz座標(正)\r\n(大，中，小)")] public float[] LaneLimitZList;
        [Header("ケーキを消せるようになる境界のx座標(正)\r\n(大，中，小)")] public float[] DeletableXList;
        [Header("ケーキが中央に「近い」位置にいると\r\n判定する境界のx座標の範囲を、↑の何倍にするか")] public float IsNearCenterXCoef;
        [Header("シーン内にケーキを何セットまで許容するか")] public int MaxCakeSet;
        [Header("ケーキの流れるスピード")] public float CakeSpeed;
        [Header("ケーキを吹っ飛ばす方向\r\n(大，中，小)")] public Vector3[] SmashVector3;
        [Header("ケーキを吹っ飛ばす力\r\n(大，中，小)")] public float[] SmashPower;
        [Header("ケーキを何個壊すごとに、スコアに換算する演出を入れるか。")] public int ToScoreDur;
        [Header("スコアの差分テキストを消すまでの秒数")] public float DelTextFadeDur;
        [Header("ゲームオーバになるケーキの数")] public int GameOverCakeNum;
        [Header("一回当たりの加速の上昇値")] public int SpeedIncrementValue;
    }
}