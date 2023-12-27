using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    /// <summary>
    /// Take input from the mouse and rotate the object to face it.
    /// </summary>
    public class PlayerLook : MonoBehaviour
    {
        // Serialized Fields
        [SerializeField] private Camera camera;
    
        // Private Fields
        private Rigidbody2D _rigidbody;
        private PlayerInput _input;
        private Vector2 _target;

        /// <summary>
        /// Called when the object is created.
        /// Grab the relevant object references and subscribe to any input actions.
        /// </summary>
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _input = GetComponent<PlayerInput>();
            _input.actions["Look"].performed += OnLook;
        }

        /// <summary>
        /// Called every physics step.
        /// Rotate the object to face the mouse.
        /// </summary>
        private void FixedUpdate()
        {
            // get the mouse position in world space
            // we do this in FixedUpdate because if the mouse doesn't move and the camera does, we want the direciton we're facing to change 
            var mousePos = camera.ScreenToWorldPoint(_target);
            // get the direction from this object to the mouse
            var direction = (mousePos - transform.position).normalized;
            
            // rotate the rigidbody
            // Atan2(y, x) gives the angle in radians between the x-axis and the direction vector
            // Multiply by Rad2Deg to convert to degrees
            _rigidbody.MoveRotation(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        }

        
        /// <summary>
        /// Called when the input is received.
        /// </summary>
        /// <param name="ctx">The input context</param>
        private void OnLook(InputAction.CallbackContext ctx)
        {
            // read the mouse position every time it moves
            _target = ctx.ReadValue<Vector2>();
        }

        /// <summary>
        /// Called when the object is destroyed.
        /// Unsubscribe from any input actions.
        /// </summary>
        private void OnDestroy()
        {
            _input.actions["Look"].performed -= OnLook;
        }
    }
}
