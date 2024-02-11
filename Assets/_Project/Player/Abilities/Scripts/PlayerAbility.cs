using System;
using UnityEngine;

namespace Player.Abilities
{
    /// <summary>
    /// Base class for player abilities
    /// </summary>
    public abstract class PlayerAbility : ScriptableObject
    {
        [field: SerializeField, Tooltip("How rare is this ability? Lower = more rare, e.g. 0.1 for super rare, 1 for common")] 
        public float Weight { get; private set; } = 1;
        [field: SerializeField, Tooltip("Should this ability appear more than once?")] 
        public bool OnlyOne { get; private set; } = true;
        
        [field: SerializeField, Tooltip("A short description to let the player know what the ability does")]
        public string Description { get; private set; }
        
        [field: SerializeField, Tooltip("The abilities to remove from the pool when this ability is equipped")]
        public PlayerAbility[] AbilitiesToRemove { get; private set; } = Array.Empty<PlayerAbility>();
        
        [field: SerializeField, Tooltip("The abilities to add to the pool when this ability is equipped")]
        public PlayerAbility[] AbilitiesToAdd { get; private set; } = Array.Empty<PlayerAbility>();
        
        /// <summary>
        /// Called when the ability is equipped
        /// </summary>
        public abstract void OnEquip();
        
        /// <summary>
        /// Called when the game is over
        /// </summary>
        public abstract void Teardown();
    }
}