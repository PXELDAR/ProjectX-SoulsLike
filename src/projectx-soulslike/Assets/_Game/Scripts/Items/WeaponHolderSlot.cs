using UnityEngine;

namespace PXELDAR
{
    public class WeaponHolderSlot : MonoBehaviour
    {
        //=================================================================================================

        public Transform parentOverride;
        public bool isLeftHandSlot;
        public bool isRightHandSlot;
        public GameObject currentWeaponModel;

        //=================================================================================================

        public void LoadWeaponModel(WeaponItem weaponItem)
        {
            UnloadWeaponAndDestroy();

            if (!weaponItem)
            {
                UnloadWeapon();
                return;
            }

            GameObject model = Instantiate(weaponItem.modelPrefab);

            if (model)
            {
                if (parentOverride)
                {
                    model.transform.parent = parentOverride;
                }
                else
                {
                    model.transform.parent = transform;
                }

                model.transform.localPosition = Vector3.zero;
                model.transform.localRotation = Quaternion.identity;
                model.transform.localScale = Vector3.one;
            }

            currentWeaponModel = model;
        }

        //=================================================================================================

        public void UnloadWeapon()
        {
            if (currentWeaponModel)
            {
                currentWeaponModel.SetActive(false);
            }
        }

        //=================================================================================================

        public void UnloadWeaponAndDestroy()
        {
            if (currentWeaponModel)
            {
                Destroy(currentWeaponModel);
            }
        }

        //=================================================================================================

    }
}
