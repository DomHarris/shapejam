using Bullet;
using Unity.Mathematics;
using UnityEngine;

namespace Entity
{
    /// <summary>
    /// Hit the attached entity based on the size of the particle that hit it
    /// </summary>
    public class HitEntityOnParticleCollide : MonoBehaviour
    {
        // Serialized fields 
        [SerializeField] private float minDamage = 1, maxDamage = 5;
        [SerializeField] private float minSize = 0.5f, maxSize = 0.9f;
        
        private BulletHit _bulletHit;
        
        /// <summary>
        /// Called when the object is created
        /// </summary>
        private void Start()
        {
            _bulletHit = GetComponent<BulletHit>();
            _bulletHit.OnHit += OnHit;
        }

        /// <summary>
        /// Called when the object is destroyed
        /// </summary>
        private void OnDestroy()
        {
            _bulletHit.OnHit -= OnHit;
        }

        /// <summary>
        /// Called when an object is hit by a particle
        /// </summary>
        /// <param name="obj">The object that was hit</param>
        /// <param name="particle">The particle</param>
        private void OnHit(GameObject obj, ParticleSystem.Particle particle)
        {
            // Calculate the damage based on the size of the particle
            var size = particle.startSize;
            var damage = Mathf.Lerp(minDamage, maxDamage,
                Mathf.InverseLerp(minSize, maxSize, size));
            // Hit the object
            var hit = obj.GetComponent<IHit>();
            hit?.Hit(damage);
        }
    }
}