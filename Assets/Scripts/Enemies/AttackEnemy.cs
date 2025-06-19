using System;
using TowerDefence.Data;
using TowerDefence.Projectile;
using TowerDefence.Towers;
using UnityEngine;
using Zenject;

namespace TowerDefence.Enemies
{
    /// <summary>
    /// Enemy type that attacks towers when in range instead of moving directly to the goal.
    /// </summary>
    public class AttackEnemy : EnemyBase
    {
        #region Fields
        [SerializeField] private LayerMask towerLayerMask;
        private AttackEnemyData attackData;
        private float lastAttackTime;
        private TowerBase currentTarget;

        [Inject] private ProjectilePool projectilePool;

        #endregion

        #region Initialization

        /// <summary>
        /// Called by the spawner to initialize the enemy with data and target information.
        /// </summary>
        public override void Initialize(EnemyData data, Vector3 spawnPosition, Vector3 targetPos, Action onEnemyRemoved)
        {
            base.Initialize(data, spawnPosition, targetPos, onEnemyRemoved);
            attackData = (AttackEnemyData)data;
            MoveToTarget();
        }

        #endregion

        #region Unity Methods

        protected override void Update()
        {
            base.Update();

            if (currentTarget == null)
            {
                ScanForTowers();
                return;
            }

            float distance = Vector3.Distance(transform.position, currentTarget.transform.position);
            if (distance <= attackData.attackRange)
            {
                StopMovement();
                Attack();
            }
        }

        private void OnDrawGizmos()
        {
            if (attackData == null)
                return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackData.attackRange);
        }

        #endregion

        #region Targeting and Attacking

        /// <summary>
        /// Looks for nearby towers using OverlapSphere.
        /// </summary>
        private void ScanForTowers()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, attackData.attackRange, towerLayerMask);

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
        /// Sends a projectile to the current target if attack cooldown allows it.
        /// </summary>
        private void Attack()
        {
            if (Time.time - lastAttackTime < attackData.attackRate) return;

            if (currentTarget == null || currentTarget.IsDead)
            {
                ResumeMovement();
                currentTarget = null;
                return;
            }

            currentTarget.RegisterAttacker(this);

            var projectile = projectilePool.Get();
            projectile.Initialize(transform.position);
            projectile.GoToTarget(
                projectilePool,
                () => currentTarget.TakeDamage(attackData.attackDamage),
                OnProjectileHit,
                currentTarget.hitPoint,
                attackData.projectileColor
            );

            lastAttackTime = Time.time;
        }

        /// <summary>
        /// Called when the projectile completes its path.
        /// </summary>
        private void OnProjectileHit()
        {
            if (this == null || gameObject == null || !this.isActiveAndEnabled)
                return;

            ResumeMovement();
        }

        #endregion

        #region Movement Control

        private void StopMovement()
        {
            if (agent.isActiveAndEnabled)
            {
                agent.isStopped = true;
                canAgentDestination = false;
            }
        }

        private void ResumeMovement()
        {
            if (agent == null || !agent.isActiveAndEnabled || !agent.isOnNavMesh)
                return;

            agent.isStopped = false;
            canAgentDestination = true;
        }

        #endregion
    }
}
