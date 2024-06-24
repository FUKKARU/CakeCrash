using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Main
{
    public class Countdown : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI countdownText;
        static readonly string[] countTimeText = { "", "3", "2", "1", "GO!" };
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
            countdownText.text = countTimeText[0];
            // トランジションを開始！
            StartCoroutine(Transision());
        }

        void Update()
        {
            // トランジション中はカウントダウンしない
            if (_isDoingTransision) return;

            unscaledTime -= Time.unscaledDeltaTime;

            if (4f <= unscaledTime) countdownText.text = countTimeText[0];
            else if (3f <= unscaledTime) countdownText.text = countTimeText[1];
            else if (2f <= unscaledTime) countdownText.text = countTimeText[2];
            else if (1f <= unscaledTime) countdownText.text = countTimeText[3];
            else if (0f <= unscaledTime) countdownText.text = countTimeText[4];
            else
            {
                countdownText.enabled = false;
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
