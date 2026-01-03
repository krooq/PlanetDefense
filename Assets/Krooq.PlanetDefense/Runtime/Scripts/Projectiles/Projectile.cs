using UnityEngine;
using System.Collections.Generic;
using Krooq.Common;
using Krooq.Core;
using Sirenix.OdinInspector;

namespace Krooq.PlanetDefense
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private ProjectileStats _stats;
        [SerializeField, ReadOnly] private Vector3 _direction;
        [SerializeField, ReadOnly] private float _timer;
        protected GameManager GameManager => this.GetSingleton<GameManager>();

        public ProjectileStats Stats => _stats;


        public void Initialize(Vector3 direction, ProjectileStats stats)
        {
            _direction = direction;
            _stats = stats;
            _timer = GameManager.Data.ProjectileLifetime;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);

            // Set scale
            transform.localScale = Vector3.one * stats.Size;
        }

        protected void Update()
        {
            if (Stats == null) return;

            var moveDist = Stats.Speed * Time.deltaTime;
            transform.position += _direction * moveDist;

            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                GameManager.Despawn(gameObject);
            }
        }

        protected void OnTriggerEnter2D(Collider2D other)
        {
            // If we have an ExplosionBehavior, it handles the trigger.
            // if (this.GetCachedComponent<ExplosionBehavior>() != null) return;
            var meteor = other.gameObject.GetCachedComponent<Meteor>();
            if (meteor != null)
            {
                meteor.TakeDamage(Stats.Damage);

                if (Stats.PierceCount > 0)
                {
                    Stats.SetPierceCount(Stats.PierceCount - 1);
                }
                else
                {
                    GameManager.Despawn(gameObject);
                }
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                GameManager.Despawn(gameObject);
            }
        }
    }
}
