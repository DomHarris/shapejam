using UnityEngine;

namespace Bullet
{
    public class BulletHit : MonoBehaviour
    {
        [SerializeField] private float minDamage, maxDamage;
        [SerializeField] private float minSize, maxSize;
        private ParticleSystem _particles;
        private ParticleSystem.Particle[] _particlesArray = new ParticleSystem.Particle[20];
    
        private void Start()
        {
            _particles = GetComponent<ParticleSystem>();
        }
    
        private void OnParticleCollision(GameObject other)
        {
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
            var size = _particlesArray[idx].startSize;
            var damage = Mathf.Lerp(minDamage, maxDamage,
                Mathf.InverseLerp(minSize, maxSize, size));
            Debug.Log(damage);
        }
    }
}