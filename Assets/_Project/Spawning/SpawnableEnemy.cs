using System;
using DG.Tweening;
using Entity;
using UnityEngine;

namespace Spawning
{
    public class SpawnableEnemy : MonoBehaviour
    {
        [field: SerializeField, Range(0, 4)] public float CameraWeight { get; private set; } = 1;
        public event Action<SpawnableEnemy> OnDeath;
        private EntityHealth _health;
        
        private void Start()
        {
            _health = GetComponent<EntityHealth>();
            _health.OnDie += Die;
        }
        
        private void OnDestroy()
        {
            _health.OnDie -= Die;
        }

        private void OnEnable()
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(1, 0.5f)
                .SetEase(Ease.OutBack);
        }

        public void Die()
        {
            OnDeath?.Invoke(this);
        }
    }
}