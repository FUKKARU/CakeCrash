using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Main
{
    public class GameOverManager : MonoBehaviour
    {
        [SerializeField] GameObject gameUI;
        [SerializeField] GameObject gameOverUI;
        [SerializeField] Image gameOverImage;
        [SerializeField] TextMeshProUGUI goBackText;
        [SerializeField] AudioSource gameOverAS;
        [SerializeField] AudioSource clickAS;
        [SerializeField] Image fadeOutImage;
        [SerializeField] AudioSource gameBGMAS;
        float nowVolume;
        bool IsGameOverHandling = false;
        bool goBackStandBy = false;
        bool fadeOutStarted = false;

        const float gameOverImageFadeInSpeed = 25f; // ゲームオーバーの画像がフェードインするスピード

        private void Update()
        {
            if (GameManager.Instance.IsGameOver && !IsGameOverHandling)
            {
                IsGameOverHandling = true;

                // 即座にカメラを警備員に向ける
                Vector3 cameraRotation = new Vector3(OtherParamsSO.Entity.GameOverCameraRotationX, 0, 0);
                Camera.main.transform.rotation = Quaternion.Euler(cameraRotation);

                // UI切り替え
                gameUI.SetActive(false);
                gameOverUI.SetActive(true);

                // 音を再生
                gameOverAS.PlayOneShot(SoundParamsSO.Entity.FoundByGuardManSE);
                nowVolume = gameBGMAS.volume;

                StartCoroutine(GameOverImageFadeIn());
            }

            // ボタンが押されたらフェードアウトしてシーン切り替え
            if (goBackStandBy && !fadeOutStarted && GameManager.Instance.IsRed >= 0.99f)
            {
                fadeOutStarted = true;

                clickAS.PlayOneShot(SoundParamsSO.Entity.ButtonClickSystem);

                StartCoroutine(FadeOut());
            }
        }

        // ゲームオーバー画像のフェードイン
        IEnumerator GameOverImageFadeIn()
        {
            float a = 0;

            while (true)
            {
                a += gameOverImageFadeInSpeed * Time.deltaTime / 100;

                if (a > 1)
                {
                    goBackText.enabled = true;
                    goBackStandBy = true;

                    yield break;
                }
                else
                {
                    Color color = gameOverImage.color;
                    color.a = a;
                    gameOverImage.color = color;
                }

                yield return null;
            }
        }

        // フェードアウトしてシーンチェンジ
        IEnumerator FadeOut()
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
    }
}