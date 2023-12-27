using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    private bool _canFire = true;
    
    public event Action Fire;
    private ParticleSystem _particles;
    private PlayerInput _input;
    private Vector2 _target;
    
    public bool SetCanFire (bool canFire) => _canFire = canFire;

    private void Start()
    {
        _particles = GetComponent<ParticleSystem>();
        _input = GetComponentInParent<PlayerInput>();
        _input.actions["Fire"].performed += OnFire;
    }

    private void OnDestroy()
    {
        _input.actions["Fire"].performed -= OnFire;
    }

    private void OnFire(InputAction.CallbackContext ctx)
    {
        if (!_canFire) return;
        _particles.Emit(1);
        Fire?.Invoke();
    }
}
