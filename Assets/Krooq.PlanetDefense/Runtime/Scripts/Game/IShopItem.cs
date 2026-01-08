using UnityEngine;

namespace Krooq.PlanetDefense
{
    public interface IShopItem
    {
        string Name { get; }
        string Description { get; }
        Sprite Icon { get; }
        int ShopCost { get; }
    }
}
