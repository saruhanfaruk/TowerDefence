using System.Collections;
using UnityEngine;
using Zenject;
using TowerDefence.Data;
using TowerDefence.Enemies;

namespace TowerDefence.Core
{
    /// <summary>
    /// Manages the flow of enemy waves, preparation phases, and spawning logic.
    /// </summary>
    public class WaveManager : MonoBehaviour
    {
        #region Fields

        private WaveData waveData;
        private EnemySpawner enemySpawner;
        private UIManager uiManager;
        private MapManager mapManager;

        private int currentWaveIndex = 0;
        private int aliveEnemyCount = 0;
        private bool isRunning = false;

        #endregion

        #region Properties

        /// <summary>
        /// The number of enemies currently alive in the scene.
        /// Updates the UI automatically when changed.
        /// </summary>
        public int AliveEnemyCount
        {
            get => aliveEnemyCount;
            private set
            {
                aliveEnemyCount = value;
                uiManager.UpdateEnemyCountText(aliveEnemyCount);
            }
        }

        #endregion

        #region Injection

        [Inject]
        public void Construct(EnemySpawner enemySpawner,WaveData waveData,UIManager uiManager,MapManager mapManager)
        {
            this.enemySpawner = enemySpawner;
            this.waveData = waveData;
            this.uiManager = uiManager;
            this.mapManager = mapManager;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Starts the wave flow if not already running.
        /// </summary>
        public void StartWaves()
        {
            if (isRunning) return;

            isRunning = true;
            StartCoroutine(RunWaves());
        }

        /// <summary>
        /// Callback used by enemies to notify death.
        /// </summary>
        public void HandleEnemyDeath()
        {
            AliveEnemyCount--;
        }

        #endregion

        #region Coroutines

        private IEnumerator RunWaves()
        {
            while (currentWaveIndex < waveData.WaveInfos.Count)
            {
                yield return StartCoroutine(HandlePreparationPhase());

                var waveInfo = waveData.WaveInfos[currentWaveIndex];
                yield return StartCoroutine(RunWave(waveInfo));

                yield return new WaitUntil(() => AliveEnemyCount <= 0);

                currentWaveIndex++;
            }

            Debug.Log("All waves completed.");
        }

        private IEnumerator HandlePreparationPhase()
        {
            mapManager.SetTowerPlacementStatus(true);
            uiManager.ShowBuildPhaseTimeRemaining(waveData.PreparationDuration);
            uiManager.UpdateEnemyCountText(0);
            uiManager.UpdateWavesRemainingText(waveData.WaveInfos.Count - currentWaveIndex - 1);

            yield return new WaitForSeconds(waveData.PreparationDuration);

            mapManager.SetTowerPlacementStatus(false);
        }

        private IEnumerator RunWave(WaveData.WaveInfo waveInfo)
        {
            foreach (var spawnInfo in waveInfo.enemies)
            {
                StartCoroutine(SpawnEnemyGroup(spawnInfo));
            }

            yield return null;
        }

        private IEnumerator SpawnEnemyGroup(WaveData.EnemySpawnInfo spawnInfo)
        {
            yield return new WaitForSeconds(spawnInfo.startTime);

            for (int i = 0; i < spawnInfo.count; i++)
            {
                enemySpawner.Spawn(spawnInfo.enemyData, HandleEnemyDeath);
                AliveEnemyCount++;

                yield return new WaitForSeconds(spawnInfo.spawnInterval);
            }
        }

        #endregion
    }
}
