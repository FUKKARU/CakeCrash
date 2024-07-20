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

        [Header("テスト用：ガードマンが来なくなる")] public bool GuardManStop;
        [Header("視点の最大鉛直振れ幅")] public float DoubleA_YRange;
        [Header("視点のうごくスピード")] public float W_Speed;
        [Header("ハンマーを振るクールタイム")] public float HammerCooltime;
        [Header("ハンマーを生成する座標(=ハンマー本体の中心)")] public Vector3 HammerGeneratePosition;
        [Header("ハンマーを振るオイラー角z (開始，終了)")] public Vector2 HammerEulerZ;
        [Header("ハンマーを振る時間")] public float HammerDur;
        [Header("スタミナの最大量")] public int MaxStamina;
        [Header("スタミナの回復頻度")] public float StaminaRecoverSpan;
        [Header("ガードマンが来る間隔[秒]")] public float EventSpan;
        [Header("ガードマンがフェイントを使う確率(の分母)")] public int FeintProbability;
        [Header("ガードマンがドアを閉じて何秒後に、ケーキのミスカウントの処理を再開するか")] public float GuardManAfterClosedDur;
        [Header("ミスった際のコンボの減少量")] public int OnMissComboDel;
    }
}