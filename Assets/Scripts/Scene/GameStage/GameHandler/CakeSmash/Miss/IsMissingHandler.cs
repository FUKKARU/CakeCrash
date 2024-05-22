using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class IsMissingHandler : MonoBehaviour
    {
        [SerializeField] Transform missCreamParent;
        [SerializeField] GameObject missCream;
        [SerializeField] AudioSource audioSource;

        GameObject creamInstance = null;

        bool isDoingPenalty = false; // 1回だけ実行するためのフラグ

        // ペナルティ
        public void MissCreamGenerate()
        {
            if (!isDoingPenalty && creamInstance == null)
            {
                isDoingPenalty = true;
                GameManager.Instance.IsDoingPenalty = true;
                StartCoroutine(MissCreamBhv());
            }
        }
        IEnumerator MissCreamBhv()
        {
            creamInstance = Instantiate(missCream, CreamParamsSO.Entity.MissCreamGeneratePos, Quaternion.identity, missCreamParent);
            audioSource.PlayOneShot(SoundParamsSO.Entity.CreamHitFaceSE);

            yield return new WaitForSeconds(CreamParamsSO.Entity.MissCreamFadePeriod);

            Destroy(creamInstance);

            GameManager.Instance.IsDoingPenalty = false;
            isDoingPenalty = false;
        }
    }
}