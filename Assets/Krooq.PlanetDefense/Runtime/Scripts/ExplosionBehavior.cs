using UnityEngine;
using System.Collections.Generic;
using Krooq.Common;
using Krooq.Core;

namespace Krooq.PlanetDefense
{
    public class ExplosionBehavior : MonoBehaviour
    {
        private float _radius;
        private float _damageMult;
        private List<UpgradeTile> _remainingChain;
        private ProjectileStats _stats;
        private bool _exploded = false;
        private GameManager _gameManager => this.GetSingleton<GameManager>();

        public void Initialize(float radius, float damageMult, List<UpgradeTile> chain, ProjectileStats stats)
        {
            _radius = radius;
            _damageMult = damageMult;
            _remainingChain = chain;
            _stats = stats;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (_exploded) return;

            if (other.GetComponent<Meteor>() != null || other.CompareTag("Ground"))
            {
                Explode();
            }
        }

        void Explode()
        {
            _exploded = true;

            // Deal AOE Damage
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _radius);
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent<Meteor>(out var meteor))
                {
                    meteor.TakeDamage(_stats.Damage * _damageMult);
                }
            }

            // Visual effect (placeholder)
            // In a real game, spawn a particle system

            // Continue Chain
            if (_remainingChain != null && _remainingChain.Count > 0)
            {
                // Spawn a carrier projectile to continue the chain
                // We use the current rotation/direction
                var p = _gameManager.SpawnProjectile();
                p.transform.SetPositionAndRotation(transform.position, transform.rotation);

                // We need to initialize the projectile component on the new object
                // But RunChain might modify it further.
                // The Projectile component needs Stats.
                p.Initialize(transform.up, _stats.Clone()); // Initialize with current stats

                var newContext = new ProjectileContext(p.gameObject, transform.position, transform.up, _stats.Clone(), false);

                TileSequence.RunChain(newContext, _remainingChain);
            }

            Destroy(gameObject);
        }
    }
}
