using DG.Tweening;
using Entity;
using Spawning;
using Stats;
using UnityEngine;

namespace Enemies
{
    public class SpriteFlash : MonoBehaviour
    {
        [SerializeField] private EventAction onHit;
        [SerializeField] private float flashTime = 0.1f;
        [SerializeField, ColorUsage(true, true)] private Color color = Color.white;
        private SpriteRenderer _spriteRenderer;
        private Color _startColor;
        private GameObject _enemy;

        private void Start()
        {
            _enemy = GetComponentInParent<EntityHealth>().gameObject;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _startColor = _spriteRenderer.color;
            onHit += OnHit;
        }

        private void OnDestroy()
        {
            onHit -= OnHit;
        }

        private void OnHit(EventParams obj)
        {
            if (obj is not HitParams healthData || healthData.Object != _enemy) return;
            _spriteRenderer.DOKill();
            _spriteRenderer.color = color;
            _spriteRenderer.DOColor(_startColor, flashTime);
        }
    }
}