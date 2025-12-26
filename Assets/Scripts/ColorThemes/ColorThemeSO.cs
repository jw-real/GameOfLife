using UnityEngine;

[CreateAssetMenu(fileName = "AlienTheme", menuName = "ColorThemes/Alien")]
public class ColorThemeSO : ScriptableObject
{
    [Header("Cell Colors")]
    public Color AliveColor = new Color(0.345f, 0.753f, 0.662f); // '#58c0a9'
    public Color DeadColor  = new Color(0.396f, 0.196f, 0.561f); // '#65328f'

    [Header("Simulation Background")]
    public Color SimulationBackgroundColor; // default can be DeadColor in Start()

    /// <summary>
    /// Returns the hex string for TextMeshPro rich text replacement
    /// </summary>
    public string GetAliveHex() => ColorUtility.ToHtmlStringRGB(AliveColor);
    public string GetDeadHex()  => ColorUtility.ToHtmlStringRGB(DeadColor);
}
