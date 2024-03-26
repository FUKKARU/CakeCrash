using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class SlideCake : MonoBehaviour
    {
        void Update()
        {
            if (!GameManager.Instance.IsLeftMode)
            {
                transform.position += Vector3.right * (CakeParamsSO.Entity.CakeSpeed * Time.deltaTime);
                // ��ʊO�ɍs���������
                if (transform.position.x > CakeParamsSO.Entity.CakeLimitX)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                transform.position -= Vector3.right * (CakeParamsSO.Entity.CakeSpeed * Time.deltaTime);
                // ��ʊO�ɍs���������
                if (transform.position.x < -CakeParamsSO.Entity.CakeLimitX)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
