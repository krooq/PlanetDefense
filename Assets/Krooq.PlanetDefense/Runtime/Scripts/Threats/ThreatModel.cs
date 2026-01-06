using UnityEngine;
using Krooq.Core;
using Sirenix.OdinInspector;
using Krooq.Common;

namespace Krooq.PlanetDefense
{
    public class ThreatModel : MonoBehaviour
    {
        [SerializeField, ReadOnly] private Threat _threat;
        [SerializeField, ReadOnly] private float _hopInterval = 0.3f;

        private float _nextBumpTime;

        protected SpringTransform SpringTransform => this.GetCachedComponent<SpringTransform>();

        public void Init(Threat threat)
        {
            _threat = threat;
        }

        public void Update()
        {
            if (_threat == null) return;
            if (_threat.Data.MovementType != ThreatMovementType.Ground) return;
            if (Time.time >= _nextBumpTime)
            {
                SpringTransform.BumpPosition(Vector3.up * 10f);
                _nextBumpTime = Time.time + _hopInterval;
            }
        }
    }
}
