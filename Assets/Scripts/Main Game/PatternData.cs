using System;
using System.Collections.Generic;

[System.Serializable]
public class PatternData
{
    public int columns;
    public int rows;
    public List<CellPosition> selectedCells;
}

[System.Serializable]
public class CellPosition
{
    public int x;
    public int y;

    public CellPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}
