using System.Collections;
using Enemies;
using UnityEngine;
using UnityEngine.Serialization;

public class E02 : MonoBehaviour, ITokenUser
{
    public float enemySpeed;
    public float playerDistance;
    public float retreatDistance;
    [SerializeField] private SerialPosition playerPosition;
    private Vector3 _playerPos;
    private Transform _playerTransform;
    private bool _activateEnemy;
    private Vector2 _enemyPos;
    
    [SerializeField] private int bulletBurstAmount = 1;
    private bool _isMoving;
    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] private new ParticleSystem particleSystem;
    [SerializeField] private AttackTokenHolder tokenHolder;
    [SerializeField] private int attackPriority = 2;
    private AttackToken _token;
    
    [SerializeField] float fireAngleOffset = 90f;
    [SerializeField] private Transform firePoint;

    private bool _isAttackingPlayer;
    [SerializeField] private float attackDelay = 3f;

    [FormerlySerializedAs("_enemyNormalColor")] [SerializeField] Color enemyNormalColor;
    [FormerlySerializedAs("_enemyChargingColor")] [SerializeField] Color enemyChargingColor;

    private bool _updateFireAngle = true;

    [SerializeField] private bool useEnemyChargeColour;
    private void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        enemyNormalColor = spriteRenderer.color;
    }

    private void Update()
    {
        _playerPos = _playerTransform.position;
    }

    private void FixedUpdate()
    {
        float characterDistance = (_playerPos - transform.position).sqrMagnitude;
        if (characterDistance > playerDistance)
        {
            // if (spriteRenderer.enabled) spriteRenderer.enabled = false;
            transform.position = Vector2.MoveTowards(transform.position, _playerPos, enemySpeed * Time.deltaTime);
        }
        else if (characterDistance < playerDistance && characterDistance > retreatDistance)
        {
            EnemyStopAndFire();
        }
        
    }
    void EnemyStopAndFire()
    {
        if (!spriteRenderer.enabled) spriteRenderer.enabled = true;
        Vector2 fireDistance = (playerPosition.Position - transform.position).normalized;
        Debug.Log("Fire Distance: " + fireDistance);
        if (_updateFireAngle)
        {
            float fireAngle = (Mathf.Atan2(fireDistance.y, fireDistance.x) * Mathf.Rad2Deg) - fireAngleOffset;
            firePoint.rotation = Quaternion.Euler(0, 0, fireAngle);
        }

        if (!_isAttackingPlayer)
        {
            StartCoroutine(AttackPlayer());
        }
    }
    IEnumerator AttackPlayer()
    {
        if (useEnemyChargeColour) spriteRenderer.color = enemyChargingColor;
        _isAttackingPlayer = true;
        _updateFireAngle = false;
        yield return new WaitForSeconds(attackDelay);
        if (!Shoot()) yield break;
        spriteRenderer.color = enemyNormalColor;
        yield return new WaitForSeconds(attackDelay);
        _isAttackingPlayer = false;
        _updateFireAngle = true;
    }

    bool Shoot()
    {
        _token = tokenHolder.RequestToken(attackPriority, this);
        if (_token == null) 
            return false; // didn't shoot

        particleSystem.Emit(bulletBurstAmount);
        tokenHolder.ReturnToken(_token);
            return true; // did shoot
    }
        
    public void StealToken()
    { 
        // Unused.
    }
}

// Attack variables/functions reused from E03. 
// References. 
// https://youtu.be/_Z1t7MNk0c4?si=WcWQEeRzqRMBZhin