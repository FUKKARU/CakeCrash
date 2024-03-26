using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    [CreateAssetMenu(menuName = "ParamsSO/Config", fileName = "ConfigParamsSO")]
    public class ConfigParamsSO : ScriptableObject
    {
        #region QOL向上処理
        // GuardManParamsSOが保存してある場所のパス
        public const string PATH = "ConfigParamsSO";

        // GuardManParamsSOの実体
        private static ConfigParamsSO _entity = null;
        public static ConfigParamsSO Entity
        {
            get
            {
                // 初アクセス時にロードする
                if (_entity == null)
                {
                    _entity = Resources.Load<ConfigParamsSO>(PATH);

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

        [Header("初期設定")]
        [Header("左手モード (0:オフ 1:オン)")] public int DefaultLeftMode = 0;
        [Header("部屋の明るさ (-10:0.5倍 0:1倍 10:2倍)\r\n(明るさは2^(n/10)倍になる)"), Range(-10, 10)] public int DefaultBrightness = 0;
        [Header("音量(dB) (BGM,SE,System)"), Range(-10, 10)] public int[] DefaultSoundVolume = { 0, 0, 0 };
        [Header("難易度 (0:簡単 1:普通 2:難しい 3:とても難しい)")] public int DefaultDifficulty = 1;
        [Space(50)]
        [Header("部屋の明るさの計算式 (I = d(=var0) * var1^(n/var2))")] public float[] DefaultBrightnessVars = { 0.5f, 2f, 10f };
        [Space(50)]
        [Header("難易度設定(E,N,H,S)")]
        [Header("クリアまでの秒数")] public float[] ClearTime = { 60f, 90f, 180f, 120f };
        [Header("吹っ飛ばすケーキの上限個数")] public int[] CakeMaxNum = { 15, 30, 70, 55 };
        [Header("スタミナの1回の減少量")] public int[] StaminaDecreaseAmount = { 0, 0, 50, 50 };
        [Header("スタミナの1回の回復量")] public int[] StaminaIncreaseAmount = { 0, 0, 50, 25 };
        [Header("間違った時の，スタミナの減少量")] public int[] OnMissedStaminaDecreaseAmount = { 0, 0, 0, 100 };
    }
}