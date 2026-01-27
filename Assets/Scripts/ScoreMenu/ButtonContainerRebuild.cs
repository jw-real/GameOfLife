using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonContainerRebuild : MonoBehaviour
{
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        // Run after the first frame to allow:
        // - Localization to apply
        // - TMP to calculate preferred sizes
        // - Buttons to finish instantiating
        StartCoroutine(RebuildNextFrame());
    }

    private IEnumerator RebuildNextFrame()
    {
        // Wait one frame
        yield return null;

        // Force layout rebuild
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);

        // Optional: second pass for stubborn layouts
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }
}