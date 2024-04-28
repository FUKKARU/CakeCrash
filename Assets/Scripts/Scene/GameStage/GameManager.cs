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
        [NonSerialized] public bool IsHammerShakable = false; // �n���}�[��U���Ă��邩�ǂ���(1�񂵂��g��Ȃ�)
        [NonSerialized] public bool IsHammerGeneratable = false; // �n���}�[�𐶐��\�ȏ�Ԃł��邩�ǂ���
        [NonSerialized] public bool IsTired = false; // ���Ă��邩�ǂ���
        [NonSerialized] public bool IsHiding = false; // �B��Ă��邩�ǂ���
        [NonSerialized] public bool IsLooking = false; // �x����������������Ă��邩�ǂ���
        [NonSerialized] public bool IsDoingPenalty = false; // �y�i���e�B�����s�����ǂ���
        [NonSerialized] public bool IsAllSmashed = false; // �P�[�L��S�ĉ󂵂����ǂ���
        [NonSerialized] public bool IsClear = false; // �N���A�ɂȂ������ǂ���
        [NonSerialized] public bool IsGameOver = false; // �Q�[���I�[�o�[�ɂȂ������ǂ���
        [Header("��,��,��")] public GameObject[] HitTutorial;
        [SerializeField] AudioSource audioSourceBGM;
        [SerializeField] AudioSource audioSourceSE;
        public Image CakeOutOfRangeUI;
        public GameObject SquatAnnounceUI;
        bool isCakeOutOfRangeUIShowing = false; // �P�[�L���͈͊O��UI���A�\�������ǂ���
        float quitTime = 0; // �^�C�g���ɖ߂�{�^����������Ă��鎞��

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

            // �A�N�e�B�u�ɂ������Ȃ�����A���͏�����B
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