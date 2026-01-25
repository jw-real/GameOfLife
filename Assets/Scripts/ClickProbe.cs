using UnityEngine;
using UnityEngine.EventSystems;

public class ClickProbe : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Pointer click detected on back button");
    }
}