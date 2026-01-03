using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GameConfig))]
public class CellSelectZoomController : MonoBehaviour
{
    [Header("References")]
    public RectTransform gridContainer;      // The object inside ScrollRect
    public ScrollRect scrollRect;            // Optional: used to recenter after zoom

    [Header("Zoom Settings")]
    [Tooltip("Scale multiplier per scroll unit")]
    public float zoomSpeed = 0.1f;

    [Tooltip("Scale multiplier per pinch gesture")]
    public float pinchSpeed = 0.01f;

    [Tooltip("Minimum scale allowed")]
    public float minScale = 0.5f;

    [Tooltip("Maximum scale allowed")]
    public float maxScale = 3f;

    private Vector3 initialScale;

    void Awake()
    {
        if (gridContainer == null)
            Debug.LogError("CellSelectZoomController: GridContainer not assigned!");

        initialScale = gridContainer.localScale;
    }

    void Update()
    {
        HandleMouseWheelZoom();
        HandleTouchPinchZoom();
    }

    // ----------------------
    // Mouse wheel zoom (desktop)
    // ----------------------
    private void HandleMouseWheelZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.0001f)
        {
            ApplyZoom(scroll * zoomSpeed);
        }
    }

    // ----------------------
    // Pinch-to-zoom (mobile)
    // ----------------------
    private void HandleTouchPinchZoom()
    {
        if (Input.touchCount == 2)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            // Current distance between touches
            float currentDistance = Vector2.Distance(t0.position, t1.position);

            // Store previous distance in touch delta
            float prevDistance = Vector2.Distance(t0.position - t0.deltaPosition, t1.position - t1.deltaPosition);

            float delta = currentDistance - prevDistance;

            ApplyZoom(delta * pinchSpeed);
        }
    }

    // ----------------------
    // Apply zoom and clamp scale
    // ----------------------
    private void ApplyZoom(float deltaScale)
    {
        Vector3 newScale = gridContainer.localScale + Vector3.one * deltaScale;
        newScale.x = Mathf.Clamp(newScale.x, minScale, maxScale);
        newScale.y = Mathf.Clamp(newScale.y, minScale, maxScale);
        newScale.z = 1f;

        gridContainer.localScale = newScale;

        // Optional: keep content centered in ScrollRect
        if (scrollRect != null)
        {
            scrollRect.normalizedPosition = new Vector2(0.5f, 0.5f);
        }
    }

    /// <summary>
    /// Resets zoom to initial scale.
    /// </summary>
    public void ResetZoom()
    {
        gridContainer.localScale = initialScale;

        if (scrollRect != null)
            scrollRect.normalizedPosition = new Vector2(0.5f, 0.5f);
    }
}