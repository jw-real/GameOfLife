using UnityEngine;

[ExecuteAlways]
public class GridLayoutHelper : MonoBehaviour
{
    [Header("Grid Settings")]
    public int columns = 4;
    public int rows = 4;
    public float cellSize = 105f;
    public float spacing = 0f;

    [Header("Cells")]
    public RectTransform[] cells;

    public void ArrangeGrid()
    {
        if (cells == null || cells.Length != columns * rows)
            return;

        float gridWidth  = (columns - 1) * (cellSize + spacing);
        float gridHeight = (rows - 1) * (cellSize + spacing);
        float offsetX = gridWidth / 2f;
        float offsetY = gridHeight / 2f;

        int index = 0;
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                RectTransform rt = cells[index];
                rt.anchoredPosition = new Vector2(
                    x * (cellSize + spacing) - offsetX,
                    -(y * (cellSize + spacing) - offsetY)
                );
                index++;
            }
        }
    }

    private void OnValidate()
    {
        ArrangeGrid();
    }
}