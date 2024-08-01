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

        [SerializeField] RectTransform _transisionUI; // �g�����W�V�����̐^������UI
        // �g�����W�V���������ǂ����i���̊Ԃ̓J�E���g�_�E�����Ȃ��j
        bool _isDoingTransision = true;

        void Awake()
        {
            Time.timeScale = 0;
        }

        void Start()
        {
            // �g�����W�V�������J�n�I
            StartCoroutine(Transision());
        }

        void Update()
        {
            // �g�����W�V�������̓J�E���g�_�E�����Ȃ�
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
