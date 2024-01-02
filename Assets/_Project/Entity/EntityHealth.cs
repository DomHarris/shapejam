using System;
using JetBrains.Annotations;
using Stats;
using UnityEngine;
using UnityEngine.Serialization;

namespace Entity
{
    /// <summary>
    /// The parameters for a hit event
    /// </summary>
    public class HitParams : EventParams
    {
        public GameObject Object;
        public float Damage;
        public float CurrentHealthPercentage;
    }
    /// <summary>
    /// Track the health of an entity
    /// </summary>
    public class EntityHealth : MonoBehaviour, IHit
    {
        // Serialized fields
        [SerializeField] private Stat maxHealth;
        public float MaxHealth => maxHealth;

        [SerializeField, CanBeNull] private EventAction onHit;
        [field: SerializeField] public EventAction Die { get; private set; }

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

        private void OnEnable()
        {
            _currentHealth = MaxHealth;
        }

        public void SetDeathEvent (EventAction newEvent)
        {
            Die = newEvent;
        }
        
        /// <summary>
        /// Called when the object is hit
        /// </summary>
        /// <param name="damage">The amount of damage from the hit</param>
        public void Hit(float damage)
        {
            // don't get hit if this object is disabled
            if (!enabled) return;
            _currentHealth -= damage;
            if (onHit != null)
                onHit.TriggerAction(new HitParams { Object = gameObject, Damage = damage, CurrentHealthPercentage = _currentHealth / maxHealth});
            OnHit?.Invoke();
            
            if (_currentHealth <= 0)
            {
                OnDie?.Invoke();
                if (Die != null) 
                    Die.TriggerAction(new HitParams { Object = gameObject, Damage = damage, CurrentHealthPercentage = _currentHealth / maxHealth});
            }
        }
    }
}