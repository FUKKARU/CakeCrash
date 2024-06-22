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

        bool _loading = false;
        float _timeAfterLoadCompleted = 0f; // ロードが完了した後、トランジションの演出が入るまでの時間のカウント
        bool _isDoingTransision = false; // ロードが完了した後の

        void Start()
        {
            startBtn.onClick.AddListener(LoadNextScene);
        }

        void Update()
        {
            //Loading中はUpdate()処理を受け付けない
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

                    // ロードが完了しているなら時間をカウントし...
                    _timeAfterLoadCompleted += Time.deltaTime;
                    // 一定時間に達したら...
                    if (_timeAfterLoadCompleted >= OtherParamsSO.Entity.DurAfterLoadCompleted)
                    {

                    }
                    if (IA.InputGetter.Instance.IsRed || IA.InputGetter.Instance.IsGreen || IA.InputGetter.Instance.IsBlue)
                    {
                        async.allowSceneActivation = true;
                    }
                }
                yield return null;
            }
        }

        //ローディング中ならtrueを返す関数
        bool CheckLoading()
        {
            return _loading;
        }

        //UIのボタンから処理される
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
