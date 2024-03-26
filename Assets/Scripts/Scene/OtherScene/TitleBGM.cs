using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class TitleBGM : MonoBehaviour
    {
        void Start()
        {
            // すでにBGMを再生中のゲームオブジェクトがあったら，自身を消す
            if (GameObject.FindGameObjectsWithTag("TitleBGM").Length > 1)
            {
                Destroy(gameObject);
            }
            // BGMを再生
            else
            {
                DontDestroyOnLoad(gameObject);

                AudioSource audioSource = GetComponent<AudioSource>();
                audioSource.clip = SoundParamsSO.Entity.TitleBGM;
                audioSource.Play();
            }
        }
    }
}