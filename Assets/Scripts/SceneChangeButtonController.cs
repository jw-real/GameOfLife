using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeButtonController : MonoBehaviour
{
    public void GoToCellSelect()
    {
        SceneManager.LoadScene("CellSelectMenu");
    }

    public void GoToRule1()
    {
        SceneManager.LoadScene("Rule1");
    }

    public void GoToRule2()
    {
        SceneManager.LoadScene("Rule2");
    }

    public void GoToRule3()
    {
        SceneManager.LoadScene("Rule3");
    }

    public void GoToRule4()
    {
        SceneManager.LoadScene("Rule4");
    }

    public void GoToStore()
    {
        SceneManager.LoadScene("Store");
    }

    public void GoToHighScore()
    {
        SceneManager.LoadScene("HighScores");
    }
    public void OnViewPatternHighScore(string patternCanonical)
    {
        PatternSelectionContext.Set(patternCanonical);
        NavigationContext.SetPreviousScene("HighScores");
        SceneManager.LoadScene("PatternViewer");
    }

    public void OnViewPatternLibrary(string patternCanonical)
    {
        PatternSelectionContext.Set(patternCanonical);
        NavigationContext.SetPreviousScene("Library");
        SceneManager.LoadScene("PatternViewer");
    }

    public void BackFromPatternViewer()
    {
        string previousScene = NavigationContext.GetPreviousScene();

        if (string.IsNullOrEmpty(previousScene))
        {
            Debug.LogWarning("Previous scene not set. Falling back to MainMenu.");
            previousScene = "MainMenu"; // or another safe default
        }

        SceneManager.LoadScene(previousScene);
    }

    public void GoToLibrary()
    {
        NavigationContext.SetPreviousScene("CellSelectMenu");
        SceneManager.LoadScene("Library");
    }

    public void BackFromLibrary()
    {
        string previousScene = NavigationContext.GetPreviousScene();

        if (string.IsNullOrEmpty(previousScene))
        {
            Debug.LogWarning("Previous scene not set. Falling back to MainMenu.");
            previousScene = "MainMenu"; // or another safe default
        }

        SceneManager.LoadScene(previousScene);
    }
}
