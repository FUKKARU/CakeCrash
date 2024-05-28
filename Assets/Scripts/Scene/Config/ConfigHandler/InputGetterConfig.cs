using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Main
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputGetterConfig : MonoBehaviour
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
                InputChecker.IsRed = context.ReadValue<float>(); // 入力値を取得
            }
            else if (context.action.name == "Green")
            {
                InputChecker.IsGreen = context.ReadValue<float>(); // 入力値を取得
            }
            else if (context.action.name == "Blue")
            {
                InputChecker.IsBlue = context.ReadValue<float>(); // 入力値を取得
            }
            else if (context.action.name == "Squat")
            {
                InputChecker.IsSquat = context.ReadValue<float>(); // 入力値を取得
            }
            else
            {
                return;
            }
        }
    }
}