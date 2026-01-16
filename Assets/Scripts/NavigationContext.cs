public static class NavigationContext
{
    public static string PreviousSceneName { get; private set; }

    public static void SetPreviousScene(string sceneName)
    {
        PreviousSceneName = sceneName;
    }

    public static void Clear()
    {
        PreviousSceneName = null;
    }
}