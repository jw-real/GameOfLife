using UnityEngine;
using UnityEngine.UI;

public class ButtonConfig : MonoBehaviour
{
    [SerializeField] private Image img;

    public int x;
    public int y;

    public System.Action<ButtonConfig> OnSelectedChanged;

    public bool isSelected = false;

    private Color aliveColor;
    private Color deadColor;

    void Awake()
    {
        if (img == null)
            img = GetComponentInChildren<Image>();

        var theme = ThemeManager.Instance.CurrentTheme;
        aliveColor = theme.AliveColor;
        deadColor  = theme.DeadColor;

        img.color = deadColor;
    }

    public void ToggleSelected()
    {
        isSelected = !isSelected;
        img.color = isSelected ? aliveColor : deadColor;
        OnSelectedChanged?.Invoke(this);
    }

    public void ForceUnselect()
    {
        isSelected = false;
        img.color = deadColor;
        OnSelectedChanged?.Invoke(this);
    }
}

