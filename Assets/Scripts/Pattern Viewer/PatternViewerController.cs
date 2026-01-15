using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PatternViewerController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private RectTransform contentRoot;
    [SerializeField] private GridLayoutGroup grid;
    [SerializeField] private GameObject cellPrefab;

    [Header("Appearance")]
    [SerializeField] private Color aliveColor = Color.black;
    [SerializeField] private Color deadColor = Color.white;
    [SerializeField] private int padding = 2;

    private const string PatternHashKey = "view_pattern_hash";

    void Start()
    {
        string canonical = PatternSelectionContext.SelectedPatternCanonical;

        if (string.IsNullOrEmpty(canonical))
        {
            Debug.LogError("PatternViewer: No pattern provided.");
            return;
        }

        RenderPattern(canonical);
    }

    private void RenderPattern(string hash)
    {
        HashSet<Vector2Int> cells = PatternNormalizer.DecodeCanonical(hash);

        if (cells.Count == 0)
        {
            Debug.LogWarning("PatternViewer: Empty pattern.");
            return;
        }

        // Compute bounding box
        int minX = int.MaxValue;
        int maxX = int.MinValue;
        int minY = int.MaxValue;
        int maxY = int.MinValue;

        foreach (var c in cells)
        {
            if (c.x < minX) minX = c.x;
            if (c.x > maxX) maxX = c.x;
            if (c.y < minY) minY = c.y;
            if (c.y > maxY) maxY = c.y;
        }

        int width  = (maxX - minX + 1) + padding * 2;
        int height = (maxY - minY + 1) + padding * 2;

        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = width;

        ClearGrid();

        // Precompute offset
        int offsetX = -minX + padding;
        int offsetY = -minY + padding;

        // Create lookup for fast access
        HashSet<Vector2Int> shifted = new HashSet<Vector2Int>();
        foreach (var c in cells)
            shifted.Add(new Vector2Int(c.x + offsetX, c.y + offsetY));

        // Generate grid
        for (int y = height - 1; y >= 0; y--)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject cell = Instantiate(cellPrefab, contentRoot);
                Image img = cell.GetComponent<Image>();

                bool alive = shifted.Contains(new Vector2Int(x, y));
                img.color = alive ? aliveColor : deadColor;
                img.raycastTarget = false;
            }
        }
    }

    private void ClearGrid()
    {
        for (int i = contentRoot.childCount - 1; i >= 0; i--)
            Destroy(contentRoot.GetChild(i).gameObject);
    }
}