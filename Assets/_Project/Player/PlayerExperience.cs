using System;
using System.Collections.Generic;
using Stats;
using UnityEngine;

namespace Player
{
    public class PlayerExperience : MonoBehaviour
    {
        [SerializeField] private List<PlayerAbility> abilities;
        [SerializeField] private Stat experienceMultiplier;

        private float _currentExperience = 0f;
        
        public void AddExperience(float experienceToAdd)
        {
            _currentExperience += experienceToAdd * experienceMultiplier;
        }
    }
}