using UnityEngine;
using Krooq.Core;
using Krooq.Common;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Krooq.PlanetDefense
{
    public class PlayerWeapon : MonoBehaviour
    {
        [SerializeField] private Transform _pivot;
        [SerializeField] private Transform _firePoint;
        [SerializeField] private SpringTransform _recoilTransform;
        [SerializeField] private List<GameObject> _fireEffects;

        [SerializeField, ReadOnly] private float _fireTimer;

        protected GameManager GameManager => this.GetSingleton<GameManager>();
        protected AudioManager AudioManager => this.GetSingleton<AudioManager>();

        protected void Update()
        {
            if (GameManager.State != GameState.Playing) return;
            _fireTimer -= Time.deltaTime;
        }

        public void Aim(Vector3 targetPosition, float rotationSpeed)
        {
            var dir = (targetPosition - _pivot.position).normalized;

            // // Restrict to upper semicircle
            // if (dir.y < 0)
            // {
            //     dir.y = 0;
            //     dir.x = Mathf.Sign(dir.x);
            // }

            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f; // Assuming sprite points up

            _pivot.rotation = Quaternion.Lerp(_pivot.rotation, Quaternion.Euler(0, 0, angle), Time.deltaTime * rotationSpeed);
        }

        public void TryFire(PlayerTargetingReticle targetingReticle)
        {
            if (_fireTimer <= 0 && GameManager.SelectedWeapon != null)
            {
                Fire(targetingReticle);
            }
        }

        private void Fire(PlayerTargetingReticle targetingReticle)
        {
            var p = GameManager.SpawnProjectile();
            if (p == null) return;

            p.transform.SetPositionAndRotation(_firePoint.position, _firePoint.rotation);

            // Get Modifiers
            var modifiers = GameManager.ActiveModifiers;

            // Finalize
            p.Init(_firePoint.up, GameManager.SelectedWeapon, modifiers, targetingReticle);

            // Set Fire Timer based on projectile stat
            _fireTimer = 1f / p.FireRate;

            foreach (var effect in _fireEffects)
            {
                effect.SetActive(false);
                effect.SetActive(true);
            }

            // _recoilTransform.BumpPosition(_firePoint.localPosition - Vector3.up * 10f);
            if (GameManager.SelectedWeapon.FireSound != null) AudioManager.PlaySound(GameManager.SelectedWeapon.FireSound);
        }
    }
}
