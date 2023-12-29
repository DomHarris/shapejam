using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitleButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private Transform indicator;
    [SerializeField] private TextMeshProUGUI text;
    
    public void OnSelect(BaseEventData eventData)
    {
        text.fontStyle = FontStyles.Bold;
        indicator.DOScale(Vector3.one, 0.25f).SetEase(Ease.InOutQuint);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        text.fontStyle = FontStyles.Normal;
        indicator.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InOutQuint);
    }
}
