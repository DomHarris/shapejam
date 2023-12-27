using DG.Tweening;
using UnityEngine;

namespace Player
{
    public class ShotChargeIndicator : MonoBehaviour
    {
        [SerializeField] private float minScale = 1f, maxScale = 2f, compressScale = 0.5f;

        private PlayerShoot _shoot;
        private float _scale;

        private void Start()
        {
            _scale = transform.localScale.x;
            _shoot = FindFirstObjectByType<PlayerShoot>();
            _shoot.Charge += OnCharge;
            _shoot.Fire += OnFire;
        }

        private void OnFire()
        {
            transform.localScale = Vector3.one * compressScale;
            transform.DOScale(_scale, _shoot.chargeDelay)
                .SetEase(Ease.OutQuint);
        }

        private void OnCharge(float percent)
        {
            transform.localScale = Vector3.one * Mathf.Lerp(minScale, maxScale, percent);
        }
    }
}