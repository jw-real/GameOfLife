using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonConfig : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image img;

    public int x;
    public int y;

    public System.Action<ButtonConfig> OnSelectedChanged;

    public bool isSelected = false;

    [SerializeField] private Color aliveColor;
    [SerializeField] private Color deadColor;

    void Awake()
    {
        if (img == null)
            img = GetComponentInChildren<Image>();
        
        img.canvasRenderer.SetAlpha(255f);

        img.color = isSelected ? aliveColor : Color.white;

        //LogAlphaState("Awake");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ToggleSelected();
    }

    public void ToggleSelected()
    {
        isSelected = !isSelected;

        img.canvasRenderer.SetAlpha(255f);

        img.color = isSelected ? aliveColor : Color.white;
        OnSelectedChanged?.Invoke(this);
        //LogAlphaState("ToggleSelected");
    }

    public void ForceUnselect()
    {
        isSelected = false;

        img.canvasRenderer.SetAlpha(255f);

        img.color = Color.white;
        OnSelectedChanged?.Invoke(this);
        //LogAlphaState("Force Unselect");
    }

    private void LogAlphaState(string label)
    {
        var cr = img.canvasRenderer;

        float rendererAlpha = cr.GetAlpha();
        float imageAlpha    = img.color.a;

        CanvasGroup cg = GetComponentInParent<CanvasGroup>();
        float canvasGroupAlpha = cg != null ? cg.alpha : 1f;

        Debug.Log(
            $"[{label}] " +
            $"RendererAlpha={rendererAlpha}, " +
            $"ImageAlpha={imageAlpha}, " +
            $"CanvasGroupAlpha={canvasGroupAlpha}"
        );
    }
}

