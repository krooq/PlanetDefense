using UnityEngine;
using Krooq.Common;
using Krooq.Core;

namespace Krooq.PlanetDefense
{
    public class Meteor : MonoBehaviour
    {
        [SerializeField] private float _speed = 2f;
        [SerializeField] private float _health = 10f;
        [SerializeField] private int _resources = 10;
        private Vector3 _direction = Vector3.down;
        protected GameManager GameManager => this.GetSingleton<GameManager>();

        public float Speed => _speed;
        public float Health => _health;
        public int Resources => _resources;

        public void Initialize(float speed, float health, int resources, Vector3 direction)
        {
            _speed = speed;
            _health = health;
            _resources = resources;
            _direction = direction;
        }

        protected void OnEnable()
        {
            if (GameManager != null) GameManager.RegisterMeteor(this);
        }

        protected void OnDisable()
        {
            if (GameManager != null) GameManager.UnregisterMeteor(this);
        }

        void Update()
        {
            transform.position += _direction * Speed * Time.deltaTime;
        }

        public void TakeDamage(float amount)
        {
            _health -= amount;
            if (_health <= 0)
            {
                Die();
            }
        }

        void Die()
        {
            GameManager.AddResources(Resources);
            Destroy(gameObject);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Building>(out var building))
            {
                building.TakeDamage(1);
                Destroy(gameObject);
            }
            else if (other.CompareTag("Ground"))
            {
                GameManager.TakeDamage(1);
                Destroy(gameObject);
            }
        }
    }
}
