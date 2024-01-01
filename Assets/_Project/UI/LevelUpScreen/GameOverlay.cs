using DG.Tweening;
using Stats;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

namespace UI.LevelUpScreen
{
    public class GameOverlay : MonoBehaviour
    {
        [SerializeField] private EventAction showEvent;
        [SerializeField] private Volume postProcessing;
        [SerializeField] private CanvasGroup canvas;
        [SerializeField] private TextMeshProUGUI mainText;

        [SerializeField] private RectTransform leftImage;
        [SerializeField] private RectTransform rightImage;
        [SerializeField] private TextMeshProUGUI[] options;

        [SerializeField] private Ease easeIn = Ease.OutBack;
        [SerializeField] private Ease easeOut = Ease.InQuint;
        [SerializeField] private float animateTime = 1f;

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
            showEvent += OnShow;
            foreach (var o in options)
                o.alpha = 0;
        }

        private void OnShow(EventParams data)
        {
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

            foreach (var o in options)
                o.DOFade(1, animateTime)
                    .SetUpdate(true);
        }

        public void Hide()
        {
            Time.timeScale = 1f;
            canvas.blocksRaycasts = false;

            DOVirtual.Float(1, 0, animateTime, val => postProcessing.weight = val)
                .SetUpdate(true);
            leftImage.DOAnchorPosX(0, animateTime)
                .SetEase(easeOut)
                .SetUpdate(true);
            rightImage.DOAnchorPosX(0, animateTime)
                .SetEase(easeOut)
                .SetUpdate(true);
            mainText.rectTransform.DOAnchorPosY(200, animateTime)
                .SetEase(easeOut)
                .SetUpdate(true);

            foreach (var o in options)
                o.DOFade(0, animateTime)
                    .SetUpdate(true);
        }
    }
}