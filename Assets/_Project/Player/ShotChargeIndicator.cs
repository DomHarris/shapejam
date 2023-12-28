using System;
using DG.Tweening;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// Scale an object based on the charge of the player's shot.
    /// </summary>
    public class ShotChargeIndicator : MonoBehaviour
    {
        // Serialized fields
        [SerializeField] private float minScale = 1f, 
            maxScale = 2f, 
            compressScale = 0.5f;

        // Private fields
        private PlayerShoot _shoot;
        private float _scale;

        /// <summary>
        /// Called when the object is created.
        /// Subscribe to the relevant events
        /// </summary>
        private void Start()
        {
            _scale = transform.localScale.x;
            _shoot = FindFirstObjectByType<PlayerShoot>();
            _shoot.Charge += OnCharge;
            _shoot.Fire += OnFire;
        }

        /// <summary>
        /// Called when the object is destroyed.
        /// Unsubscribe from the relevant events
        /// </summary>
        private void OnDestroy()
        {
            _shoot.Charge -= OnCharge;
            _shoot.Fire -= OnFire;
        }

        /// <summary>
        /// Called when a shot is fired
        /// </summary>
        private void OnFire()
        {
            // Animate the scale of the object
            transform.localScale = Vector3.one * compressScale;
            transform.DOScale(_scale, _shoot.ChargeDelay)
                .SetEase(Ease.OutQuint);
        }

        /// <summary>
        /// Called when the player's shot charge is changed
        /// </summary>
        /// <param name="percent">0 = smallest shot, 1 = fully charged</param>
        private void OnCharge(float percent)
        {
            // scale the object based on the charge
            transform.localScale = Vector3.one * Mathf.Lerp(minScale, maxScale, percent);
        }
    }
}