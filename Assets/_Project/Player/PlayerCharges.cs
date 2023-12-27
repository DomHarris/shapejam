using System;
using UnityEngine;

namespace Player
{
    public class PlayerCharges : MonoBehaviour
    {
        [SerializeField] private int charges = 5;
        [SerializeField] private PlayerShoot shoot;

        [SerializeField] private float timeToRecharge = 1f;
    
        public event Action<float> ChargesChanged;
    
        private int _currentCharges = 5;
        private float _timer;

        private void Start()
        {
            shoot.Fire += OnFire;
            _currentCharges = charges;
        }

        private void OnDestroy()
        {
            shoot.Fire -= OnFire;
        }

        private void Update()
        {
            if (_currentCharges < charges)
            {
                _timer -= Time.deltaTime;
                if (_timer <= 0)
                {
                    _currentCharges++;
                    shoot.SetCanFire(true);
                    ChargesChanged?.Invoke(_currentCharges / (float)charges);
                    _timer += timeToRecharge;
                }
            }
        }

        private void OnFire()
        {
            _currentCharges--;
            ChargesChanged?.Invoke(_currentCharges / (float)charges);
            _timer = timeToRecharge;
            if (_currentCharges <= 0)
                shoot.SetCanFire(false);
        }
    }
}