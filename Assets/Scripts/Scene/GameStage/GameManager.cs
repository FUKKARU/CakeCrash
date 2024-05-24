using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

        #region �ݒ�(�����l��Normal)
        [NonSerialized] public bool IsLeftMode;
        [NonSerialized] public float ClearTime;
        [NonSerialized] public int CakeMaxNum;
        [NonSerialized] public int StaminaDecreaseAmount;
        [NonSerialized] public int StaminaIncreaseAmount;
        [NonSerialized] public int OnMissedStaminaDecreaseAmount;
        #endregion
        [NonSerialized] public float TimePassed = 0f; // �Q�[���J�n������̌o�ߎ���
        [NonSerialized] public int LeftNum = 0; // �c��̃P�[�L�̐�
        #region ����(float)
        [NonSerialized] public float IsRed = 0; // �ԂɑΉ�����{�^���̓���
        [NonSerialized] public float IsGreen = 0; // �΂ɑΉ�����{�^���̓���
        [NonSerialized] public float IsBlue = 0; // �ɑΉ�����{�^���̓���
        [NonSerialized] public float IsSquat = 0; // ���Ⴊ�݂ɑΉ�����{�^���̓���
        #endregion
        [NonSerialized] public int CurrentStamina;
        [NonSerialized] public bool IsHammerCoolTime = false; // �n���}�[��U��N�[���^�C�������ǂ���
        [NonSerialized] public bool IsHammerShakable = false; // �n���}�[��U���Ă��邩�ǂ���(1�񂵂��g��Ȃ�)
        [NonSerialized] public bool IsHammerGeneratable = false; // �n���}�[�𐶐��\�ȏ�Ԃł��邩�ǂ���
        [NonSerialized] public bool IsDoingPenalty = false; // �~�X���Ă��邩�ǂ���
        [NonSerialized] public bool IsTired = false; // ���Ă��邩�ǂ���
        [NonSerialized] public bool IsHiding = false; // �B��Ă��邩�ǂ���
        [NonSerialized] public bool IsLooking = false; // �x����������������Ă��邩�ǂ���
        [NonSerialized] public bool IsAllSmashed = false; // �P�[�L��S�ĉ󂵂����ǂ���
        [NonSerialized] public bool IsClear = false; // �N���A�ɂȂ������ǂ���
        [NonSerialized] public bool IsGameOver = false; // �Q�[���I�[�o�[�ɂȂ������ǂ���
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

        public Image CakeOutOfRangeUI;
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
        float quitTime = 0; // �^�C�g���ɖ߂�{�^����������Ă��鎞��
        float hammerCooltime = 0f;

        void Start()
        {
            StunEFF.Pause();
            // �X�R�A��������
            LeftNum = CakeMaxNum;

            CakeOutOfRangeUI.enabled = false;
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
            TimePassed += Time.deltaTime; // �o�ߎ��Ԃ��J�E���g

            #region ���͂��󂯎���āA�n���}�[��U�鍇�}�𑗂�
            if (!IsHammerCoolTime)
            {
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
            #endregion

            #region �P�[�L��S�ĉ󂵂�������N���A
            if (LeftNum <= 0)
            {
                IsAllSmashed = true;
                IsClear = true;
            }
            #endregion

            #region ����������N���A
            if (TimePassed >= ClearTime && !IsGameOver)
            {
                IsClear = true;
            }
            #endregion

            #region �Q�[���I�[�o�[�𔻒�
            if (IsLooking && !IsHiding && !IsClear && !stun)
            {
                //IsGameOver = true;
                StartCoroutine(Stun());
                stun = true;
            }
            #endregion

            #region �^�C�g���ɖ߂锻��
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