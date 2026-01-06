using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Krooq.Common;
using Krooq.Core;

namespace Krooq.PlanetDefense
{
    public class SpellSlotUI : MonoBehaviour, IDropHandler
    {
        [SerializeField] private int _slotIndex;
        [SerializeField] private Image _icon;

        protected ShopUI ShopUI => this.GetSingleton<ShopUI>();
        protected Player Player => this.GetSingleton<Player>();

        public int SlotIndex => _slotIndex;

        public void Init(int index)
        {
            _slotIndex = index;
        }

        protected void OnDisable()
        {
            _slotIndex = -1;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (ShopUI) ShopUI.SetDirty();
            if (eventData.pointerDrag != null && eventData.pointerDrag.TryGetComponent<SpellTileUI>(out var tileUI))
            {
                if (tileUI.IsShopItem)
                {
                    if (this.GetSingleton<Player>().SpendResources(tileUI.Spell.ShopCost))
                    {
                        Player.SetSpell(_slotIndex, tileUI.Spell);

                        var spellBarUI = this.GetSingleton<SpellBarUI>();
                        if (spellBarUI) spellBarUI.Refresh();
                    }
                }
            }
        }
    }
}
