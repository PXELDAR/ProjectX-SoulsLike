using UnityEngine;

namespace PXELDAR
{
    public class AnimatorHandler : MonoBehaviour
    {
        //=================================================================================================

        public Animator animator;
        public bool canRotate;

        private PlayerManager _playerManager;
        private InputHandler _inputHandler;
        private PlayerLocomotion _playerLocomotion;

        private int _vertical;
        private int _horizontal;

        private const string _verticalKey = "Vertical";
        private const string _horizontalKey = "Horizontal";

        //=================================================================================================

        public void Initialize()
        {
            _playerManager = GetComponentInChildren<PlayerManager>();
            animator = GetComponent<Animator>();
            _inputHandler = GetComponentInParent<InputHandler>();
            _playerLocomotion = GetComponentInParent<PlayerLocomotion>();
            _vertical = Animator.StringToHash(_verticalKey);
            _horizontal = Animator.StringToHash(_horizontalKey);
        }

        //=================================================================================================

        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
        {
            #region Vertical
            float v = 0;

            if (verticalMovement > 0 && verticalMovement < 0.55f)
            {
                v = 0.5f;
            }
            else if (verticalMovement > 0.55f)
            {
                v = 1;
            }
            else if (verticalMovement < 0 && verticalMovement > -0.55f)
            {
                v = -0.5f;
            }
            else if (verticalMovement < -0.55f)
            {
                v = -1;
            }
            else
            {
                v = 0;
            }
            #endregion

            #region Horizontal
            float h = 0;

            if (horizontalMovement > 0 && horizontalMovement < 0.55f)
            {
                h = 0.5f;
            }
            else if (horizontalMovement > 0.55f)
            {
                h = 1;
            }
            else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
            {
                h = -0.5f;
            }
            else if (horizontalMovement < -0.55f)
            {
                h = -1;
            }
            else
            {
                h = 0;
            }
            #endregion

            if (isSprinting)
            {
                v = 2;
                h = horizontalMovement;
            }

            animator.SetFloat(_vertical, v, 0.1f, Time.deltaTime);
            animator.SetFloat(_horizontal, h, 0.1f, Time.deltaTime);
        }

        //=================================================================================================

        public void PlayTargetAnimation(string targetAnim, bool isInteracting)
        {
            animator.applyRootMotion = isInteracting;
            animator.SetBool("isInteracting", isInteracting);
            animator.CrossFade(targetAnim, 0.2f);
        }

        //=================================================================================================

        public void CanRotate()
        {
            canRotate = true;
        }

        //=================================================================================================

        public void StopRotation()
        {
            canRotate = false;
        }

        //=================================================================================================

        private void OnAnimatorMove()
        {
            if (!_playerManager.isInteracting) return;

            float delta = Time.deltaTime;
            _playerLocomotion.rigidBody.drag = 0;
            Vector3 deltaPosition = animator.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            _playerLocomotion.rigidBody.velocity = velocity;
        }

        //=================================================================================================

    }
}