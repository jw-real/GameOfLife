using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGame : MonoBehaviour
{
    private const string ProgressionFileName = "runtime_progression.json";

    public void OnNextRoundButtonPressed()
    {
        SceneManager.LoadScene("CellSelectMenu");
    }
}
