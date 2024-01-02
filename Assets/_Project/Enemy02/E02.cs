using System;
using Enemies;
using UnityEngine;
using Random = UnityEngine.Random;

public class E02 : MonoBehaviour
{
    [SerializeField] private SerialPosition playerPosition;
    private bool _activateEnemy;
    private Vector2 _playerPos;
    private Vector2 _enemyPos;
    [SerializeField] private bool attackMode;
    private bool _isMoving;
    [SerializeField] private float minimum = 1;
    [SerializeField] private float maximum = 5;
    [SerializeField] SpriteRenderer spriteRenderer;
    
    
}
