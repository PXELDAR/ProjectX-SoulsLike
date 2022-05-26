using UnityEngine;

namespace PXELDAR
{
    public class DamagePlayer : MonoBehaviour
    {
        //=================================================================================================

        public int damage = 25;

        //=================================================================================================

        private void OnTriggerEnter(Collider other)
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();

            if (playerStats)
            {
                playerStats.TakeDamage(damage);
            }
        }

        //=================================================================================================
    }
}