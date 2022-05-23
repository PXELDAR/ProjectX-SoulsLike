using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PXELDAR
{
    public class PlayerAttacker : MonoBehaviour
    {
        //=================================================================================================

        private AnimatorHandler _animatorHandler;

        //=================================================================================================

        private void Awake()
        {
            _animatorHandler = GetComponentInChildren<AnimatorHandler>();
        }

        //=================================================================================================

        public void HandleLightAttack(WeaponItem weapon)
        {
            _animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_1, true);
        }

        //=================================================================================================

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            _animatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true);
        }

        //=================================================================================================

    }
}
