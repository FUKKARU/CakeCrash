using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Main
{
    public class Skip : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI skipText;
        [SerializeField] Image fadeOutImage;

        const float deleteWaitTime = 5f; // ゲームが開始してから、スキップテキストを消すまでの秒数
        const float fadeOutPeriod = 1f; // フェードアウトする期間(秒)

        IEnumerator Start()
        {
            skipText.enabled = true;
            StartCoroutine(DeleteWait());
            yield return new WaitForSeconds(85);
            SceneChange();
            yield return null;
        }

        // クリックされたらシーン切り替え
        public void SceneChange()
        {
            StartCoroutine(SceneChangeBehaviour());
        }

        IEnumerator DeleteWait()
        {
            yield return new WaitForSeconds(deleteWaitTime);
            skipText.enabled = false;
        }

        IEnumerator SceneChangeBehaviour()
        {
            float time = 0f;

            while (true)
            {
                yield return null;
                time += Time.deltaTime;

                Color color = fadeOutImage.color;
                color.a = time / fadeOutPeriod;

                if (color.a > 1f)
                {
                    color.a = 1f;
                    fadeOutImage.color = color;
                    break;
                }

                fadeOutImage.color = color;
            }

            SceneManager.LoadScene("GameStage");
        }
    }
}
