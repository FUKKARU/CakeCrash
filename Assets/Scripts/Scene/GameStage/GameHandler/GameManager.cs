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
        #region static���V���O���g���ɂ���
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

        [NonSerialized] public float TimePassed = 0f; // �Q�[���J�n������̌o�ߎ���
        [NonSerialized] public int CurrentStamina;
        [NonSerialized] public float CakeSpeed; // �P�[�L�̃X�s�[�h
        [NonSerialized] public bool IsPause = true; // �Q�[�����ꎞ��~�����ǂ���(�ꎞ��~���ɓ��͂���Ȃ��悤�ɂ��Ă���)
        [NonSerialized] public bool IsHammerCoolTime = false; // �n���}�[��U��N�[���^�C�������ǂ���
        [NonSerialized] public bool IsHammerShakable = false; // �n���}�[��U���Ă��邩�ǂ���(1�񂵂��g��Ȃ�)
        [NonSerialized] public bool IsHammerGeneratable = false; // �n���}�[�𐶐��\�ȏ�Ԃł��邩�ǂ���
        [NonSerialized] public bool IsDoingPenalty = false; // �~�X���Ă��邩�ǂ���
        [NonSerialized] public bool IsHiding = false; // �B��Ă��邩�ǂ���
        [NonSerialized] public bool IsLooking = false; // �x����������������Ă��邩�ǂ���
        [NonSerialized] public bool IsOpening = false; // �x�������h�A���J���Ă��邩
        [NonSerialized] public bool IsStun = false; // �X�^�����ł��邩�ǂ���
        [NonSerialized] public bool IsGameOver = false; // �Q�[���I�[�o�[�ɂȂ������ǂ���(�ǂ�����true�ɂ��Ă�)
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

        //�Q�[���I�[�o�p
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
            TimePassed += Time.deltaTime; // �o�ߎ��Ԃ��J�E���g

            #region ���͂��󂯎���āA�n���}�[��U�鍇�}�𑗂�
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

            // �x�����Ɍ���������X�^��
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
            //�R���{�p���̎��̃e�L�X�g�A�j���[�V����
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
            //�R���{�p���̎� DeleteCake.cs��������SquatMove.cs�Ă�
            if (ComboCounter == 0) ComboUI.text = "Combo " + ComboCounter;
            ComboCounter++;
            StartCoroutine(ComboAnim(repeat));
        }

        public void ComboEnd(bool keibiin = false)
        {
            //�R���{�I���̎� DeleteCake.cs���Ă�
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