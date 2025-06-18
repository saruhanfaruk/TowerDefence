using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;

namespace TowerDefence.Addressable
{
    /// <summary>
    /// Responsible for instantiating and releasing Addressable assets in the game.
    /// </summary>
    public class AddressableManager : IAddressableManager
    {
        #region Addressable Instantiation

        /// <summary>
        /// Instantiates an Addressable GameObject at a given position and rotation.
        /// </summary>
        /// <param name="address">The address of the prefab.</param>
        /// <param name="position">Spawn position.</param>
        /// <param name="rotation">Spawn rotation.</param>
        /// <returns>The instantiated GameObject.</returns>
        public async Task<GameObject> InstantiateAsync(string address, Vector3 position, Quaternion rotation)
        {
            var handle = Addressables.InstantiateAsync(address, position, rotation);
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                return handle.Result;
            }

            throw new System.Exception($"[AddressableManager] Failed to instantiate prefab at address: {address}");
        }

        #endregion

        #region Asset Loading

        /// <summary>
        /// Loads an Addressable asset of a given type.
        /// </summary>
        /// <typeparam name="T">Type of asset to load.</typeparam>
        /// <param name="address">The address of the asset.</param>
        /// <returns>The loaded asset.</returns>
        public async Task<T> LoadAssetAsync<T>(string address) where T : Object
        {
            var handle = Addressables.LoadAssetAsync<T>(address);
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                return handle.Result;
            }

            throw new System.Exception($"[AddressableManager] Failed to load asset at address: {address}");
        }

        #endregion

        #region Asset Release

        /// <summary>
        /// Releases an instantiated Addressable GameObject.
        /// </summary>
        /// <param name="obj">The object to release.</param>
        public void Release(GameObject obj)
        {
            Addressables.ReleaseInstance(obj);
        }

        #endregion
    }
}
