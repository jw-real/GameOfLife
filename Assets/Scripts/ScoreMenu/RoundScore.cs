using System.IO;
using UnityEngine;
using TMPro;

public class RoundScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

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

        if (!File.Exists(filePath))
        {
            Debug.LogWarning($"Progression file not found at: {filePath}");
            scoreText.text = "Score: 0";
            return;
        }

        try
        {
            string json = File.ReadAllText(filePath);
            ProgressionData data = JsonUtility.FromJson<ProgressionData>(json);

            scoreText.text = $"Score: {data.totalScore}";
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load score: {e.Message}");
            scoreText.text = "Score: --";
        }
    }

    [System.Serializable]
    private class ProgressionData
    {
        public int totalScore;
    }
}