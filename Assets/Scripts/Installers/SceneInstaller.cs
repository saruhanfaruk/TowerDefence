using TowerDefence.Core;
using TowerDefence.Enemies;
using UnityEngine;
using Zenject;

namespace TowerDefence.Installers
{
    /// <summary>
    /// Scene-specific installer that binds scene-only dependencies.
    /// </summary>
    public class SceneInstaller : MonoInstaller
    {
        #region Serialized Fields

        [SerializeField] private MapManager mapManager;
        [SerializeField] private Camera mainCamera;

        #endregion

        public override void InstallBindings()
        {
            #region Enemy Spawner

            // Binds the central enemy spawner service as a singleton
            Container.Bind<EnemySpawner>().AsSingle();

            #endregion

            #region Camera Binding

            // Binds the main camera instance for systems like HealthBar
            Container.Bind<Camera>().FromInstance(mainCamera).AsSingle();

            #endregion
        }
    }
}
