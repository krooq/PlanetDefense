using UnityEngine;
using System.Collections.Generic;

namespace Krooq.PlanetDefense
{
    [CreateAssetMenu(menuName = "PlanetDefense/Tiles/Explosive")]
    public class ExplosiveTile : UpgradeTile
    {
        [SerializeField] private float _radius = 2f;
        [SerializeField] private float _damageMultiplier = 0.5f;

        public float Radius => _radius;
        public float DamageMultiplier => _damageMultiplier;

        public override bool Process(ProjectileContext context, List<UpgradeTile> remainingChain, GameManager gameManager)
        {
            if (context.ProjectileObject != null)
            {
                // var exp = context.ProjectileObject.AddComponent<ExplosionBehavior>();
                // exp.Initialize(Radius, DamageMultiplier, new List<UpgradeTile>(remainingChain), context.Stats.Clone());
            }

            return false; // Stop chain execution for this projectile
        }
    }
}
