using System.Collections.Generic;
using TowerDefence.Addressable;
using TowerDefence.Core;
using TowerDefence.Data;
using TowerDefence.Health;
using TowerDefence.Projectile;
using UnityEngine;
using Zenject;

namespace TowerDefence.Installers
{
    /// <summary>
    /// Central dependency injection installer for the game.
    /// Defines how core services and prefabs are provided at runtime.
    /// </summary>
    [CreateAssetMenu(fileName = "GameInstaller", menuName = "TowerDefence/Game Installer")]
    public class GameInstaller : ScriptableObjectInstaller<GameInstaller>
    {
        #region Serialized Fields

        [SerializeField] private List<TowerData> towerDataList;
        [SerializeField] private HealthBar healthBarPrefab;
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private WaveData waveData;

        #endregion

        public override void InstallBindings()
        {
            #region Addressable System

            // Binds the addressable manager as a singleton
            Container.Bind<IAddressableManager>().To<AddressableManager>().AsSingle();

            #endregion

            #region Game Data

            // Provides tower data list to systems that need it
            Container.Bind<List<TowerData>>().FromInstance(towerDataList).AsSingle();

            // Provides wave configuration
            Container.Bind<WaveData>().FromInstance(waveData).AsSingle();

            #endregion

            #region Health System

            // Binds a transient health system implementation
            Container.Bind<IHealthSystem>().To<HealthSystem>().AsTransient();

            // Binds the health bar prefab for use with health system
            Container.BindFactory<HealthBar, HealthBar.Factory>().FromComponentInNewPrefab(healthBarPrefab).AsTransient();

            #endregion

            #region Projectile System

            // Binds the projectile prefab for use with the pool
            Container.Bind<GameObject>().WithId("ProjectilePrefab").FromInstance(projectilePrefab);

            #endregion

            #region Core Game Managers

            // Core managers bound from the active scene hierarchy
            Container.BindInterfacesAndSelfTo<GameManager>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<WaveManager>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<MapManager>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<UIManager>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<ProjectilePool>().FromComponentInHierarchy().AsSingle();

            #endregion
        }
    }
}
