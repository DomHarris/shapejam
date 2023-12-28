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
        [field: SerializeField] public float InitialValue { get; private set; }

        public float Value =>
            InitialValue * _modifiers.Where(m => m.type == ModifierType.Multiply).Aggregate(1f, (current, modifier) => current * modifier.value) + _modifiers.Where(m => m.type == ModifierType.Add).Sum(m => m.value);

        [NonSerialized] private readonly List<(float value, ModifierType type)> _modifiers = new ();
        public event Action ValueChanged;
        
        public int AddModifier (float modifier, ModifierType type)
        {
            _modifiers.Add((modifier, type));
            return _modifiers.Count - 1;
        }
    
        public void RemoveModifier (int index)
        {
            _modifiers.RemoveAt(index);
        }
        
        // implicit float to Stat conversion - base value multiplied by all modifiers
        public static implicit operator float(Stat stat)
        {
            return stat.Value;
        }
    }
}