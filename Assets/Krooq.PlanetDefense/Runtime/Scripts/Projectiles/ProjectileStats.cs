using System;
using UnityEngine;

namespace Krooq.PlanetDefense
{
    [Serializable]
    public class ProjectileStats
    {
        [SerializeField] private float _damage = 10f;
        [SerializeField] private float _speed = 10f;
        [SerializeField] private float _size = 1f;
        [SerializeField] private int _pierceCount = 0;

        public float Damage => _damage;
        public float Speed => _speed;
        public float Size => _size;
        public int PierceCount => _pierceCount;

        public void SetDamage(float value) => _damage = value;
        public void SetSpeed(float value) => _speed = value;
        public void SetSize(float value) => _size = value;
        public void SetPierceCount(int value) => _pierceCount = value;

        public ProjectileStats Clone()
        {
            return (ProjectileStats)this.MemberwiseClone();
        }
    }
}