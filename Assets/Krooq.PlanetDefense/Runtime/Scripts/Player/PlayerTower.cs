using UnityEngine;
using Krooq.Common;
using Krooq.Core;

namespace Krooq.PlanetDefense
{
    public class PlayerTower : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        protected Player Player => this.GetSingleton<Player>();

        public void TakeDamage(int amount)
        {
            Player.TakeDamage(amount);
            if (Player.CurrentHealth <= 0) _animator.SetTrigger("Death");
        }
        public void Attack()
        {
            _animator.SetTrigger("Attack");
        }
    }
}
