using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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
        [NonSerialized] public int Score = 0; // �c��̃P�[�L�̐�
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
        [SerializeField] AudioSource audioSourceBGM;
        [SerializeField] AudioSource audioSourceSE;
        [SerializeField] IsMissingHandler isMissingHandler;
        public Image CakeOutOfRangeUI;
        public GameObject SquatAnnounceUI;
        public TextMeshProUGUI ComboUI;
        uint comboCounter;
        bool onSquatCombo = false;
        [SerializeField] AnimationCurve comboUISize;
        public enum PUSHED_COLOR { NULL, RED, GREEN, BLUE }
        public PUSHED_COLOR PushedColor = PUSHED_COLOR.NULL;
        public List<GameObject> Hammers;
        float quitTime = 0; // �^�C�g���ɖ߂�{�^����������Ă��鎞��
        float hammerCooltime = 0f;

        void Start()
        {
            // �X�R�A��������
            Score = CakeMaxNum;

            CakeOutOfRangeUI.enabled = false;
            SquatAnnounceUI.SetActive(false);

            audioSourceBGM.clip = SoundParamsSO.Entity.GameBGM;
            audioSourceBGM.Play();
        }

        void Update()
        {
            TimePassed += Time.deltaTime; // �o�ߎ��Ԃ��J�E���g

            if (!IsHammerCoolTime)
            {
                // ���͂��󂯎���āA�n���}�[��U�鍇�}�𑗂�B
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

            // �P�[�L��S�ĉ󂵂�������N���A
            if (Score <= 0)
            {
                IsAllSmashed = true;
                IsClear = true;
            }

            // ����������N���A
            if (TimePassed >= ClearTime && !IsGameOver)
            {
                IsClear = true;
            }

            // �Q�[���I�[�o�[�𔻒�
            if (IsLooking && !IsHiding && !IsClear)
            {
                IsGameOver = true;
            }

            // �^�C�g���ɖ߂锻��
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

        #region Combo
        IEnumerator ComboAnim(bool repeat)
        {
            //�R���{�p���̎��̃e�L�X�g�A�j���[�V����
            float t = 0;
            float textSize = 1;
            float animTime  = 0.3f;
            print(comboUISize.length);
            bool changeText = false;
            while (t < animTime)
            {
                textSize = comboUISize.Evaluate(t);
                ComboUI.rectTransform.localScale = new Vector2(textSize, textSize);
                if (t > animTime / 2.0f && !changeText)
                {
                    ComboUI.text = "Combo " + comboCounter;
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
            if (comboCounter  == 0) ComboUI.text = "Combo " + comboCounter;
            comboCounter++;
            StartCoroutine(ComboAnim(repeat));
        }

        public void ComboEnd()
        {
            //�R���{�I���̎� DeleteCake.cs���Ă�
            comboCounter = 0;
            ComboUI.text = "";
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
    }


}