using System;
using Stats;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class ShootEventParams : EventParams
    {
        public Vector3 Position;
        public float ChargePercent;
    }
    
    /// <summary>
    /// Shoot a particle with size based on the charge time.
    /// </summary>
    public class PlayerShoot : MonoBehaviour
    {
        // Serialized fields
        [SerializeField] private float minStartSize = 5f;
        [SerializeField] private float maxStartSize = 10f;
        [SerializeField] private Stat chargeTime;

        [SerializeField] private Stat chargeDelay;
        
        [SerializeField] private EventAction shootEvent;
        public float ChargeDelay => chargeDelay;
        
        // Private fields: state
        private bool _canFire = true;
        private Vector2 _target;
        private float _chargeTimer = 0f;
        
        // Private fields: object references
        private ParticleSystem _particles;
        private PlayerInput _input;
    
        // Events
        public event Action Fire;
        public event Action<float> Charge;
        
    
        /// <summary>
        /// Sets whether the player can fire or not.
        /// </summary>
        /// <param name="canFire"></param>
        public void SetCanFire (bool canFire) => _canFire = canFire;
        
        /// <summary>
        /// Called when the object is created.
        /// Grab the relevant object references and subscribe to any input actions.
        /// </summary>
        private void Start()
        {
            _particles = GetComponent<ParticleSystem>();
            _input = GetComponentInParent<PlayerInput>();
            _input.actions["Fire"].performed += OnFire;
        }

        /// <summary>
        /// Called every frame.
        /// </summary>
        private void Update()
        {
            // only update the timer if we can fire
            if (_chargeTimer < 0f || !_canFire) return;
            _chargeTimer -= Time.deltaTime;
            
            // only invoke the charge event if we're not in the cooldown period
            if (_chargeTimer < chargeTime)
                // 0 is fully charged, 1 is not charged, so invoke the charge event with 1-timer/chargeTime
                Charge?.Invoke(1- _chargeTimer / chargeTime);
        }

        /// <summary>
        /// Called when the object is destroyed.
        /// Unsubscribe from any input actions.
        /// </summary>
        private void OnDestroy()
        {
            _input.actions["Fire"].performed -= OnFire;
        }

        /// <summary>
        /// Called when the player presses the fire button.
        /// </summary>
        /// <param name="ctx"></param>
        private void OnFire(InputAction.CallbackContext ctx)
        {
            // if we can't fire, don't do anything
            if (!_canFire) return;
            
            // Emit a single particle with a new start size
            // The start size is lerped between the max and min start size based on the charge timer
            _particles.Emit(new ParticleSystem.EmitParams
            {
                startSize = Mathf.Lerp(maxStartSize, minStartSize, Mathf.Clamp01(_chargeTimer / chargeTime))
            }, 1);
            
            shootEvent.TriggerAction(new ShootEventParams { Position = transform.position, ChargePercent = 1 - Mathf.Clamp01(_chargeTimer / chargeTime) });
            
            // Reset the charge timer and invoke the charge and fire events
            _chargeTimer = chargeTime + chargeDelay;
            Charge?.Invoke(0f);
            Fire?.Invoke();
        }
    }
}
