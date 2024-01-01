using Entity;
using UnityEngine;
using UnityEngine.Serialization;

public class E02 : MonoBehaviour, IHit
{
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color hitColor;
    [SerializeField] SpriteRenderer spriteRenderer;

    private bool _activateEnemy;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.color = defaultColor;
    }

    public void Hit(float damage)
    {
        if (!_activateEnemy)
        {
            spriteRenderer.color = hitColor;
            _activateEnemy = true;
        }
    }
}
