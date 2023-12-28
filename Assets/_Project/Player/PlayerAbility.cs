using UnityEngine;

namespace Player
{
    public abstract class PlayerAbility : ScriptableObject
    {
        [field: SerializeField] public float Weight { get; private set; }

        public abstract void OnEquip();

        public abstract void Teardown();
    }
}