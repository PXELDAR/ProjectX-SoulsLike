using UnityEngine;

namespace PXELDAR
{
    public class PlayerLocomotion : MonoBehaviour
    {
        //=================================================================================================

        public Rigidbody rigidBody;
        public GameObject normalCamera;

        [HideInInspector] public Transform myTransform;
        [HideInInspector] public AnimatorHandler animatorHandler;

        private InputHandler _inputHandler;
        private PlayerManager _playerManager;
        private Transform _cameraObject;
        private Vector3 _moveDirection;


        [Header("MOVEMENT STATS")]
        [SerializeField] private float _movementSpeed = 5;
        [SerializeField] private float _rotationSpeed = 10;
        [SerializeField] private float _sprintSpeed = 7;

        //MOVEMENT
        private Vector3 _normalVector;
        private Vector3 _targetPosition;

        //=================================================================================================

        private void Start()
        {
            _playerManager = GetComponent<PlayerManager>();
            rigidBody = GetComponent<Rigidbody>();
            _inputHandler = GetComponent<InputHandler>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            _cameraObject = Camera.main.transform;
            myTransform = transform;
            animatorHandler.Initialize();
        }

        //=================================================================================================

        //MOVEMENT
        private void HandleRotation(float delta)
        {
            Vector3 targetDirection = Vector3.zero;
            float moveOverride = _inputHandler.moveAmount;

            targetDirection = _cameraObject.forward * _inputHandler.vertical;
            targetDirection += _cameraObject.right * _inputHandler.horizontal;

            targetDirection.Normalize();
            targetDirection.y = 0;

            if (targetDirection == Vector3.zero)
            {
                targetDirection = myTransform.forward;
            }

            float rotationSpeed = _rotationSpeed;

            Quaternion tr = Quaternion.LookRotation(targetDirection);
            Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rotationSpeed * delta);

            myTransform.rotation = targetRotation;
        }

        //=================================================================================================

        public void HandleMovement(float delta)
        {
            if (_inputHandler.rollFlag) return;

            _moveDirection = _cameraObject.forward * _inputHandler.vertical;
            _moveDirection += _cameraObject.right * _inputHandler.horizontal;
            _moveDirection.Normalize();
            _moveDirection.y = 0;

            float speed = _movementSpeed;

            if (_inputHandler.sprintFlag)
            {
                speed = _sprintSpeed;
                _playerManager.isSprinting = true;
                _moveDirection *= speed;
            }
            else
            {
                _moveDirection *= speed;
            }

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(_moveDirection, _normalVector);
            rigidBody.velocity = projectedVelocity;

            animatorHandler.UpdateAnimatorValues(_inputHandler.moveAmount, 0, _playerManager.isSprinting);

            if (animatorHandler.canRotate)
            {
                HandleRotation(delta);
            }
        }

        //=================================================================================================

        public void HandleRollingAndSprinting(float delta)
        {
            if (animatorHandler.animator.GetBool("isInteracting")) return;

            if (_inputHandler.rollFlag)
            {
                _moveDirection = _cameraObject.forward * _inputHandler.vertical;
                _moveDirection += _cameraObject.right * _inputHandler.horizontal;

                if (_inputHandler.moveAmount > 0)
                {
                    animatorHandler.PlayTargetAnimation("Roll", true);
                    _moveDirection.y = 0;
                    Quaternion rollRotation = Quaternion.LookRotation(_moveDirection);
                    myTransform.rotation = rollRotation;
                }
                else
                {
                    // animatorHandler.PlayTargetAnimation("Backstep", true);
                }
            }
        }

        //=================================================================================================

    }
}