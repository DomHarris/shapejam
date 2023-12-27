using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class ShotChargeIndicator : MonoBehaviour
{
    [SerializeField] private float minScale = 1f, maxScale = 2f, compressScale = 0.5f;

    private PlayerShoot _shoot;

    private void Start()
    {
        _shoot = GetComponentInParent<PlayerShoot>();
        _shoot.Charge += OnCharge;
        _shoot.Fire += OnFire;
    }

    private async void OnFire()
    {
        await Task.Yield();
        transform.localScale = Vector3.one * compressScale;
        transform.DOScale(1, _shoot.chargeDelay)
            .SetEase(Ease.OutQuint);
    }

    private void OnCharge(float percent)
    {
        transform.localScale = Vector3.one * Mathf.Lerp(minScale, maxScale, percent);
    }
}