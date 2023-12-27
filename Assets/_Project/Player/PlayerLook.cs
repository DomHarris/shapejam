using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Camera camera;
    
    private Rigidbody2D _rigidbody;
    private PlayerInput _input;
    private Vector2 _target;
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _input = GetComponent<PlayerInput>();
        _input.actions["Look"].performed += OnLook;
    }

    private void FixedUpdate()
    {
        var mousePos = camera.ScreenToWorldPoint(_target);
        var direction = (mousePos - transform.position).normalized;
        _rigidbody.MoveRotation(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
    }

    private void OnLook(InputAction.CallbackContext ctx)
    {
        _target = ctx.ReadValue<Vector2>();
    }

    private void OnDestroy()
    {
        _input.actions["Look"].performed -= OnLook;
    }
}
