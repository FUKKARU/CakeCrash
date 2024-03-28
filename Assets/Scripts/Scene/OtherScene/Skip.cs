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

        const float deleteWaitTime = 5f; // �Q�[�����J�n���Ă���A�X�L�b�v�e�L�X�g�������܂ł̕b��
        const float fadeOutPeriod = 1f; // �t�F�[�h�A�E�g�������(�b)

        IEnumerator Start()
        {
            skipText.enabled = true;
            StartCoroutine(DeleteWait());
            yield return new WaitForSeconds(85);
            SceneChange();
            yield return null;
        }

        // �N���b�N���ꂽ��V�[���؂�ւ�
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
