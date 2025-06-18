using System.Collections.Generic;
using UnityEngine;

namespace TowerDefence.Projectile
{
    /// <summary>
    /// Manages a pool of reusable projectile instances to improve performance.
    /// </summary>
    public class ProjectilePool : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private Projectile projectilePrefab;
        [SerializeField] private int initialSize = 20;

        #endregion

        #region Private Fields

        private readonly Queue<Projectile> pool = new Queue<Projectile>();

        #endregion

        #region Unity Methods

        private void Awake()
        {
            for (int i = 0; i < initialSize; i++)
                CreateNewProjectile();
        }

        #endregion

        #region Pool Methods

        /// <summary>
        /// Creates a new projectile instance and adds it to the pool.
        /// </summary>
        private Projectile CreateNewProjectile()
        {
            Projectile newProjectile = Instantiate(projectilePrefab, transform);
            newProjectile.gameObject.SetActive(false);
            pool.Enqueue(newProjectile);
            return newProjectile;
        }

        /// <summary>
        /// Retrieves a projectile from the pool or creates a new one if the pool is empty.
        /// </summary>
        public Projectile Get()
        {
            if (pool.Count == 0)
                CreateNewProjectile();

            Projectile proj = pool.Dequeue();

            if (proj.gameObject.activeInHierarchy)
                Debug.LogWarning("Warning! Attempting to reuse an already active projectile: " + proj.name);

            proj.gameObject.SetActive(true);
            return proj;
        }

        /// <summary>
        /// Returns a projectile to the pool for future reuse.
        /// </summary>
        public void ReturnToPool(Projectile proj)
        {
            if (!proj.gameObject.activeInHierarchy)
                return;

            proj.gameObject.SetActive(false);
            pool.Enqueue(proj);
        }

        #endregion
    }
}
