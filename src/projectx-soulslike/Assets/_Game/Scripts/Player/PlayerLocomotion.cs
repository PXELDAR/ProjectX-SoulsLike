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
        public Vector3 moveDirection;


        [Header("GROUND & AIR DETECTION STATS")]
        public float inAirTimer;
        [SerializeField] private float _groundDetectionRayStartPoint = 0.5f;
        [SerializeField] private float _minimumDistanceNeededToFall = 1f;
        [SerializeField] private float _groundDirectionRayDistance = 0.2f;
        private LayerMask _ignoreForGroundCheck;


        [Header("MOVEMENT STATS")]
        [SerializeField] private float _movementSpeed = 5;
        [SerializeField] private float _walkSpeed = 2;
        [SerializeField] private float _rotationSpeed = 10;
        [SerializeField] private float _sprintSpeed = 7;
        [SerializeField] private float _fallSpeed = 45;

        private Vector3 _normalVector;
        private Vector3 _targetPosition;

        private const string _fallAnimationKey = "Fall";
        private const string _locomotionAnimationKey = "Locomotion";
        private const string _landAnimationKey = "Land2";
        private const string _rollAnimationKey = "Roll";
        private const string _backstepKey = "Backstep";
        private const string _isInteractingKey = "isInteracting";


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

            _playerManager.isGrounded = true;
            _ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
        }

        //=================================================================================================

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

            if (_playerManager.isInteracting) return;

            moveDirection = _cameraObject.forward * _inputHandler.vertical;
            moveDirection += _cameraObject.right * _inputHandler.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;

            float speed = _movementSpeed;

            if (_inputHandler.sprintFlag && _inputHandler.moveAmount > 0.5f)
            {
                speed = _sprintSpeed;
                _playerManager.isSprinting = true;
                moveDirection *= speed;
            }
            else
            {
                if (_inputHandler.moveAmount < 0.5f)
                {
                    moveDirection *= _walkSpeed;
                    _playerManager.isSprinting = false;
                }
                else
                {
                    moveDirection *= speed;
                    _playerManager.isSprinting = false;
                }
            }

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, _normalVector);
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
            if (animatorHandler.animator.GetBool(_isInteractingKey)) return;

            if (_inputHandler.rollFlag)
            {
                moveDirection = _cameraObject.forward * _inputHandler.vertical;
                moveDirection += _cameraObject.right * _inputHandler.horizontal;

                if (_inputHandler.moveAmount > 0)
                {
                    animatorHandler.PlayTargetAnimation(_rollAnimationKey, true);
                    moveDirection.y = 0;
                    Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                    myTransform.rotation = rollRotation;
                }
                else
                {
                    animatorHandler.PlayTargetAnimation(_backstepKey, true);
                }
            }
        }

        //=================================================================================================

        public void HandleFalling(float delta, Vector3 moveDirection)
        {
            _playerManager.isGrounded = false;
            RaycastHit hit;
            Vector3 origin = myTransform.position;
            origin.y += _groundDetectionRayStartPoint;

            if (Physics.Raycast(origin, myTransform.forward, out hit, 0.4f))
            {
                moveDirection = Vector3.zero;
            }

            if (_playerManager.isInAir)
            {
                rigidBody.AddForce(-Vector3.up * _fallSpeed);
                rigidBody.AddForce(moveDirection * _fallSpeed / 10f);
            }

            Vector3 direction = moveDirection;
            direction.Normalize();
            origin = origin + direction * _groundDirectionRayDistance;

            _targetPosition = myTransform.position;

            Debug.DrawRay(origin, -Vector3.up * _minimumDistanceNeededToFall, Color.red, 0.1f, false);

            if (Physics.Raycast(origin, -Vector3.up, out hit, _minimumDistanceNeededToFall, ~_ignoreForGroundCheck))
            {
                _normalVector = hit.normal;
                Vector3 targetPosition = hit.point;
                _playerManager.isGrounded = true;
                _targetPosition.y = targetPosition.y;

                if (_playerManager.isInAir)
                {
                    if (inAirTimer > 0.5f)
                    {
                        animatorHandler.PlayTargetAnimation(_landAnimationKey, true);
                        inAirTimer = 0;
                    }
                    else
                    {
                        animatorHandler.PlayTargetAnimation(_locomotionAnimationKey, false);
                        inAirTimer = 0;
                    }

                    _playerManager.isInAir = false;
                }
            }
            else
            {
                if (_playerManager.isGrounded)
                {
                    _playerManager.isGrounded = false;
                }

                if (!_playerManager.isInAir)
                {
                    if (!_playerManager.isInteracting)
                    {
                        animatorHandler.PlayTargetAnimation(_fallAnimationKey, true);
                    }

                    Vector3 velocity = rigidBody.velocity;
                    velocity.Normalize();
                    rigidBody.velocity = velocity * (_movementSpeed / 2);
                    _playerManager.isInAir = true;
                }
            }

            if (_playerManager.isInteracting || _inputHandler.moveAmount > 0)
            {
                myTransform.position = Vector3.Lerp(myTransform.position, _targetPosition, Time.deltaTime / 0.1f);
            }
            else
            {
                myTransform.position = _targetPosition;
            }
        }

        //=================================================================================================

    }
}