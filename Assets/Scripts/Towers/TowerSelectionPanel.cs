using System;
using System.Collections.Generic;
using TowerDefence.Data;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TowerDefence.Towers
{
    /// <summary>
    /// Handles the UI for selecting and placing towers in the game.
    /// </summary>
    public class TowerSelectionPanel : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private GameObject buttonPrefab;
        [SerializeField] private Transform buttonParent;

        #endregion

        #region Injected Dependencies

        [Inject] private List<TowerData> _towerDataList;

        #endregion

        #region Private Fields

        private Action<TowerData> _onTowerSelected;

        #endregion

        #region Public Methods

        /// <summary>
        /// Displays the tower selection panel and generates buttons based on available tower data.
        /// </summary>
        /// <param name="onSelected">Callback when a tower is selected.</param>
        public void Show(Action<TowerData> onSelected)
        {
            _onTowerSelected = onSelected;

            ClearExistingButtons();
            CreateTowerButtons();

            gameObject.SetActive(true);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Destroys any existing tower buttons to refresh the UI.
        /// </summary>
        private void ClearExistingButtons()
        {
            foreach (Transform child in buttonParent)
            {
                Destroy(child.gameObject);
            }
        }

        /// <summary>
        /// Instantiates buttons for each tower type defined in towerDataList.
        /// </summary>
        private void CreateTowerButtons()
        {
            foreach (var tower in _towerDataList)
            {
                var buttonGO = Instantiate(buttonPrefab, buttonParent);
                var buttonText = buttonGO.GetComponentInChildren<Text>();
                if (buttonText != null)
                {
                    buttonText.text = tower.TowerName;
                }

                var button = buttonGO.GetComponent<Button>();
                if (button != null)
                {
                    TowerData localTower = tower; // Prevent closure issue in loop
                    button.onClick.AddListener(() =>
                    {
                        _onTowerSelected?.Invoke(localTower);
                        gameObject.SetActive(false);
                    });
                }
            }
        }

        #endregion
    }
}
