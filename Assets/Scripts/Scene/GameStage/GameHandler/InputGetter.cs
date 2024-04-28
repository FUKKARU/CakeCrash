using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Main
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputGetter : MonoBehaviour
    {
        private PlayerInput _playerInput;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
        }

        private void OnEnable()
        {
            if (_playerInput == null) return;

            // デリゲート登録
            _playerInput.onActionTriggered += OnMove;
        }

        private void OnDisable()
        {
            if (_playerInput == null) return;

            // デリゲート登録解除
            _playerInput.onActionTriggered -= OnMove;
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            // 設定したもの以外は処理しない
            if (context.action.name == "Red")
            {
                GameManager.Instance.IsRed = context.ReadValue<float>(); // 入力値を取得
            }
            else if (context.action.name == "Green")
            {
                GameManager.Instance.IsGreen = context.ReadValue<float>(); // 入力値を取得
            }
            else if (context.action.name == "Blue")
            {
                GameManager.Instance.IsBlue = context.ReadValue<float>(); // 入力値を取得
            }
            else if (context.action.name == "Squat")
            {
                GameManager.Instance.IsSquat = context.ReadValue<float>(); // 入力値を取得
            }
            else
            {
                return;
            }
        }
    }
}