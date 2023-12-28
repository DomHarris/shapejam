using System;
using Stats;
using UnityEngine;

namespace Player
{
    public class ChargeParams : EventParams
    {
        public int NumCharges;
        public int MaxCharges;
        public GameObject Player;
        
    }
    /// <summary>
    /// Only allow the player's weapon to be fired a set number of times before recharging
    /// </summary>
    public class PlayerCharges : MonoBehaviour
    {
        // Serialized fields
        [SerializeField] private Stat charges;
        [SerializeField] private PlayerShoot shoot;

        [SerializeField] private Stat timeToRecharge;
        [SerializeField] private EventAction onCharge;
    
        // Events
        public event Action<float> ChargesChanged;
    
        // Private fields
        private int _currentCharges = 5;
        private float _timer;

        /// <summary>
        /// Called when the component is created
        /// </summary>
        private void Start()
        {
            shoot.Fire += OnFire;
            charges.ValueChanged += ChargesOnValueChanged;
            _currentCharges = Mathf.RoundToInt(charges);
        }

        private void ChargesOnValueChanged()
        {
            ChargesChanged?.Invoke(_currentCharges / charges);
        }

        /// <summary>
        /// Called when the component is destroyed
        /// </summary>
        private void OnDestroy()
        {
            shoot.Fire -= OnFire;
        }

        /// <summary>
        /// Called every frame
        /// </summary>
        private void Update()
        {
            // if we have max charges, don't do anything
            if (_currentCharges >= charges) return;
            
            // if we don't have max charges, recharge
            _timer -= Time.deltaTime;
            // if we've recharged, add a charge and reset the timer
            if (_timer <= 0)
            {
                _currentCharges++;
                _timer += timeToRecharge;
                shoot.SetCanFire(true);
                ChargesChanged?.Invoke(_currentCharges / charges);
                onCharge.TriggerAction(new ChargeParams { NumCharges = _currentCharges, MaxCharges = Mathf.RoundToInt(charges), Player = gameObject});
            }
        }

        
        /// <summary>
        /// Called when the player fires
        /// Remove a charge, reset the timer, and if we're out of charges, disable the player's ability to fire
        /// </summary>
        private void OnFire()
        {
            _currentCharges--;
            ChargesChanged?.Invoke(_currentCharges / charges);
            _timer = timeToRecharge;
            if (_currentCharges <= 0)
                shoot.SetCanFire(false);
        }
    }
}