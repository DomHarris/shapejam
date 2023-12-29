using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShowTitle : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private CanvasGroup textCanvas;
    [SerializeField] private RectTransform background;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Additive);
        textCanvas.alpha = 0;
        background.sizeDelta = new Vector2(background.sizeDelta.x, 0);
        yield return new WaitForSeconds(1f);
        textCanvas.DOFade(1, 3f);
        DOVirtual.Float(0, text.characterSpacing, 3f, SetSpacing)
            .SetEase(Ease.OutQuint);
        background.DOSizeDelta(new Vector2(background.sizeDelta.x, 300), 3f)
            .SetEase(Ease.OutQuint);
    }

    private void SetSpacing(float value)
    {
        text.characterSpacing = value;
    }
}
