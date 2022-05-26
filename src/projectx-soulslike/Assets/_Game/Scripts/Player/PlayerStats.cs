using UnityEngine;

namespace PXELDAR
{
    public class PlayerStats : MonoBehaviour
    {
        //=================================================================================================

        public int healthLevel = 10;
        public int maxHealth;
        public int currentHealth;

        public HealthBar healthBar;

        private AnimatorHandler _animatorHandler;
        private const string _takeDamageKey = "TakeDamage";
        private const string _deathKey = "Death";

        //=================================================================================================

        private void Awake()
        {
            _animatorHandler = GetComponentInChildren<AnimatorHandler>();
        }

        //=================================================================================================

        private void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
        }

        //=================================================================================================

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        //=================================================================================================

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;

            healthBar.SetCurrentHealth(currentHealth);

            _animatorHandler.PlayTargetAnimation(_takeDamageKey, true);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                _animatorHandler.PlayTargetAnimation(_deathKey, true);

                //death
            }
        }

        //=================================================================================================

    }
}