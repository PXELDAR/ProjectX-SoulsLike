using UnityEngine;

namespace PXELDAR
{
    public class InputHandler : MonoBehaviour
    {
        //=================================================================================================

        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;
        public float rollInputTimer;

        public bool b_Input;
        public bool rb_Input;
        public bool rt_Input;
        public bool rollFlag;
        public bool sprintFlag;

        private PlayerControls _inputActions;
        private PlayerAttacker _playerAttacker;
        private PlayerInventory _playerInventory;

        private Vector2 _movementInput;
        private Vector2 _cameraInput;

        //=================================================================================================

        private void Awake()
        {
            _playerAttacker = GetComponent<PlayerAttacker>();
            _playerInventory = GetComponent<PlayerInventory>();
        }

        //=================================================================================================

        private void OnEnable()
        {
            if (_inputActions == null)
            {
                _inputActions = new PlayerControls();
                _inputActions.PlayerMovement.Movement.performed += _inputActions => _movementInput = _inputActions.ReadValue<Vector2>();
                _inputActions.PlayerMovement.Camera.performed += i => _cameraInput = i.ReadValue<Vector2>();
            }

            _inputActions.Enable();
        }

        private void OnDisable()
        {
            _inputActions.Disable();
        }

        //=================================================================================================

        public void TickInput(float delta)
        {
            MoveInput(delta);
            HandleRollInput(delta);
            HandleAttackInput(delta);
        }

        //=================================================================================================

        private void MoveInput(float delta)
        {
            horizontal = _movementInput.x;
            vertical = _movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = _cameraInput.x;
            mouseY = _cameraInput.y;
        }

        //=================================================================================================

        private void HandleRollInput(float delta)
        {
            b_Input = _inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Performed;

            if (b_Input)
            {
                rollInputTimer += delta;
                sprintFlag = true;
            }
            else
            {
                if (rollInputTimer > 0 && rollInputTimer < 0.5f)
                {
                    sprintFlag = false;
                    rollFlag = true;
                }

                rollInputTimer = 0;
            }
        }

        //=================================================================================================

        private void HandleAttackInput(float delta)
        {
            _inputActions.PlayerActions.RB.performed += i => rb_Input = true;
            _inputActions.PlayerActions.RT.performed += i => rt_Input = true;

            if (rb_Input)
            {
                _playerAttacker.HandleLightAttack(_playerInventory.rightWeapon);
            }

            if (rt_Input)
            {
                _playerAttacker.HandleHeavyAttack(_playerInventory.rightWeapon);
            }
        }

        //=================================================================================================

    }
}