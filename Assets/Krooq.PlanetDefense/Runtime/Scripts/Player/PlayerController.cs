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
            var mousePos = _cam.ScreenToWorldPoint(InputManager.PointAction.ReadValue<Vector2>());
            mousePos.z = 0f;

            _weapon.Aim(mousePos, GameManager.Data.RotationSpeed);
        }

        protected void HandleFiring()
        {
            if (InputManager.ClickAction.IsPressed())
            {
                _weapon.TryFire();
            }
        }
    }
}
