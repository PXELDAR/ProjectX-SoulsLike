using UnityEngine;

namespace PXELDAR
{
    public class PlayerManager : MonoBehaviour
    {
        //=================================================================================================

        [Header("PLAYER FLAGS")]
        public bool isInteracting;
        public bool isSprinting;
        public bool isInAir;
        public bool isGrounded;


        private CameraHandler _cameraHandler;
        private InputHandler _inputHandler;
        private PlayerLocomotion _playerLocomotion;

        private Animator _animator;

        private const string _isInteractingKey = "isInteracting";

        //=================================================================================================

        private void Awake()
        {
            _cameraHandler = CameraHandler.instance;
        }

        //=================================================================================================

        private void Start()
        {
            _inputHandler = GetComponent<InputHandler>();
            _playerLocomotion = GetComponent<PlayerLocomotion>();
            _animator = GetComponentInChildren<Animator>();
        }

        //=================================================================================================

        private void Update()
        {
            float delta = Time.deltaTime;
            isInteracting = _animator.GetBool(_isInteractingKey);

            _inputHandler.TickInput(delta);
            _playerLocomotion.HandleMovement(delta);
            _playerLocomotion.HandleRollingAndSprinting(delta);
            _playerLocomotion.HandleFalling(delta, _playerLocomotion.moveDirection);
        }

        //=================================================================================================

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;

            if (_cameraHandler)
            {
                _cameraHandler.FollowTarget(delta);
                _cameraHandler.HandleCameraRotation(delta, _inputHandler.mouseX, _inputHandler.mouseY);
            }
        }

        //=================================================================================================

        private void LateUpdate()
        {
            _inputHandler.rollFlag = false;
            _inputHandler.sprintFlag = false;
            _inputHandler.rb_Input = false;
            _inputHandler.rt_Input = false;

            if (isInAir)
            {
                _playerLocomotion.inAirTimer = _playerLocomotion.inAirTimer + Time.deltaTime;
            }
        }

        //=================================================================================================

    }
}