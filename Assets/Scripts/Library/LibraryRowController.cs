using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LibraryRowController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI cellCountText;
    [SerializeField] private Button viewButton;
    private string patternCanonical;

    public void Bind(LibraryEntry entry)
    {
        nameText.text = entry.name;
        cellCountText.text = entry.cellCount.ToString();
        patternCanonical = entry.patternCanonical;

        viewButton.onClick.RemoveAllListeners();
        viewButton.onClick.AddListener(OnViewPatternClicked);
    }

    public void OnViewPatternClicked()
    {
        PatternSelectionContext.Set(patternCanonical);
        NavigationContext.SetPreviousScene("Library");
        SceneManager.LoadScene("PatternViewer");
    }
}
