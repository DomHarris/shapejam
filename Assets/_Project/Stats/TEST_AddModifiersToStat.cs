using System.Collections.Generic;
using UnityEngine;

namespace Stats
{
    public class TEST_AddModifiersToStat : MonoBehaviour
    {
        [SerializeField] private Stat stat;
        [SerializeField] private float multiplier = 1.4f;
        
        private Stack<int> _modifiers = new();

        private void OnEnable()
        {
            _modifiers.Push(stat.AddModifier(multiplier));
        }

        private void OnDisable()
        {
            stat.RemoveModifier(_modifiers.Pop());
        }
    }
}