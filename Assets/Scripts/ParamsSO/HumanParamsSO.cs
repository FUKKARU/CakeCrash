using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    [CreateAssetMenu(menuName = "ParamsSO/Human", fileName = "HumanParamsSO")]
    public class HumanParamsSO : ScriptableObject
    {
        #region QOL向上処理
        // GuardManParamsSOが保存してある場所のパス
        public const string PATH = "HumanParamsSO";

        // GuardManParamsSOの実体
        private static HumanParamsSO _entity = null;
        public static HumanParamsSO Entity
        {
            get
            {
                // 初アクセス時にロードする
                if (_entity == null)
                {
                    _entity = Resources.Load<HumanParamsSO>(PATH);

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

        [Header("テスト用：ガードマンが来なくなる")] public bool GuardManStop = false;
        [Header("視点の最大鉛直振れ幅")] public float DoubleA_YRange = 21f;
        [Header("視点のうごくスピード")] public float W_Speed = 10f;
        [Header("ハンマーを振るクールタイム")] public float HammerCooltime = 0.1f;
        [Header("ハンマーを生成する座標(=ハンマー本体の中心)")] public Vector3 HammerGeneratePosition = new Vector3(0.5f, 30.35f, -25f);
        [Header("ハンマーを振るオイラー角z (開始，終了)")] public Vector2 HammerEulerZ = new Vector2(0f, 90f);
        [Header("ハンマーを振る時間")] public float HammerDur = 0.1f;
        [Header("スタミナの最大量")] public int MaxStamina = 1000;
        [Header("スタミナの回復頻度")] public float StaminaRecoverSpan = 1.5f;
        [Header("ガードマンが来る間隔[秒]")] public float EventSpan = 15f;
        [Header("ガードマンがフェイントを使う確率(の分母)")] public int FeintProbability = 10;
    }
}