using UnityEngine;
using Zenject;

namespace TowerDefence.Health
{
    /// <summary>
    /// Manages health logic including damage, healing, and death handling.
    /// Displays a health bar above the unit.
    /// </summary>
    public class HealthSystem : IHealthSystem
    {
        #region Fields and Properties

        private float _currentHealth;
        private float _maxHealth;

        private HealthBar healthBar;

        public float CurrentHealth => _currentHealth;
        public float MaxHealth => _maxHealth;
        public bool IsDead => _currentHealth <= 0f;

        #endregion

        #region Dependency Injection

        [Inject]
        public void Construct(HealthBar.Factory healthBarFactory)
        {
            healthBar = healthBarFactory.Create();
        }

        #endregion

        #region Public Methods

        public void Initialize(Transform healthParent, float maxHealth)
        {
            healthBar.transform.SetParent(healthParent, false);
            healthBar.transform.localPosition = Vector3.zero;

            _maxHealth = maxHealth;
            _currentHealth = maxHealth;

            UpdateUI();
        }

        public void TakeDamage(float amount)
        {
            if (IsDead) return;

            _currentHealth -= amount;

            if (_currentHealth <= 0)
                OnDeath();
            else
                UpdateUI();
        }

        public void Heal(float amount)
        {
            if (IsDead) return;

            _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth);
            UpdateUI();
        }

        #endregion

        #region Private Methods

        private void UpdateUI()
        {
            healthBar?.SetHealth(_currentHealth, _maxHealth);
        }

        private void OnDeath()
        {
            healthBar?.KillDotween();
        }

        #endregion
    }
}
