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

        public bool rollInput;
        public bool rollFlag;
        public bool isInteracting;

        private PlayerControls _inputActions;
        private CameraHandler _cameraHandler;

        private Vector2 _movementInput;
        private Vector2 _cameraInput;

        //=================================================================================================

        private void Awake()
        {
            _cameraHandler = CameraHandler.instance;
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

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;

            if (_cameraHandler)
            {
                _cameraHandler.FollowTarget(delta);
                _cameraHandler.HandleCameraRotation(delta, mouseX, mouseY);
            }
        }

        //=================================================================================================

        public void TickInput(float delta)
        {
            MoveInput(delta);
            HandleRollInput(delta);
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
            rollInput = _inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Performed;

            if (rollInput)
            {
                rollFlag = true;
            }
        }


        //=================================================================================================


    }
}