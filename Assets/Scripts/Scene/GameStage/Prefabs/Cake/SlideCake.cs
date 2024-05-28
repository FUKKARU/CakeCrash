using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class SlideCake : MonoBehaviour
    {
        static readonly float initialSpeed = 15;
        bool decSpeedForGameOver;
        void Update()
        {
            transform.position += Vector3.right * (CakeParamsSO.Entity.CakeSpeed * Time.deltaTime);
            // 画面外に行ったら消す
            if (transform.position.x > CakeParamsSO.Entity.CakeLimitX)
            {
                if (!GameManager.Instance.isGameOver)
                    GameManager.Instance.HappinessIncrement(transform.childCount - 1);
                Destroy(gameObject);
            }
            if (GameManager.Instance.isGameOver && !decSpeedForGameOver)
            {
                decSpeedForGameOver = true;
                StartCoroutine(DecSpeed());
            }
        }

        IEnumerator DecSpeed()
        {
            float fastestSpeed = CakeParamsSO.Entity.CakeSpeed;
            float endTime = 3;
            float t = 0;
            while (t < endTime)
            {
                t += Time.deltaTime;
                CakeParamsSO.Entity.CakeSpeed = ((initialSpeed - fastestSpeed) / endTime) * t + fastestSpeed;
                yield return null;
            }
            yield break;
        }
    }
}
