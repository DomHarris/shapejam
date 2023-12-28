using UnityEngine;

public class E02 : MonoBehaviour
{
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color hitColor;
    private SpriteRenderer _spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = (SpriteRenderer)FindFirstObjectByType(typeof(SpriteRenderer));
        _spriteRenderer.color = defaultColor;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) _spriteRenderer.color = hitColor;
    }
}
