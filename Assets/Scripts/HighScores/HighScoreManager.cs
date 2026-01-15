using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class HighScoreManager
{
    private const string FileName = "player_high_scores.json";
    private const int MaxEntries = 10;

    public static void AddScore(int score, string patternHash)
    {
        HighScoreTable table = Load();

        table.entries.Add(new HighScoreEntry
        {
            score = score,
            patternHash = patternHash
        });

        table.entries = table.entries
            .OrderByDescending(e => e.score)
            .Take(MaxEntries)
            .ToList();

        Save(table);
    }

    public static HighScoreTable Load()
    {
        string path = Path.Combine(Application.persistentDataPath, FileName);

        if (!File.Exists(path))
            return new HighScoreTable();

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<HighScoreTable>(json);
    }

    private static void Save(HighScoreTable table)
    {
        string path = Path.Combine(Application.persistentDataPath, FileName);
        string json = JsonUtility.ToJson(table, true);
        File.WriteAllText(path, json);
    }
}