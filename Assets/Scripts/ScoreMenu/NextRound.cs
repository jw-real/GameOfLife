using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGame : MonoBehaviour
{
    private const string ProgressionFileName = "run_result.json";

    public void OnNextRoundButtonPressed()
    {
        SceneManager.LoadScene("CellSelectMenu");
    }
}
