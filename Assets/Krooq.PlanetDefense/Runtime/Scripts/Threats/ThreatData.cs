using UnityEngine;

namespace Krooq.PlanetDefense
{
    [CreateAssetMenu(fileName = "ThreatData", menuName = "PlanetDefense/ThreatData")]
    public class ThreatData : ScriptableObject
    {
        [SerializeField] private float _speed = 2f;
        [SerializeField] private float _health = 10f;
        [SerializeField] private int _resources = 10;
        [SerializeField] private ThreatMovementType _movementType;
        [SerializeField] private ThreatModel _modelPrefab;

        public float Speed => _speed;
        public float Health => _health;
        public int Resources => _resources;
        public ThreatMovementType MovementType => _movementType;
        public ThreatModel ModelPrefab => _modelPrefab;
    }
}
