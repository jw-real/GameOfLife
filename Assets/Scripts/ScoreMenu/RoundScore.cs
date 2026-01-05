using System.IO;
using UnityEngine;
using TMPro;

public class RoundScript : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField]
    private TextMeshProUGUI scoreText;     // Localized "Score" label
    [SerializeField]
    private TextMeshProUGUI scoreNumber;   // Numeric value

    private const string ProgressionFileName = "runtime_progression.json";

    private void OnEnable()
    {
        LoadAndDisplayScore();
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
                ProgressionData data = JsonUtility.FromJson<ProgressionData>(json);
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

    [System.Serializable]
    private class ProgressionData
    {
        public int roundScore;
    }
}