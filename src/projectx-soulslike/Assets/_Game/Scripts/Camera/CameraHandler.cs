using UnityEngine;

namespace PXELDAR
{
    [DefaultExecutionOrder(-1)]
    public class CameraHandler : MonoBehaviour
    {
        //=================================================================================================

        public static CameraHandler instance;

        public Transform targetTransform;
        public Transform cameraTransform;
        public Transform cameraPivotTransform;
        public float lookSpeed = 0.1f;
        public float followSpeed = 0.1f;
        public float pivotSpeed = 0.03f;
        public float minimumPivot = -35;
        public float maximumPivot = 35;
        public float cameraSphereRadius = 0.2f;
        public float cameraCollisionOffset = 0.2f;
        public float minimumCollisionOffset = 0.2f;

        private Transform _myTransform;
        private LayerMask _ignoreLayers;
        private Vector3 _cameraTransformPosition;
        private Vector3 _cameraFollowVelocity;

        private float _targetPosition;
        private float _defaultPosition;
        private float _lookAngle;
        private float _pivotAngle;


        //=================================================================================================

        private void Awake()
        {
            instance = this;

            _myTransform = transform;
            _defaultPosition = cameraTransform.localPosition.z;
            _ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
        }

        //=================================================================================================

        public void FollowTarget(float delta)
        {
            // Vector3 targetPosition = Vector3.Lerp(transform.position, targetTransform.position, delta / followSpeed);
            Vector3 targetPosition =
                Vector3.SmoothDamp(_myTransform.position, targetTransform.position, ref _cameraFollowVelocity, delta / followSpeed);

            _myTransform.position = targetPosition;

            HandleCameraCollisions(delta);
        }

        //=================================================================================================

        public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
        {
            _lookAngle += (mouseXInput * lookSpeed) / delta;
            _pivotAngle -= (mouseYInput * pivotSpeed) / delta;
            _pivotAngle = Mathf.Clamp(_pivotAngle, minimumPivot, maximumPivot);

            Vector3 rotation = Vector3.zero;
            rotation.y = _lookAngle;
            Quaternion targetRotation = Quaternion.Euler(rotation);
            _myTransform.rotation = targetRotation;

            rotation = Vector3.zero;
            rotation.x = _pivotAngle;

            targetRotation = Quaternion.Euler(rotation);
            cameraPivotTransform.localRotation = targetRotation;
        }

        //=================================================================================================

        private void HandleCameraCollisions(float delta)
        {
            _targetPosition = _defaultPosition;
            RaycastHit hit;
            Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
            direction.Normalize();

            if (Physics.SphereCast(cameraPivotTransform.position, cameraSphereRadius, direction, out hit, Mathf.Abs(_targetPosition), _ignoreLayers))
            {
                float distance = Vector3.Distance(cameraPivotTransform.position, hit.point);
                _targetPosition = -(distance - cameraCollisionOffset);
            }

            if (Mathf.Abs(_targetPosition) < minimumCollisionOffset)
            {
                _targetPosition = -minimumCollisionOffset;
            }

            _cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, _targetPosition, delta / 0.2f);
            cameraTransform.localPosition = _cameraTransformPosition;
        }

        //=================================================================================================

    }
}


