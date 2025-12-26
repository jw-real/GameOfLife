using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[ExecuteAlways]
public class CellVisual : MonoBehaviour
{
    public bool IsAlive;

    private Image img;

    private void Awake()
    {
        img = GetComponent<Image>();
        UpdateColor();
    }

    private void OnValidate()
    {
        UpdateColor();
    }

    public void SetAlive(bool alive)
    {
        IsAlive = alive;
        UpdateColor();
    }

    private void UpdateColor()
    {
        if (img == null || ThemeManager.Instance == null) return;

        img.color = IsAlive
            ? ThemeManager.Instance.CurrentTheme.AliveColor
            : ThemeManager.Instance.CurrentTheme.DeadColor;
    }
}
