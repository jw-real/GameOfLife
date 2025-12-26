using System.Collections.Generic;
using UnityEngine;

public class ThemeManager : MonoBehaviour
{
    // Singleton instance
    public static ThemeManager Instance { get; private set; }

    [Header("Available Themes")]
    public List<ColorThemeSO> AvailableThemes;

    [Header("Default Theme (optional)")]
    public ColorThemeSO DefaultTheme;

    // The currently active theme
    public ColorThemeSO CurrentTheme { get; private set; }

    private void Awake()
    {
        // Singleton enforcement
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Set default theme at start
        if (DefaultTheme != null)
            CurrentTheme = DefaultTheme;
        else if (AvailableThemes.Count > 0)
            CurrentTheme = AvailableThemes[0];
    }

    /// <summary>
    /// Sets the active theme at runtime
    /// </summary>
    public void SetTheme(ColorThemeSO theme)
    {
        if (theme != null)
        {
            CurrentTheme = theme;
            // Optional: Notify listeners that theme has changed
            // e.g., via an event, if you want live updates
        }
    }
}