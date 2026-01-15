using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class HighScoreManager : MonoBehaviour
{
    private const string HighScoreFileName = "player_high_scores.json";

    [System.Serializable]
    public class HighScoreRowUI
    {
        public TextMeshProUGUI rankText;
        public TextMeshProUGUI scoreText;
        public Button viewPatternButton;
    }

    [Header("UI Rows (Size = 10)")]
    [SerializeField]
    private List<HighScoreRowUI> rows = new List<HighScoreRowUI>();

    private void OnEnable()
    {
        PopulateHighScoreTable();
    }

    private void PopulateHighScoreTable()
    {
        HighScoreTable table = LoadHighScores();

        // Defensive sort (highest score first)
        table.entries.Sort((a, b) => b.score.CompareTo(a.score));

        for (int i = 0; i < rows.Count; i++)
        {
            if (i < table.entries.Count)
            {
                BindRow(rows[i], i + 1, table.entries[i]);
            }
            else
            {
                ClearRow(rows[i], i + 1);
            }
        }
    }

    private void BindRow(HighScoreRowUI row, int rank, HighScoreEntry entry)
    {
        row.rankText.text = rank.ToString();
        row.scoreText.text = entry.score.ToString();

        row.viewPatternButton.interactable = true;
        row.viewPatternButton.onClick.RemoveAllListeners();
        row.viewPatternButton.onClick.AddListener(() =>
        {
            PlayerPrefs.SetString("view_pattern_hash", entry.patternHash);
            SceneManager.LoadScene("PatternViewer");
        });
    }

    private void ClearRow(HighScoreRowUI row, int rank)
    {
        row.rankText.text = rank.ToString();
        row.scoreText.text = "—";
        row.viewPatternButton.interactable = false;
        row.viewPatternButton.onClick.RemoveAllListeners();
    }

    private HighScoreTable LoadHighScores()
    {
        string path = Path.Combine(Application.persistentDataPath, HighScoreFileName);

        if (!File.Exists(path))
        {
            return new HighScoreTable();
        }

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<HighScoreTable>(json);
    }
}
