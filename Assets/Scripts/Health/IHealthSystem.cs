using UnityEngine;

namespace TowerDefence.Health
{
    /// <summary>
    /// Defines a contract for health-based logic used by enemies or towers.
    /// </summary>
    public interface IHealthSystem
    {
        void Initialize(Transform healthParent, float maxHealth);
        void TakeDamage(float amount);
        void Heal(float amount);

        bool IsDead { get; }
        float CurrentHealth { get; }
        float MaxHealth { get; }
    }
}
