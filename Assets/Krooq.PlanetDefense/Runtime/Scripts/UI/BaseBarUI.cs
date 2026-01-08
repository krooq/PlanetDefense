using UnityEngine;
using Krooq.Common;
using Krooq.Core;
using Sirenix.OdinInspector;

namespace Krooq.PlanetDefense
{
    public abstract class BaseBarUI : MonoBehaviour
    {
        [SerializeField] protected Transform _slotContainer;

        protected GameManager GameManager => this.GetSingleton<GameManager>();

        protected virtual void OnEnable()
        {
            if (_slotContainer.childCount == 0)
            {
                for (int i = 0; i < MaxSlots; i++)
                {
                    var slot = SpawnSlot();
                    slot.transform.SetParent(_slotContainer, false);
                    slot.Init(i);
                }
            }
            Refresh();
        }

        public void Refresh()
        {
            foreach (Transform slotTransform in _slotContainer)
            {
                for (var i = slotTransform.childCount - 1; i >= 0; i--)
                    GameManager.Despawn(slotTransform.GetChild(i).gameObject);
            }

            OnRefreshTiles();
        }

        protected abstract int MaxSlots { get; }
        protected abstract BaseSlotUI SpawnSlot();
        protected abstract void OnRefreshTiles();
    }
}
