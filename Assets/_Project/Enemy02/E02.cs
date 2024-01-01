using Entity;
using UnityEngine;

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

    private void Update()
    {
        if (_activateEnemy) ShootBullet();
    }

    public void Hit(float damage)
    {
        if (!_activateEnemy)
        {
            spriteRenderer.color = hitColor;
            _activateEnemy = true;
        }
    }

    void ShootBullet()
    {
        
    }
}
