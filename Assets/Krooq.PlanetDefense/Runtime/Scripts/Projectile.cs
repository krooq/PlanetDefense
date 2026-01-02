using UnityEngine;
using System.Collections.Generic;
using Krooq.Common;
using Krooq.Core;

namespace Krooq.PlanetDefense
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private ProjectileStats _stats;
        private Vector3 _direction;
        private float _timer;

        public ProjectileStats Stats => _stats;


        public void Initialize(Vector3 direction, ProjectileStats stats)
        {
            _direction = direction;
            _stats = stats;
            _timer = stats.Lifetime;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);

            // Set scale
            transform.localScale = Vector3.one * stats.Size;
        }

        void Update()
        {
            if (Stats == null) return;

            float moveDist = Stats.Speed * Time.deltaTime;
            transform.position += _direction * moveDist;

            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                Destroy(gameObject);
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            // If we have an ExplosionBehavior, it handles the trigger.
            if (this.GetCachedComponent<ExplosionBehavior>() != null) return;

            if (other.TryGetComponent<Meteor>(out var meteor))
            {
                meteor.TakeDamage(Stats.Damage);

                if (Stats.PierceCount > 0)
                {
                    Stats.SetPierceCount(Stats.PierceCount - 1);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            else if (other.CompareTag("Ground"))
            {
                Destroy(gameObject);
            }
        }
    }
}
