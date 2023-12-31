using System;
using System.Collections;
using System.Threading.Tasks;
using DG.Tweening;
using Stats;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class Dash : MonoBehaviour, ITokenUser
    {
        [SerializeField] private float minTimeBetweenDashes = 3f;
        [SerializeField] private float dashChance;

        [SerializeField] private float dashWindupTime = 0.2f;
        [SerializeField] private float dashDuration = 0.5f;
        [SerializeField] private Ease dashEase = Ease.OutQuint;

        [SerializeField] private float dashLength;
        [SerializeField] private float lookRadius = 5f;
        
        [SerializeField] private MonoBehaviour movement;
        [SerializeField] private AttackTokenHolder tokenHolder;

        [SerializeField] private int attackWindupPriority = 1;
        [SerializeField] private int attackPriority = 20;

        private Sequence _sequence;
        
        private bool _canDash = true;
        private AttackToken _token;
        
        private Rigidbody2D _rigidbody;
        private static Transform _player;

        private void Start()
        {
            if (_player == null)
                _player = GameObject.FindWithTag("Player").transform;
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private async void OnEnable()
        {
            _canDash = false;
            var timer = minTimeBetweenDashes;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                await Task.Yield();
            }

            _canDash = true;
        }

        private void Update()
        {
            if (_canDash && (_player.position - transform.position).sqrMagnitude < lookRadius * lookRadius && Random.Range(0f, 1f) < dashChance)
                DashTowardsPlayer();
        }

        private void DashTowardsPlayer()
        {
            _token = tokenHolder.RequestToken(attackWindupPriority, this);
            if (_token == null)
                return;
            
            _canDash = false;
            movement.enabled = false;
            
            // get quaternion rotation to player in 2d
            var direction = (Vector2) _player.position - _rigidbody.position;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            _sequence = DOTween.Sequence()
                .Append(transform.DORotateQuaternion(rotation, dashWindupTime).SetEase(Ease.InOutQuint))
                .AppendCallback(() => _token.Priority = attackPriority)
                .Append(_rigidbody.DOMove(_rigidbody.position + direction.normalized * (direction.magnitude + dashLength), dashDuration)
                    .SetEase(dashEase))
                .AppendInterval(0.2f)
                .AppendCallback(() =>
                {
                    movement.enabled = true;
                    tokenHolder.ReturnToken(_token);
                    _token = null;
                })
                .AppendInterval(minTimeBetweenDashes)
                .AppendCallback(() => _canDash = true);
        }

        public void StealToken()
        {
            _sequence.Kill();
        }
    }
}