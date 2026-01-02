using UnityEngine;
using System.Collections.Generic;
using System;

namespace Krooq.PlanetDefense
{
    [CreateAssetMenu(menuName = "PlanetDefense/Tiles/Split")]
    public class SplitTile : UpgradeTile
    {
        [SerializeField] private float _splitAngle = 15f;
        public float SplitAngle => _splitAngle;

        protected GameManager GameManager => this.GetSingleton<GameManager>();

        private T GetSingleton<T>()
        {
            throw new NotImplementedException();
        }

        public override bool Process(ProjectileContext context, List<UpgradeTile> remainingChain)
        {
            // Rotate current
            Quaternion rot1 = Quaternion.Euler(0, 0, -SplitAngle);
            Quaternion rot2 = Quaternion.Euler(0, 0, SplitAngle);

            Vector3 originalDir = context.Direction;

            // Modify current
            context.SetDirection(rot1 * originalDir);
            if (context.ProjectileObject != null)
            {
                context.ProjectileObject.transform.rotation = Quaternion.LookRotation(Vector3.forward, context.Direction);
                // Update projectile component if it exists
                var p = context.ProjectileObject.GetComponent<Projectile>();
                if (p != null) p.Initialize(context.Direction, context.Stats);
            }

            // Spawn new
            var newP = GameManager.SpawnProjectile();
            newP.transform.SetPositionAndRotation(context.Position, Quaternion.LookRotation(Vector3.forward, rot2 * originalDir));
            newP.Initialize(rot2 * originalDir, context.Stats.Clone());

            // Create context for new projectile
            var newContext = new ProjectileContext(newP.gameObject, context.Position, rot2 * originalDir, context.Stats.Clone(), context.IsSetupPhase);

            // Run chain on new projectile
            TileSequence.RunChain(newContext, remainingChain);

            // Re-initialize with potentially modified stats
            newP.Initialize(newContext.Direction, newContext.Stats);

            return true; // Continue chain on current projectile
        }
    }
}
