using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Entity;
using Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private EventAction playerHit;
    [SerializeField] private float animationTime = 0.2f;
    [SerializeField] private float animationDelay = 0.2f;

    [SerializeField] private TextMeshProUGUI healthText;
    private EntityHealth _health;
    private Image _image;
    
    private void Start()
    {
        _image = GetComponent<Image>();
        playerHit += OnPlayerHit;
    }
    
    private void OnDestroy()
    {
        playerHit -= OnPlayerHit;
    }
    
    private void OnPlayerHit(EventParams obj)
    {
        if (obj is not HitParams healthData) return;
        if (_health == null)
            _health = healthData.Object.GetComponent<EntityHealth>();
        
        healthText.text = $"{_health.CurrentHealth} / {_health.MaxHealth}";
        
        _image.DOKill();
        _image.DOFillAmount(healthData.CurrentHealthPercentage, animationTime)
            .SetEase(Ease.OutQuint)
            .SetDelay(animationDelay);
    }
}