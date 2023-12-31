using CameraControl;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class RandomWander : MonoBehaviour
    {
        [SerializeField] private float maxDistanceToPlayer = 15f;
        [SerializeField] private float speed = 30f;
        [SerializeField] private float directionFrequency = 1f;
        [SerializeField] private SerialPosition playerPosition;
        private float _seed;
        private Rigidbody2D _rigidbody;
        

        private float _maxDistSq;
        private void Start()
        {
            _maxDistSq = maxDistanceToPlayer * maxDistanceToPlayer;
            _seed = Random.Range(0, 99999);
            _rigidbody = GetComponent<Rigidbody2D>();
        }
    
        private void FixedUpdate()
        {
            Vector3 direction;
            if (_maxDistSq < (playerPosition.Position - transform.position).sqrMagnitude)
            {
                direction = (playerPosition.Position - transform.position).normalized;
                _rigidbody.AddForce(direction * speed);
            }
            
            var angle = PerlinHelper.GetPerlin(_seed, directionFrequency, 360) * Mathf.Deg2Rad;
            direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            _rigidbody.AddForce(direction * speed);
        }

        private void Update()
        {
            transform.up = _rigidbody.velocity;
        }
    }
}
