using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShowTitle : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private CanvasGroup textCanvas;
    [SerializeField] private RectTransform background;

    [SerializeField] private CanvasGroup[] buttons;
    
    [SerializeField] private TextMeshProUGUI continueText;

    private bool _pressedButton = false;
    
    // Start is called before the first frame update
    IEnumerator Start()
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
        SceneManager.LoadScene("Main", LoadSceneMode.Additive);
        textCanvas.alpha = 0;
        continueText.alpha = 0;
        background.sizeDelta = new Vector2(background.sizeDelta.x, 0);
        yield return new WaitForSeconds(0.25f);
        textCanvas.DOFade(1, 3f);
        DOVirtual.Float(0, text.characterSpacing, 3f, SetSpacing)
            .SetEase(Ease.OutQuint);
        background.DOSizeDelta(new Vector2(background.sizeDelta.x, 300), 3f)
            .SetEase(Ease.OutQuint);

        text.rectTransform.DOAnchorPosY(100, 2f)
            .SetEase(Ease.OutQuint)
            .SetDelay(2.5f);
        background.DOAnchorPosY(100, 2f)
            .SetEase(Ease.OutQuint)
            .SetDelay(2.5f);
        
        continueText.rectTransform.DOAnchorPosY(-100, 2f)
            .SetEase(Ease.OutQuint)
            .SetDelay(3f);
        continueText.DOFade(1, 2f)
            .SetDelay(3f);

        yield return new WaitForSeconds(5f);
        while (!_pressedButton)
            yield return null;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        continueText.DOFade(0f, 0.5f);
        
        text.rectTransform.DOAnchorPosY(300, 1f)
            .SetEase(Ease.OutQuint);
        background.DOAnchorPosY(300, 1f)
            .SetEase(Ease.OutQuint);

        for (int i = 0; i < buttons.Length; ++i)
        {
            buttons[i].DOFade(1, 1f)
                .SetDelay(0.5f + i * 0.1f);
            buttons[i].interactable = true;
            buttons[i].blocksRaycasts = true;
        }

        EventSystem.current.SetSelectedGameObject(buttons[0].gameObject);
    }

    public void PressButton()
    {
        _pressedButton = true;
    }
    
    private void SetSpacing(float value)
    {
        text.characterSpacing = value;
    }
}
