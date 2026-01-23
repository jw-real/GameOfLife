using System.IO;
using UnityEngine;
using TMPro;

public class RoundScore : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField]
    private TextMeshProUGUI scoreText;     // Localized "Score" label
    [SerializeField]
    private TextMeshProUGUI scoreNumber;   // Numeric value

    private const string ProgressionFileName = "run_result.json";
    private const string ProfileFileName = "player_profile.json";

    private void OnEnable()
    {
        LoadAndDisplayScore();
        HighScorePersistence.TryAddRun();
        AwardPlayerCoins();
    }

    private void LoadAndDisplayScore()
    {
        string filePath = Path.Combine(
            Application.persistentDataPath,
            ProgressionFileName
        );

        int roundScore = 0;

        if (!File.Exists(filePath))
        {
            Debug.LogWarning($"Progression file not found at: {filePath}");
        }
        else
        {
            try
            {
                string json = File.ReadAllText(filePath);
                RunResultData data = JsonUtility.FromJson<RunResultData>(json);
                roundScore = data.roundScore;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load score: {e.Message}");
            }
        }

        // Update UI
        if (scoreText != null)
        {
            // Only the label — localized text via LocalizeStringEvent
            // Do not set scoreText.text here if using LocalizeStringEvent
        }

        if (scoreNumber != null)
        {
            // The numeric value — updated separately
            scoreNumber.text = roundScore.ToString();
        }
    }

    private void AwardPlayerCoins()
    {
        string runResultPath = Path.Combine(Application.persistentDataPath, ProgressionFileName);
        string profilePath = Path.Combine(Application.persistentDataPath, ProfileFileName);

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
    }
}