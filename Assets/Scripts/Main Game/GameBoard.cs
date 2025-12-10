using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

[DefaultExecutionOrder(-1)]
public class GameBoard : MonoBehaviour
{
    [Header("Tilemaps")]
    [SerializeField] private Tilemap currentState;
    [SerializeField] private Tilemap nextState;
    [SerializeField] private Tile aliveTile;
    [SerializeField] private Tile deadTile;

    [Header("Simulation")]
    [SerializeField] private float updateInterval = 0.05f;
    [SerializeField] private int iterationCap = 500;

    [Header("Starting Constraints (set by PatternLoader)")]
    public int startRows;
    public int startCols;
    public int startMaxSelectable;

    private readonly HashSet<Vector3Int> aliveCells = new();
    private readonly HashSet<Vector3Int> cellsToCheck = new();

    public int population { get; private set; }
    public int iterations { get; private set; }
    public float time { get; private set; }

    private List<int> populationHistory = new();

    private void Start() { }

    // Called by PatternLoader
    public void ApplyPattern(PatternData pattern)
    {
        Clear();

        // Compute center of the selected cells
        Vector2Int center = ComputeCenter(pattern.selectedCells);

        foreach (var cell in pattern.selectedCells)
        {
            Vector3Int pos = new Vector3Int(cell.x - center.x, cell.y - center.y, 0);

            currentState.SetTile(pos, aliveTile);
            aliveCells.Add(pos);
        }

        population = aliveCells.Count;
        populationHistory.Add(population);
    }

    private Vector2Int ComputeCenter(List<CellPosition> cells)
    {
        if (cells == null || cells.Count == 0)
            return Vector2Int.zero;

        int minX = int.MaxValue, maxX = int.MinValue;
        int minY = int.MaxValue, maxY = int.MinValue;

        foreach (var c in cells)
        {
            if (c.x < minX) minX = c.x;
            if (c.x > maxX) maxX = c.x;
            if (c.y < minY) minY = c.y;
            if (c.y > maxY) maxY = c.y;
        }

        return new Vector2Int((minX + maxX) / 2, (minY + maxY) / 2);
    }

    private void Clear()
    {
        aliveCells.Clear();
        cellsToCheck.Clear();
        currentState.ClearAllTiles();
        nextState.ClearAllTiles();

        population = 0;
        iterations = 0;
        time = 0.0f;
        populationHistory.Clear();
    }

    private void OnEnable()
    {
        StartCoroutine(Simulate());
    }

    private IEnumerator Simulate()
    {
        // caching to prevent memory heap consumption - better methods are available
        var interval = new WaitForSeconds(updateInterval);
        yield return interval;

        while (enabled)
        {
            UpdateState();

            population = aliveCells.Count;
            populationHistory.Add(population);

            iterations++;
            time += updateInterval;

            // 1️⃣ Stop if no live cells remain
            if (population == 0)
            {
                Debug.Log("Simulation ended early — no live cells.");
                EndSimulation();
                yield break;
            }

            // 2️⃣ Stop if iteration cap reached
            if (iterations >= iterationCap)
            {
                Debug.Log("Simulation reached iteration cap.");
                EndSimulation();
                yield break;
            }

            yield return interval;
        }
    }

    private void UpdateState()
    {
        cellsToCheck.Clear();

        foreach (Vector3Int cell in aliveCells)
        {
            for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 1; y++)
                cellsToCheck.Add(cell + new Vector3Int(x, y, 0));
        }

        // Transition cells to the next state
        foreach (Vector3Int cell in cellsToCheck)
        {
            int neighbors = CountNeighbors(cell);
            bool alive = IsAlive(cell);

            if (!alive && neighbors == 3)
            {
                nextState.SetTile(cell, aliveTile);
                aliveCells.Add(cell);
            }
            else if (alive && (neighbors < 2 || neighbors > 3))
            {
                nextState.SetTile(cell, deadTile);
                aliveCells.Remove(cell);
            }
            else // no change
            {
                nextState.SetTile(cell, currentState.GetTile(cell));
            }
        }

        // Swap current state with next state
        Tilemap temp = currentState;
        currentState = nextState;
        nextState = temp;
        nextState.ClearAllTiles();
    }

    private int CountNeighbors(Vector3Int cell)
    {
        int count = 0;

        for (int x = -1; x <= 1; x++)
        for (int y = -1; y <= 1; y++)
        {
            if (x == 0 && y == 0) continue;
            if (IsAlive(cell + new Vector3Int(x, y))) count++;
        }
        return count;
    }

    private bool IsAlive(Vector3Int cell)
    {
        return currentState.GetTile(cell) == aliveTile;
    }

    private void EndSimulation()
    {
        StopAllCoroutines();
        // Build progression data and save JSON
        var next = BuildNextProgression();  
        SaveProgression(next);

        // Return to menu scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("CellSelectMenu");
    }

    private ProgressionData BuildNextProgression()
    {
        ProgressionData prog = new ProgressionData(startRows, startCols, startMaxSelectable);

        // Basic scoring — tune this later
        prog.totalScore = 0;   // Why?
        prog.totalIterations = iterations;
        foreach (int p in populationHistory)
            prog.totalScore += p;

        // Difficulty progression logic
        if (prog.totalIterations > 250) prog.rows++;
        if (prog.totalIterations > 250) prog.cols++;
        if (prog.totalScore > 250) prog.maxSelectable++;

        return prog;
    }

    private void SaveProgression(ProgressionData prog)
    {
        string json = JsonUtility.ToJson(prog, prettyPrint: true);
        string path = Path.Combine(Application.streamingAssetsPath, "runtime_progression.json");

        File.WriteAllText(path, json);
        Debug.Log("Saved progression: " + json);
    }
}
