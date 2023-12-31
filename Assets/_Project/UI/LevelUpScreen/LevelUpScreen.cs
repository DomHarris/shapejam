using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Player;
using Player.Abilities;
using Stats;
using TMPro;
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
        [SerializeField] private EventAction levelUpEvent;
        [SerializeField] private Volume postProcessing;
        [SerializeField] private CanvasGroup canvas;
        [SerializeField] private TextMeshProUGUI mainText;
        
        [SerializeField] private RectTransform leftImage;
        [SerializeField] private RectTransform rightImage;

        [SerializeField] private Ease easeIn = Ease.OutBack;
        [SerializeField] private Ease easeOut = Ease.InQuint;
        [SerializeField] private float animateTime = 1f;

        [SerializeField] private TextMeshProUGUI[] availablePowers;
        
        [SerializeField] private List<PlayerAbility> abilities;


        private Dictionary<int, PlayerAbility> _currentAbilities = new ();
        private float _leftStartX, _rightStartX, _textStartY;

        private void Start()
        {
            _leftStartX = leftImage.anchoredPosition.x;
            _rightStartX = rightImage.anchoredPosition.x;
            _textStartY = mainText.rectTransform.anchoredPosition.y;
            leftImage.anchoredPosition = new Vector2(0, leftImage.anchoredPosition.y);
            rightImage.anchoredPosition = new Vector2(0, rightImage.anchoredPosition.y);
            mainText.rectTransform.anchoredPosition = new Vector2(mainText.rectTransform.anchoredPosition.x, 200);
            postProcessing.weight = 0;
            canvas.blocksRaycasts = false;
            foreach (var text in availablePowers)
            {
                text.text = "";
                text.alpha = 0;
            }
            levelUpEvent += OnLevelUp;
        }

        private void OnLevelUp(EventParams data)
        {
            if (data is not ExperienceParams levelUpData)
                return;

            canvas.blocksRaycasts = true;
            Time.timeScale = 0f;
            DOVirtual.Float(0, 1, animateTime, val => postProcessing.weight = val)
                .SetUpdate(true);
            leftImage.DOAnchorPosX(_leftStartX, animateTime)
                .SetEase(easeIn)
                .SetUpdate(true);
            rightImage.DOAnchorPosX(_rightStartX, animateTime)
                .SetEase(easeIn)
                .SetUpdate(true);
            mainText.rectTransform.DOAnchorPosY(_textStartY, animateTime)
                .SetEase(easeIn)
                .SetUpdate(true);

            _currentAbilities.Clear();
            for (var i = 0; i < availablePowers.Length; i++)
            {
                var foundPower = abilities.WeightedRandom();
                if (foundPower == null) continue;
                if (!_currentAbilities.TryAdd(i, foundPower)) continue;
                
                abilities.Remove(foundPower);
                
                availablePowers[i].text = foundPower.Description;
                availablePowers[i].DOFade(1, animateTime)
                    .SetDelay(i * 0.25f)
                    .SetUpdate(true);
            }
        }
        
        public void SelectPower(int idx)
        {
            if (!_currentAbilities.TryGetValue(idx, out var ability))
                return;
            
            Time.timeScale = 1f;
            canvas.blocksRaycasts = false;

            ability.OnEquip();
            DOVirtual.Float(1, 0, animateTime, val => postProcessing.weight = val)
                .SetUpdate(true);
            leftImage.DOAnchorPosX(0, animateTime)
                .SetEase(easeIn)
                .SetUpdate(true);
            rightImage.DOAnchorPosX(0, animateTime)
                .SetEase(easeIn)
                .SetUpdate(true);
            mainText.rectTransform.DOAnchorPosY(200, animateTime)
                .SetEase(easeIn)
                .SetUpdate(true);
            foreach (var t in availablePowers)
                t.DOFade(0, animateTime)
                .SetUpdate(true);
        }
    }
}