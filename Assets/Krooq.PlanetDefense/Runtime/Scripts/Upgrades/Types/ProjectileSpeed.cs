using UnityEngine;
using System.Collections.Generic;

namespace Krooq.PlanetDefense
{
    [CreateAssetMenu(menuName = "PlanetDefense/Upgrades/ProjectileSpeed")]
    public class ProjectileSpeed : Upgrade
    {
        [SerializeField] private float _speedBonus = 5f;
        public float SpeedBonus => _speedBonus;

        public override bool Process(ProjectileContext context, List<Upgrade> remainingChain, GameManager gameManager)
        {
            context.Stats.SetSpeed(context.Stats.Speed + SpeedBonus);
            return true;
        }
    }
}
