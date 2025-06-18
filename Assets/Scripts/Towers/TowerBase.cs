using System;
using System.Collections.Generic;
using DG.Tweening;
using TowerDefence.Core;
using TowerDefence.Data;
using TowerDefence.Enemies;
using TowerDefence.Health;
using TowerDefence.Projectile;
using UnityEngine;
using Zenject;

namespace TowerDefence.Towers
{
    /// <summary>
    /// Handles tower initialization, targeting, firing, taking damage, and integration with the strategy pattern.
    /// </summary>
    public class TowerBase : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private LayerMask layerMaskToAttack; // Which layers this tower can attack
        [SerializeField] private Transform healthPoint;       // Parent transform for health bar
        public Transform hitPoint;                            // Point where projectile starts
        #endregion

        #region Private Fields
        private TowerData towerData;
        private TowerPlacementArea towerPlacementArea;
        private IHealthSystem healthSystem;
        private List<IEnemy> attackers = new();

        private ITowerAttackStrategy attackStrategy;
        #endregion

        #region Injected Dependencies
        [Inject] private ProjectilePool projectilePool;
        [Inject] private MapManager mapManager;
        #endregion

        #region Public Properties
        public bool IsDead => healthSystem.IsDead;
        public Transform Transform => transform;
        #endregion

        #region Initialization
        [Inject]
        public void Construct(IHealthSystem healthSystem)
        {
            this.healthSystem = healthSystem;
        }

        public void Initialize(TowerData towerData, TowerPlacementArea towerPlacementArea)
        {
            this.towerData = towerData;
            this.towerPlacementArea = towerPlacementArea;

            // Initialize health system
            healthSystem.Initialize(healthPoint, towerData.Health);

            // Select appropriate attack strategy
            attackStrategy = towerData.AttackType switch
            {
                TowerAttackType.Single => new SingleTargetAttackStrategy(),
                TowerAttackType.Multi => new MultiTargetAttackStrategy(),
                _ => throw new NotImplementedException()
            };

            attackStrategy.Initialize(this, towerData);
        }
        #endregion

        #region Unity Loop
        private void Update()
        {
            attackStrategy?.Tick();
        }
        #endregion

        #region Attack Logic
        public void FireAt(IEnemy enemy)
        {
            var projectile = projectilePool.Get();
            projectile.Initialize(hitPoint.position);
            projectile.GoToTarget(projectilePool, () => enemy.TakeDamage(towerData.Damage), null, enemy.Transform, towerData.ProjectileColor);
        }

        public List<IEnemy> GetEnemiesInRange()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, towerData.Range, layerMaskToAttack);
            List<IEnemy> enemies = new();

            foreach (var hit in hits)
            {
                if (hit.TryGetComponent(out IEnemy enemy) && !enemy.IsDead)
                    enemies.Add(enemy);
            }

            return enemies;
        }

        public IEnemy SelectTarget()
        {
            CleanDeadAttackers();

            if (attackers.Count > 0)
            {
                IEnemy lowestHealthEnemy = null;
                float lowestHealth = float.MaxValue;

                foreach (var enemy in attackers)
                {
                    if (!enemy.IsDead && enemy.CurrentHealth < lowestHealth)
                    {
                        lowestHealth = enemy.CurrentHealth;
                        lowestHealthEnemy = enemy;
                    }
                }

                if (lowestHealthEnemy != null)
                    return lowestHealthEnemy;
            }

            var inRange = GetEnemiesInRange();
            return inRange.Count > 0 ? inRange[0] : null;
        }
        #endregion

        #region Attacker Tracking
        public void RegisterAttacker(IEnemy enemy)
        {
            if (!attackers.Contains(enemy))
                attackers.Add(enemy);
        }

        private void CleanDeadAttackers()
        {
            attackers.RemoveAll(e => e == null || e.IsDead);
        }

        public bool HasAttackers() => attackers.Count > 0;
        #endregion

        #region Health / Damage
        public void TakeDamage(float amount)
        {
            if (healthSystem.IsDead) return;

            healthSystem.TakeDamage(amount);
            if (healthSystem.IsDead)
            {
                mapManager.TowerCount--;
                Destroy(gameObject);
                towerPlacementArea.IsOccupied = false;
            }
        }
        #endregion

        #region Cleanup
        private void OnDestroy()
        {
            DOTween.Kill(healthSystem);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            if (towerData != null)
                Gizmos.DrawWireSphere(transform.position, towerData.Range);
        }
        #endregion
    }
}
