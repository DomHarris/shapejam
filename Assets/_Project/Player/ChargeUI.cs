using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class ChargeUI : MonoBehaviour
    {
        [SerializeField] private float indicatorAnimationTime = 0.25f;
        [SerializeField] private Ease indicatorAnimationEase = Ease.OutQuint;
        [SerializeField] private float delay;
    
        private Image _indicator;
        private PlayerCharges _charges;
        private Tween _tween;

        private void Start()
        {
            _indicator = GetComponent<Image>();
            _charges = FindFirstObjectByType<PlayerCharges>();
            _charges.ChargesChanged += OnChargesChanged;
        }
    
        private void OnDestroy()
        {
            _charges.ChargesChanged -= OnChargesChanged;
        }

        private void OnChargesChanged(float percent)
        {
            _tween?.Kill();
            _tween = _indicator.DOFillAmount(percent, indicatorAnimationTime)
                .SetEase(indicatorAnimationEase)
                .SetDelay(delay);
        }
    }
}