using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TowerDefence.Health
{
    /// <summary>
    /// Represents the UI health bar shown above units (enemies or towers).
    /// It follows the camera and animates health changes.
    /// </summary>
    public class HealthBar : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private Image fillImage;
        [SerializeField] private TextMeshProUGUI textMesh;

        #endregion

        #region Private Fields

        private Camera mainCamera;

        #endregion

        #region Dependency Injection

        [Inject]
        public void Construct(Camera mainCamera)
        {
            this.mainCamera = mainCamera;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the health bar's fill and text.
        /// </summary>
        public void SetHealth(float current, float max)
        {
            textMesh.text = $"{(int)current}/{(int)max}";
            fillImage.DOKill();
            float fill = Mathf.Clamp01(current / max);
            fillImage.DOFillAmount(fill, 0.2f);
        }

        /// <summary>
        /// Stops any active tween animations.
        /// </summary>
        public void KillDotween()
        {
            fillImage.DOKill();
        }

        #endregion

        #region Unity Events

        private void LateUpdate()
        {
            // Always face the camera
            transform.forward = mainCamera.transform.forward;
        }

        #endregion

        #region Factory

        public class Factory : PlaceholderFactory<HealthBar> { }

        #endregion
    }
}
