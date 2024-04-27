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
        int starNum; // 星の数
        int doneStarNum = 0; // 処理が終了した星の数
        bool isGoBackStandBy = false; // タイトルに戻るボタンの押下判定を取るかどうか

        const float starEffectPeriod = 0.2f; // 星が出る演出の間の秒数
        const float firstScaleAmount = 3; // 演出で，最初に何倍にするか
        const float scaleSpeed = 10; // 演出で，小さくなる速さ
        const float goBackButtonBlinkPeriod = 0.75f; // タイトルに戻るボタンが点滅する周期(秒)
        const int maxStarPattern = 4; // 星のパターンの最大個数
        readonly int[] dClearTimes = { 0, 0, 0, 0 }; // 初期設定：クリア回数 (E,N,H,SH)

        void Update()
        {
            if (GameManager.Instance.IsClear && !IsClearHandling)
            {
                IsClearHandling = true;

                // クリアしたら，今いる"GuardMan"を全て消し，もう警備員が来ることはない。
                foreach (GameObject guardMan in GameObject.FindGameObjectsWithTag("GuardMan"))
                {
                    Destroy(guardMan);
                }

                // UI切り替え
                gameUI.SetActive(false);
                clearUI.SetActive(true);

                nowVolume = gameBGMAS.volume;

                // 星の数を計算
                if (GameManager.Instance.Score <= GameManager.Instance.CakeMaxNum / (float)maxStarPattern) { starNum = 3; }
                else if (GameManager.Instance.Score <= GameManager.Instance.CakeMaxNum * 2 / (float)maxStarPattern) { starNum = 2; }
                else if (GameManager.Instance.Score <= GameManager.Instance.CakeMaxNum * 3 / (float)maxStarPattern) { starNum = 1; }
                else { starNum = 0; }

                StartCoroutine(StarShow(starNum));
            }

            // 全ての星の処理が終了したら，スコアのテキストの演出を開始
            if (doneStarNum >= maxStarPattern - 1)
            {
                doneStarNum = 0; // このif分はもう実行されない

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

        // 星の演出
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

        // スコアのテキストの演出
        IEnumerator ScoreTextEffect()
        {
            Vector3 firstScale = scoreText.transform.localScale;
            scoreText.transform.localScale = firstScale * firstScaleAmount;
            if (GameManager.Instance.Score > 0)
            {
                // 達成度を小数第1位まで（四捨五入して）表示
                float p = (GameManager.Instance.CakeMaxNum - GameManager.Instance.Score) / (float)GameManager.Instance.CakeMaxNum * 100;
                scoreText.GetComponent<TextMeshProUGUI>().text = $"達成度：{Math.Round(p, 1, MidpointRounding.AwayFromZero)}<size=54>%</size>";
            }
            else
            {
                // クリア回数を1増やし，セーブ
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
                
                scoreText.GetComponent<TextMeshProUGUI>().text = $"<size=54><color=#FF0000>[ノルマ達成] お見事！</color></size>";
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

        // ボタンの演出
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

        // タイトルに戻る
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