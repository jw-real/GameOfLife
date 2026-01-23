using UnityEngine;
using UnityEngine.SceneManagement;

public class LibraryRowController : MonoBehaviour
{
    private string patternCanonical;

    public void Bind(string canonical)
    {
        patternCanonical = canonical;
    }

    public void OnViewPatternClicked()
    {
        PatternSelectionContext.Set(patternCanonical);
        SceneManager.LoadScene("PatternViewer");
    }
}
