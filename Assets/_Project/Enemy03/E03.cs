using UnityEngine;

public class E03 : MonoBehaviour
{
    [Header("Lazer")]
    [SerializeField] private Transform lazerObj;
    [SerializeField] private Transform lazerParent;

    [Header("Timer")] 
    private float _timer;
    [SerializeField] private float maxTimer = 3;
    
    [Header("Rotation")]
    [SerializeField] private float rotationSpeed;
    [SerializeField] bool canRotate = true;
    private void Update()
    {
        if (canRotate) RotateParent();
        SeekPlayer();
    }
    void RotateParent()
    {
        lazerParent.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
    }
    void SeekPlayer()
    {
        RaycastHit2D enemyRaycast = Physics2D.Raycast(lazerObj.position, lazerObj.transform.right);
        Debug.DrawRay(transform.position, lazerObj.transform.right, Color.red);
        if (enemyRaycast.collider != null && enemyRaycast.collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("Lazer has hit player.");
            FireLazer();
        }
    }
    void FireLazer()
    {
        canRotate = false;
    }
}
