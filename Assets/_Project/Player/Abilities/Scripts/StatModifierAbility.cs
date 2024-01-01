using System;
using Stats;
using UnityEngine;

namespace Player.Abilities
{
    /// <summary>
    /// Ability that modifies a single stat
    /// </summary>
    [CreateAssetMenu]
    public class StatModifierAbility : PlayerAbility
    {
        // Serialized fields
        [SerializeField, Tooltip("The stat to modify")] 
        private Stat stat;
        [SerializeField, Tooltip("The modifier value")] 
        private float modifier = 1f;
        [SerializeField, Tooltip("The type of modification")] 
        private ModifierType type = ModifierType.Multiply;
        
        // Private fields
        [NonSerialized] private int _modifierIndex;
        
        /// <summary>
        /// Called when the ability is equipped
        /// </summary>
        public override void OnEquip()
        {
            // Add the modifier to the stat, store the index so we can remove it later
            _modifierIndex = stat.AddModifier(modifier, type);
        }
        
        /// <summary>
        /// Called when the game is over
        /// </summary>
        public override void Teardown()
        {
            // Remove the modifier from the stat
            stat.RemoveModifier(_modifierIndex);
        }
    }
}