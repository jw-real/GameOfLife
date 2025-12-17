using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [Header("References")]
    public CinemachineVirtualCamera vcam;   // assign in Inspector
    public Transform boundsTarget;          // assign in Inspector

    [Header("Framing")]
    public float padding = 2f;
    public float zoomLerpSpeed = 2f;

    // Optional: minimal orthographic size to avoid zero or negative zoom
    public float minOrthoSize = 5f;

    void Reset()
    {
        if (vcam == null)
            vcam = GetComponent<CinemachineVirtualCamera>();
    }

    /// <summary>
    /// Moves the bounds target to the center of the living-cell bounds
    /// and adjusts orthographic size smoothly.
    /// Safe if bounds are empty or references are missing.
    /// </summary>
    public void ApplyBounds(BoundsInt bounds)
    {
        // Safety checks
        if (vcam == null || boundsTarget == null)
            return;

        if (bounds.size == Vector3Int.zero)
            return; // no living cells

        // Move the follow target to the center of the bounds
        boundsTarget.position = bounds.center;

        // Convert size to float and compute half-extents
        Vector3 halfSize = (Vector3)bounds.size * 0.5f;

        // Compute required orthographic size
        float requiredSize = Mathf.Max(
            halfSize.y + padding,
            (halfSize.x + padding) / vcam.m_Lens.Aspect,
            minOrthoSize
        );

        // Smoothly interpolate camera zoom
        vcam.m_Lens.OrthographicSize = Mathf.Lerp(
            vcam.m_Lens.OrthographicSize,
            requiredSize,
            Time.deltaTime * zoomLerpSpeed
        );
        Debug.Log($"ApplyBounds called. Bounds center: {bounds.center}");
    }
}