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
            transform.position += Vector3.right * (GameManager.Instance.CakeSpeed * Time.deltaTime);
            // ‰æ–ÊŠO‚És‚Á‚½‚çÁ‚·
            if (transform.position.x > CakeParamsSO.Entity.CakeLimitX)
            {
                if (!GameManager.Instance.IsGameOver && !GameManager.Instance.IsOpening)
                    GameManager.Instance.HappinessIncrement(transform.childCount - 1);
                Destroy(gameObject);
            }
            if (GameManager.Instance.IsGameOver && !decSpeedForGameOver)
            {
                decSpeedForGameOver = true;
                StartCoroutine(DecSpeed());
            }
        }

        IEnumerator DecSpeed()
        {
            float fastestSpeed = GameManager.Instance.CakeSpeed;
            float endTime = 3;
            float t = 0;
            while (t < endTime)
            {
                t += Time.deltaTime;
                GameManager.Instance.CakeSpeed = ((initialSpeed - fastestSpeed) / endTime) * t + fastestSpeed;
                yield return null;
            }
            yield break;
        }
    }
}
