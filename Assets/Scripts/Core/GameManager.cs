using UnityEngine;
using Zenject;

namespace TowerDefence.Core
{
    /// <summary>
    /// Central controller for game state initialization and lifecycle.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        #region Injected Dependencies
        [Inject] private readonly WaveManager waveManager;

        #endregion

        #region Unity Methods

        private void Start()
        {
            if (waveManager == null)
            {
                Debug.LogError("[GameManager] WaveManager is not initialized.");
                return;
            }

            waveManager.StartWaves();
        }

        #endregion
    }
}
