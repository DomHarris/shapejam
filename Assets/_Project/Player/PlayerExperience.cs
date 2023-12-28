using System;
using Stats;
using UnityEngine;

namespace Player
{
    public class ExperienceParams : EventParams
    {
        public int CurrentLevel;
        public GameObject Player;
    }
    public class PlayerExperience : MonoBehaviour
    {
        [SerializeField] private Stat experienceMultiplier;
        [SerializeField] private EventAction onLevelUp;
        [SerializeField] private float[] experienceThresholds;

        public event Action<int> OnLevelUp;
        
        private int _currentLevel = 1;
        private float _currentExperience = 0f;
        
        public void AddExperience(float experienceToAdd)
        {
            _currentExperience += experienceToAdd * experienceMultiplier;
            if (experienceThresholds.Length < _currentLevel && _currentExperience > experienceThresholds[_currentLevel - 1])
            {
                _currentLevel++;
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