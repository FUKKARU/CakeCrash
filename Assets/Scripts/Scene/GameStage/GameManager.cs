using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Main
{
    public class GameManager : MonoBehaviour
    {
        #region staticかつシングルトンにする
        public static GameManager Instance { get; set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        #endregion

        #region 設定(初期値はNormal)
        [NonSerialized] public bool IsLeftMode;
        [NonSerialized] public float ClearTime;
        [NonSerialized] public int CakeMaxNum;
        [NonSerialized] public int StaminaDecreaseAmount;
        [NonSerialized] public int StaminaIncreaseAmount;
        [NonSerialized] public int OnMissedStaminaDecreaseAmount;
        #endregion
        [NonSerialized] public float TimePassed = 0f; // ゲーム開始時からの経過時間
        [NonSerialized] public int Score = 0; // 残りのケーキの数
        #region 入力(float)
        [NonSerialized] public float IsRed = 0; // 赤に対応するボタンの入力
        [NonSerialized] public float IsGreen = 0; // 緑に対応するボタンの入力
        [NonSerialized] public float IsBlue = 0; // 青に対応するボタンの入力
        [NonSerialized] public float IsSquat = 0; // しゃがみに対応するボタンの入力
        #endregion
        [NonSerialized] public int CurrentStamina;
        [NonSerialized] public bool IsHammerShakable = false; // ハンマーを振っているかどうか(1回しか使わない)
        [NonSerialized] public bool IsHammerGeneratable = false; // ハンマーを生成可能な状態であるかどうか
        [NonSerialized] public bool IsTired = false; // 疲れているかどうか
        [NonSerialized] public bool IsHiding = false; // 隠れているかどうか
        [NonSerialized] public bool IsLooking = false; // 警備員がこちらを見ているかどうか
        [NonSerialized] public bool IsDoingPenalty = false; // ペナルティを実行中かどうか
        [NonSerialized] public bool IsAllSmashed = false; // ケーキを全て壊したかどうか
        [NonSerialized] public bool IsClear = false; // クリアになったかどうか
        [NonSerialized] public bool IsGameOver = false; // ゲームオーバーになったかどうか
        [Header("大,中,小")] public GameObject[] HitTutorial;
        [SerializeField] AudioSource audioSourceBGM;
        [SerializeField] AudioSource audioSourceSE;
        public Image CakeOutOfRangeUI;
        public GameObject SquatAnnounceUI;
        bool isCakeOutOfRangeUIShowing = false; // ケーキが範囲外のUIを、表示中かどうか
        float quitTime = 0; // タイトルに戻るボタンが押されている時間

        void Start()
        {
            // スコアを初期化
            Score = CakeMaxNum;

            CakeOutOfRangeUI.enabled = false;
            SquatAnnounceUI.SetActive(false);

            audioSourceBGM.clip = SoundParamsSO.Entity.GameBGM;
            audioSourceBGM.Play();
        }

        void Update()
        {
            TimePassed += Time.deltaTime; // 経過時間をカウント

            // ケーキを全て壊しきったらクリア
            if (Score <= 0)
            {
                IsAllSmashed = true;
                IsClear = true;
            }

            // 朝が来たらクリア
            if (TimePassed >= ClearTime && !IsGameOver)
            {
                IsClear = true;
            }

            // ゲームオーバーを判定
            if (IsLooking && !IsHiding && !IsClear)
            {
                IsGameOver = true;
            }

            // タイトルに戻る判定
            if (Input.GetKey(KeyCode.Alpha0))
            {
                quitTime += Time.deltaTime;
                if (quitTime >= OtherParamsSO.Entity.QuitHoldPeriod)
                {
                    SceneManager.LoadScene("Title");
                }
            }
            else
            {
                quitTime = 0;
            }

            // アクティブにし続けない限り、矢印は消える。
            HitTutorial[0].SetActive(false);
            HitTutorial[1].SetActive(false);
            HitTutorial[2].SetActive(false);
        }

        public void ShowCakeOutOfRangeUI()
        {
            if (!isCakeOutOfRangeUIShowing)
            {
                isCakeOutOfRangeUIShowing = true;

                CakeOutOfRangeUI.enabled = true;
                StartCoroutine(ShowCakeOutOfRangeUIBehaviour());
                audioSourceSE.PlayOneShot(SoundParamsSO.Entity.CakeOutOfRangeSE);
            }
        }
        public IEnumerator ShowCakeOutOfRangeUIBehaviour()
        {
            yield return new WaitForSeconds(OtherParamsSO.Entity.CakeOutOfRangeUIHideDuration);
            CakeOutOfRangeUI.enabled = false;

            isCakeOutOfRangeUIShowing = false;
        }
    }
}