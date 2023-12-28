using System;
using Stats;
using UnityEngine;

namespace Entity
{
    /// <summary>
    /// The parameters for a hit event
    /// </summary>
    public class HitParams : EventParams
    {
        public GameObject Object;
        public float Damage;
    }
    /// <summary>
    /// Track the health of an entity
    /// </summary>
    public class EntityHealth : MonoBehaviour, IHit
    {
        // Serialized fields
        [SerializeField] public Stat maxHealth;
        public float MaxHealth => maxHealth;

        [SerializeField] private EventAction onHit;
        [SerializeField] private EventAction onDie;

        // Events
        public event Action OnHit;
        public event Action OnDie;
        
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
            onHit.TriggerAction(new HitParams { Object = gameObject, Damage = damage });
            OnHit?.Invoke();
            
            if (_currentHealth <= 0)
            {
                OnDie?.Invoke();
                onDie.TriggerAction(new HitParams { Object = gameObject, Damage = damage });
            }
        }
    }
}