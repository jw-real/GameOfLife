using UnityEngine;

[CreateAssetMenu(
    fileName = "GameConfig",
    menuName = "GameConfig/InitialConfig")]
public class GameConfigSO : ScriptableObject
{
    public int rows = 4;
    public int cols = 4;
    public int maxSelectable = 5;

    public int iterationCap = 300;

    // For debugging visibility:
    //public void ApplyProgression(ProgressionData prog)
    //{
    //    rows = prog.rows;
    //    cols = prog.cols;
    //    maxSelectable = prog.maxSelectable;
    //}
}