using UnityEngine;
using System.Collections.Generic;

namespace Krooq.PlanetDefense
{
    [CreateAssetMenu(menuName = "PlanetDefense/Upgrades/Damage")]
    public class Damage : Upgrade
    {
        [SerializeField] private float _damageBonus = 5f;
        public float DamageBonus => _damageBonus;

        public override bool Process(ProjectileContext context, List<Upgrade> remainingChain, GameManager gameManager)
        {
            context.Stats.SetDamage(context.Stats.Damage + DamageBonus);
            return true;
        }
    }
}
