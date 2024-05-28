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
        [SerializeField] GameObject titleBGM;
        [SerializeField] Button startBtn;
        [SerializeField] GameObject _loadingUI;
        [SerializeField] Slider _slider;
        [SerializeField] TextMeshProUGUI _text;
        [SerializeField] TextMeshProUGUI _startAbleText;
        [SerializeField] TitleInputGetter inputScript;

        bool _loading = false;

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
            AsyncOperation async = SceneManager.LoadSceneAsync("GameStage");
            async.allowSceneActivation = false;
            while (!async.isDone)
            {
                _slider.value = async.progress;
                if (async.progress >= 0.9f)
                {
                    _text.text = "Load complete!";
                    if (!_startAbleText.enabled) _startAbleText.enabled = true;
                    if (inputScript.IsRed >= 0.99f || inputScript.IsGreen >= 0.99f || inputScript.IsBlue >= 0.99f)
                    {
                        async.allowSceneActivation = true;
                    }
                }
                yield return null;
            }
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
            Destroy(titleBGM);
            startBtn.gameObject.SetActive(false);
            _loadingUI.SetActive(true);
            StartCoroutine(LoadScene());
        }
    }
}
