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

        [SerializeField] RectTransform _transisionUI; // �g�����W�V�����̐^������UI
        // �g�����W�V���������ǂ����i���̊Ԃ̓J�E���g�_�E�����Ȃ��j
        bool _isDoingTransision = true;

        void Awake()
        {
            Time.timeScale = 0;
        }

        void Start()
        {
            countdownText.text = countTimeText[0];
            // �g�����W�V�������J�n�I
            StartCoroutine(Transision());
        }

        void Update()
        {
            // �g�����W�V�������̓J�E���g�_�E�����Ȃ�
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

        // �g�����W�V����UI��x���W���A
        // �w��b���ŁA
        // 0 => 800 �ɂ���B
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

            // [�ʍ�����] �g�����W�V�����̉��o�����I�i�J�E���g�_�E�����J�n����j
            _isDoingTransision = false;
        }
    }
}
