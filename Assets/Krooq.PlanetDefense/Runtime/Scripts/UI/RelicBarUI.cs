using UnityEngine;
using Krooq.Common;
using Krooq.Core;

namespace Krooq.PlanetDefense
{
    public class RelicBarUI : BaseBarUI
    {
        protected override int MaxSlots => GameManager.Data.MaxRelicSlots;

        protected override BaseSlotUI SpawnSlot()
        {
            return GameManager.Spawn(GameManager.Data.RelicSlotPrefab);
        }

        protected override void OnRefreshTiles()
        {
            var activeRelics = this.GetSingleton<Player>().Relics;
            if (activeRelics == null) return;

            for (int i = 0; i < activeRelics.Count; i++)
            {
                var relic = activeRelics[i];
                if (relic != null && i < _slotContainer.childCount)
                {
                    var slot = _slotContainer.GetChild(i);
                    var ui = GameManager.Spawn(GameManager.Data.RelicTilePrefab);
                    ui.transform.SetParent(slot);
                    ui.transform.localPosition = Vector3.zero;
                    ui.Init(relic, false, i);
                }
            }
        }
    }
}
