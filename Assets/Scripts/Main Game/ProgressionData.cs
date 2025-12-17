[System.Serializable]
public class ProgressionData
{
    public int rows;
    public int cols;
    public int maxSelectable;

    public int roundScore;          // sum of all alive-cell counts over time
    public int totalIterations;     // how many iterations the run lasted

    public ProgressionData(int rows, int cols, int maxSelectable)
    {
        this.rows = rows;
        this.cols = cols;
        this.maxSelectable = maxSelectable;
    }
}

