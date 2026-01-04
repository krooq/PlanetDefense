using UnityEngine;
using System.Collections.Generic;

namespace Krooq.PlanetDefense
{
    [CreateAssetMenu(menuName = "PlanetDefense/Upgrades/Pierce")]
    public class Pierce : Upgrade
    {
        [SerializeField] private int _pierceBonus = 1;
        public int PierceBonus => _pierceBonus;

        public override bool Process(ProjectileContext context, List<Upgrade> remainingChain, GameManager gameManager)
        {
            context.Stats.SetPierceCount(context.Stats.PierceCount + PierceBonus);
            return true;
        }
    }
}
