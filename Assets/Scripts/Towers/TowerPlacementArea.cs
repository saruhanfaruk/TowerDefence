using System;
using TowerDefence.Addressable;
using TowerDefence.Core;
using TowerDefence.Data;
using TowerDefence.Towers;
using UnityEngine;
using Zenject;

namespace TowerDefence.Towers
{
    /// <summary>
    /// Responsible for handling tower placement interactions and instantiation logic.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class TowerPlacementArea : MonoBehaviour
    {
        #region Dependencies

        [Inject] private UIManager _uiManager;
        [Inject] private DiContainer _container;
        [Inject] private MapManager _mapManager;
        [Inject] private IAddressableManager _addressableManager;

        #endregion

        #region Properties

        public bool IsOccupied { get; set; } = false;

        #endregion

        #region Unity Methods

        private void Start()
        {
            // Register this placement area with the MapManager
            _mapManager.AddTowerPlacement(gameObject);
        }

        private void OnMouseDown()
        {
            if (!IsOccupied)
            {
                _uiManager.ShowTowerSelection(this);
            }
        }

        #endregion

        #region Tower Placement

        /// <summary>
        /// Instantiates a tower at this placement area.
        /// </summary>
        /// <param name="towerData">The data for the selected tower.</param>
        public async void PlaceTower(TowerData towerData)
        {
            GameObject prefab = await _addressableManager.LoadAssetAsync<GameObject>(towerData.PrefabAddress);

            // Instantiate the tower with Zenject and inject dependencies
            TowerBase tower = _container.InstantiatePrefabForComponent<TowerBase>(
                prefab,
                transform.position,
                Quaternion.identity,
                null
            );

            tower.Initialize(towerData, this);
            IsOccupied = true;
            _mapManager.TowerCount++;
        }

        #endregion
    }
}
