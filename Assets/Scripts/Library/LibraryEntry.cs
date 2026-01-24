[System.Serializable]
public class LibraryEntry
{
    public string name;
    public string patternCanonical;

        // Derived, not serialized
    public int cellCount => CountLiveCells(patternCanonical);

    private static int CountLiveCells(string canonical)
    {
        int count = 0;
        for (int i = 0; i < canonical.Length; i++)
        {
            if (canonical[i] == '1')
                count++;
        }
        return count;
    }
}
