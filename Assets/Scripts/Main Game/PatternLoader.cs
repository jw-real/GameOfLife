using UnityEngine;

public class PatternLoader : MonoBehaviour
{
    public GameBoard gameBoard;  // assign in Inspector

    void Start()
    {
        string json = PlayerPrefs.GetString("runtime_pattern", null);

        if (string.IsNullOrEmpty(json))
        {
            Debug.LogError("No runtime pattern found!");
            return;
        }

        PatternData pattern = JsonUtility.FromJson<PatternData>(json);

        gameBoard.ApplyPattern(pattern);
    }
}

