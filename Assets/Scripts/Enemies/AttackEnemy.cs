using System;
using TowerDefence.Data;
using TowerDefence.Projectile;
using TowerDefence.Towers;
using UnityEngine;
using Zenject;

namespace TowerDefence.Enemies
{
    /// <summary>
    /// Enemy type that stops when a tower is within range and attacks it instead of continuing to the main objective.
    /// </summary>
    public class AttackEnemy : EnemyBase
    {
        #region Fields

        private AttackEnemyData attackData;
        private float lastAttackTime;
        private TowerBase currentTarget;

        [Inject] private ProjectilePool projectilePool;

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes the enemy with data and begins movement toward the target.
        /// </summary>
        public override void Initialize(EnemyData data, Vector3 spawnPosition, Vector3 targetPos, Action onEnemyRemoved)
        {
            base.Initialize(data, spawnPosition, targetPos, onEnemyRemoved);
            attackData = (AttackEnemyData)data;
            MoveToTarget();
        }

        #endregion

        #region Unity Callbacks

        protected override void Update()
        {
            base.Update();

            if (currentTarget == null)
            {
                ScanForTowers();
            }
            else
            {
                float distance = Vector3.Distance(transform.position, currentTarget.transform.position);
                if (distance <= attackData.attackRange)
                {
                    agent.isStopped = true;
                    canAgentDestination = false;
                    Attack();
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, attackData.attackRange);
        }

        #endregion

        #region Targeting & Attacking

        /// <summary>
        /// Scans for nearby towers within attack range using OverlapSphere.
        /// </summary>
        private void ScanForTowers()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, attackData.attackRange);
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent(out TowerBase tower))
                {
                    currentTarget = tower;
                    break;
                }
            }
        }

        /// <summary>
        /// Attacks the currently targeted tower using projectiles.
        /// </summary>
        private void Attack()
        {
            if (Time.time - lastAttackTime >= attackData.attackRate)
            {
                if (currentTarget == null || currentTarget.IsDead)
                {
                    currentTarget = null;
                    agent.isStopped = false;
                    return;
                }

                currentTarget.RegisterAttacker(this);

                var projectile = projectilePool.Get();
                projectile.Initialize(transform.position);
                projectile.GoToTarget(
                    projectilePool,
                    () => currentTarget.TakeDamage(attackData.attackDamage),
                    () => CallBackHit(),
                    currentTarget.hitPoint,
                    attackData.projectileColor
                );

                lastAttackTime = Time.time;
            }
        }

        /// <summary>
        /// Callback invoked after the projectile hits the target. Resumes movement.
        /// </summary>
        private void CallBackHit()
        {
            if (this == null) return;

            if (agent.isActiveAndEnabled && agent.isOnNavMesh)
            {
                agent.isStopped = false;
            }
        }

        #endregion
    }
}
