using UnityEngine;

namespace Enemies
{
    public class CirclePlayer : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float minDistance;
        [SerializeField] private float maxDistance;
        [SerializeField] private SerialPosition playerPosition;

        private Rigidbody2D _rigidbody;
        private float _minDistSq, _maxDistSq;

        private void Start()
        {
            _minDistSq = minDistance * minDistance;
            _maxDistSq = maxDistance * maxDistance;
            _rigidbody = GetComponent<Rigidbody2D>();
        }
        
        private void FixedUpdate()
        {
            Vector3 direction = (playerPosition.Position - transform.position).normalized;
            if (_maxDistSq < (playerPosition.Position - transform.position).sqrMagnitude)
                _rigidbody.AddForce(direction * speed);
            else if (_minDistSq > (playerPosition.Position - transform.position).sqrMagnitude)
                _rigidbody.AddForce(-direction * speed);
            
            direction = Quaternion.Euler(0,0,90) * direction;
            
            _rigidbody.AddForce(direction * speed);
        }
    }
}