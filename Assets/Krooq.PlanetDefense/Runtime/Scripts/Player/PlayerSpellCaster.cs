using UnityEngine;
using System.Collections.Generic;
using Krooq.Core;
using Krooq.Common;
using Sirenix.OdinInspector;

namespace Krooq.PlanetDefense
{
    public class PlayerSpellCaster : MonoBehaviour
    {
        [SerializeField] private PlayerWeapon _weapon;
        [SerializeField] private PlayerTargetingReticle _targetingReticle;

        [SerializeField, ReadOnly] private int _selectedSlotIndex = 0;
        [SerializeField, ReadOnly] private Spell _lastCastSpell;
        [SerializeField, ReadOnly] private float _nextSpellDamageMultiplier = 1f;

        protected GameManager GameManager => this.GetSingleton<GameManager>();
        protected Player Player => this.GetSingleton<Player>();

        private void Update()
        {
            if (GameManager.State != GameState.Playing) return;
            HandleInput();
        }

        private void HandleInput()
        {
            // Slot Selection
            if (Player.Inputs.Spell1Action.WasPressedThisFrame()) _selectedSlotIndex = 0;
            if (Player.Inputs.Spell2Action.WasPressedThisFrame()) _selectedSlotIndex = 1;
            if (Player.Inputs.Spell3Action.WasPressedThisFrame()) _selectedSlotIndex = 2;
            if (Player.Inputs.Spell4Action.WasPressedThisFrame()) _selectedSlotIndex = 3;

            // Firing
            if (Player.Inputs.ClickAction.WasPressedThisFrame())
            {
                CastSelectedSpell();
            }

            // Quick Casting
            if (Player.Inputs.QuickCast1Action.WasPressedThisFrame()) CastSpell(0);
            if (Player.Inputs.QuickCast2Action.WasPressedThisFrame()) CastSpell(1);
            if (Player.Inputs.QuickCast3Action.WasPressedThisFrame()) CastSpell(2);
            if (Player.Inputs.QuickCast4Action.WasPressedThisFrame()) CastSpell(3);
        }

        private void CastSelectedSpell() => CastSpell(_selectedSlotIndex);

        private void CastSpell(int slotIndex)
        {
            var spells = GameManager.Spells;
            if (slotIndex < 0 || slotIndex >= spells.Count) return;
            var spell = spells[slotIndex];
            if (spell == null) return;

            // Capture current multiplier
            float dmgMult = _nextSpellDamageMultiplier;

            // Reset for next separate cast chain
            _nextSpellDamageMultiplier = 1f;

            ProcessSpell(spell, slotIndex, 1f, dmgMult);
        }

        private void ProcessSpell(Spell spell, int slotIndex, float manaCostMult, float damageMult)
        {
            float cost = spell.ManaCost * manaCostMult;
            if (!Player.TrySpendMana(cost))
            {
                // TODO: Feedback
                return;
            }

            foreach (var effect in spell.Effects)
            {
                if (CheckCondition(effect, _lastCastSpell))
                {
                    ApplyEffect(effect, spell, slotIndex, damageMult);
                }
            }

            _lastCastSpell = spell;
        }

        private bool CheckCondition(SpellEffect effect, Spell lastSpell)
        {
            if (!effect.HasCondition) return true;
            var condition = effect.Condition;

            switch (condition.Type)
            {
                case SpellCondition.ConditionType.PreviousSpellTag:
                    if (lastSpell == null) return false;
                    foreach (var tag in lastSpell.Tags) if (tag == condition.Tag) return true;
                    return false;
                case SpellCondition.ConditionType.ManaPercentageAbove:
                    if (GameManager.Data.BaseMana <= 0) return false;
                    return (GameManager.CurrentMana / GameManager.Data.BaseMana) >= condition.MinManaPercent;
                case SpellCondition.ConditionType.Chance:
                    return Random.value <= condition.Chance;
            }
            return true;
        }

        private void ApplyEffect(SpellEffect effect, Spell sourceSpell, int sourceSlotIndex, float damageMult)
        {
            if (effect.Type == SpellEffectType.FireProjectile)
            {
                _weapon.TryFire(_targetingReticle, effect.ProjectileData, effect.ProjectileModifiers, damageMult);
            }
            else if (effect.Type == SpellEffectType.CastSlot)
            {
                int targetSlot = sourceSlotIndex + effect.SlotOffset;
                var spells = GameManager.Spells;
                if (targetSlot >= 0 && targetSlot < spells.Count)
                {
                    var targetSpell = spells[targetSlot];
                    if (targetSpell != null)
                    {
                        // Recursive cast
                        ProcessSpell(targetSpell, targetSlot, effect.ManaCostMultiplier, damageMult);
                    }
                }
            }
            else if (effect.Type == SpellEffectType.ModifyNextSpell)
            {
                _nextSpellDamageMultiplier *= effect.DamageMultiplier;
            }
        }
    }
}
