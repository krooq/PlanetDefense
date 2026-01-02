using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using Krooq.Common;
using Krooq.Core;

namespace Krooq.PlanetDefense
{
    public class ShopUI : MonoBehaviour
    {
        [SerializeField] private GameObject _shopPanel;
        [SerializeField] private Transform _availableContainer;
        [SerializeField] private Transform _inventoryContainer;
        [SerializeField] private Transform _activeContainer;

        [SerializeField] private GameObject _tileButtonPrefab;
        [SerializeField] private TextMeshProUGUI _resourcesText;
        [SerializeField] private Button _nextWaveButton;

        private bool _dirty = true;
        protected GameManager GameManager => this.GetSingleton<GameManager>();

        void Start()
        {
            _nextWaveButton.onClick.AddListener(OnNextWave);
        }

        void Update()
        {
            if (GameManager.State == GameState.Shop)
            {
                if (!_shopPanel.activeSelf)
                {
                    _shopPanel.SetActive(true);
                    _dirty = true;
                }

                if (_dirty)
                {
                    UpdateUI();
                    _dirty = false;
                }
            }
            else
            {
                _shopPanel.SetActive(false);
            }
        }

        public void SetDirty()
        {
            _dirty = true;
        }

        void UpdateUI()
        {
            _resourcesText.text = $"Resources: {GameManager.Resources}";

            // Clear containers
            foreach (Transform t in _availableContainer) Destroy(t.gameObject);
            foreach (Transform t in _inventoryContainer) Destroy(t.gameObject);
            foreach (Transform t in _activeContainer) Destroy(t.gameObject);

            // Populate Available
            if (GameManager.Data.AvailableUpgrades != null)
            {
                foreach (var tile in GameManager.Data.AvailableUpgrades)
                {
                    var btnObj = Instantiate(_tileButtonPrefab, _availableContainer);
                    var btn = btnObj.GetComponent<Button>();
                    var txt = btnObj.GetComponentInChildren<TextMeshProUGUI>();
                    if (txt) txt.text = $"{tile.TileName} (${tile.Cost})";
                    btn.onClick.AddListener(() => BuyTile(tile));
                }
            }

            // Populate Inventory
            for (int i = 0; i < GameManager.OwnedUpgrades.Count; i++)
            {
                int index = i;
                var tile = GameManager.OwnedUpgrades[i];
                var btnObj = Instantiate(_tileButtonPrefab, _inventoryContainer);
                var btn = btnObj.GetComponent<Button>();
                var txt = btnObj.GetComponentInChildren<TextMeshProUGUI>();
                if (txt) txt.text = $"{tile.TileName}\n(Equip)";
                btn.onClick.AddListener(() => EquipTile(index));
            }

            // Populate Active
            for (int i = 0; i < GameManager.ActiveUpgrades.Count; i++)
            {
                int index = i;
                var tile = GameManager.ActiveUpgrades[i];
                var btnObj = Instantiate(_tileButtonPrefab, _activeContainer);
                var btn = btnObj.GetComponent<Button>();
                var txt = btnObj.GetComponentInChildren<TextMeshProUGUI>();
                if (txt) txt.text = $"{tile.TileName}\n(Unequip)";
                btn.onClick.AddListener(() => UnequipTile(index));
            }
        }

        void BuyTile(UpgradeTile tile)
        {
            if (GameManager.SpendResources(tile.Cost))
            {
                GameManager.OwnedUpgrades.Add(tile);
                SetDirty();
            }
        }

        void EquipTile(int inventoryIndex)
        {
            if (GameManager.ActiveUpgrades.Count < GameManager.Data.MaxSlots)
            {
                var tile = GameManager.OwnedUpgrades[inventoryIndex];
                GameManager.OwnedUpgrades.RemoveAt(inventoryIndex);
                GameManager.ActiveUpgrades.Add(tile);
                SetDirty();
            }
        }

        void UnequipTile(int activeIndex)
        {
            var tile = GameManager.ActiveUpgrades[activeIndex];
            GameManager.ActiveUpgrades.RemoveAt(activeIndex);
            GameManager.OwnedUpgrades.Add(tile);
            SetDirty();
        }

        void OnNextWave()
        {
            GameManager.NextWave();
        }
    }
}
