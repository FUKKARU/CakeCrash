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

        // Title -> Config (�ݒ��ʂɍs��)
        public void TitleToConfig()
        {
            StartCoroutine(ChangeScene("Config"));
        }

        // Config -> Title (�^�C�g���ɖ߂�)
        public void ConfigToTitle()
        {
            StartCoroutine(ChangeScene("Title"));
        }

        // �Q�[���I��
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