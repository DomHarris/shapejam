using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerShoot : MonoBehaviour
    {
        [SerializeField] private float minStartSize = 5f;
        [SerializeField] private float maxStartSize = 10f;
        [SerializeField] private float chargeTime = 1f;
        [field: SerializeField] public float chargeDelay { get; private set; } = 0.2f;
        private bool _canFire = true;
    
        public event Action Fire;
        public event Action<float> Charge;
    
        private ParticleSystem _particles;
        private PlayerInput _input;
        private Vector2 _target;
        private float _chargeTimer = 0f;
    
        public bool SetCanFire (bool canFire) => _canFire = canFire;

        private void Start()
        {
            _particles = GetComponent<ParticleSystem>();
            _input = GetComponentInParent<PlayerInput>();
            _input.actions["Fire"].performed += OnFire;
        }

        private void Update()
        {
            if (_chargeTimer > 0f && _canFire)
            {
                _chargeTimer -= Time.deltaTime;
                if (_chargeTimer < chargeTime)
                    Charge?.Invoke(1-Mathf.Clamp01(_chargeTimer / chargeTime));
            }
        }

        private void OnDestroy()
        {
            _input.actions["Fire"].performed -= OnFire;
        }

        private void OnFire(InputAction.CallbackContext ctx)
        {
            if (!_canFire) return;
            _particles.Emit(new ParticleSystem.EmitParams { startSize = Mathf.Lerp(maxStartSize, minStartSize, Mathf.Clamp01(_chargeTimer / chargeTime))}, 1);
            _chargeTimer = chargeTime + chargeDelay;
            Charge?.Invoke(0f);
            Fire?.Invoke();
        }
    }
}
