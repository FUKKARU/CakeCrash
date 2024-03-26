using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Main
{
    public class StaminaChecker : MonoBehaviour
    {
        [SerializeField] Slider slider;

        void Start()
        {
            slider.value = 1;
            GameManager.Instance.CurrentStamina = HumanParamsSO.Entity.MaxStamina;
            InvokeRepeating("Kaifuku", HumanParamsSO.Entity.StaminaRecoverSpan, HumanParamsSO.Entity.StaminaRecoverSpan);
        }
        private void Update()
        {
            // ハンマーを振ったとき，疲れていない，かつ隠れていない，かつ警備員に見られていない，
            // かつクリアになっていない，かつゲームオーバーになっていないならば
            if (GameManager.Instance.IsHammerShakable)
            {
                GameManager.Instance.IsHammerShakable = false;
                if (!GameManager.Instance.IsTired && !GameManager.Instance.IsHiding && !GameManager.Instance.IsLooking && !GameManager.Instance.IsClear && !GameManager.Instance.IsGameOver)
                {
                    GameManager.Instance.CurrentStamina -= GameManager.Instance.StaminaDecreaseAmount;
                }
            }

            slider.value = GameManager.Instance.CurrentStamina / (float)HumanParamsSO.Entity.MaxStamina;
            if (GameManager.Instance.CurrentStamina <= 0)
            {
                GameManager.Instance.CurrentStamina = 0;
                GameManager.Instance.IsTired = true;
            }
            else if (GameManager.Instance.CurrentStamina >= HumanParamsSO.Entity.MaxStamina)
            {
                GameManager.Instance.CurrentStamina = HumanParamsSO.Entity.MaxStamina;
                GameManager.Instance.IsTired = false;
            }
        }
        void Kaifuku()
        {
            GameManager.Instance.CurrentStamina += GameManager.Instance.StaminaIncreaseAmount;
        }
    }
}