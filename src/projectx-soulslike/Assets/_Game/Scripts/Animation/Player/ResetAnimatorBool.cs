using UnityEngine;

namespace PXELDAR
{
    public class ResetAnimatorBool : StateMachineBehaviour
    {
        //=================================================================================================

        public string targetBool;
        public bool status;

        //=================================================================================================

        override public void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
        {
            animator.SetBool(targetBool, status);
        }

        //=================================================================================================

    }
}