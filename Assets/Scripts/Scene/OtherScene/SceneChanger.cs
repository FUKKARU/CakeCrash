using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main
{
    public class SceneChanger : MonoBehaviour
    {
        [SerializeField] LoadAsync loadScript;
        [SerializeField] AudioSource clickAS;

        // Title -> Config (設定画面に行く)
        public void TitleToConfig()
        {
            StartCoroutine(ChangeScene("Config"));
        }

        // Config -> Title (タイトルに戻る)
        public void ConfigToTitle()
        {
            StartCoroutine(ChangeScene("Title"));
        }

        // ゲーム終了
        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
      Application.Quit();
#endif
        }

        IEnumerator ChangeScene(string sceneName)
        {
            clickAS.PlayOneShot(SoundParamsSO.Entity.ButtonClickSystem);
            yield return new WaitForSeconds(OtherParamsSO.Entity.SceneChangeWaitTime);
            SceneManager.LoadScene(sceneName);
        }
    }
}