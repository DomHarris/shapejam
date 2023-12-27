using Entity;
using UnityEngine;

namespace Bullet
{
    /// <summary>
    /// Hit an entity with a bullet
    /// </summary>
    public class BulletHit : MonoBehaviour
    {
        // Serialized fields 
        [SerializeField] private float minDamage, maxDamage;
        [SerializeField] private float minSize, maxSize;
        
        // Private fields
        private ParticleSystem _particles;
        private ParticleSystem.Particle[] _particlesArray = new ParticleSystem.Particle[20];
    
        /// <summary>
        /// Called when the object is created
        /// </summary>
        private void Start()
        {
            _particles = GetComponent<ParticleSystem>();
        }
    
        
        /// <summary>
        /// Called when a particle from the particle system attached to this object collides with another object
        /// </summary>
        /// <param name="other">The object it hit</param>
        private void OnParticleCollision(GameObject other)
        {
            // Find the particle that collided with the object
            var count = _particles.GetParticles(_particlesArray);
            var idx = 0;
            var sqDist = float.MaxValue;
            for (var i = 0; i < count; i++)
            {
                var dist = Vector3.SqrMagnitude(_particlesArray[i].position - transform.position);
                if (dist < sqDist)
                {
                    idx = i;
                    sqDist = dist;
                }
            }
            
            // Calculate the damage based on the size of the particle
            var size = _particlesArray[idx].startSize;
            var damage = Mathf.Lerp(minDamage, maxDamage,
                Mathf.InverseLerp(minSize, maxSize, size));
            
            // Hit the object
            var hit = other.GetComponent<IHit>();
            hit?.Hit(damage);
        }
    }
}