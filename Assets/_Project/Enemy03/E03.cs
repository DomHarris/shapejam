using System;
using System.Collections;
using UnityEngine;

public class E03 : MonoBehaviour
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
        // Put bullet attack code here.
        yield return new WaitForSeconds(reloadTime);
    }
}
