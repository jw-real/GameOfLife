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
    [SerializeField] private int iterationCap = 300;

    [Header("Starting Constraints (set by PatternLoader)")]
    public int startRows;
    public int startCols;
    public int startMaxSelectable;

    private readonly HashSet<Vector3Int> aliveCells = new();
    private readonly HashSet<Vector3Int> cellsToCheck = new();

    public int population { get; private set; }
    public int iterations { get; private set; }
    public float time { get; private set; }
    public CameraController cameraController;

    private List<int> populationHistory = new();

    private void Start() { }

    // Called by PatternLoader
    public void ApplyPattern(PatternData pattern)
    {
        Clear();

        // 1️⃣ Compute center of the selected cells
        Vector2Int center = ComputeCenter(pattern.selectedCells);

        // 2️⃣ Place tiles and populate aliveCells HashSet
        foreach (var cell in pattern.selectedCells)
        {
            // Convert from CellPosition to Vector3Int and center
            Vector3Int pos = new Vector3Int(cell.x - center.x, cell.y - center.y, 0);

            currentState.SetTile(pos, aliveTile);
            aliveCells.Add(pos);
        }

        population = aliveCells.Count;
        populationHistory.Add(population);

        // 3️⃣ Update camera if we have a CameraController
        if (aliveCells.Count > 0 && cameraController != null)
        {
            // Convert HashSet<Vector3Int> → List<Vector3Int> for TryComputeBounds
            var aliveList = new List<Vector3Int>(aliveCells);

            if (TryComputeBounds(aliveList, out var bounds))
            {
                cameraController.ApplyBounds(bounds);
            }
        }
    }
    private bool TryComputeBounds(List<Vector3Int> cells, out BoundsInt bounds)
    {
        bounds = default;
        if (cells == null || cells.Count == 0) return false;

        int minX = int.MaxValue, maxX = int.MinValue;
        int minY = int.MaxValue, maxY = int.MinValue;

        foreach (var c in cells)
        {
            if (c.x < minX) minX = c.x;
            if (c.x > maxX) maxX = c.x;
            if (c.y < minY) minY = c.y;
            if (c.y > maxY) maxY = c.y;
        }

        bounds = new BoundsInt(
            minX,
            minY,
            0,
            maxX - minX + 1,
            maxY - minY + 1,
            1
        );
        return true;
    }
// Compute the center of a pattern given List<CellPosition>
    private Vector2Int ComputeCenter(List<CellPosition> cells)
    {
        if (cells == null || cells.Count == 0)
            return Vector2Int.zero;

        // Convert List<CellPosition> → List<Vector3Int> for TryComputeBounds
        var vectorCells = new List<Vector3Int>();
        foreach (var c in cells)
            vectorCells.Add(new Vector3Int(c.x, c.y, 0));

        if (!TryComputeBounds(vectorCells, out var bounds))
            return Vector2Int.zero;

        Vector3 center = bounds.center;
        return new Vector2Int(
            Mathf.RoundToInt(center.x),
            Mathf.RoundToInt(center.y)
        );
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

    // Collect all cells to check (alive + neighbors)
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

        if (!alive && neighbors == 3) // dead → alive
        {
            SetCellNextState(cell, true);
            aliveCells.Add(cell);
        }
        else if (alive && (neighbors < 2 || neighbors > 3)) // alive → dead
        {
            SetCellNextState(cell, false);
            aliveCells.Remove(cell);
        }
        else // no change
        {
            SetCellNextState(cell, alive); // keeps current alive/dead state
        }
    }

    // Swap current and next state Tilemaps
    Tilemap temp = currentState;
    currentState = nextState;
    nextState = temp;
    nextState.ClearAllTiles();

    // Optional: update background color once per iteration
    //if (Camera.main != null)
        //Camera.main.backgroundColor = ThemeManager.Instance.CurrentTheme.DeadColor;
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
        //var next = BuildNextProgression();  
        //SaveProgression(next);

        // Return to menu scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("ScoreScene");
    }

    /*private RunResultData BuildResultData()
    // Instead, create RunResultData inside the CellSelectMenu scene, perhaps in GameConfig.
    // Call the PatternNormalizer script to generate the hash value and save it while initializing totalIterations and roundScore to 0
    // Then use this function to mutate these two values with the outcome of the simulation run
    {
        totalIterations = iterations;
        foreach (int p in populationHistory)
            roundScore += p;
    }
    */

 /*   private ProgressionData BuildNextProgression()
    {
        ProgressionData prog = LoadProgressionOrDefaults();

        // Update all this logic. Player should start with 0 coins, 5 rows, 5 columns, 5 selectable cells. Rather than incrementing automatically
        // based on iterations and round score, players should earn 1 coin per point and be prompted to purchase additional rows, columns, selectable cells
        // at the beginning of each iteration. Cost per each should increase as more are purchased. Also start with 300 iterations and allow additional iterations
        //  to be purchased. The time per round should decrease as iterations increase to ensure each round ends in a timely manner. 
        // Basic scoring — tune this later
        prog.roundScore = 0;   // Why?
        prog.totalIterations = iterations;
        foreach (int p in populationHistory)
            prog.roundScore += p;

        // Difficulty progression logic
        if (prog.totalIterations > 250) prog.rows++;
        if (prog.totalIterations > 250) prog.cols++;
        if (prog.roundScore > 250) prog.maxSelectable++;

        return prog;
    }
*/
/*
    private void SaveProgression(ProgressionData prog)
    {
        string json = JsonUtility.ToJson(prog, prettyPrint: true);
        string path = Path.Combine(Application.persistentDataPath, "runtime_progression.json");

        File.WriteAllText(path, json);
        //Debug.Log("Saved progression: " + json);
    }
*/
 /*   private ProgressionData LoadProgressionOrDefaults()
    {
        string path = Path.Combine(Application.persistentDataPath, "runtime_progression.json");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            ProgressionData loaded = JsonUtility.FromJson<ProgressionData>(json);
            //Debug.Log("Loaded progression: " + json);
            return loaded;
        }

        // No file yet → seed from starting values
        return new ProgressionData(startRows, startCols, startMaxSelectable);
    }
*/
    void SetCell(Vector3Int cellPos, bool isAlive)
    {
        Tile tileToSet = isAlive ? aliveTile : deadTile;
        //Color colorToSet = isAlive ? AliveColor 
        //                        : DeadColor;

        // Apply to both Tilemaps
        currentState.SetTile(cellPos, tileToSet);
        //currentState.SetColor(cellPos, colorToSet);

        nextState.SetTile(cellPos, tileToSet);
        //nextState.SetColor(cellPos, colorToSet);

        // Optional: set background color once per scene
 //       if (Camera.main != null)
 //           Camera.main.backgroundColor = ThemeManager.Instance.CurrentTheme.DeadColor;
    }

    /// <summary>
    /// Writes the alive/dead state and theme colors to nextState only.
    /// Does NOT touch currentState.
    /// </summary>
    private void SetCellNextState(Vector3Int cellPos, bool isAlive)
    {
        Tile tileToSet = isAlive ? aliveTile : deadTile;
        //Color colorToSet = isAlive ? AliveColor 
        //                       : DeadColor;

        nextState.SetTile(cellPos, tileToSet);
        //nextState.SetColor(cellPos, colorToSet);
    }

    void Awake()
    {
        //Debug.Log($"[GameBoard Awake] rows={startRows}, cols={startCols}, maxSelectable={startMaxSelectable}");
    }
    void LateUpdate()
    {
        //Debug.Log("GameBoard LateUpdate running");
        if (aliveCells.Count > 0 && cameraController != null)
        {
            //Debug.Log($"aliveCells={aliveCells.Count}, cameraController={(cameraController == null ? "NULL" : "OK")}");
            var aliveList = new List<Vector3Int>(aliveCells);
            if (TryComputeBounds(aliveList, out var bounds))
            {
                cameraController.ApplyBounds(bounds);
            }
        }
    }
}