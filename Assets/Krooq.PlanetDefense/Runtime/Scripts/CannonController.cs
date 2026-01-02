using UnityEngine;
using Krooq.Core;
using Krooq.Common;

namespace Krooq.PlanetDefense
{
    public class CannonController : MonoBehaviour
    {
        [SerializeField] private Transform _pivot;
        [SerializeField] private Transform _firePoint;

        private float _fireTimer;
        private Camera _cam;
        protected GameManager GameManager => this.GetSingleton<GameManager>();
        protected InputManager InputManager => this.GetSingleton<InputManager>();

        void Start()
        {
            _cam = Camera.main;
        }

        void Update()
        {
            if (GameManager.State != GameState.Playing) return;

            HandleAiming();
            HandleFiring();
        }

        void HandleAiming()
        {
            var mousePos = _cam.ScreenToWorldPoint(InputManager.PointAction.ReadValue<Vector2>());
            mousePos.z = 0f;

            var dir = (mousePos - _pivot.position).normalized;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f; // Assuming sprite points up

            _pivot.rotation = Quaternion.Lerp(_pivot.rotation, Quaternion.Euler(0, 0, angle), Time.deltaTime * GameManager.Data.RotationSpeed);
        }

        void HandleFiring()
        {
            _fireTimer -= Time.deltaTime;

            if (InputManager.ClickAction.IsPressed() && _fireTimer <= 0)
            {
                Fire();
                _fireTimer = GameManager.Data.FireRate;
            }
        }

        void Fire()
        {
            var p = GameManager.SpawnProjectile();
            p.transform.SetPositionAndRotation(_firePoint.position, _firePoint.rotation);

            // Base Stats
            var stats = new ProjectileStats(); // Should probably come from GameData or be configurable

            var context = new ProjectileContext(p.gameObject, _firePoint.position, _firePoint.up, stats, true);

            TileSequence.RunChain(context, GameManager.ActiveUpgrades);

            // Finalize
            p.Initialize(context.Direction, context.Stats);
        }
    }
}
