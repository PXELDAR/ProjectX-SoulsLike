using UnityEngine;

namespace PXELDAR
{
    public class PlayerLocomotion : MonoBehaviour
    {
        //=================================================================================================

        private Transform _cameraObject;
        private InputHandler _inputHandler;
        private Vector3 _moveDirection;

        [HideInInspector] public Transform myTransform;
        [HideInInspector] public AnimatorHandler animatorHandler;

        public Rigidbody rigidbody;
        public GameObject normalCamera;


        [Header("STATS")]
        [SerializeField] private float _movementSpeed = 5;
        [SerializeField] private float _rotationSpeed = 10;

        //MOVEMENT
        private Vector3 _normalVector;
        private Vector3 _targetPosition;

        //=================================================================================================

        private void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            _inputHandler = GetComponent<InputHandler>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            _cameraObject = Camera.main.transform;
            myTransform = transform;
            animatorHandler.Initialize();
        }

        //=================================================================================================

        private void Update()
        {
            float delta = Time.deltaTime;

            _inputHandler.TickInput(delta);

            _moveDirection = _cameraObject.forward * _inputHandler.vertical;
            _moveDirection += _cameraObject.right * _inputHandler.horizontal;
            _moveDirection.Normalize();

            float speed = _movementSpeed;
            _moveDirection *= speed;

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(_moveDirection, _normalVector);
            rigidbody.velocity = projectedVelocity;

            animatorHandler.UpdateAnimatorValues(_inputHandler.moveAmount, 0);

            if (animatorHandler.canRotate)
            {
                HandleRotation(delta);
            }
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

    }
}