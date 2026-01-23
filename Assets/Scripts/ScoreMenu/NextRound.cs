using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class NewGame : MonoBehaviour
{
    private const string RunResultFileName = "run_result.json";
    private const string PlayerProfileFileName = "player_profile.json";

    public void OnNextRoundButtonPressed()
    {
        SceneManager.LoadScene("Store");
    }
}