using System;
using Stats;
using UnityEngine;

namespace Player.Abilities
{
    [CreateAssetMenu]
    public class StatModifierAbility : PlayerAbility
    {
        [SerializeField] private Stat stat;
        [SerializeField] private float modifier = 1f;
        [SerializeField] private ModifierType type = ModifierType.Multiply;
        [NonSerialized] private int _modifierIndex;
        
        public override void OnEquip()
        {
            _modifierIndex = stat.AddModifier(modifier, type);
        }
        
        public override void Teardown()
        {
            stat.RemoveModifier(_modifierIndex);
        }
    }
}