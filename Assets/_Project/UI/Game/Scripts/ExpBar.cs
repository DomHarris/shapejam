using System.Threading.Tasks;
using DG.Tweening;
using Player;
using Stats;
using UnityEngine;
using UnityEngine.UI;

public class ExpBar : MonoBehaviour
{
    [SerializeField] private EventAction expGained;
    [SerializeField] private EventAction levelUp;
    [SerializeField] private float animationTime = 0.2f;
    [SerializeField] private float animationDelay = 0.2f;

    private Image _image;
    
    private void Start()
    {
        _image = GetComponent<Image>();
        expGained += OnExpGain;
        levelUp += OnLevelUp;
    }
    
    private void OnDestroy()
    {
        expGained -= OnExpGain;
        levelUp -= OnLevelUp;
    }
    
    private async void OnExpGain(EventParams obj)
    {
        if (obj is not ExperienceParams expData) return;

        await Task.Yield();
        
        _image.DOFillAmount(1-expData.Experience.GetExpToNextLevel() / expData.Experience.GetExpForNextLevel(), animationTime)
            .SetEase(Ease.OutQuint)
            .SetDelay(animationDelay);
    }
    
    private void OnLevelUp(EventParams obj)
    {
        if (obj is not ExperienceParams expData) return;
        _image.DOKill();
        _image.DOFillAmount(1, animationTime)
            .SetEase(Ease.OutQuint)
            .SetDelay(animationDelay)
            .SetUpdate(true);
    }
}