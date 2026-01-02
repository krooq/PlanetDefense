using UnityEngine;

namespace Krooq.PlanetDefense
{
    public class Building : MonoBehaviour
    {
        [SerializeField] private int _health = 3;
        public int Health => _health;

        public void TakeDamage(int amount)
        {
            _health -= amount;
            if (_health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
