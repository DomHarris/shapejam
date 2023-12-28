using System;
using Entity;
using Stats;
using UnityEngine;

namespace Bullet
{
    public class BulletHitParams : EventParams
    {
        public GameObject WeaponObject;
        public GameObject HitObject;
        public ParticleSystem.Particle Particle;
    }
    
    /// <summary>
    /// Hit an entity with a bullet
    /// </summary>
    public class BulletHit : MonoBehaviour
    {
        [SerializeField] private EventAction onHit;
        public event Action<GameObject, ParticleSystem.Particle> OnHit;
        
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
            
            // Invoke the event
            OnHit?.Invoke(other, _particlesArray[idx]);
            
            onHit.TriggerAction(new BulletHitParams { HitObject = other, Particle = _particlesArray[idx], WeaponObject = gameObject});
        }
    }
}