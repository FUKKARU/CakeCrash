using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        [NonSerialized] public bool IsLeftMode;
        [NonSerialized] public float TimePassed = 0f; // ゲーム開始時からの経過時間
        [NonSerialized] public int CurrentStamina;
        [NonSerialized] public bool IsHammerCoolTime = false; // ハンマーを振るクールタイム中かどうか
        [NonSerialized] public bool IsHammerShakable = false; // ハンマーを振っているかどうか(1回しか使わない)
        [NonSerialized] public bool IsHammerGeneratable = false; // ハンマーを生成可能な状態であるかどうか
        [NonSerialized] public bool IsDoingPenalty = false; // ミスっているかどうか
        [NonSerialized] public bool IsHiding = false; // 隠れているかどうか
        [NonSerialized] public bool IsLooking = false; // 警備員がこちらを見ているかどうか
        [NonSerialized] public bool IsGameOver = false; // ゲームオーバーになったかどうか（どこかでtrueにしてね）
        [NonSerialized] public GameObject missCream = null;
        [NonSerialized] public bool inputCont = true;
        [NonSerialized] public PUSHED_COLOR PushedColor = PUSHED_COLOR.NULL;
        [NonSerialized] public List<GameObject> Hammers = new();

        [SerializeField] AudioSource audioSourceBGM;
        [SerializeField] AudioSource audioSourceSE;
        [SerializeField] IsMissingHandler isMissingHandler;
        [SerializeField] AnimationCurve comboUISize;
        [SerializeField] TextMeshProUGUI scoreText;
        [SerializeField] TextMeshProUGUI deltaScoreText;

        public enum PUSHED_COLOR { NULL, RED, GREEN, BLUE }

        public GameObject SquatAnnounceUI;
        public TextMeshProUGUI ComboUI;

        int comboCounter = 0; public int ComboCounter
        {
            get
            {
                return comboCounter;
            }
            set
            {
                comboCounter = Mathf.Clamp(value, 0, int.MaxValue);
            }
        }
        int cakeCrashNum = 0; public int CakeCrashNum
        {
            get
            {
                return cakeCrashNum;
            }
            set
            {
                cakeCrashNum = Mathf.Clamp(value, 0, int.MaxValue);
            }
        }
        int score = 0; public int Score
        {
            get
            {
                return score;
            }
            set
            {
                score = Mathf.Clamp(value, 0, int.MaxValue);
            }
        }
        int preScore = 0;
        bool onSquatCombo = false;
        float quitTime = 0; // タイトルに戻るボタンが押されている時間
        float hammerCooltime = 0f;

        //ゲームオーバ用
        int happiness_familyCount;
        public void HappinessIncrement(int amount)
        {
            happiness_familyCount += amount;
            happinessFamilyText.text = happiness_familyCount.ToString();
        }
        [SerializeField] TextMeshProUGUI happinessFamilyText;
        [SerializeField] TextMeshProUGUI resultScoreText;
        public bool isGameOver { get; private set; }
        public bool GuardStop; 
        IEnumerator ResultShow()
        {
            GuardStop = true;
            inputCont = false;

            yield return new WaitForSeconds(1);
            resultScoreText.text = "Score : " + Score.ToString();
        }
        void Start()
        {
            StunEFF.Pause();

            SquatAnnounceUI.SetActive(false);

            deltaScoreText.enabled = false;
            preScore = Score;
            scoreText.text = Score.ToString("D8");
            deltaScoreText.text = "+ " + (Score - preScore).ToString();

            audioSourceBGM.clip = SoundParamsSO.Entity.GameBGM;
            audioSourceBGM.Play();
        }

        void Update()
        {
            TimePassed += Time.deltaTime; // 経過時間をカウント

            #region 入力を受け取って、ハンマーを振る合図を送る
            if (!IsHammerCoolTime)
            {
                if (!IsHiding && !IsLooking && !IsDoingPenalty && !IsGameOver)
                {
                    if (IA.InputGetter.Instance.IsRed)
                    {
                        IsHammerCoolTime = true;
                        hammerCooltime = HumanParamsSO.Entity.HammerCooltime;

                        PushedColor = PUSHED_COLOR.RED;
                        IsHammerGeneratable = true;
                    }
                    else if (IA.InputGetter.Instance.IsGreen)
                    {
                        IsHammerCoolTime = true;
                        hammerCooltime = HumanParamsSO.Entity.HammerCooltime;

                        PushedColor = PUSHED_COLOR.GREEN;
                        IsHammerGeneratable = true;
                    }
                    else if (IA.InputGetter.Instance.IsBlue)
                    {
                        IsHammerCoolTime = true;
                        hammerCooltime = HumanParamsSO.Entity.HammerCooltime;

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
            #endregion

            #region ゲームオーバーを判定
            if (IsLooking && !IsHiding && !stun)
            {
                StartCoroutine(Stun());
                stun = true;
            }
            #endregion

            #region タイトルに戻る判定
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
            #endregion

            if (IsGameOver) ComboUI.text = "";

            SquatComboStarter();


            if( happiness_familyCount > CakeParamsSO.Entity.GameOverCakeNum && !isGameOver)
            {
                isGameOver = true;
                HumanParamsSO.Entity.GuardManStop = true; 
                StartCoroutine(ResultShow());
            }
        }

        public void DeleteAllHammers()
        {
            foreach (GameObject hammer in Hammers)
            {
                Destroy(hammer);
            }
        }

        Coroutine delTextFade = null;
        public void ShowScore()
        {
            while (CakeCrashNum >= CakeParamsSO.Entity.ToScoreDur)
            {
                CakeCrashNum -= CakeParamsSO.Entity.ToScoreDur;
            }

            scoreText.text = Score.ToString("D8");
            deltaScoreText.text = "+ " + (Score - preScore).ToString();
            preScore = Score;
            CakeParamsSO.Entity.CakeSpeed += CakeParamsSO.Entity.SpeedIncrementValue;
            if (delTextFade != null) StopCoroutine(delTextFade);
            deltaScoreText.enabled = true;
            delTextFade = StartCoroutine(DelTextFade());
        }
        IEnumerator DelTextFade()
        {
            yield return new WaitForSeconds(CakeParamsSO.Entity.DelTextFadeDur);
            deltaScoreText.enabled = false;
        }

        #region Combo
        IEnumerator ComboAnim(bool repeat)
        {
            //コンボ継続の時のテキストアニメーション
            float t = 0;
            float textSize = 1;
            float animTime = 0.3f;
            bool changeText = false;
            while (t < animTime)
            {
                textSize = comboUISize.Evaluate(t);
                ComboUI.rectTransform.localScale = new Vector2(textSize, textSize);
                if (t > animTime / 2.0f && !changeText)
                {
                    ComboUI.text = "Combo " + ComboCounter;
                    changeText = true;
                }
                t += Time.deltaTime;
                yield return null;
            }
            if (repeat && onSquatCombo)
            {
                yield return new WaitForSeconds(0.3f);
                ComboContinuation(true);
            }
        }

        public void ComboContinuation(bool repeat = false)
        {
            //コンボ継続の時 DeleteCake.csもしくはSquatMove.cs呼ぶ
            if (ComboCounter == 0) ComboUI.text = "Combo " + ComboCounter;
            ComboCounter++;
            StartCoroutine(ComboAnim(repeat));
        }

        public void ComboEnd(bool keibiin = false)
        {
            //コンボ終了の時 DeleteCake.csが呼ぶ
            if (keibiin) ComboCounter = 0;
            ComboCounter -= HumanParamsSO.Entity.OnMissComboDel;
            ComboUI.text = comboCounter == 0 ? "" : "Combo " + ComboCounter;
        }


        void SquatComboStarter()
        {
            if (IsLooking && IsHiding)
            {
                if (!onSquatCombo)
                {
                    onSquatCombo = true;
                    ComboContinuation(true);
                }

            }
            else
            {
                onSquatCombo = false;
            }
        }
        #endregion

        #region Stunned
        bool stun;
        [SerializeField] ParticleSystem StunEFF;
        [SerializeField] AnimationCurve StunCurve;
        float r = 0.5f;
        float p = 0.1f;
        IEnumerator Stun(float endT = 3.0f)
        {
            ComboEnd(true);
            StunEFF.Play();
            Camera camera = Camera.main;
            Vector3 startPos = camera.transform.position;
            Quaternion startRot = camera.transform.rotation;
            inputCont = false;
            float t = 0;
            while (t < endT)
            {
                t += Time.deltaTime;
                float strength = StunCurve.Evaluate(t / endT);
                camera.transform.position = startPos + UnityEngine.Random.insideUnitSphere * strength * p;
                camera.transform.rotation = startRot * Quaternion.Euler(UnityEngine.Random.Range(-r, r), UnityEngine.Random.Range(-r, r), UnityEngine.Random.Range(-r, r));
                yield return null;
            }
            StunEFF.Pause();
            camera.transform.position = startPos;
            camera.transform.rotation = startRot;
            inputCont = true;
            stun = false;
        }

        #endregion
    }
}