using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class IsMissingHandler : MonoBehaviour
    {
        [SerializeField] GameObject missCream;
        [SerializeField] AudioSource audioSource;
        bool isReallyDoingPenalty = false;

        void Update()
        {
            // �~�X�����m���āC�y�i���e�B���łȂ��Ƃ��C�y�i���e�B�����s
            if (GameManager.Instance.IsDoingPenalty && !isReallyDoingPenalty)
            {
                isReallyDoingPenalty = true;
                StartCoroutine(MissCreamGenerate());
            }
        }

        // �y�i���e�B
        IEnumerator MissCreamGenerate()
        {
            GameObject missCreamInstance = Instantiate(missCream, CreamParamsSO.Entity.MissCreamGeneratePos, Quaternion.identity);
            audioSource.PlayOneShot(SoundParamsSO.Entity.CreamHitFaceSE);

            yield return new WaitForSeconds(CreamParamsSO.Entity.MissCreamFadePeriod);

            Destroy(missCreamInstance);
            
            isReallyDoingPenalty = false;
            GameManager.Instance.IsDoingPenalty = false;
            yield break;
        }
    }
}