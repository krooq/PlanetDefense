using UnityEngine;
using System.Collections.Generic;

namespace Krooq.PlanetDefense
{
    public abstract class UpgradeTile : ScriptableObject
    {
        [SerializeField] private string _tileName;
        [SerializeField] private int _cost;
        [SerializeField] private Color _tileColor = Color.white;

        public string TileName => _tileName;
        public int Cost => _cost;
        public Color TileColor => _tileColor;

        // Returns true if the chain should continue immediately, false if this tile stops execution (e.g. Explosive waiting for impact)
        public abstract bool Process(ProjectileContext context, List<UpgradeTile> remainingChain, GameManager gameManager);
    }
}
