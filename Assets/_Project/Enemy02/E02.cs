using Entity;
using UnityEngine;

public class E02 : MonoBehaviour, IHit
{
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color hitColor;
    private SpriteRenderer _spriteRenderer;

    private bool _activateEnemy;
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _spriteRenderer.color = defaultColor;
    }

    public void Hit(float damage)
    {
        if (!_activateEnemy)
        {
            _spriteRenderer.color = hitColor;
            _activateEnemy = true;
        }
        
    }
}
