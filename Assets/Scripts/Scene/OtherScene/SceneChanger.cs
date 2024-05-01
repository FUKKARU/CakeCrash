using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Main
{
    public class SceneChanger : MonoBehaviour
    {
        [Header("Configシーンでは設定の必要なし")][SerializeField] Image fadeOutImage;
        [Space(50)]
        [SerializeField] AudioSource clickAS;
        bool isStartPlaced = false;

        // Title -> GameStage
        public void TitleToGameStage()
        {
            isStartPlaced = true;
            StartCoroutine(FadeOutToGameStage());
        }

        // Title -> Config (設定画面に行く)
        public void TitleToConfig()
        {
            if (!isStartPlaced)
            {
                StartCoroutine(ChangeScene("Config"));
            }
        }

        // Config -> Title (タイトルに戻る)
        public void ConfigToTitle()
        {
            StartCoroutine(ChangeScene("Title"));
        }

        // ゲーム終了
        public void Quit()
        {
            if (!isStartPlaced)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
      Application.Quit();
#endif
            }
        }

        IEnumerator ChangeScene(string sceneName)
        {
            clickAS.PlayOneShot(SoundParamsSO.Entity.ButtonClickSystem);
            yield return new WaitForSeconds(OtherParamsSO.Entity.SceneChangeWaitTime);
            SceneManager.LoadScene(sceneName);
            yield break;
        }

        IEnumerator FadeOutToGameStage()
        {
            clickAS.PlayOneShot(SoundParamsSO.Entity.ButtonClickSystem);

            // カウントダウン
            GameObject.FindGameObjectWithTag("StartButton").SetActive(false);
            TextMeshProUGUI countdown = GameObject.FindGameObjectWithTag("Countdown").GetComponent<TextMeshProUGUI>();
            countdown.enabled = true;
            for (int i = 3; i >= 0; i--)
            {
                // 表示
                countdown.text = i > 0 ? i.ToString() : "GO!";

                yield return new WaitForSeconds(1);
            }

            GameObject titleBGM = GameObject.FindGameObjectsWithTag("TitleBGM")[0];
            AudioSource titleBGMAS = titleBGM.GetComponent<AudioSource>();
            float nowVolume = titleBGMAS.volume;
            Color color = fadeOutImage.color;
            float a = 0;

            while (true)
            {
                a += OtherParamsSO.Entity.FadeOutSpeed * Time.deltaTime / 100;

                if (a > 1)
                {
                    Destroy(titleBGM);
                    SceneManager.LoadScene("GameStage");

                    yield break;
                }
                else
                {
                    color.a = a;
                    fadeOutImage.color = color;

                    titleBGMAS.volume = nowVolume * (1 - a);
                }

                yield return null;
            }
        }
    }
}