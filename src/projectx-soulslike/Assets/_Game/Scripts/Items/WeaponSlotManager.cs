using UnityEngine;

namespace PXELDAR
{
    public class WeaponSlotManager : MonoBehaviour
    {
        //=================================================================================================

        private WeaponHolderSlot _leftHandSlot;
        private WeaponHolderSlot _rightHandSlot;

        //=================================================================================================

        private void Awake()
        {
            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();

            foreach (WeaponHolderSlot weaponHolderSlot in weaponHolderSlots)
            {
                if (weaponHolderSlot.isLeftHandSlot)
                {
                    _leftHandSlot = weaponHolderSlot;
                }
                else if (weaponHolderSlot.isRightHandSlot)
                {
                    _rightHandSlot = weaponHolderSlot;
                }
            }
        }

        //=================================================================================================

        public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (isLeft)
            {
                _leftHandSlot.LoadWeaponModel(weaponItem);
            }
            else
            {
                _rightHandSlot.LoadWeaponModel(weaponItem);
            }
        }

        //=================================================================================================

    }
}
