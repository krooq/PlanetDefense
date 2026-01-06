using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Krooq.PlanetDefense
{
    public enum SpellEffectType
    {
        FireProjectile,
        CastSlot,
        ModifyNextSpell,
        ApplyModifiers,
        Custom,
    }

    [System.Serializable]
    public class SpellEffect
    {
        [SerializeField] private SpellEffectType _type;

        [SerializeField] private bool _hasCondition;
        [SerializeField, ShowIf("_hasCondition")] private SpellCondition _condition;

        // Fire Projectile
        [SerializeField, ShowIf("_type", SpellEffectType.FireProjectile)]
        private ProjectileWeaponData _projectileData;

        [SerializeField, ShowIf("_type", SpellEffectType.FireProjectile)]
        private List<Modifier> _projectileModifiers = new();

        // Cast Slot
        [SerializeField, ShowIf("_type", SpellEffectType.CastSlot)]
        private int _slotOffset; // e.g. 1 for right, -1 for left

        [SerializeField, ShowIf("_type", SpellEffectType.CastSlot)]
        private float _manaCostMultiplier = 0.5f;

        // Modify Next Spell
        [SerializeField, ShowIf("_type", SpellEffectType.ModifyNextSpell)]
        private float _damageMultiplier = 2f;

        public SpellEffectType Type => _type;
        public bool HasCondition => _hasCondition;
        public SpellCondition Condition => _condition;

        public ProjectileWeaponData ProjectileData => _projectileData;
        public IReadOnlyList<Modifier> ProjectileModifiers => _projectileModifiers;

        public int SlotOffset => _slotOffset;
        public float ManaCostMultiplier => _manaCostMultiplier;

        public float DamageMultiplier => _damageMultiplier;
    }

    [System.Serializable]
    public class SpellCondition
    {
        public enum ConditionType
        {
            PreviousSpellTag,
            ManaPercentageAbove,
            Chance
        }

        public ConditionType Type;

        [ShowIf("Type", ConditionType.PreviousSpellTag)]
        public string Tag;

        [ShowIf("Type", ConditionType.ManaPercentageAbove)]
        public float MinManaPercent;

        [ShowIf("Type", ConditionType.Chance)]
        public float Chance;
    }
}
