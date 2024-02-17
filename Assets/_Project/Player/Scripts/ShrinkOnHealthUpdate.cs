using DG.Tweening;
using Entity;
using Stats;
using UnityEngine;

namespace Player
{
    public class ShrinkOnHealthUpdate : MonoBehaviour
    {
        [SerializeField] private EventAction onHealthUpdate;
        [SerializeField] private float minSize;
        [SerializeField] private float animationTime = 0.2f;
        [SerializeField] private float animationDelay = 0.1f;
        [SerializeField, ColorUsage(true, true)] private Color color = Color.white;
        private float _startSize;
        private SpriteRenderer _spriteRenderer;
        
        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.color = color;
            _startSize = transform.localScale.x;
            onHealthUpdate += OnHealthUpdate;
        }
        
        private void OnDestroy()
        {
            onHealthUpdate -= OnHealthUpdate;
        }

        private void OnHealthUpdate(EventParams obj)
        {
            if (obj is not HitParams healthData) return;
            transform.DOScale(Mathf.Lerp(minSize, _startSize, healthData.CurrentHealthPercentage), animationTime)
                .SetEase(Ease.OutQuint)
                .SetDelay(animationDelay);
        }
    }
}