using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PXELDAR
{
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

        private Transform _myTransform;
        private Vector3 _cameraTransformPosition;
        private LayerMask _ignoreLayers;
        private float _defaultPosition;
        private float _lookAngle;
        private float _pivotAngle;
        public float minimumPivot = -35;
        public float maximumPivot = 35;

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
            Vector3 targetPosition = Vector3.Lerp(transform.position, targetTransform.position, delta / followSpeed);
            _myTransform.position = targetPosition;
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

    }
}


