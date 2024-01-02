using DG.Tweening;
using Enemies;
using UnityEngine;

public class FireBulletAfterTime : MonoBehaviour, ITokenUser
{
    [SerializeField] private float aimTime = 2f;
    [SerializeField] private float delay = 0.2f;
    [SerializeField] private AttackTokenHolder attackTokenHolder;

    [SerializeField] private float originalAngle = 20f;
    [SerializeField] private SpriteRenderer tellLeft, tellRight;

    [SerializeField] private float attackChance = 0.005f;
    [SerializeField] private int attackPriority = 5;

    [SerializeField] private ParticleSystem bullets;
    [SerializeField] private LookAtPosition lookAt;

    private Color _color;
    private AttackToken _token;

    private bool _attacking = false;
    private Sequence _sequence;

    private void Start()
    {
        _color = tellLeft.color;
        tellLeft.color = Color.clear;
        tellRight.color = Color.clear;
        tellLeft.transform.eulerAngles = new Vector3(0, 0, -originalAngle);
        tellRight.transform.eulerAngles = new Vector3(0, 0, originalAngle);
    }

    private void Update()
    {
        if (!_attacking && Random.Range(0f, 1f) < attackChance)
            Attack();
    }

    private void Attack()
    {
        _token = attackTokenHolder.RequestToken(attackPriority, this);
        if (_token == null) return;

        _attacking = true;
        tellLeft.transform.eulerAngles = new Vector3(0, 0, -originalAngle);
        tellRight.transform.eulerAngles = new Vector3(0, 0, originalAngle);
        tellLeft.color = _color;
        tellRight.color = _color;

        DOTween.Sequence()
            .Insert(0, tellLeft.transform.DOLocalRotate(Vector3.zero, aimTime).SetEase(Ease.OutQuint))
            .Insert(0, tellRight.transform.DOLocalRotate(Vector3.zero, aimTime).SetEase(Ease.OutQuint))
            .AppendCallback(() => lookAt.enabled = false)
            .Insert(aimTime, tellLeft.DOFade(0, delay))
            .Insert(aimTime, tellRight.DOFade(0, delay))
            .InsertCallback(aimTime + delay, () =>
            {
                bullets.Emit(1);
                _attacking = false;
                lookAt.enabled = true;
                attackTokenHolder.ReturnToken(_token);
            });
    }

    public void StealToken()
    {
        _sequence?.Kill();
        tellLeft.DOFade(0, delay);
        tellRight.DOFade(0, delay);
        _attacking = false;
        lookAt.enabled = true;
    }
}