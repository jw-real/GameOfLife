using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class StartMenuThemeSelector : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;

    private List<ColorThemeSO> themes;

    void Start()
    {
        themes = new List<ColorThemeSO>(ThemeManager.Instance.AvailableThemes);

        dropdown.ClearOptions();

        var options = new List<string>();
        foreach (var theme in themes)
        {
            // Convert AliveColor and DeadColor to hex
            string aliveHex = theme.GetAliveHex();
            string deadHex = theme.GetDeadHex();

            // Compose a swatch string - small solid squares
            // Unicode character U+25A0 is a solid square: ■
            string swatch = $"<color=#{aliveHex}>■</color><color=#{deadHex}>■</color>";

            // combine swatch + theme name
            string label = $"{swatch} {theme.name}";
            options.Add(label);
        }

        dropdown.AddOptions(options);

        // Select current theme
        int currentIndex = themes.IndexOf(ThemeManager.Instance.CurrentTheme);
        dropdown.SetValueWithoutNotify(Mathf.Max(0, currentIndex));

        dropdown.onValueChanged.AddListener(OnThemeSelected);
    }

    private void OnThemeSelected(int index)
    {
        ThemeManager.Instance.SetTheme(themes[index]);
    }
}