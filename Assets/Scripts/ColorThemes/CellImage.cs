using UnityEngine;
using UnityEngine.UI;

public class CellImage : MonoBehaviour
{
    public bool IsAlive;
    private Image image;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    void Start()
    {
        ApplyTheme();
    }

    public void ApplyTheme()
    {
        var theme = ThemeManager.Instance.CurrentTheme;
        image.color = IsAlive ? theme.AliveColor : theme.DeadColor;
    }
}