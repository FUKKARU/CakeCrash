using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Main
{
    public class Countdown : MonoBehaviour
    {
        [SerializeField] Image countdownImage;
        [SerializeField] Sprite[] countdownSprites = new Sprite[4];
        float unscaledTime = 4f;

        [SerializeField] RectTransform _transisionUI; // トランジションの真っ黒なUI
        // トランジション中かどうか（この間はカウントダウンしない）
        bool _isDoingTransision = true;

        void Awake()
        {
            Time.timeScale = 0;
        }

        void Start()
        {
            // トランジションを開始！
            StartCoroutine(Transision());
        }

        void Update()
        {
            // トランジション中はカウントダウンしない
            if (_isDoingTransision) return;

            unscaledTime -= Time.unscaledDeltaTime;

            if (3f <= unscaledTime) countdownImage.sprite = countdownSprites[0];
            else if (2f <= unscaledTime) countdownImage.sprite = countdownSprites[1];
            else if (1f <= unscaledTime) countdownImage.sprite = countdownSprites[2];
            else if (0f <= unscaledTime) countdownImage.sprite = countdownSprites[3];
            else
            {
                countdownImage.enabled = false;
                GameManager.Instance.PlayBGM();
                Time.timeScale = 1;
                GameManager.Instance.IsPause = false;
                gameObject.SetActive(false);
            }
        }

        // トランジションUIのx座標を、
        // 指定秒数で、
        // 0 => 800 にする。
        IEnumerator Transision()
        {
            yield return new WaitForSecondsRealtime(OtherParamsSO.Entity.BetweenTransisionDur);

            float time = 0;
            float DUR = OtherParamsSO.Entity.LoadTransisionDur;

            while (time < DUR)
            {
                time += Time.unscaledDeltaTime;

                Vector3 uiPos = _transisionUI.localPosition;
                uiPos.x = time * 800 / DUR;
                _transisionUI.localPosition = uiPos;

                yield return null;
            }

            // [通告する] トランジションの演出完了！（カウントダウンを開始せよ）
            _isDoingTransision = false;
        }
    }
}
