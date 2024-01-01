using System;
using System.Collections;
using Enemies;
using UnityEngine;

public class E03 : MonoBehaviour, ITokenUser
{
    [Header("Lazer")]
    [SerializeField] private Transform lazerObj;
    [SerializeField] private Transform lazerParent;
    [SerializeField] private float reloadTime = 0.5f;

    [Header("Timer")] 
    private float _timer;
    [SerializeField] private float maxTimer = 3;
    
    [Header("Rotation")]
    [SerializeField] private float rotationSpeed;
    [SerializeField] bool canRotate = true;

    [SerializeField] private new ParticleSystem particleSystem;
    [SerializeField] private AttackTokenHolder tokenHolder;
    [SerializeField] private int attackPriority = 2;

    private AttackToken _token;

    private void Start()
    {
        _timer = 0;
    }

    private void Update()
    {
        if (canRotate) RotateParent();
        else
        {
            _timer += 1 * Time.deltaTime;
            if (_timer >= maxTimer) StartCoroutine(StartEnemyAttack());
        }
    }
    void RotateParent()
    {
        lazerParent.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
        SeekPlayer();
    }
    void SeekPlayer()
    {
        RaycastHit2D enemyRaycast = Physics2D.Raycast(lazerObj.position, lazerObj.transform.right);
        Debug.DrawRay(transform.position, lazerObj.transform.right, Color.red);
        if (enemyRaycast.collider != null && enemyRaycast.collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("Lazer has hit player.");
            canRotate = false;
        }
    }

    IEnumerator StartEnemyAttack()
    {
        // try to shoot, if it was unsuccessful, don't do anything else
        if (!Shoot()) yield break;
        
        yield return new WaitForSeconds(reloadTime);
    }
    
    bool Shoot()
    {
        _token = tokenHolder.RequestToken(attackPriority, this);
        if (_token == null)
            return false; // didn't shoot

        particleSystem.Emit(1);
        return true; // did shoot
    }
    public void StealToken()
    {
        // unused
    }
}
