using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spawning;
using Stats;
using TMPro;
using UI.LevelUpScreen;
using UnityEngine;

public class WaveOverlay : MonoBehaviour
{
    [SerializeField] private EventAction onWaveStart;
    [SerializeField] private EventAction onGameStart;
    [SerializeField] private GameOverlay gameOverlay;
    [SerializeField] private float timeToShow = 1f;
    [SerializeField] private TextMeshProUGUI text;
    
    private int _currentWave = 1;
    
    // Start is called before the first frame update
    private void Start()
    {
        onWaveStart += OnWaveStart;
        onGameStart += OnGameStart;
    }
    
    private void OnDestroy()
    {
        onWaveStart -= OnWaveStart;
        onGameStart -= OnGameStart;
    }

    private void OnGameStart(EventParams obj)
    {
        _currentWave = 0;
        text.text = $"Wave {_currentWave}";
    }

    private void OnWaveStart(EventParams obj)
    {
        ++_currentWave;
        text.text = $"Wave {_currentWave}";
        DOVirtual.DelayedCall(timeToShow, gameOverlay.Hide);
    }
}
