using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Player;
using Player.Abilities;
using Stats;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace UI.LevelUpScreen
{
    public static class RandomExtensions
    {
        public static PlayerAbility WeightedRandom(this List<PlayerAbility> items)
        {
            var sum = items.Sum(item => item.Weight) * Random.value;
            foreach (var item in items)
            {
                sum -= item.Weight;
                if (sum > 0)
                    continue;

                return item;
            }

            return null;
        }
    }

    public class LevelUpScreen : MonoBehaviour
    {
        [SerializeField] private EventAction gameStart;
        [SerializeField] private EventAction showEvent;
        [SerializeField] private TextMeshProUGUI[] availablePowers;
        [SerializeField] private float textFadeTime;

        [SerializeField] private List<PlayerAbility> abilities;

        private Dictionary<int, PlayerAbility> _currentAbilities = new();

        private List<PlayerAbility> _activeAbilities = new();
        private List<PlayerAbility> _startingAbilities = new();
         
        private void Start()
        {
            _startingAbilities = new List<PlayerAbility>(abilities);
            foreach (var text in availablePowers)
            {
                text.text = "";
                text.alpha = 0;
            }
            
            
            showEvent += OnShow;
            gameStart += OnGameStart;
        }

        private void OnGameStart(EventParams obj)
        {
            foreach (var ability in _activeAbilities)
                ability.Teardown();
            abilities = new List<PlayerAbility>(_startingAbilities);
            _activeAbilities.Clear();
        }

        private void OnShow(EventParams data)
        {
            if (data is not ExperienceParams { CurrentLevel: > 1 }) return;
            _currentAbilities.Clear();
            for (var i = 0; i < availablePowers.Length; i++)
            {
                var foundPower = abilities.WeightedRandom();
                if (foundPower == null) continue;
                if (!_currentAbilities.TryAdd(i, foundPower)) continue;

                abilities.Remove(foundPower);
                foreach (var remove in foundPower.AbilitiesToRemove)
                    abilities.Remove(remove);

                abilities.AddRange(foundPower.AbilitiesToAdd);
                
                availablePowers[i].text = foundPower.Description;
                availablePowers[i].DOFade(1, textFadeTime)
                    .SetDelay(i * 0.25f)
                    .SetUpdate(true);
            }
            
            foreach (var p in _currentAbilities.Values)
                abilities.Add(p);
        }

        public void SelectPower(int idx)
        {
            if (!_currentAbilities.TryGetValue(idx, out var ability))
                return;

            ability.OnEquip();
            if (ability.OnlyOne)
                abilities.Remove(ability);

            foreach (var t in availablePowers)
                t.DOFade(0, textFadeTime)
                    .SetUpdate(true);
        }
    }
}