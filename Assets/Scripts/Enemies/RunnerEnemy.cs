using System;
using TowerDefence.Data;
using UnityEngine;

namespace TowerDefence.Enemies
{
    /// <summary>
    /// Represents a runner-type enemy that moves directly to the target.
    /// Does not attack or stop — only navigates forward.
    /// </summary>
    public class RunnerEnemy : EnemyBase
    {
        #region Initialization

        /// <summary>
        /// Initializes the runner enemy and begins movement toward the target.
        /// </summary>
        /// <param name="data">Enemy configuration data</param>
        /// <param name="spawnPosition">Where the enemy spawns</param>
        /// <param name="targetPos">Where the enemy should go</param>
        /// <param name="onEnemyRemoved">Callback when the enemy dies</param>
        public override void Initialize(EnemyData data, Vector3 spawnPosition, Vector3 targetPos, Action onEnemyRemoved)
        {
            base.Initialize(data, spawnPosition, targetPos, onEnemyRemoved);
            MoveToTarget(); 
        }

        #endregion
    }
}
