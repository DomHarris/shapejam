using System;
using UnityEngine;

namespace Entity
{
    /// <summary>
    /// Track the health of an entity
    /// </summary>
    public class EntityHit : MonoBehaviour, IHit
    {
        // Serialized fields
        [field: SerializeField] public float MaxHealth { get; private set; }

        // Private fields
        private float _currentHealth;
        
        /// <summary>
        /// Called when the object is created.
        /// </summary>
        private void Start()
        {
            _currentHealth = MaxHealth;
        }
        
        /// <summary>
        /// Called when the object is hit
        /// </summary>
        /// <param name="damage">The amount of damage from the hit</param>
        public void Hit(float damage)
        {
            _currentHealth -= damage;
            if (_currentHealth <= 0)
                Destroy(gameObject);
        }
    }
}