using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Enemies;
using Entity;
using UnityEngine;
using Random = UnityEngine.Random;

public class PopOutAndShoot : MonoBehaviour, ITokenUser
{
    [SerializeField] private Color inactiveColor;
    [SerializeField] private Color flashColor;
    [SerializeField] private Transform spikes;
    [SerializeField] private ParticleSystem bullets;
    [SerializeField] private float spikesMinSize = 0.85f;
    [SerializeField] private float attackChance = 0.1f;
    [SerializeField] private float wanderTime = 2f;
    [SerializeField] private AttackTokenHolder tokenHolder;
    [SerializeField] private int attackPriority = 5;
    [SerializeField] private MonoBehaviour wander;
    [SerializeField] private ContactDamage damage;
    [SerializeField] private EntityHealth health;
    [SerializeField] private float minClosedTime = 2f;

    private AttackToken _token;
    private bool _open = true;
    private SpriteRenderer[] _sprites;
    private Color[] _colors;
    // Start is called before the first frame update
    void Start()
    {
        if (_sprites != null) return;
        _sprites = GetComponentsInChildren<SpriteRenderer>();
        _colors = new Color[_sprites.Length];
        for (int i = 0; i < _sprites.Length; ++i)
            _colors[i] = _sprites[i].color;
    }

    private void OnEnable()
    {
        if (_sprites == null)
            Start();
        Close();
    }

    private void Update()
    {
        if (!_open && Random.Range(0f, 1f) < attackChance)
            PopOut();
    }

    private async void PopOut()
    {
        _token = tokenHolder.RequestToken(attackPriority, this);
        if (_token == null) return;
        
        _open = true;
        wander.enabled = true;
        damage.enabled = true;
        health.enabled = true;
        spikes.DOScale(1f, 0.2f)
            .SetEase(Ease.OutBack)
            .OnComplete(() => bullets.Emit(8));

        for (var i = 0; i < _sprites.Length; i++)
        {
            DOTween.Sequence()
                .Append(_sprites[i].DOColor(flashColor, 0.2f))
                .Append(_sprites[i].DOColor(_colors[i], 0.2f));
        }

        var timer = wanderTime;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            await Task.Yield();
        }

        if (!Application.isPlaying) return;
        Close();
    }

    private async void Close()
    {
        if (_token != null)
            tokenHolder.ReturnToken(_token);
        damage.enabled = false;
        wander.enabled = false;
        health.enabled = false;
        spikes.DOScale(spikesMinSize, 0.2f)
            .SetEase(Ease.OutQuint);
        foreach (var t in _sprites)
            t.DOColor(inactiveColor, 0.2f);
        
        var timer = minClosedTime;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            await Task.Yield();
        }
        
        _open = false;
    }

    public void StealToken()
    {
        spikes.DOKill();
        foreach (var t in _sprites)
            t.DOKill();
        Close();
    }
}
