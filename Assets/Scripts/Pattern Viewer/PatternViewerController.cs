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

    private const string PatternHashKey = "view_pattern_hash"; // Possibly delete this

    void Start()
    {
        Debug.Log("PatternViewer Start()");
        string canonical = PatternSelectionContext.SelectedPatternCanonical;
        Debug.Log($"Canonical = {canonical}");

        if (string.IsNullOrEmpty(canonical))
        {
            Debug.LogError("PatternViewer: No pattern provided.");
            return;
        }

        RenderPattern(canonical);
    }

    private void RenderPattern(string patternCanonical)
    {
        bool[,] gridData = PatternNormalizer.DecodeCanonicalToGrid(patternCanonical);

        int dataWidth  = gridData.GetLength(0);
        int dataHeight = gridData.GetLength(1);

        if (dataWidth == 0 || dataHeight == 0)
        {
            Debug.LogWarning("PatternViewer: Empty pattern.");
            return;
        }

        int width  = dataWidth  + padding * 2;
        int height = dataHeight + padding * 2;

        // Configure grid layout
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = width;

        // Optional: set cell size
        grid.cellSize = new Vector2(20f, 20f); // adjust to fit
        grid.spacing = new Vector2(2f, 2f);

        ClearGrid();

        // Render top-to-bottom
        for (int y = height - 1; y >= 0; y--)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject cell = Instantiate(cellPrefab, grid.transform); // <-- change parent here
                Image img = cell.GetComponent<Image>();

                int dataX = x - padding;
                int dataY = y - padding;

                bool alive =
                    dataX >= 0 && dataX < dataWidth &&
                    dataY >= 0 && dataY < dataHeight &&
                    gridData[dataX, dataY];

                img.color = alive ? aliveColor : deadColor;
                img.raycastTarget = false;
            }
        }
    }

    private void ClearGrid()
    {
        // Only destroy child objects, not the root
        for (int i = grid.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(grid.transform.GetChild(i).gameObject);
        }
    }
}