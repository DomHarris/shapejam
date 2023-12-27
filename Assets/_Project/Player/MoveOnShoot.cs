using UnityEngine;

namespace Player
{
    /// <summary>
    /// Move the player when they shoot
    /// </summary>
    public class MoveOnShoot : MonoBehaviour
    {
        // Serialized fields
        [SerializeField] private float moveForce = 600;
        
        // Private fields
        private Rigidbody2D _rigidbody;
        private PlayerShoot _shoot;
    
        
        /// <summary>
        /// Called when the object is created
        /// Find the relevant components and subscribe to events
        /// </summary>
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _shoot = GetComponentInChildren<PlayerShoot>();
            _shoot.Fire += OnFire;
        }
        
        /// <summary>
        /// Called when the object is destroyed
        /// </summary>
        private void OnDestroy()
        {
            _shoot.Fire -= OnFire;
        }

        /// <summary>
        /// Called when the player shoots
        /// Add a force to the player based on the direction they are facing
        /// </summary>
        public void OnFire () => _rigidbody.AddForce(-transform.right * moveForce);
    }
}