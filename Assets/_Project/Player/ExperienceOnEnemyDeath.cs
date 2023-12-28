using Entity;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// Add experience to the player when an enemy dies
    /// </summary>
    public class ExperienceOnEnemyDeath : MonoBehaviour
    {
        // Serialized fields
        [SerializeField] private float experienceToAdd;
        
        // Private fields
        private EntityHealth _health;
        private PlayerExperience _playerExperience;
        
        /// <summary>
        /// Called when the object is created
        /// </summary>
        private void Start()
        {
            _health = GetComponent<EntityHealth>();
            _playerExperience = FindFirstObjectByType<PlayerExperience>();
            _health.OnDie += OnDeath;
        }

        /// <summary>
        /// Called when the object is destroyed
        /// </summary>
        private void OnDestroy()
        {
            _health.OnDie -= OnDeath;
        }

        /// <summary>
        /// Called when the enemy dies
        /// </summary>
        private void OnDeath()
        {
            _playerExperience.AddExperience(experienceToAdd);
        }
    }
}