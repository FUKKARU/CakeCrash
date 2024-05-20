using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class HammerSmash : MonoBehaviour
    {
        [SerializeField] AudioSource audioSource;
        float startEulerZ;
        float endEulerZ;
        float eulerZ;
        float t = 0;

        void Start()
        {
            startEulerZ = HumanParamsSO.Entity.HammerEulerZ.x;
            endEulerZ = HumanParamsSO.Entity.HammerEulerZ.y;
            eulerZ = startEulerZ;
            transform.rotation = Quaternion.Euler(0, 90, eulerZ);

            audioSource.PlayOneShot(SoundParamsSO.Entity.HammerSmashSE);
        }

        void Update()
        {
            t += Time.deltaTime;
            eulerZ = (endEulerZ - startEulerZ) * t / HumanParamsSO.Entity.HammerDur;
            if (t >= HumanParamsSO.Entity.HammerDur)
            {
                Destroy(gameObject);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 90, eulerZ);
            }
        }
    }
}