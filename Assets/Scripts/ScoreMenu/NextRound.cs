using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class NewGame : MonoBehaviour
{
    private const string RunResultFileName = "run_result.json";
    private const string PlayerProfileFileName = "player_profile.json";

    public void OnNextRoundButtonPressed()
    {
        string runResultPath = Path.Combine(Application.persistentDataPath, RunResultFileName);
        string profilePath = Path.Combine(Application.persistentDataPath, PlayerProfileFileName);

        // Load run result
        if (File.Exists(runResultPath) && File.Exists(profilePath))
        {
            string runJson = File.ReadAllText(runResultPath);
            RunResultData runResult = JsonUtility.FromJson<RunResultData>(runJson);

            string profileJson = File.ReadAllText(profilePath);
            PlayerProfileData profile = JsonUtility.FromJson<PlayerProfileData>(profileJson);

            // Increment coins
            profile.coins += runResult.roundScore;

            // Save updated profile
            string updatedProfileJson = JsonUtility.ToJson(profile, true);
            File.WriteAllText(profilePath, updatedProfileJson);
        }

        SceneManager.LoadScene("Store");
    }
}