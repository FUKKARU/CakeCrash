using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    [CreateAssetMenu(menuName = "ParamsSO/Sound", fileName = "SoundParamsSO")]
    public class SoundParamsSO : ScriptableObject
    {
        #region QOL向上処理
        // ParamsSOが保存してある場所のパス
        public const string PATH = "SoundParamsSO";

        // ParamsSOの実体
        private static SoundParamsSO _entity = null;
        public static SoundParamsSO Entity
        {
            get
            {
                // 初アクセス時にロードする
                if (_entity == null)
                {
                    _entity = Resources.Load<SoundParamsSO>(PATH);

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

        [Header("BGM")]
        [Header("タイトル")] public AudioClip TitleBGM;
        [Header("ゲーム中")] public AudioClip GameBGM;
        [Space(50)]
        [Header("SE")]
        [Header("フクロウが鳴く")] public AudioClip OwlHootSE;
        [Header("ケーキが落下する")] public AudioClip CakeFallSE;
        [Header("ハンマーを振る")] public AudioClip HammerSmashSE;
        [Header("ハンマーとケーキが衝突する")] public AudioClip HammerHitCakeSE;
        [Header("ケーキが壁に当たってつぶれ，クリームが壁につく")] public AudioClip CakeCrushSE;
        [Header("範囲外のケーキを叩こうとした")] public AudioClip CakeOutOfRangeSE;
        [Header("警備員が歩いてくる")] public AudioClip GuardManComeSE;
        [Header("扉が少し開く")] public AudioClip DoorHalfOpenSE;
        [Header("扉が大きく開く")] public AudioClip DoorOpenSE;
        [Header("扉が閉まる")] public AudioClip DoorCloseSE;
        [Header("警備員が立ち去る")] public AudioClip GuardManGoSE;
        [Header("しゃがむ")] public AudioClip SquatSE;
        [Header("しゃがんで息をひそめ，心拍音が耳に響く")] public AudioClip HideSE;
        [Header("立ち上がる")] public AudioClip StandUpSE;
        [Header("クリームが顔につく")] public AudioClip CreamHitFaceSE;
        [Header("スタミナが切れる")] public AudioClip StaminaRunOutSE;
        [Header("警備員に見つかる")] public AudioClip FoundByGuardManSE;
        [Header("クリア画面で星が付く")] public AudioClip StarSE;
        [Header("クリア画面でスコアのテキストが出る")] public AudioClip ScoreTextSE;
        [Space(50)]
        [Header("System")]
        [Header("ボタンのクリック")] public AudioClip ButtonClickSystem;
    }
}