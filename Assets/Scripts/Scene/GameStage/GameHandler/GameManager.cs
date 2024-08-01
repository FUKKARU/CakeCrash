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

        [NonSerialized] public float TimePassed = 0f; // ゲーム開始時からの経過時間
        [NonSerialized] public int CurrentStamina;
        [NonSerialized] public float CakeSpeed; // ケーキのスピード
        [NonSerialized] public bool IsPause = true; // ゲームが一時停止中かどうか(一時停止中に入力されないようにしている)
        [NonSerialized] public bool IsHammerCoolTime = false; // ハンマーを振るクールタイム中かどうか
        [NonSerialized] public bool IsHammerShakable = false; // ハンマーを振っているかどうか(1回しか使わない)
        [NonSerialized] public bool IsHammerGeneratable = false; // ハンマーを生成可能な状態であるかどうか
        [NonSerialized] public bool IsDoingPenalty = false; // ミスっているかどうか
        [NonSerialized] public bool IsHiding = false; // 隠れているかどうか
        [NonSerialized] public bool IsLooking = false; // 警備員がこちらを見ているかどうか
        [NonSerialized] public bool IsOpening = false; // 警備員がドアを開いているか
        [NonSerialized] public bool IsStun = false; // スタン中であるかどうか
        [NonSerialized] public bool IsGameOver = false; // ゲームオーバーになったかどうか(どこかでtrueにしてね)
        [NonSerialized] public GameObject missCream = null;
        [NonSerialized] public bool inputCont = true;
        [NonSerialized] public PUSHED_COLOR PushedColor = PUSHED_COLOR.NULL;
        [NonSerialized] public List<GameObject> Hammers = new();

        [SerializeField] GameObject directionalLight;
        [SerializeField] AudioSource audioSourceBGM;
        [SerializeField] AudioSource audioSourceSE;
        [SerializeField] IsMissingHandler isMissingHandler;
        [SerializeField] AnimationCurve comboUISize;
        [SerializeField] TextMeshProUGUI scoreText;
        [SerializeField] TextMeshProUGUI deltaScoreText;

        public enum PUSHED_COLOR { NULL, RED, GREEN, BLUE }

        public GameObject SquatAnnounceUI;
        [SerializeField] private TextMeshProUGUI ComboUI;

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
        float hammerCooltime = 0f;

        //ゲームオーバ用
        int happiness_familyCount;
        [NonSerialized] public bool GuardManStop;

        void Start()
        {
            bool GuardManStop = HumanParamsSO.Entity.GuardManStop;

            StunEFF.Pause();

            SquatAnnounceUI.SetActive(false);

            CakeSpeed = CakeParamsSO.Entity.CakeSpeed;

            deltaScoreText.enabled = false;
            preScore = Score;
            scoreText.text = Score.ToString("D8");
            deltaScoreText.text = "+ " + (Score - preScore).ToString();

            audioSourceBGM.clip = SoundParamsSO.Entity.GameBGM;
        }

        void Update()
        {
            TimePassed += Time.deltaTime; // 経過時間をカウント

            #region 入力を受け取って、ハンマーを振る合図を送る
            if (!IsHammerCoolTime)
            {
                if (!IsHiding && !IsLooking && !IsDoingPenalty && !IsGameOver)
                {
                    if (IA.InputGetter.Instance.IsRed && !IsPause)
                    {
                        IsHammerCoolTime = true;
                        hammerCooltime = HumanParamsSO.Entity.HammerCooltime;

                        PushedColor = PUSHED_COLOR.RED;
                        IsHammerGeneratable = true;
                    }
                    else if (IA.InputGetter.Instance.IsGreen && !IsPause)
                    {
                        IsHammerCoolTime = true;
                        hammerCooltime = HumanParamsSO.Entity.HammerCooltime;

                        PushedColor = PUSHED_COLOR.GREEN;
                        IsHammerGeneratable = true;
                    }
                    else if (IA.InputGetter.Instance.IsBlue && !IsPause)
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

            // 警備員に見つかったらスタン
            if (IsLooking && !IsHiding && !IsStun)
            {
                StartCoroutine(Stun());
                IsStun = true;
            }

            SquatComboStarter();

            if (IA.InputGetter.Instance.Debug_IsToTitle)
            {
                Time.timeScale = 1;
                SceneManager.LoadScene("Title");
            }

            if (IsGameOver) ComboUI.text = "";

            if (happiness_familyCount > CakeParamsSO.Entity.GameOverCakeNum && !isGameOver)
            {
                isGameOver = true;
                GuardManStop = true;
                directionalLight.transform.rotation = Quaternion.Euler(-190, -90, 0);
                StartCoroutine(ResultShow());
            }
        }

        public void PlayBGM()
        {
            audioSourceBGM.Play();
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
            scoreText.text = Score.ToString("D8");
            deltaScoreText.text = "+ " + (Score - preScore).ToString();
            preScore = Score;
            if (ComboCounter >= 2) CakeSpeed += CakeParamsSO.Entity.SpeedIncrementValue;
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
            IsStun = false;
        }

        #endregion

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
    }
}