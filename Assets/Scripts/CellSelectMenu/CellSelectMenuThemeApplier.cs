using UnityEngine;

public class CellSelectMenuThemeApplier : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;
    void Start()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;

        var theme = ThemeManager.Instance.CurrentTheme;
        targetCamera.backgroundColor = theme.SimulationBackgroundColor;
    }
}