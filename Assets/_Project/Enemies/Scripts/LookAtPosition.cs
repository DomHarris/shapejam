using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class LookAtPosition : MonoBehaviour
{
    [SerializeField] private SerialPosition position;
    [SerializeField] private float speed = .5f;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.up = Vector3.RotateTowards(transform.up, (position.Position - transform.position).normalized, speed * Time.deltaTime, 0);
    }
}