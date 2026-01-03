using System;
using UnityEngine;

namespace Krooq.PlanetDefense
{
    public class ProjectileContext
    {
        [SerializeField] private Projectile _projectileObject;
        [SerializeField] private Vector3 _position;
        [SerializeField] private Vector3 _direction;
        [SerializeField] private ProjectileStats _stats;
        [SerializeField] private bool _isSetupPhase; // True if configuring before launch, False if triggered by event (like explosion)

        public Projectile ProjectileObject => _projectileObject;
        public Vector3 Position => _position;
        public Vector3 Direction => _direction;
        public ProjectileStats Stats => _stats;
        public bool IsSetupPhase => _isSetupPhase;

        public ProjectileContext(Projectile obj, Vector3 pos, Vector3 dir, ProjectileStats stats, bool isSetup)
        {
            _projectileObject = obj;
            _position = pos;
            _direction = dir;
            _stats = stats;
            _isSetupPhase = isSetup;
        }

        public void SetDirection(Vector3 direction) => _direction = direction;
    }
}