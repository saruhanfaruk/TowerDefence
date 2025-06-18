using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace TowerDefence.Projectile
{
    /// <summary>
    /// Handles projectile behavior such as movement and appearance.
    /// </summary>
    public class Projectile : MonoBehaviour
    {
        #region Private Fields

        private List<Renderer> targetRenderers = new List<Renderer>();
        private Tween moveTween;

        #endregion

        #region Initialization

        /// <summary>
        /// Prepares the projectile's start position and caches its renderers.
        /// </summary>
        public void Initialize(Vector3 startPos)
        {
            transform.position = startPos;
            targetRenderers.Clear(); // Prevent duplicate entries
            foreach (var renderer in GetComponentsInChildren<Renderer>())
                targetRenderers.Add(renderer);
        }

        #endregion

        #region Movement

        /// <summary>
        /// Moves the projectile toward a target with tweening and handles callbacks.
        /// </summary>
        public void GoToTarget(ProjectilePool projectilePool,Action onHit,Action onCompletedCallback,Transform target,Color projectileColor)
        {
            ChangeColor(projectileColor);
            Vector3 currentPos = transform.position;
            float time = 0;

            moveTween = DOTween.To(() => time, x => time = x, 1, 1) 
                .SetSpeedBased()
                .OnUpdate(() =>
                {
                    if (target == null)
                    {
                        moveTween.Kill();
                        onCompletedCallback?.Invoke();
                        projectilePool.ReturnToPool(this);
                        return;
                    }

                    transform.position = Vector3.Lerp(currentPos, target.position, time);
                })
                .OnComplete(() =>
                {
                    projectilePool.ReturnToPool(this);

                    if (target != null)
                        onHit?.Invoke();

                    onCompletedCallback?.Invoke();
                });
        }

        #endregion

        #region Visual

        /// <summary>
        /// Changes the color of the projectile's renderers.
        /// </summary>
        private void ChangeColor(Color projectileColor)
        {
            foreach (var renderer in targetRenderers)
                renderer.material.color = projectileColor;
        }

        #endregion
    }
}
