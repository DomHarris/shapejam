using UnityEngine;

namespace CameraControl
{
    public class ImmediateFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = Vector3.up;
        private void FixedUpdate()
        {
            transform.position = target.position + target.rotation * offset;
        }
    }
}