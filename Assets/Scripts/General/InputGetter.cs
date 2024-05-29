using UnityEngine;
using UnityEngine.InputSystem;

namespace IA
{
    public class InputGetter : MonoBehaviour
    {
        #region インスタンスの管理、コールバックとのリンク、staticかつシングルトンにする
        IA _inputs;

        public static InputGetter Instance { get; set; } = null;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            _inputs = new IA();

            Link(true);
        }

        void OnDestroy()
        {
            Link(false);

            _inputs.Dispose();
        }

        void OnEnable()
        {
            _inputs.Enable();
        }

        void OnDisable()
        {
            _inputs.Disable();
        }
        #endregion

        #region 変数宣言
        public bool IsRed { get; private set; } = false;
        public bool IsBlue { get; private set; } = false;
        public bool IsGreen { get; private set; } = false;
        public bool IsSquat { get; private set; } = false;
        #endregion

        #region【LateUpdate】毎フレームの最後で、フラグを初期化する
        void LateUpdate()
        {
            if (IsRed) IsRed = false;
            if (IsBlue) IsBlue = false;
            if (IsGreen) IsGreen = false;
        }
        #endregion

        #region コールバックとのリンクの詳細
        void Link(bool isLink)
        {
            // インスタンス名.Map名.Action名.コールバック名
            if (isLink)
            {
                _inputs.General.Red.performed += OnRed;

                _inputs.General.Blue.performed += OnBlue;

                _inputs.General.Green.performed += OnGreen;

                _inputs.General.Squat.performed += OnSquatDown;
                _inputs.General.Squat.canceled += OnSquatUp;
            }
            else
            {
                _inputs.General.Red.performed -= OnRed;

                _inputs.General.Blue.performed -= OnBlue;

                _inputs.General.Green.performed -= OnGreen;

                _inputs.General.Squat.performed -= OnSquatDown;
                _inputs.General.Squat.canceled -= OnSquatUp;
            }
        }
        #endregion

        #region 処理の詳細
        void OnRed(InputAction.CallbackContext context)
        {
            IsRed = true;
        }

        void OnBlue(InputAction.CallbackContext context)
        {
            IsBlue = true;
        }

        void OnGreen(InputAction.CallbackContext context)
        {
            IsGreen = true;
        }

        void OnSquatDown(InputAction.CallbackContext context)
        {
            IsSquat = true;
        }
        void OnSquatUp(InputAction.CallbackContext context)
        {
            IsSquat = false;
        }
        #endregion
    }
}