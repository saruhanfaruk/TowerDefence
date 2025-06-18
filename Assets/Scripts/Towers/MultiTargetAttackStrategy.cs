using TowerDefence.Data;
using UnityEngine;

namespace TowerDefence.Towers
{
    /// <summary>
    /// A tower attack strategy that targets and fires at all enemies within range.
    /// </summary>
    public class MultiTargetAttackStrategy : ITowerAttackStrategy
    {
        private TowerBase tower;
        private TowerData towerData;
        private float lastFireTime;

        /// <summary>
        /// Initializes the strategy with tower reference and data.
        /// </summary>
        public void Initialize(TowerBase tower, TowerData towerData)
        {
            this.tower = tower;
            this.towerData = towerData;
        }

        /// <summary>
        /// Called every frame. If cooldown allows, fire at all enemies in range.
        /// </summary>
        public void Tick()
        {
            // Check if enough time has passed since last fire
            if (Time.time - lastFireTime < towerData.FireRate)
                return;

            // Get all enemies in range
            var enemies = tower.GetEnemiesInRange();

            // Fire at each enemy
            foreach (var enemy in enemies)
            {
                tower.FireAt(enemy);
            }

            // Reset fire timer
            lastFireTime = Time.time;
        }
    }
}
