using System.Threading.Tasks;
using UnityEngine;

namespace TowerDefence.Addressable
{
    /// <summary>
    /// Interface for managing Addressable assets.
    /// </summary>
    public interface IAddressableManager
    {
        /// <summary>
        /// Instantiates an Addressable GameObject asynchronously.
        /// </summary>
        /// <param name="address">The address of the prefab to instantiate.</param>
        /// <param name="position">The position to spawn the object.</param>
        /// <param name="rotation">The rotation to apply to the spawned object.</param>
        /// <returns>A Task containing the instantiated GameObject.</returns>
        Task<GameObject> InstantiateAsync(string address, Vector3 position, Quaternion rotation);

        /// <summary>
        /// Loads an Addressable asset asynchronously.
        /// </summary>
        /// <typeparam name="T">Type of asset to load (must derive from UnityEngine.Object).</typeparam>
        /// <param name="address">The address of the asset to load.</param>
        /// <returns>A Task containing the loaded asset.</returns>
        Task<T> LoadAssetAsync<T>(string address) where T : UnityEngine.Object;

        /// <summary>
        /// Releases an instantiated Addressable GameObject.
        /// </summary>
        /// <param name="obj">The GameObject instance to release.</param>
        void Release(GameObject obj);
    }
}
