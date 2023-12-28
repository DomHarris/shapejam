using Entity;
using UnityEngine;

namespace Player
{
    public class ExperienceOnEnemyDeath : MonoBehaviour
    {
        [SerializeField] private float experienceToAdd;
        private EntityHealth _health;
        private PlayerExperience _playerExperience;
        
        private void Start()
        {
            _health = GetComponent<EntityHealth>();
            _playerExperience = FindFirstObjectByType<PlayerExperience>();
            _health.OnDie += OnDeath;
        }

        private void OnDestroy()
        {
            _health.OnDie -= OnDeath;
        }

        private void OnDeath()
        {
            _playerExperience.AddExperience(experienceToAdd);
        }
    }
}