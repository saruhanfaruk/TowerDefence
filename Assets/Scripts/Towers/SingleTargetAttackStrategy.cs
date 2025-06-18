using System.Collections.Generic;
using TowerDefence.Data;
using TowerDefence.Enemies;
using UnityEngine;

namespace TowerDefence.Towers
{
    /// <summary>
    /// A tower attack strategy that targets and fires at a single enemy.
    /// </summary>
    public class SingleTargetAttackStrategy : ITowerAttackStrategy
    {
        #region Private Fields

        private TowerBase tower;
        private TowerData towerData;
        private IEnemy currentTarget;
        private float lastFireTime;

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes the strategy with the associated tower and its data.
        /// </summary>
        public void Initialize(TowerBase tower, TowerData towerData)
        {
            this.tower = tower;
            this.towerData = towerData;
        }

        #endregion

        #region Tick Logic

        /// <summary>
        /// Called every frame to check and attack a single target.
        /// </summary>
        public void Tick()
        {
            // If there is no target, target is dead, or something changed (e.g. new attacker exists), select a new one
            if (currentTarget == null || currentTarget.IsDead || tower.HasAttackers())
            {
                currentTarget = tower.SelectTarget();
            }

            if (currentTarget != null && !currentTarget.IsDead)
            {
                float distance = Vector3.Distance(tower.transform.position, currentTarget.Transform.position);

                // If within range and cooldown passed, fire
                if (distance <= towerData.Range && Time.time - lastFireTime >= towerData.FireRate)
                {
                    tower.FireAt(currentTarget);
                    lastFireTime = Time.time;
                }
                // If out of range, clear current target
                else if (distance > towerData.Range)
                {
                    currentTarget = null;
                }
            }
        }

        #endregion
    }
}
