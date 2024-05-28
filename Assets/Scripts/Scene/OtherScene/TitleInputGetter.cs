using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Main
{
    [RequireComponent(typeof(PlayerInput))]
    public class TitleInputGetter : MonoBehaviour
    {
        public float IsRed { get; private set; } = 0f;
        public float IsGreen { get; private set; } = 0f;
        public float IsBlue { get; private set; } = 0f;
        public float IsSquat { get; private set; } = 0f;

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
                IsRed = context.ReadValue<float>(); // ���͒l���擾
            }
            else if (context.action.name == "Green")
            {
                IsGreen = context.ReadValue<float>(); // ���͒l���擾
            }
            else if (context.action.name == "Blue")
            {
                IsBlue = context.ReadValue<float>(); // ���͒l���擾
            }
            else if (context.action.name == "Squat")
            {
                IsSquat = context.ReadValue<float>(); // ���͒l���擾
            }
            else
            {
                return;
            }
        }
    }
}