using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Stats
{
    public enum ModifierType { Multiply, Add }
    
    /// <summary>
    /// A stat that can be modified by other scripts
    /// </summary>
    [CreateAssetMenu]
    public class Stat : ScriptableObject
    {
        // Serialized fields
        [field: SerializeField, Tooltip("The base value of the stat")] 
        public float InitialValue { get; private set; }
        
        // Public properties
        public float Value
        {
            get
            {
                // Return the base value multiplied by all multiplicative modifiers and added to all additive modifiers
                return InitialValue * 
                       _modifiers.Where(m => m.type == ModifierType.Multiply)
                           .Aggregate(1f, (current, modifier) => current * modifier.value) +
                       _modifiers.Where(m => m.type == ModifierType.Add)
                           .Sum(m => m.value);
            }
        }
        // Events
        public event Action ValueChanged;

        // Private fields
        [NonSerialized] 
        private readonly List<(float value, ModifierType type)> _modifiers = new ();
        
        /// <summary>
        /// Add a modifier to the stat
        /// </summary>
        /// <param name="modifier">The value of the modifier</param>
        /// <param name="type">The type of modifier</param>
        /// <returns>The index of the added modifier - use this to remove it when needed</returns>
        public int AddModifier (float modifier, ModifierType type)
        {
            _modifiers.Add((modifier, type));
            return _modifiers.Count - 1;
        }
    
        /// <summary>
        /// Remove a modifier from the stat
        /// </summary>
        /// <param name="index">The index you get from AddModifier</param>
        public void RemoveModifier (int index)
        {
            _modifiers.RemoveAt(index);
        }

        public void ClearModifiers()
        {
            _modifiers.Clear();
        }
        
        /// <summary>
        /// Implicitly convert the stat to a float
        /// </summary>
        /// <param name="stat">The stat to convert</param>
        /// <returns>The final value of the stat, after modification</returns>
        public static implicit operator float(Stat stat)
        {
            return stat.Value;
        }
    }
}