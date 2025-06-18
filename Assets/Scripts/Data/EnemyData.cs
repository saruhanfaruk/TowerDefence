using System;
using UnityEngine;
using TowerDefence.Enemies;

namespace TowerDefence.Data
{
    /// <summary>
    /// Base class for all enemy configuration data used in the game.
    /// Derived classes define specialized enemy behavior.
    /// </summary>
    public abstract class EnemyData : ScriptableObject
    {
        [SerializeField] private EnemyType type;
        [SerializeField] private float maxHealth;
        [SerializeField] private float speed;
        [SerializeField] private string prefabAddress;

        public EnemyType Type => type;
        public float MaxHealth => maxHealth;
        public float Speed => speed;
        public string PrefabAddress => prefabAddress;

        /// <summary>
        /// Must return the component type that should be attached or instantiated at runtime.
        /// Used to differentiate behavior (e.g., AttackEnemy vs RunnerEnemy).
        /// </summary>
        public abstract Type GetRuntimeEnemyComponentType();
    }
}
