using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Krooq.Common;
using Krooq.Core;

namespace Krooq.PlanetDefense
{
    public class SpellSlotUI : BaseSlotUI
    {
        public override void OnDrop(PointerEventData eventData)
        {
            if (ShopUI) ShopUI.SetDirty();
            if (eventData.pointerDrag != null && eventData.pointerDrag.TryGetComponent<SpellTileUI>(out var tileUI))
            {
                if (tileUI.IsShopItem)
                {
                    if (Player.SpendResources(tileUI.Spell.ShopCost))
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
