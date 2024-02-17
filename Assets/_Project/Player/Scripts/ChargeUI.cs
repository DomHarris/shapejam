using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    /// <summary>
    /// Visually display the player's shots left
    /// </summary>
    public class ChargeUI : MonoBehaviour
    {
        // Serialized fields
        [SerializeField] private float indicatorAnimationTime = 0.25f;
        [SerializeField] private Ease indicatorAnimationEase = Ease.OutQuint;
        [SerializeField] private float delay;
    
        // Private fields
        private Image _indicator;
        private PlayerCharges _charges;
        private Tween _tween;

        /// <summary>
        /// Called when the object is created
        /// Find the relevant components and subscribe to events
        /// </summary>
        private void Start()
        {
            _indicator = GetComponent<Image>();
            _charges = FindFirstObjectByType<PlayerCharges>();
            _charges.ChargesChanged += OnChargesChanged;
        }
    
        
        /// <summary>
        /// Called when the object is destroyed
        /// Unsubscribe from events
        /// </summary>
        private void OnDestroy()
        {
            _charges.ChargesChanged -= OnChargesChanged;
        }

        /// <summary>
        /// Called when the player's charges change
        /// </summary>
        /// <param name="percent"></param>
        private void OnChargesChanged(float percent)
        {
            // kill the current tween and animate the indicator
            _tween?.Kill();
            _tween = _indicator.DOFillAmount(percent, indicatorAnimationTime)
                .SetEase(indicatorAnimationEase)
                .SetDelay(delay);
        }
    }
}