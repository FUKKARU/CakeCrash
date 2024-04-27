using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

namespace Main
{
    public class ClearManager : MonoBehaviour
    {
        [SerializeField] GameObject gameUI;
        [SerializeField] GameObject clearUI;
        [Header("1,2,3")][SerializeField] GameObject[] offStars;
        [Header("1,2,3")][SerializeField] GameObject[] onStars;
        [SerializeField] GameObject scoreText;
        [SerializeField] Image goBack;
        [SerializeField] Image fadeOutImage;
        [SerializeField] AudioSource StarAS;
        [SerializeField] AudioSource ScoreTextAS;
        [SerializeField] AudioSource GoBackAS;
        [SerializeField] AudioSource gameBGMAS;
        bool IsClearHandling = false;
        float nowVolume;
        int starNum; // ���̐�
        int doneStarNum = 0; // �������I���������̐�
        bool isGoBackStandBy = false; // �^�C�g���ɖ߂�{�^���̉����������邩�ǂ���

        const float starEffectPeriod = 0.2f; // �����o�鉉�o�̊Ԃ̕b��
        const float firstScaleAmount = 3; // ���o�ŁC�ŏ��ɉ��{�ɂ��邩
        const float scaleSpeed = 10; // ���o�ŁC�������Ȃ鑬��
        const float goBackButtonBlinkPeriod = 0.75f; // �^�C�g���ɖ߂�{�^�����_�ł������(�b)
        const int maxStarPattern = 4; // ���̃p�^�[���̍ő��
        readonly int[] dClearTimes = { 0, 0, 0, 0 }; // �����ݒ�F�N���A�� (E,N,H,SH)

        void Update()
        {
            if (GameManager.Instance.IsClear && !IsClearHandling)
            {
                IsClearHandling = true;

                // �N���A������C������"GuardMan"��S�ď����C�����x���������邱�Ƃ͂Ȃ��B
                foreach (GameObject guardMan in GameObject.FindGameObjectsWithTag("GuardMan"))
                {
                    Destroy(guardMan);
                }

                // UI�؂�ւ�
                gameUI.SetActive(false);
                clearUI.SetActive(true);

                nowVolume = gameBGMAS.volume;

                // ���̐����v�Z
                if (GameManager.Instance.Score <= GameManager.Instance.CakeMaxNum / (float)maxStarPattern) { starNum = 3; }
                else if (GameManager.Instance.Score <= GameManager.Instance.CakeMaxNum * 2 / (float)maxStarPattern) { starNum = 2; }
                else if (GameManager.Instance.Score <= GameManager.Instance.CakeMaxNum * 3 / (float)maxStarPattern) { starNum = 1; }
                else { starNum = 0; }

                StartCoroutine(StarShow(starNum));
            }

            // �S�Ă̐��̏������I��������C�X�R�A�̃e�L�X�g�̉��o���J�n
            if (doneStarNum >= maxStarPattern - 1)
            {
                doneStarNum = 0; // ����if���͂������s����Ȃ�

                StartCoroutine(ScoreTextEffect());
            }
        }

        IEnumerator StarShow(int starNum)
        {
            if (starNum >= 1)
            {
                StartCoroutine(StarEffect(0));

                if (starNum >= 2)
                {
                    yield return new WaitForSeconds(starEffectPeriod);

                    StartCoroutine(StarEffect(1));

                    if (starNum == 3)
                    {
                        yield return new WaitForSeconds(starEffectPeriod);

                        StartCoroutine(StarEffect(2));
                    }
                    else
                    {
                        doneStarNum += maxStarPattern - 3;
                    }
                }
                else
                {
                    doneStarNum += maxStarPattern - 2;
                }
            }
            else
            {
                doneStarNum += maxStarPattern - 1;
            }
        }

        // ���̉��o
        IEnumerator StarEffect(int starIndex)
        {
            GameObject onStar = onStars[starIndex];
            Vector3 firstScale = onStar.transform.localScale;
            onStar.transform.localScale = firstScale * firstScaleAmount;
            onStar.SetActive(true);

            while (onStar.transform.localScale.sqrMagnitude >= firstScale.sqrMagnitude)
            {
                onStar.transform.localScale -= firstScale * scaleSpeed * Time.deltaTime;
                yield return null;
            }

            StarAS.PlayOneShot(SoundParamsSO.Entity.StarSE);
            doneStarNum += 1;
            yield break;
        }

        // �X�R�A�̃e�L�X�g�̉��o
        IEnumerator ScoreTextEffect()
        {
            Vector3 firstScale = scoreText.transform.localScale;
            scoreText.transform.localScale = firstScale * firstScaleAmount;
            if (GameManager.Instance.Score > 0)
            {
                // �B���x��������1�ʂ܂Łi�l�̌ܓ����āj�\��
                float p = (GameManager.Instance.CakeMaxNum - GameManager.Instance.Score) / (float)GameManager.Instance.CakeMaxNum * 100;
                scoreText.GetComponent<TextMeshProUGUI>().text = $"�B���x�F{Math.Round(p, 1, MidpointRounding.AwayFromZero)}<size=54>%</size>";
            }
            else
            {
                // �N���A�񐔂�1���₵�C�Z�[�u
                int dValue = ConfigParamsSO.Entity.DefaultDifficulty;
                int difficulty = PlayerPrefs.GetInt("Difficulty", dValue);
                if (difficulty == 0)
                {
                    int times = PlayerPrefs.GetInt("EasyClearTimes", dClearTimes[0]);
                    times++;
                    PlayerPrefs.SetInt("EasyClearTimes", times);
                }
                else if (difficulty == 1)
                {
                    int times = PlayerPrefs.GetInt("NormalClearTimes", dClearTimes[1]);
                    times++;
                    PlayerPrefs.SetInt("NormalClearTimes", times);
                }
                else if (difficulty == 2)
                {
                    int times = PlayerPrefs.GetInt("HardClearTimes", dClearTimes[2]);
                    times++;
                    PlayerPrefs.SetInt("HardClearTimes", times);
                }
                else
                {
                    int times = PlayerPrefs.GetInt("SuperHardClearTimes", dClearTimes[3]);
                    times++;
                    PlayerPrefs.SetInt("SuperHardClearTimes", times);
                }
                
                scoreText.GetComponent<TextMeshProUGUI>().text = $"<size=54><color=#FF0000>[�m���}�B��] �������I</color></size>";
            }
            scoreText.SetActive(true);

            while (scoreText.transform.localScale.sqrMagnitude >= firstScale.sqrMagnitude)
            {
                scoreText.transform.localScale -= firstScale * scaleSpeed * Time.deltaTime;
                yield return null;
            }

            ScoreTextAS.PlayOneShot(SoundParamsSO.Entity.ScoreTextSE);
            StartCoroutine(GoBackStandBy());
            yield break;
        }

        // �{�^���̉��o
        IEnumerator GoBackStandBy()
        {
            isGoBackStandBy = true;

            Color color = goBack.color;
            while (true)
            {
                yield return new WaitForSeconds(goBackButtonBlinkPeriod / 2);
                color.a = 1;
                goBack.color = color;
                yield return new WaitForSeconds(goBackButtonBlinkPeriod / 2);
                color.a = 0;
                goBack.color = color;
            }
        }

        // �^�C�g���ɖ߂�
        IEnumerator ToTitle()
        {
            Color color = fadeOutImage.color;
            float a = 0;

            while (true)
            {
                a += OtherParamsSO.Entity.FadeOutSpeed * Time.deltaTime / 100;

                if (a > 1)
                {
                    SceneManager.LoadScene("Title");

                    yield break;
                }
                else
                {
                    color.a = a;
                    fadeOutImage.color = color;

                    gameBGMAS.volume = nowVolume * (1 - a);
                }

                yield return null;
            }
        }

        public void OnClick()
        {
            if (isGoBackStandBy)
            {
                GoBackAS.PlayOneShot(SoundParamsSO.Entity.ButtonClickSystem);
                StartCoroutine(ToTitle());
            }
        }
    }
}