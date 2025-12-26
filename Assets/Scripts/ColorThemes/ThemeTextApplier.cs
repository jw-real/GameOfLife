using TMPro;
using UnityEngine;

public class ThemeTextApplier : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    [TextArea] public string localizedStringWithTokens;

    void Start()
    {
        var theme = ThemeManager.Instance.CurrentTheme;
        string finalText = localizedStringWithTokens
            .Replace("{ALIVE}", "#" + theme.GetAliveHex())
            .Replace("{DEAD}", "#" + theme.GetDeadHex());

        textComponent.text = finalText;
    }
}