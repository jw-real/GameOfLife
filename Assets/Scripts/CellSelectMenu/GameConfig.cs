using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class GameConfig : MonoBehaviour
{
    [Header("Button Configuration")]
    public int rows = 4;
    public int cols = 3;
    public int maxSelectable = 4;
    public float cellPadding = 4f;
    public float cellSpacing = 8f;

    [Header("References")]
    public Transform gridContainer;      // The object with GridLayoutGroup
    public ButtonConfig buttonPrefab;    // Your button prefab
    
    private List<ButtonConfig> selectedButtons = new List<ButtonConfig>();

        void Start()
    {
        LoadLevelConfigFromJson();
        ApplyGridStyling();
        BuildGrid();
    }

    // -------------------------------
    // JSON support (rows / cols / maxSelectable)
    // -------------------------------
    [System.Serializable]
    private class LevelData
    {
        public int rows;
        public int cols;
        public int maxSelectable;
    }

    private void LoadLevelConfigFromJson()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "level.json");

        if (!File.Exists(path))
        {
            Debug.LogWarning("JSON config not found: " + path);
            return;
        }

        string json = File.ReadAllText(path);
        LevelData data = JsonUtility.FromJson<LevelData>(json);

        rows = data.rows;
        cols = data.cols;
        maxSelectable = data.maxSelectable;
    }

    // -------------------------------
    // Styling
    // -------------------------------
    private void ApplyGridStyling()
    {
        GridLayoutGroup grid = gridContainer.GetComponent<GridLayoutGroup>();

        grid.spacing = new Vector2(cellSpacing, cellSpacing);
        grid.padding = new RectOffset(
            (int)cellPadding,
            (int)cellPadding,
            (int)cellPadding,
            (int)cellPadding
        );

        // Not required but often helps keep the grid centered
        grid.childAlignment = TextAnchor.MiddleCenter;
    }

    // -------------------------------
    // Grid generation
    // -------------------------------
    private void BuildGrid()
    {
        GridLayoutGroup grid = gridContainer.GetComponent<GridLayoutGroup>();

        // Clear existing children (important if regenerating)
        foreach (Transform child in gridContainer)
            Destroy(child.gameObject);

        // Use FixedRowCount because you want rows × cols
        grid.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        grid.constraintCount = rows;

        // Create grid cells
        for (int i = 0; i < rows * cols; i++)
        {
            var btn = Instantiate(buttonPrefab, gridContainer);
            btn.OnSelectedChanged += HandleButtonSelection;
        }
    }

    // -------------------------------
    // Selection logic (unchanged)
    // -------------------------------
    private void HandleButtonSelection(ButtonConfig btn)
    {
        if (btn.isSelected)
        {
            if (selectedButtons.Count >= maxSelectable)
            {
                btn.ForceUnselect();
                return;
            }

            selectedButtons.Add(btn);
        }
        else
        {
            selectedButtons.Remove(btn);
        }
    }
}