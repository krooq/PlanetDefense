using UnityEngine;
using Krooq.Common;
using Krooq.Core;

namespace Krooq.PlanetDefense
{
    public class PlayerTower : MonoBehaviour
    {
        protected Player Player => this.GetSingleton<Player>();

        public void TakeDamage(int amount)
        {
            Player.TakeDamage(amount);
        }
    }
}
