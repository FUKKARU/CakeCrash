using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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
        [NonSerialized] public bool IsHammerCoolTime = false; // ハンマーを振るクールタイム中かどうか
        [NonSerialized] public bool IsHammerShakable = false; // ハンマーを振っているかどうか(1回しか使わない)
        [NonSerialized] public bool IsHammerGeneratable = false; // ハンマーを生成可能な状態であるかどうか
        [NonSerialized] public bool IsDoingPenalty = false; // ミスっているかどうか
        [NonSerialized] public bool IsTired = false; // 疲れているかどうか
        [NonSerialized] public bool IsHiding = false; // 隠れているかどうか
        [NonSerialized] public bool IsLooking = false; // 警備員がこちらを見ているかどうか
        [NonSerialized] public bool IsAllSmashed = false; // ケーキを全て壊したかどうか
        [NonSerialized] public bool IsClear = false; // クリアになったかどうか
        [NonSerialized] public bool IsGameOver = false; // ゲームオーバーになったかどうか
        [SerializeField] AudioSource audioSourceBGM;
        [SerializeField] AudioSource audioSourceSE;
        [SerializeField] IsMissingHandler isMissingHandler;
        public Image CakeOutOfRangeUI;
        public GameObject SquatAnnounceUI;
        public enum PUSHED_COLOR { NULL, RED, GREEN, BLUE }
        public PUSHED_COLOR PushedColor = PUSHED_COLOR.NULL;
        public List<GameObject> Hammers;
        float quitTime = 0; // タイトルに戻るボタンが押されている時間
        float hammerCooltime = 0f;

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

            if (!IsHammerCoolTime)
            {
                // 入力を受け取って、ハンマーを振る合図を送る。
                if (!IsTired && !IsHiding && !IsLooking && !IsDoingPenalty && !IsClear && !IsGameOver)
                {
                    if (IsRed >= 0.99f)
                    {
                        IsHammerCoolTime = true;
                        hammerCooltime = HumanParamsSO.Entity.HammerCooltime;

                        IsRed = 0;
                        PushedColor = PUSHED_COLOR.RED;
                        IsHammerGeneratable = true;
                    }
                    else if (IsGreen >= 0.99f)
                    {
                        IsHammerCoolTime = true;
                        hammerCooltime = HumanParamsSO.Entity.HammerCooltime;

                        IsGreen = 0;
                        PushedColor = PUSHED_COLOR.GREEN;
                        IsHammerGeneratable = true;
                    }
                    else if (IsBlue >= 0.99f)
                    {
                        IsHammerCoolTime = true;
                        hammerCooltime = HumanParamsSO.Entity.HammerCooltime;

                        IsBlue = 0;
                        PushedColor = PUSHED_COLOR.BLUE;
                        IsHammerGeneratable = true;
                    }
                }
            }
            else
            {
                hammerCooltime -= Time.deltaTime;
                if (hammerCooltime <= 0)
                {
                    hammerCooltime = 0;
                    IsHammerCoolTime = false;
                }
            }

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
        }

        public void DeleteAllHammers()
        {
            foreach (GameObject hammer in Hammers)
            {
                Destroy(hammer);
            }
        }
    }
}