using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace Main
{
    public class LoadAsync : MonoBehaviour
    {
        [SerializeField] Button startBtn;
        [SerializeField] GameObject _loadingUI;
        [SerializeField] Slider _slider;
        [SerializeField] TextMeshProUGUI _text;
        [SerializeField] TextMeshProUGUI _startAbleText;

        [SerializeField] RectTransform transisionUI; // �g�����W�V�����̐^������UI
        AsyncOperation async = null; // ���[�h�̏����ŁA���[�h��Ԃ��Ԃ����ނ�
        bool _loading = false;
        float _timeAfterLoadCompleted = 0f; // ���[�h������������A�g�����W�V�����̉��o������܂ł̎��Ԃ̃J�E���g

        void Start()
        {
            startBtn.onClick.AddListener(LoadNextScene);
        }

        void Update()
        {
            //Loading����Update()�������󂯕t���Ȃ�
            if (CheckLoading())
            {
                return;
            }
        }

        IEnumerator LoadScene()
        {
            async = SceneManager.LoadSceneAsync("GameStage");
            async.allowSceneActivation = false;
            while (!async.isDone)
            {
                _slider.value = async.progress;
                if (async.progress >= 0.9f)
                {
                    _text.text = "Load complete!";
                    if (!_startAbleText.enabled) _startAbleText.enabled = true;

                    // ���[�h���������Ă���Ȃ玞�Ԃ��J�E���g��...
                    _timeAfterLoadCompleted += Time.deltaTime;
                    // ��莞�ԂɒB������...
                    if (_timeAfterLoadCompleted >= OtherParamsSO.Entity.DurAfterLoadCompleted)
                    {
                        // [�ʍ�����] �g�����W�V�������J�n����I
                        StartCoroutine(Transision());
                        // ���[�h�̏������I������
                        yield break;
                    }
                }
                yield return null;
            }
        }

        // LoadScene�R���[�`�����ŌĂ΂��
        // �^�C�g�� => �Q�[���V�[�� �̃g�����W�V����
        // ���b���ŁA�g�����W�V����UI��x���W�� -800 => 0 �ɂȂ�
        IEnumerator Transision()
        {
            // �J�E���g�̎��Ԃ�0�ɂ���...
            float time = 0f;
            // �g�����W�V�����̎��Ԃ��L���b�V������...
            float DUR = OtherParamsSO.Entity.LoadTransisionDur;
            // ���̎��Ԃ܂ŃJ�E���g���AUI��x���W��ς���
            while (time < DUR)
            {
                time += Time.deltaTime;

                Vector3 uiPos = transisionUI.localPosition;
                uiPos.x = (time - DUR) * 800 / DUR;
                transisionUI.localPosition = uiPos;

                yield return null;
            }

            // �g�����W�V�����̉��o���I�������A�V�[���؂�ւ��I
            async.allowSceneActivation = true;
        }

        //���[�f�B���O���Ȃ�true��Ԃ��֐�
        bool CheckLoading()
        {
            return _loading;
        }

        //UI�̃{�^�����珈�������
        public void LoadNextScene()
        {
            if (CheckLoading())
            {
                return;
            }
            _loading = true;
            Destroy(GameObject.FindGameObjectWithTag("TitleBGM"));
            startBtn.gameObject.SetActive(false);
            _loadingUI.SetActive(true);
            StartCoroutine(LoadScene());
        }
    }
}
