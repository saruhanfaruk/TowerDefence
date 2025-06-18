using System;
using TowerDefence.Core;
using TowerDefence.Data;
using TowerDefence.Health;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace TowerDefence.Enemies
{
    /// <summary>
    /// Base class for all enemies. Handles movement, health, and death logic.
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class EnemyBase : MonoBehaviour, IEnemy
    {
        #region Fields

        private bool isDead;
        private EnemyData enemyData;
        private Action onEnemyRemoved;

        [SerializeField] public Transform healthParent;

        protected NavMeshAgent agent;
        protected Vector3 targetPosition;
        protected IHealthSystem healthSystem;
        protected bool canAgentDestination = true;

        #endregion

        #region Properties

        public EnemyData EnemyData => enemyData;
        public Transform Transform => transform;
        public bool IsDead => isDead;
        public float CurrentHealth => healthSystem.CurrentHealth;

        #endregion

        #region Injection

        [Inject]
        public void Construct(IHealthSystem healthSystem)
        {
            this.healthSystem = healthSystem;
        }

        #endregion

        #region Unity Lifecycle

        protected virtual void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        protected virtual void Update()
        {
            CheckIfReachedTarget();
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes the enemy's data, position, movement, and health.
        /// </summary>
        public virtual void Initialize(EnemyData data, Vector3 spawnPosition, Vector3 targetPos, Action onEnemyRemoved)
        {
            enemyData = data;
            targetPosition = targetPos;
            transform.position = spawnPosition;
            this.onEnemyRemoved = onEnemyRemoved;

            if (healthSystem == null)
                Debug.LogError("HealthSystem is not injected!");

            healthSystem.Initialize(healthParent, data.MaxHealth);
            agent.speed = data.Speed;
        }

        #endregion

        #region Movement

        /// <summary>
        /// Commands the NavMeshAgent to move toward the final target.
        /// </summary>
        protected void MoveToTarget()
        {
            if (agent != null && canAgentDestination)
            {
                canAgentDestination = false;
                agent.SetDestination(targetPosition);
            }
        }

        /// <summary>
        /// Checks if the enemy has reached its movement destination.
        /// </summary>
        private void CheckIfReachedTarget()
        {
            if (agent == null || isDead) return;

            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    OnReachedTarget();
                }
            }
        }

        /// <summary>
        /// Called when enemy reaches the end goal. Default behavior is self-destruction.
        /// </summary>
        protected virtual void OnReachedTarget()
        {
            Die();
        }

        #endregion

        #region Damage & Death

        /// <summary>
        /// Applies incoming damage and checks for death.
        /// </summary>
        public void TakeDamage(float damage)
        {
            if (isDead || healthSystem == null) return;

            healthSystem.TakeDamage(damage);

            if (healthSystem.IsDead)
            {
                Die();
            }
        }

        /// <summary>
        /// Handles enemy death logic and notifies external systems.
        /// </summary>
        private void Die()
        {
            if (isDead) return;

            isDead = true;
            onEnemyRemoved?.Invoke();
            Destroy(gameObject);
        }

        #endregion
    }
}
