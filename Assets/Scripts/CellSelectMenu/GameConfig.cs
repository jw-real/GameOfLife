using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine.UIElements;

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
    public TMP_Text selectionCounterText;
    private int remainingSelectable;
    
    private List<ButtonConfig> selectedButtons = new List<ButtonConfig>();

        void Start()
    {
        LoadLevelConfigFromJson();
        ApplyGridStyling();
        BuildGrid();
        remainingSelectable = maxSelectable;
        UpdateSelectionUI();
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
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                var btn = Instantiate(buttonPrefab, gridContainer);

                // Assign grid coordinates
                btn.x = c;
                btn.y = r;

                btn.OnSelectedChanged += HandleButtonSelection;
            }
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
            remainingSelectable--;
            Debug.Log("Remaining Decremented");
        }
        else
        {
            selectedButtons.Remove(btn);
            remainingSelectable++;
            Debug.Log("Remaining Incremented");
        }

        UpdateSelectionUI();
    }
 
    private void UpdateSelectionUI()
    {
        if (selectionCounterText != null)
        {
            selectionCounterText.text = remainingSelectable.ToString();
        }
    }

    private PatternData BuildPatternData()
    {
        PatternData data = new PatternData();
        data.columns = cols;
        data.rows = rows;

        data.selectedCells = new List<CellPosition>();

        foreach (var btn in selectedButtons)
        {
            data.selectedCells.Add(new CellPosition(btn.x, btn.y));
        }
        return data;
    }

    public void ConfirmSelection()
    {
        if (selectedButtons.Count != maxSelectable)
        {
            Debug.LogWarning("You must select exactly " + maxSelectable + " tiles!");
            return;
        }

        Debug.Log("Selection confirmed!");
        // JSON creation + scene switch goes here later
        PatternData data = BuildPatternData();
        string json = OutputPatternToJson(data);

        Debug.Log("Pattern JSON: " + json);

        PlayerPrefs.SetString("runtime_pattern", json);
        PlayerPrefs.Save();
    }

    private string OutputPatternToJson(PatternData data)
    {
        return JsonUtility.ToJson(data);
    }
}