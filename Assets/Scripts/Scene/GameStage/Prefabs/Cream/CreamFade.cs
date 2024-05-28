using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class CreamFade : MonoBehaviour
    {
        [SerializeField] AudioSource audioSource;

        void Start()
        {
            audioSource.PlayOneShot(SoundParamsSO.Entity.CakeCrushSE);

            StartCoroutine(Fade());
        }

        IEnumerator Fade()
        {
            yield return new WaitForSeconds(CreamParamsSO.Entity.CreamFadePeriod);
            Destroy(gameObject);
            yield break;
        }
    }
}