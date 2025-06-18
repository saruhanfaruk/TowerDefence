using DG.Tweening;
using TMPro;
using TowerDefence.Towers;
using TowerDefence.Data;
using UnityEngine;

namespace TowerDefence.Core
{
    /// <summary>
    /// Handles UI elements such as wave, enemy, and tower counters, as well as tower selection.
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private TowerSelectionPanel selectionPanel;
        [SerializeField] private TextMeshProUGUI enemyCountText;
        [SerializeField] private TextMeshProUGUI wavesRemainingText;
        [SerializeField] private TextMeshProUGUI towerRemainingText;
        [SerializeField] private TextMeshProUGUI buildTimeRemainingText;

        #endregion

        #region Private Fields

        private TowerPlacementArea _currentPlacementArea;

        #endregion

        #region Tower Selection

        /// <summary>
        /// Opens tower selection panel for a given placement area.
        /// </summary>
        /// <param name="area">The tower placement area selected.</param>
        public void ShowTowerSelection(TowerPlacementArea area)
        {
            _currentPlacementArea = area;
            selectionPanel.Show(OnTowerSelected);
        }

        private void OnTowerSelected(TowerData selectedTower)
        {
            if (_currentPlacementArea != null && !_currentPlacementArea.IsOccupied)
            {
                _currentPlacementArea.PlaceTower(selectedTower);
                _currentPlacementArea = null;
            }
        }

        #endregion

        #region UI Text Updates

        public void UpdateEnemyCountText(int liveEnemyCount)
        {
            enemyCountText.text = $"Enemies Remaining: {liveEnemyCount}";
        }

        public void UpdateWavesRemainingText(int waveRemaining)
        {
            wavesRemainingText.text = $"Waves Remaining: {waveRemaining}";
        }

        public void UpdateTowerRemainingText(int towerCount)
        {
            towerRemainingText.text = $"Towers Remaining: {towerCount}";
        }

        #endregion

        #region Build Phase Timer

        public void ShowBuildPhaseTimeRemaining(float time)
        {
            buildTimeRemainingText.gameObject.SetActive(true);

            DOTween.To(() => time, x =>
            {
                buildTimeRemainingText.text = Mathf.CeilToInt(x).ToString();
            }, 0, time)
            .SetEase(Ease.Linear)
            .SetUpdate(UpdateType.Fixed)
            .OnComplete(() =>
            {
                buildTimeRemainingText.gameObject.SetActive(false);
            });
        }

        #endregion
    }
}
