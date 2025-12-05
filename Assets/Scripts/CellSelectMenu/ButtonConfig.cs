using UnityEngine;
using UnityEngine.UI;

public class ButtonConfig : MonoBehaviour
{
    [SerializeField] private Image img; // The Image component on the button
    [SerializeField] private Color selectedColor = Color.blue;
    [SerializeField] private Color unselectedColor = Color.white;
    public int x;
    public int y;
    public System.Action<ButtonConfig> OnSelectedChanged;


    public bool isSelected = false;

    void Awake()
    {
        // If you forget to assign the Image manually in the inspector,
        // this auto-finds it on the same GameObject.
        if (img == null)
        {
            img = GetComponentInChildren<Image>();
        }

        img.color = unselectedColor;
    }

    public void ToggleSelected()
    {
        isSelected = !isSelected;
        Debug.Log("ToggleSelected called. New state: " + isSelected);
        img.color = isSelected ? Color.blue : Color.white;
        OnSelectedChanged?.Invoke(this);
    }

    public void ForceUnselect()
    {
        isSelected = false;
        img.color = unselectedColor;
        OnSelectedChanged?.Invoke(this);
    }
}
