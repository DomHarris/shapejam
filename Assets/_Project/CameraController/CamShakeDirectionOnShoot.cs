using DG.Tweening;
using Player;
using UnityEngine;

namespace CameraController
{
    /// <summary>
    /// Shake the camera in a direction when the player shoots
    /// </summary>
    public class CamShakeDirectionOnShoot : MonoBehaviour
    {
        // Serialized fields
        [SerializeField] private CameraController cam;
        [SerializeField] private float shotStrength = 1f;
        [SerializeField] private float returnDuration = 0.1f;

        // Private fields
        private PlayerShoot _shoot;
        
        /// <summary>
        /// Called when the object is created
        /// </summary>
        private void Start()
        {
            _shoot = GetComponentInChildren<PlayerShoot>();
            _shoot.Fire += OnWeaponFired;
        }
        
        /// <summary>
        /// Called when the object is destroyed
        /// </summary>
        private void OnDestroy()
        {
            _shoot.Fire -= OnWeaponFired;
        }
        
        /// <summary>
        /// Called when the player shoots
        /// </summary>
        public void OnWeaponFired()
        {
            // Shake the camera in the opposite direction of the player's shot
            cam.ShakeDirectional(-transform.right, shotStrength, returnDuration, Ease.OutQuint);
        }
    }
}