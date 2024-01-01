using System;
using System.Collections;
using System.Collections.Generic;
using Entity;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    [SerializeField] private float minSpeed = 6f;
    [SerializeField] private float damage = 1f;
    private EntityHealth _health;
    private float _minSpeedSqr;

    private void Start()
    {
        _health = GetComponent<EntityHealth>();
        _minSpeedSqr = minSpeed * minSpeed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.relativeVelocity.sqrMagnitude < _minSpeedSqr) return;
        _health.Hit(damage);
    }
}
