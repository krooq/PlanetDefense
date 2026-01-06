using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Krooq.Common;
using Krooq.Core;

namespace Krooq.PlanetDefense
{
    public class SpellTileUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private CanvasGroup _canvasGroup;

        private Spell _spell;
        private bool _isShopItem;
        private Transform _originalParent;
        private Vector3 _originalPosition;
        private int _slotIndex = -1;

        protected Player Player => this.GetSingleton<Player>();
        protected ShopUI ShopUI => this.GetSingleton<ShopUI>();
        protected GameManager GameManager => this.GetSingleton<GameManager>();

        public Spell Spell => _spell;
        public bool IsShopItem => _isShopItem;

        public void Init(Spell spell, bool isShopItem, int slotIndex = -1, UnityAction onClick = null)
        {
            _spell = spell;
            _isShopItem = isShopItem;
            _slotIndex = slotIndex;

            if (_text != null)
            {
                if (isShopItem)
                    _text.text = $"{spell.SpellName} (${spell.ShopCost})";
                else
                    _text.text = spell.SpellName;
            }

            if (_button != null && onClick != null)
            {
                _button.onClick.RemoveAllListeners();
                _button.onClick.AddListener(onClick);
            }
        }

        protected void OnDisable()
        {
            if (_button != null)
            {
                _button.onClick.RemoveAllListeners();
            }
            _spell = null;
            _slotIndex = -1;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                if (!_isShopItem && GameManager.State == GameState.Shop && _slotIndex != -1)
                {
                    // Sell
                    Player.AddResources(_spell.ShopCost); // 100% refund for now
                    Player.SetSpell(_slotIndex, null);

                    if (ShopUI) ShopUI.SetDirty();
                    var spellBarUI = this.GetSingleton<SpellBarUI>();
                    if (spellBarUI) spellBarUI.Refresh();
                }
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _originalParent = transform.parent;
            _originalPosition = transform.position;

            _canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = true;
            transform.position = _originalPosition;
        }
    }
}
