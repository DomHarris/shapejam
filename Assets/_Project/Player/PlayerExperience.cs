using System;
using Stats;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// Parameters for the level up event
    /// </summary>
    public class ExperienceParams : EventParams
    {
        public int CurrentLevel;
        public GameObject Player;
    }
    
    /// <summary>
    /// Container for the player's experience and level
    /// </summary>
    public class PlayerExperience : MonoBehaviour
    {
        // Serialized fields
        [SerializeField, Tooltip("Any multipliers for player experience gain")] 
        private Stat experienceMultiplier;
        [SerializeField, Tooltip("An event to run when the player levels up")] 
        private EventAction onLevelUp;
        [SerializeField, Tooltip("The thresholds for levelling up")] 
        private float[] experienceThresholds;

        [SerializeField] private EventAction onGameStart;
        
        // Events
        public event Action<int> OnLevelUp;
        
        // Private fields
        private int _currentLevel = 1;
        private float _currentExperience = 0f;

        /// <summary>
        /// Called when the level is loaded
        /// </summary>
        private void Start()
        {
            onGameStart += OnGameStart;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            onGameStart -= OnGameStart;
        }

        private void OnGameStart(EventParams obj)
        {
            gameObject.SetActive(false);
            _currentLevel = 1;
            _currentExperience = 0f;
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Add experience to the player
        /// </summary>
        /// <param name="experienceToAdd">The amount of experience to add</param>
        public void AddExperience(float experienceToAdd)
        {
            // Add the experience using the multiplier
            _currentExperience += experienceToAdd * experienceMultiplier;
            // Check if the player has levelled up
            if (_currentLevel < experienceThresholds.Length && _currentExperience >= experienceThresholds[_currentLevel - 1])
            {
                // Level up
                _currentLevel++;
                // Invoke the events
                OnLevelUp?.Invoke(_currentLevel);
                onLevelUp.TriggerAction(new ExperienceParams
                {
                    CurrentLevel = _currentLevel,
                    Player = gameObject
                });
            }
        }
    }
}