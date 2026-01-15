using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class GameConfig : MonoBehaviour
{
    [Header("Grid Styling")]
    public float cellPadding = 4f;
    public float cellSpacing = 8f;

    [Header("References")]
    public Transform gridContainer;      // The object with GridLayoutGroup
    public ButtonConfig buttonPrefab;    // Your button prefab
    public TMP_Text selectionCounterText;
    public GameConfigSO config;

    private int rows;
    private int cols;
    private int maxSelectable;
    //private int remainingSelectable;
    
    private readonly List<ButtonConfig> selectedButtons = new List<ButtonConfig>();

    // ----------------------------------------------------------------------------
        void Start()
    {
        LoadConfig();
        ApplyGridStyling();
        BuildGrid();

        //remainingSelectable = maxSelectable;
        UpdateSelectionUI();
    }

    // -------------------------------
    // GameConfig ScriptableObject support (rows / cols / maxSelectable)
    // -------------------------------
    private void LoadConfig()
    {
        if (config == null)
        {
            Debug.LogError("GameConfigSO not assigned in Inspector!");
            return;
        }

        string path = Path.Combine(Application.persistentDataPath, "player_profile.json");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            ProgressionData prog = JsonUtility.FromJson<ProgressionData>(json);

            rows = prog.rows;
            cols = prog.cols;
            maxSelectable = prog.maxSelectable;

            Debug.Log($"Loaded progression → rows={rows}, cols={cols}, maxSelectable={maxSelectable}");
        }
        else
        {
            // Only here do we touch the ScriptableObject
            rows = config.rows;
            cols = config.cols;
            maxSelectable = config.maxSelectable;

            Debug.Log("No runtime progression found — using default config.");
        }
    }

    // -------------------------------
    // Grid Styling
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
    // Grid Generation
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
    // Selection Logic
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
            //remainingSelectable--;
        }
        else
        {
            selectedButtons.Remove(btn);
            //remainingSelectable++;
        }

        UpdateSelectionUI();
    }
 
    private void UpdateSelectionUI()
    {
        int remainingSelectable = maxSelectable - selectedButtons.Count;

        if (selectionCounterText != null)
            selectionCounterText.text = remainingSelectable.ToString();
    }

    private PatternData BuildPatternData()
    {
        PatternData data = new PatternData();
        data.rows = rows;
        data.columns = cols;
        data.selectedCells = new List<CellPosition>();

        foreach (var btn in selectedButtons)
            data.selectedCells.Add(new CellPosition(btn.x, btn.y));

        return data;
    }

    private RunResultData InitializeRunResult(HashSet<Vector2Int> selectedCells)
    {
        RunResultData runResult = new RunResultData();

        // Identity (set once)
        runResult.patternCanonical =
            PatternNormalizer.ComputeCanonicalPattern(selectedCells);

        // Metrics (initialized, not yet accumulated)
        runResult.roundScore = 0;
        runResult.totalIterations = 0;

        // Persist for the duration of the run
        string json = JsonUtility.ToJson(runResult);
        PlayerPrefs.SetString("run_result", json);
        PlayerPrefs.Save();

        return runResult;
    }

    private HashSet<Vector2Int> ToCellSet(List<ButtonConfig> buttons)
    {
        HashSet<Vector2Int> set = new HashSet<Vector2Int>();

        foreach (var btn in buttons)
        {
            set.Add(btn.GridPosition);
        }

        return set;
    }

    public void ConfirmSelection()
    {
        if (selectedButtons.Count != maxSelectable)
        {
            Debug.LogWarning($"You must select exactly {maxSelectable} tiles!");
            return;
        }

        // 1. Persist the pattern for the simulation
        var patternData = BuildPatternData();
        PlayerPrefs.SetString("runtime_pattern", JsonUtility.ToJson(patternData));

        // 2. Initialize run result (identity + zeroed metrics)
        HashSet<Vector2Int> cellSet = ToCellSet(selectedButtons);
        InitializeRunResult(cellSet);

        // 3. Commit and transition
        PlayerPrefs.Save();
        SceneManager.LoadScene("Game of Life");
    }
}