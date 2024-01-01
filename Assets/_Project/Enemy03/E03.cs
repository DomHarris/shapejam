using System.Collections;
using Enemies;
using Entity;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class E03 : MonoBehaviour, ITokenUser
{
    [Header("Lazer")]
    [SerializeField] private Transform lazerObj;
    [SerializeField] private Transform lazerParent;
    [SerializeField] private float reloadTime = 0.5f;
    private bool _canAttack;
    [SerializeField] private int bulletBurstAmount = 5;

    [Header("Timer")] 
    private float _timer;
    [SerializeField] private float maxTimer = 3;
    
    [Header("Rotation")]
    [SerializeField] private float rotationSpeed;
    [SerializeField] bool canRotate = true;

    [SerializeField] private new ParticleSystem particleSystem;
    [SerializeField] private AttackTokenHolder tokenHolder;
    [SerializeField] private int attackPriority = 2;

    [Header("Post Processing")] 
    [SerializeField] Volume postProcessingVolume;

    [SerializeField] private float bloomMinimum = 0.0f;
    [SerializeField] private float bloomMaximum = 0.9f;
    [SerializeField] private float bloomTime = 0.5f;

    private Bloom _bloom;

    private AttackToken _token;

    private void Start()
    {
        postProcessingVolume.profile.TryGet(out _bloom);
        _timer = 0;
    }
    private void Update()
    {
        if (canRotate) RotateParent();
        else if (_canAttack)
        {
            _timer += 1 * Time.deltaTime;
            Debug.Log("Timer: " + _timer);
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
            canRotate = false;
            _canAttack = true;
        }
    }
    IEnumerator StartEnemyAttack()
    {
        _timer = 0;
        // try to shoot, if it was unsuccessful, don't do anything else
        if (!Shoot()) yield break;
        yield return new WaitForSeconds(reloadTime);
        canRotate = true;
        _canAttack = false;
    }
    bool Shoot()
    {
        _token = tokenHolder.RequestToken(attackPriority, this);
        if (_token == null)
            return false; // didn't shoot

        particleSystem.Emit(bulletBurstAmount);
        return true; // did shoot
    }
    public void StealToken()
    {
        // unused
    }
}
