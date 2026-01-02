using UnityEngine;
using System.Collections.Generic;

namespace Krooq.PlanetDefense
{
    [CreateAssetMenu(menuName = "PlanetDefense/Tiles/Pierce")]
    public class PierceTile : UpgradeTile
    {
        [SerializeField] private int _pierceBonus = 1;
        public int PierceBonus => _pierceBonus;

        public override bool Process(ProjectileContext context, List<UpgradeTile> remainingChain)
        {
            context.Stats.SetPierceCount(context.Stats.PierceCount + PierceBonus);
            return true;
        }
    }
}
