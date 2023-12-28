using Bullet;
using UnityEngine;

namespace Entity
{
    /// <summary>
    /// Spawn particles when an entity is hit
    /// </summary>
    public class ParticlesOnHit : MonoBehaviour
    {
        // Serialized fields
        [SerializeField] private ParticleSystem hitParticles;
        [SerializeField] private int numParticles = 30;
        
        // Private fields
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
            // Spawn particles
            hitParticles.transform.position = particle.position;
            hitParticles.transform.up = obj.transform.position - particle.position;
            hitParticles.Emit(numParticles);
        }
    }
}