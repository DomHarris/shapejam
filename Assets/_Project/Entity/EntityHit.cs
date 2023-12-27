using UnityEngine;

namespace Entity
{
    public class EntityHit : MonoBehaviour, IHit
    {
        [field: SerializeField] public float MaxHealth { get; private set; }

        private float _currentHealth;
        
        private void Start()
        {
            _currentHealth = MaxHealth;
        }
        
        public void Hit(float damage)
        {
            _currentHealth -= damage;
            if (_currentHealth <= 0)
                Destroy(gameObject);
        }
    }
}