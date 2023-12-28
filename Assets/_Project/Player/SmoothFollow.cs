using UnityEngine;

namespace Player
{
    public class SmoothFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = Vector3.up;
        [SerializeField] private float movePercentPerSecond;
        private void FixedUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, target.position + target.rotation * offset, movePercentPerSecond * Time.deltaTime);
        }
    }
    
}