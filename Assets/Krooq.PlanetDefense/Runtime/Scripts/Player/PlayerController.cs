using UnityEngine;
using Krooq.Core;
using Krooq.Common;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Krooq.PlanetDefense
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerWeapon _weapon;
        [SerializeField] private PlayerTargetingReticle _targetingReticle;

        private Camera _cam;
        protected GameManager GameManager => this.GetSingleton<GameManager>();
        protected InputManager InputManager => this.GetSingleton<InputManager>();
        protected AudioManager AudioManager => this.GetSingleton<AudioManager>();

        protected void Start()
        {
            _cam = Camera.main;
        }

        protected void Update()
        {
            if (GameManager.State != GameState.Playing) return;

            HandleMovement();
            HandleAiming();
            HandleFiring();
        }

        protected void HandleMovement()
        {
            var moveInput = InputManager.MoveAction.ReadValue<Vector2>();
            transform.Translate(GameManager.Data.MoveSpeed * moveInput.x * Time.deltaTime * Vector3.right);
        }

        protected void HandleAiming()
        {
            if (_targetingReticle == null) return;
            _weapon.Aim(_targetingReticle.TargetPosition, GameManager.Data.RotationSpeed);
        }

        protected void HandleFiring()
        {
            if (InputManager.ClickAction.IsPressed())
            {
                if (_targetingReticle != null)
                {
                    _weapon.TryFire(_targetingReticle);
                }
            }
        }
    }
}
