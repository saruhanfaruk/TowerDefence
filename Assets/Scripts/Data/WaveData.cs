using System.Collections.Generic;
using UnityEngine;

namespace TowerDefence.Data
{
    /// <summary>
    /// Stores configuration for all enemy waves in the game.
    /// </summary>
    [CreateAssetMenu(fileName = "NewWaveData", menuName = "TowerDefence/Wave Data")]
    public class WaveData : ScriptableObject
    {
        [SerializeField] private List<WaveInfo> waveInfos = new List<WaveInfo>();
        [SerializeField] private float preparationDuration = 5f;

        /// <summary>
        /// Time given to the player between waves to build or prepare.
        /// </summary>
        public float PreparationDuration => preparationDuration;

        /// <summary>
        /// All waves in this wave set.
        /// </summary>
        public IReadOnlyList<WaveInfo> WaveInfos => waveInfos;

        /// <summary>
        /// Describes how many enemies spawn, when, and which type.
        /// </summary>
        [System.Serializable]
        public class EnemySpawnInfo
        {
            public EnemyData enemyData;
            public int count;
            public float startTime;
            public float spawnInterval;
        }

        /// <summary>
        /// Represents a single wave, which can contain multiple enemy groups.
        /// </summary>
        [System.Serializable]
        public class WaveInfo
        {
            /// <summary>List of enemy groups that spawn in this wave.</summary>
            public List<EnemySpawnInfo> enemies = new List<EnemySpawnInfo>();
        }
    }
}
