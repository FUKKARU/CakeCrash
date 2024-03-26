using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Main
{
    public class VolumeChanger : MonoBehaviour
    {
        [SerializeField] Volume postFXVolume;
        [SerializeField] AudioSource audioSource;
        private DepthOfField depthOfField;
        private VolumeChanger bokeh;
        float bokehState;
        bool max;

        void Start()
        {
            postFXVolume.profile.TryGet(out depthOfField);
        }

        private void Update()
        {
            //Debug.Log(Mathf.Sin(Time.time)*100);
            bokehState = depthOfField.focalLength.value;
            if (bokehState == 1 && GameManager.Instance.IsTired == true)
            {
                StartCoroutine(BokehHajime());
            }
            if (GameManager.Instance.IsTired == false)
            {
                bokehState = 1f;

                audioSource.enabled = false; // çƒê∂í‚é~
            }

        }

        IEnumerator BokehHajime()
        {
            // çƒê∂äJén
            audioSource.enabled = true;
            audioSource.clip = SoundParamsSO.Entity.StaminaRunOutSE;
            audioSource.Play();

            depthOfField.focalLength.value = 250f;
            while (bokehState < 300f && max == false)
            {
                yield return new WaitForSeconds(0.01f);
                depthOfField.focalLength.value = bokehState++;
                if (bokehState == 299f)
                {
                    max = true;
                }

            }
            if (max == true)
            {
                Debug.Log("off");
            }
            //while (max == true && GameManager.Instance.IsTired == true)
            //{
            //    depthOfField.focalLength.value -= Mathf.Sin(Time.time)*100;
            //}
        }
    }

}








