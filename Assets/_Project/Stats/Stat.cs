using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Stats
{
    /// <summary>
    /// A stat that can be modified by other scripts
    /// </summary>
    [CreateAssetMenu]
    public class Stat : ScriptableObject
    {
        [field: SerializeField] public float InitialValue { get; private set; }

        public float Value =>
            InitialValue * _modifiers.Aggregate(1f, (current, modifier) => current * modifier);

        [NonSerialized] private readonly List<float> _modifiers = new ();
    
        public event Action ValueChanged;
        
        public int AddModifier (float modifier)
        {
            _modifiers.Add(modifier);
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