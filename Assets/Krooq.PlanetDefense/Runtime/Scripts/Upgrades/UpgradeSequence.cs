using UnityEngine;
using System.Collections.Generic;

namespace Krooq.PlanetDefense
{
    public static class UpgradeSequence
    {
        public static void RunChain(ProjectileContext context, List<Upgrade> chain, GameManager gameManager)
        {
            if (chain == null || chain.Count == 0) return;

            // Create a copy of the list to avoid modifying the original if we pass it around
            var currentChain = new List<Upgrade>(chain);

            while (currentChain.Count > 0)
            {
                var upgrade = currentChain[0];
                currentChain.RemoveAt(0);

                var continueChain = upgrade.Process(context, currentChain, gameManager);
                if (!continueChain)
                {
                    break;
                }
            }
        }
    }
}
