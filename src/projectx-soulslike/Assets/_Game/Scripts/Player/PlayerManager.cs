using UnityEngine;


namespace PXELDAR
{
    public class PlayerManager : MonoBehaviour
    {
        //=================================================================================================

        private InputHandler _inputHandler;
        private Animator _animator;

        private const string _isInteractingKey = "isInteracting";

        //=================================================================================================

        private void Start()
        {
            _inputHandler = GetComponent<InputHandler>();
            _animator = GetComponentInChildren<Animator>();
        }

        //=================================================================================================

        private void Update()
        {
            _inputHandler.isInteracting = _animator.GetBool(_isInteractingKey);
            _inputHandler.rollFlag = false;
            _inputHandler.sprintFlag = false;
        }

        //=================================================================================================

    }
}