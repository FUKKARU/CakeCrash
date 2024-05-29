using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Main
{
    public class Countdown : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI countdownText;
        static readonly string[] countTimeText = { "3", "2", "1", "GO!" };
        float unscaledTime = 4f;

        // Å‰‚Ì5ƒtƒŒ[ƒ€‚Í‰½‚à‚µ‚È‚¢B
        int frameCount = 5;

        void Awake()
        {
            Time.timeScale = 0;
        }

        void Start()
        {
            countdownText.text = countTimeText[0];
        }

        void Update()
        {
            if (frameCount > 0)
            {
                frameCount--;
                return;
            }

            unscaledTime -= Time.unscaledDeltaTime;

            if (3f <= unscaledTime) countdownText.text = countTimeText[0];
            else if (2f <= unscaledTime) countdownText.text = countTimeText[1];
            else if (1f <= unscaledTime) countdownText.text = countTimeText[2];
            else if (0f <= unscaledTime) countdownText.text = countTimeText[3];
            else
            {
                countdownText.enabled = false;
                GameManager.Instance.PlayBGM();
                Time.timeScale = 1;
                GameManager.Instance.IsPause = false;
                gameObject.SetActive(false);
            }
        }
    }
}
