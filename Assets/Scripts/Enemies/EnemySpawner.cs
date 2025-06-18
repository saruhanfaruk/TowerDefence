using System;
using TowerDefence.Data;
using UnityEngine;
using Zenject;
using TowerDefence.Addressable;
using TowerDefence.Core;

namespace TowerDefence.Enemies
{
    /// <summary>
    /// Responsible for spawning enemy units using addressables and DI-based prefab instantiation.
    /// </summary>
    public class EnemySpawner
    {
        #region Fields

        private readonly DiContainer container;
        private readonly MapManager _mapManager;
        private readonly IAddressableManager addressableManager;

        #endregion

        #region Constructor

        [Inject]
        public EnemySpawner(DiContainer container, MapManager mapManager, IAddressableManager addressableManager)
        {
            this.container = container;
            _mapManager = mapManager;
            this.addressableManager = addressableManager;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Spawns an enemy based on the given EnemyData. Loads the prefab via addressables and initializes it.
        /// </summary>
        /// <param name="enemyData">Data describing the enemy type</param>
        /// <param name="onEnemyRemoved">Callback for when enemy dies</param>
        public async void Spawn(EnemyData enemyData, Action onEnemyRemoved)
        {
            try
            {
                Vector3 spawnPosition = GetRandomSpawnPosition();

                // Load the prefab asynchronously
                GameObject prefab = await addressableManager.LoadAssetAsync<GameObject>(enemyData.PrefabAddress);

                // Instantiate the prefab using Zenject's container
                GameObject enemyObj = container.InstantiatePrefab(prefab, spawnPosition, Quaternion.identity, null);

                // Get enemy behaviour component dynamically based on EnemyData type
                Type enemyType = enemyData.GetRuntimeEnemyComponentType();
                if (enemyObj.TryGetComponent(enemyType, out var component) && component is IEnemy enemy)
                {
                    enemy.Initialize(enemyData, spawnPosition, _mapManager.GetFinishPoint(), onEnemyRemoved);
                }
                else
                {
                    Debug.LogError($"Enemy prefab does not contain component of type {enemyType.Name}");
                }
            }
            catch (Exception e)
            {
                Debug.LogError("An error occurred while spawning the enemy: " + e.Message);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Returns a random spawn position within the configured radius of the spawn point.
        /// </summary>
        private Vector3 GetRandomSpawnPosition()
        {
            Vector3 basePosition = _mapManager.GetSpawnPoint();
            float radius = _mapManager.GetSpawnRadius();
            Vector2 offset = UnityEngine.Random.insideUnitCircle * radius;
            return basePosition + new Vector3(offset.x, 0, offset.y);
        }

        #endregion
    }
}
