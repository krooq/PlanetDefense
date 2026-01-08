using Krooq.Common;
using Cysharp.Threading.Tasks;

namespace Krooq.PlanetDefense
{
    public abstract class Ability : IAbility
    {
        protected Player Player;
        protected IAbilitySource Source;

        public virtual void Init(Player player, IAbilitySource source)
        {
            Player = player;
            Source = source;
        }

        public abstract UniTask OnGameEvent(IGameEvent gameEvent);
    }
}
