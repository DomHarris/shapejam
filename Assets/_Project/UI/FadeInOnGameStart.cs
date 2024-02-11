using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Stats;
using UnityEngine;

public class FadeInOnGameStart : MonoBehaviour
{
    [SerializeField] private EventAction gameStart;
    
    // Start is called before the first frame update
    void Start()
    {
        gameStart += OnGameStart;
    }
    
    private void OnDestroy()
    {
        gameStart -= OnGameStart;
    }
    
    private void OnGameStart(EventParams obj)
    {
        GetComponent<CanvasGroup>().DOFade(1, 0.5f);
    }
}
