using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class HammerHead : MonoBehaviour
    {
        [SerializeField] GameObject hammer;

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("HitZone"))
            {
                DeleteCake deleteCake = other.transform.parent.GetComponent<DeleteCake>();
                if (!deleteCake.IsTouchedHammerHead)
                {
                    deleteCake.HitHammer();
                    GameManager.Instance.DeleteAllHammers();
                }
            }
        }
    }
}
