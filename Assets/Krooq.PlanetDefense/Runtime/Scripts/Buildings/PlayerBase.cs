using UnityEngine;
using Krooq.Common;
using Krooq.Core;

namespace Krooq.PlanetDefense
{
    public class PlayerBase : MonoBehaviour
    {
        protected GameManager GameManager => this.GetSingleton<GameManager>();

        public void TakeDamage(int amount)
        {
            GameManager.TakeDamage(amount);
        }
    }
}
