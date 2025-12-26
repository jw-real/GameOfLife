using UnityEngine;

public class GridLayoutHelper : MonoBehaviour
{
    [Header("Grid Settings")]
    public int columns = 4;
    public int rows = 4;
    public float cellSize = 256f;    // Size of each cell in local units
    public float spacing = 0f;       // Gap between cells

    [Header("Cells")]
    public Transform[] cells;        // Assign all 16 cells here in Inspector

    public void ArrangeGrid()
    {
        if (cells == null || cells.Length != columns * rows)
        {
            Debug.LogError("Please assign all cells in the Inspector.");
            return;
        }

        float gridWidth  = (columns - 1) * (cellSize + spacing);
        float gridHeight = (rows - 1) * (cellSize + spacing);
        float offsetX = gridWidth / 2f;
        float offsetY = gridHeight / 2f;

        int index = 0;
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                if (index >= cells.Length) break;

                RectTransform rt = cells[index] as RectTransform;
                if (rt == null)
                {
                    Debug.LogError("Cell is not a RectTransform");
                }

                rt.anchoredPosition = new Vector2(
                x * (cellSize + spacing) - offsetX,
                -(y * (cellSize + spacing) - offsetY)
                );

                index++;
            }
        }
    }

    // Optional: auto-arrange on start
    private void Start()
    {
        ArrangeGrid();
    }
}
