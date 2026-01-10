[System.Serializable]
public class PlayerProfileData
{
    public int rows;
    public int cols;
    public int maxSelectable;
    public int coins;

    public PlayerProfileData()
    {
        rows = 5;
        cols = 5;
        maxSelectable = 5;
        coins = 0;
    }
}