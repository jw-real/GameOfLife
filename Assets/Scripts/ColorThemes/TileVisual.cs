using UnityEngine;

public class TileVisual : MonoBehaviour
{
    public bool IsAlive;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        ApplyTheme();
    }

    public void ApplyTheme()
    {
        var theme = ThemeManager.Instance.CurrentTheme;
        spriteRenderer.color = IsAlive ? theme.AliveColor : theme.DeadColor;
    }
}