using UnityEngine;

namespace Player.Abilities
{
    public abstract class PlayerAbility : ScriptableObject
    {
        [field: SerializeField] public float Weight { get; private set; } = 1;
        [field: SerializeField] public bool OnlyOne { get; private set; } = true;
        public abstract void OnEquip();
        public abstract void Teardown();
    }
}