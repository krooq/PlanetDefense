using UnityEngine;
using Krooq.Common;
using Krooq.Core;
using Sirenix.OdinInspector;
using System.Linq;

namespace Krooq.PlanetDefense
{
    public class SpellBarUI : BaseBarUI
    {
        protected override int MaxSlots => GameManager.Data.MaxSlots;

        protected override BaseSlotUI SpawnSlot()
        {
            return GameManager.Spawn(GameManager.Data.SpellSlotPrefab);
        }

        protected override void OnRefreshTiles()
        {
            var activeSpells = GameManager.Spells;
            if (activeSpells == null) return;

            for (int i = 0; i < activeSpells.Count; i++)
            {
                var spell = activeSpells[i];
                if (spell != null && i < _slotContainer.childCount)
                {
                    var slot = _slotContainer.GetChild(i);
                    var ui = GameManager.Spawn(GameManager.Data.SpellTilePrefab);
                    ui.transform.SetParent(slot);
                    ui.transform.localPosition = Vector3.zero;
                    ui.Init(spell, false, i);
                }
            }
        }
    }
}
