using UnityEngine;
using TowerDefence.Data;
using System;

namespace TowerDefence.Enemies
{
    /// <summary>
    /// Interface defining the common behaviors for all enemy types.
    /// </summary>
    public interface IEnemy
    {
        #region Properties

        /// <summary>
        /// Data reference holding stats and prefab info.
        /// </summary>
        EnemyData EnemyData { get; }

        /// <summary>
        /// Cached Transform of the enemy object.
        /// </summary>
        Transform Transform { get; }

        /// <summary>
        /// Indicates whether the enemy is dead.
        /// </summary>
        bool IsDead { get; }

        /// <summary>
        /// Returns the current health value.
        /// </summary>
        float CurrentHealth { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the enemy with necessary data and position.
        /// </summary>
        /// <param name="data">Enemy stats</param>
        /// <param name="spawnPosition">Spawn point in world</param>
        /// <param name="targetPosition">Target to move toward</param>
        /// <param name="onEnemyRemoved">Callback when enemy dies</param>
        void Initialize(EnemyData data, Vector3 spawnPosition, Vector3 targetPosition, Action onEnemyRemoved);

        /// <summary>
        /// Applies damage to the enemy.
        /// </summary>
        /// <param name="damage">Amount of damage</param>
        void TakeDamage(float damage);

        #endregion
    }

}
