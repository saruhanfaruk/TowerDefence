using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace TowerDefence.Core
{
    /// <summary>
    /// Handles map data like spawn/finish positions and manages tower placements.
    /// </summary>
    public class MapManager : MonoBehaviour
    {
        #region Dependencies
        [Inject] private UIManager uiManager;
        #endregion

        #region Serialized Fields
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Transform finishPoint;
        [SerializeField] private float spawnRadius = 2f;
        #endregion

        #region Private Fields
        private int towerCount;
        private List<GameObject> towerPlacements = new List<GameObject>();
        #endregion

        #region Properties
        public int TowerCount { get { return towerCount; } set { towerCount = value; uiManager.UpdateTowerRemainingText(towerCount); } }
        #endregion

        #region Public Methods
        public Vector3 GetSpawnPoint() => spawnPoint.position;
        public Vector3 GetFinishPoint() => finishPoint.position;
        public float GetSpawnRadius() => spawnRadius;
        private void Start()
        {
            TowerCount = 0;
        }

        public void AddTowerPlacement(GameObject tower)
        {
            if (tower != null && !towerPlacements.Contains(tower))
                towerPlacements.Add(tower);
        }
        public void SetTowerPlacementStatus(bool status)
        {
            foreach (var item in towerPlacements)
                item.SetActive(status);
        }
        #endregion
    }
}
