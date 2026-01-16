using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class HighScorePersistence
{
    private const string HighScoreFileName = "player_high_scores.json";

    public static void TryAddRun()
    {
        RunResultData run = LoadRunResult();
        if (run == null) return;

        HighScoreTable table = LoadHighScores();

        table.entries.Add(new HighScoreEntry
        {
            score = run.roundScore,
            patternCanonical = run.patternCanonical
        });

        // Keep top 10
        table.entries.Sort((a, b) => b.score.CompareTo(a.score));
        if (table.entries.Count > 10)
            table.entries.RemoveRange(10, table.entries.Count - 10);

        SaveHighScores(table);
    }

    public static HighScoreTable LoadHighScores()
    {
        string path = Path.Combine(Application.persistentDataPath, HighScoreFileName);

        if (!File.Exists(path))
            return new HighScoreTable();

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<HighScoreTable>(json);
    }

    private static void SaveHighScores(HighScoreTable table)
    {
        string path = Path.Combine(Application.persistentDataPath, HighScoreFileName);
        string json = JsonUtility.ToJson(table, true);
        File.WriteAllText(path, json);
    }

    private static RunResultData LoadRunResult()
    {
        if (!PlayerPrefs.HasKey("run_result"))
            return null;

        string json = PlayerPrefs.GetString("run_result");
        return JsonUtility.FromJson<RunResultData>(json);
    }
}