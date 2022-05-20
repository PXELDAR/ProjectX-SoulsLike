using UnityEngine;

public class ResetIsInteracting : StateMachineBehaviour
{
    //=================================================================================================

    private const string _isInteractingKey = "isInteracting";

    //=================================================================================================

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(_isInteractingKey, false);
    }

    //=================================================================================================

}
