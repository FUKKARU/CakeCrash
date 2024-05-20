using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class IsMissingHandler : MonoBehaviour
    {
        [SerializeField] GameObject missCream;
        [SerializeField] AudioSource audioSource;

        // ペナルティ
        public IEnumerator MissCreamGenerate()
        {
            GameObject missCreamInstance = Instantiate(missCream, CreamParamsSO.Entity.MissCreamGeneratePos, Quaternion.identity);
            audioSource.PlayOneShot(SoundParamsSO.Entity.CreamHitFaceSE);

            yield return new WaitForSeconds(CreamParamsSO.Entity.MissCreamFadePeriod);

            Destroy(missCreamInstance);

            GameManager.Instance.IsDoingPenalty = false;
            GameManager.Instance.IsReallyDoingPenalty = true;
        }
    }
}