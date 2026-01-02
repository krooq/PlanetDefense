using UnityEngine;
using System.Collections.Generic;

namespace Krooq.PlanetDefense
{
    public static class TileSequence
    {
        public static void RunChain(ProjectileContext context, List<UpgradeTile> chain)
        {
            if (chain == null || chain.Count == 0) return;

            // Create a copy of the list to avoid modifying the original if we pass it around
            List<UpgradeTile> currentChain = new List<UpgradeTile>(chain);

            while (currentChain.Count > 0)
            {
                UpgradeTile tile = currentChain[0];
                currentChain.RemoveAt(0);

                bool continueChain = tile.Process(context, currentChain);
                if (!continueChain)
                {
                    break;
                }
            }
        }
    }
}
