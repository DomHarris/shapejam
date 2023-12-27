using UnityEngine;

namespace Player
{
    public class DepleteOnShoot : MonoBehaviour
    {
    
    }

    public class MoveOnShoot : MonoBehaviour
    {
        [SerializeField] private float moveForce = 600;
        private Rigidbody2D _rigidbody;
        private PlayerShoot _shoot;
    
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _shoot = GetComponentInChildren<PlayerShoot>();
            _shoot.Fire += OnFire;
        }
    
        private void OnDestroy()
        {
            _shoot.Fire -= OnFire;
        }

        public void OnFire () => _rigidbody.AddForce(-transform.right * moveForce);
    }
}