using UnityEngine;
using Krooq.Core;
using Krooq.Common;

namespace Krooq.PlanetDefense
{
    public class Building : MonoBehaviour
    {
        [SerializeField] private int _health = 3;
        protected GameManager GameManager => this.GetSingleton<GameManager>();
        public int Health => _health;

        public void TakeDamage(int amount)
        {
            _health -= amount;
            if (_health <= 0)
            {
                GameManager.Despawn(gameObject);
            }
        }
    }
}
