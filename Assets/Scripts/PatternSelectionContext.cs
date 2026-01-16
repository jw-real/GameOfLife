public static class PatternSelectionContext
{
    public static string SelectedPatternCanonical { get; private set; }

    public static void Set(string patternCanonical)
    {
        SelectedPatternCanonical = patternCanonical;
    }

    public static bool HasPattern =>
        !string.IsNullOrEmpty(SelectedPatternCanonical);

    public static void Clear()
    {
        SelectedPatternCanonical = null;
    }
}