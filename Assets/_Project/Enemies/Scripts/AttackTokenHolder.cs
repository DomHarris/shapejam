using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Stats;
using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu]
    public class AttackTokenHolder : ScriptableObject
    {
        [SerializeField] private int maxTokens = 5;
        [SerializeField] private EventAction gameStart;
        [NonSerialized] private bool _initialized = false;
        [NonSerialized] private readonly List<AttackToken> _activeTokens = new List<AttackToken>();
        [NonSerialized] private readonly List<AttackToken> _usedTokens = new List<AttackToken>();

        private void Initialise()
        {
            _initialized = true;
            gameStart += OnGameStart;
            OnGameStart(null);
        }

        private void OnGameStart(EventParams obj)
        {
            _activeTokens.Clear();
            for (int i = 0; i < maxTokens; ++i)
                _activeTokens.Add(new AttackToken { Id = i, Priority = 0, Holder = null });
            _usedTokens.Clear();
        }

        private void OnDestroy()
        {
            gameStart -= OnGameStart;
        }

        [CanBeNull] 
        public AttackToken RequestToken(int priority, ITokenUser user)
        {
            if (!_initialized)
                Initialise();

            if (_activeTokens.Count > 0)
            {
                var token = _activeTokens[0];
                token.Holder = user;
                token.Priority = priority;
                _activeTokens.RemoveAt(0);
                _usedTokens.Add(token);
                return token;
            }
            
            if (_usedTokens.Any(t => t.Priority < priority))
            {
                var token = _usedTokens.First(t => t.Priority < priority);
                token.Holder?.StealToken();
                token.Holder = user;
                token.Priority = priority;
                return token;
            }

            return null;
        }

        public void ReturnToken(AttackToken token)
        {
            token.Holder = null;
            token.Priority = 0;
            
            _usedTokens.Remove(token);
            _activeTokens.Add(token);
        }
    }
}