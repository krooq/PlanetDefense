using UnityEngine;
using System.Collections.Generic;

namespace Krooq.PlanetDefense
{
    [CreateAssetMenu(menuName = "PlanetDefense/Tiles/Damage")]
    public class DamageTile : UpgradeTile
    {
        [SerializeField] private float _damageBonus = 5f;
        public float DamageBonus => _damageBonus;

        public override bool Process(ProjectileContext context, List<UpgradeTile> remainingChain, GameManager gameManager)
        {
            context.Stats.SetDamage(context.Stats.Damage + DamageBonus);
            return true;
        }
    }
}
