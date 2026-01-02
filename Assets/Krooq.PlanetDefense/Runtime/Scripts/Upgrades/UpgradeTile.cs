using UnityEngine;
using System.Collections.Generic;

namespace Krooq.PlanetDefense
{
    public class ProjectileContext
    {
        [SerializeField] private GameObject _projectileObject;
        [SerializeField] private Vector3 _position;
        [SerializeField] private Vector3 _direction;
        [SerializeField] private ProjectileStats _stats;
        [SerializeField] private bool _isSetupPhase; // True if configuring before launch, False if triggered by event (like explosion)

        public GameObject ProjectileObject => _projectileObject;
        public Vector3 Position => _position;
        public Vector3 Direction => _direction;
        public ProjectileStats Stats => _stats;
        public bool IsSetupPhase => _isSetupPhase;

        public ProjectileContext(GameObject obj, Vector3 pos, Vector3 dir, ProjectileStats stats, bool isSetup)
        {
            _projectileObject = obj;
            _position = pos;
            _direction = dir;
            _stats = stats;
            _isSetupPhase = isSetup;
        }

        public void SetDirection(Vector3 direction) => _direction = direction;
    }

    [System.Serializable]
    public class ProjectileStats
    {
        [SerializeField] private float _damage = 10f;
        [SerializeField] private float _speed = 10f;
        [SerializeField] private float _size = 1f;
        [SerializeField] private int _pierceCount = 0;
        [SerializeField] private float _lifetime = 30f;

        public float Damage => _damage;
        public float Speed => _speed;
        public float Size => _size;
        public int PierceCount => _pierceCount;
        public float Lifetime => _lifetime;

        public void SetDamage(float value) => _damage = value;
        public void SetSpeed(float value) => _speed = value;
        public void SetSize(float value) => _size = value;
        public void SetPierceCount(int value) => _pierceCount = value;
        public void SetLifetime(float value) => _lifetime = value;

        public ProjectileStats Clone()
        {
            return (ProjectileStats)this.MemberwiseClone();
        }
    }

    public abstract class UpgradeTile : ScriptableObject
    {
        [SerializeField] private string _tileName;
        [SerializeField] private int _cost;
        [SerializeField] private Color _tileColor = Color.white;

        public string TileName => _tileName;
        public int Cost => _cost;
        public Color TileColor => _tileColor;

        // Returns true if the chain should continue immediately, false if this tile stops execution (e.g. Explosive waiting for impact)
        public abstract bool Process(ProjectileContext context, List<UpgradeTile> remainingChain);
    }
}
