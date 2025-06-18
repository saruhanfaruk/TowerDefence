using System;
using TowerDefence.Enemies;
using UnityEngine;

namespace TowerDefence.Data
{
    /// <summary>
    /// Data container for runner-type enemies.
    /// These enemies avoid attacking towers and aim to reach the goal directly.
    /// </summary>
    [CreateAssetMenu(fileName = "NewRunnerEnemyData", menuName = "TowerDefence/Enemies/Runner Enemy Data")]
    public class RunnerEnemyData : EnemyData
    {
        /// <summary>
        /// Returns the component type to be used when spawning this enemy.
        /// Used at runtime to attach correct behavior.
        /// </summary>
        public override Type GetRuntimeEnemyComponentType()
        {
            return typeof(RunnerEnemy);
        }
    }
}
