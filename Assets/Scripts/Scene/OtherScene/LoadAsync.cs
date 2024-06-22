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

        [SerializeField] RectTransform transisionUI; // トランジションの真っ黒なUI
        AsyncOperation async = null; // ロードの処理で、ロード状態をぶち込むよ
        bool _loading = false;
        float _timeAfterLoadCompleted = 0f; // ロードが完了した後、トランジションの演出が入るまでの時間のカウント

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
            async = SceneManager.LoadSceneAsync("GameStage");
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
                        // [通告する] トランジションを開始せよ！
                        StartCoroutine(Transision());
                        // ロードの処理を終了する
                        yield break;
                    }
                }
                yield return null;
            }
        }

        // LoadSceneコルーチン内で呼ばれる
        // タイトル => ゲームシーン のトランジション
        // 一定秒数で、トランジションUIのx座標が -800 => 0 になる
        IEnumerator Transision()
        {
            // カウントの時間を0にして...
            float time = 0f;
            // トランジションの時間をキャッシュして...
            float DUR = OtherParamsSO.Entity.LoadTransisionDur;
            // その時間までカウントし、UIのx座標を変える
            while (time < DUR)
            {
                time += Time.deltaTime;

                Vector3 uiPos = transisionUI.localPosition;
                uiPos.x = (time - DUR) * 800 / DUR;
                transisionUI.localPosition = uiPos;

                yield return null;
            }

            // トランジションの演出が終わったら、シーン切り替え！
            async.allowSceneActivation = true;
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
