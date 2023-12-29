using Stats;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// Move the player when they shoot
    /// </summary>
    public class MoveOnShoot : MonoBehaviour
    {
        // Serialized fields
        [SerializeField] private Stat moveForce; // 600
        [SerializeField] private EventAction gameStart;

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
            gameStart += OnGameStart;
        }

        /// <summary>
        /// Called when the "Play" button is pressed
        /// </summary>
        /// <param name="_"></param>
        private void OnGameStart(EventParams _)
        {
            _rigidbody.position = Vector2.zero;
        }
        
        /// <summary>
        /// Called when the object is destroyed
        /// </summary>
        private void OnDestroy()
        {
            _shoot.Fire -= OnFire;
            gameStart -= OnGameStart;
        }

        /// <summary>
        /// Called when the player shoots
        /// Add a force to the player based on the direction they are facing
        /// </summary>
        public void OnFire () => _rigidbody.AddForce(-transform.right * moveForce);
    }
}