using System.Collections.Generic;
using UnityEngine;

namespace Stats
{
    public class TEST_AddModifiersToStat : MonoBehaviour
    {
        [SerializeField] private Stat stat;
        [SerializeField] private float modifier = 1.4f;
        [SerializeField] private ModifierType type = ModifierType.Multiply;
        
        private Stack<int> _modifiers = new();

        private void OnEnable()
        {
            _modifiers.Push(stat.AddModifier(modifier, type));
        }

        private void OnDisable()
        {
            stat.RemoveModifier(_modifiers.Pop());
        }
    }
}