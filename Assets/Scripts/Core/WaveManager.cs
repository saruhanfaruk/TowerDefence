using System.Collections;
using UnityEngine;
using Zenject;
using TowerDefence.Data;
using TowerDefence.Enemies;

namespace TowerDefence.Core
{
    /// <summary>
    /// Handles enemy wave progression, preparation phases, and spawn logic.
    /// </summary>
    public class WaveManager : MonoBehaviour
    {
        private WaveData _waveData;
        private EnemySpawner _enemySpawner;
        private UIManager _uiManager;
        private MapManager _mapManager;

        private int _currentWaveIndex = 0;
        private int _aliveEnemyCount = 0;
        private bool _isRunning = false;

        public int AliveEnemyCount
        {
            get => _aliveEnemyCount;
            set
            {
                _aliveEnemyCount = value;
                _uiManager.UpdateEnemyCountText(_aliveEnemyCount);
            }
        }

        [Inject]
        public void Construct(EnemySpawner enemySpawner, WaveData waveData, UIManager uiManager, MapManager mapManager)
        {
            _enemySpawner = enemySpawner;
            _waveData = waveData;
            _uiManager = uiManager;
            _mapManager = mapManager;
        }

        public void StartWaves()
        {
            if (_isRunning) return;
            _isRunning = true;
            StartCoroutine(RunWaves());
        }

        private IEnumerator RunWaves()
        {
            while (_currentWaveIndex < _waveData.WaveInfos.Count)
            {
                // Build phase
                _mapManager.SetTowerPlacementStatus(true);
                _uiManager.ShowBuildPhaseTimeRemaining(_waveData.PreparationDuration);
                _uiManager.UpdateEnemyCountText(0);
                _uiManager.UpdateWavesRemainingText(_waveData.WaveInfos.Count - _currentWaveIndex - 1);

                yield return new WaitForSeconds(_waveData.PreparationDuration);

                _mapManager.SetTowerPlacementStatus(false);

                // Run current wave
                var waveInfo = _waveData.WaveInfos[_currentWaveIndex];
                yield return StartCoroutine(RunWave(waveInfo));

                // Wait for enemies to die
                yield return new WaitUntil(() => _aliveEnemyCount <= 0);

                _currentWaveIndex++;
            }

            Debug.Log("All waves completed."); 
        }

        private IEnumerator RunWave(WaveData.WaveInfo waveInfo)
        {
            foreach (var spawnInfo in waveInfo.enemies)
                StartCoroutine(SpawnEnemyGroup(spawnInfo));

            yield return null;
        }

        private IEnumerator SpawnEnemyGroup(WaveData.EnemySpawnInfo spawnInfo)
        {
            yield return new WaitForSeconds(spawnInfo.startTime);

            for (int i = 0; i < spawnInfo.count; i++)
            {
                _enemySpawner.Spawn(spawnInfo.enemyData, HandleEnemyDeath);
                AliveEnemyCount++;
                yield return new WaitForSeconds(spawnInfo.spawnInterval);
            }
        }

        public void HandleEnemyDeath()
        {
            AliveEnemyCount--;
        }
    }
}
