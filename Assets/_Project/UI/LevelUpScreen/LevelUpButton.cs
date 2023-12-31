using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.LevelUpScreen
{
    public class LevelUpButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Transform[] indicators;
        [SerializeField] private CanvasGroup canvas;
        [SerializeField] private TextMeshProUGUI text;

        private void Start()
        {
            foreach (var indicator in indicators)
                indicator.localScale = Vector3.zero;
            canvas.alpha = 0;
            text.fontStyle = FontStyles.Normal;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            canvas.DOKill();
            text.fontStyle = FontStyles.Bold;
            canvas.DOFade(1f, 0.25f)
                .SetUpdate(true);

            foreach (var indicator in indicators)
            {
                indicator.DOKill();
                indicator.localScale = Vector3.zero;
                indicator.DOScale(Vector3.one, 0.25f)
                    .SetEase(Ease.InOutQuint)
                    .SetUpdate(true);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            text.fontStyle = FontStyles.Normal;
            canvas.DOFade(0f, 0.25f)
                .SetUpdate(true);
        }
    }

}