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

            // �f���Q�[�g�o�^
            _playerInput.onActionTriggered += OnMove;
        }

        private void OnDisable()
        {
            if (_playerInput == null) return;

            // �f���Q�[�g�o�^����
            _playerInput.onActionTriggered -= OnMove;
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            // �ݒ肵�����̈ȊO�͏������Ȃ�
            if (context.action.name == "Red")
            {
                InputChecker.IsRed = context.ReadValue<float>(); // ���͒l���擾
            }
            else if (context.action.name == "Green")
            {
                InputChecker.IsGreen = context.ReadValue<float>(); // ���͒l���擾
            }
            else if (context.action.name == "Blue")
            {
                InputChecker.IsBlue = context.ReadValue<float>(); // ���͒l���擾
            }
            else if (context.action.name == "Squat")
            {
                InputChecker.IsSquat = context.ReadValue<float>(); // ���͒l���擾
            }
            else
            {
                return;
            }
        }
    }
}