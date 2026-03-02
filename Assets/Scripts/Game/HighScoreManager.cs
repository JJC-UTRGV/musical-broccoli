using System.Collections.Generic;
using UnityEngine;

public static class HighScoreManager
{
    private const string HighScoresKey = "HighScores";
    private const int MaxScores = 5;

    public static List<int> GetHighScores()
    {
        string data = PlayerPrefs.GetString(HighScoresKey, "");
        List<int> scores = new List<int>();

        if (!string.IsNullOrEmpty(data))
        {
            string[] split = data.Split(',');
            foreach (string s in split)
            {
                if (int.TryParse(s, out int val))
                    scores.Add(val);
            }
        }

        return scores;
    }

    public static void AddScore(int score)
    {
        List<int> scores = GetHighScores();
        scores.Add(score);
        scores.Sort((a, b) => b.CompareTo(a)); // descending

        if (scores.Count > MaxScores)
            scores.RemoveRange(MaxScores, scores.Count - MaxScores);

        string saveData = string.Join(",", scores);
        PlayerPrefs.SetString(HighScoresKey, saveData);
        PlayerPrefs.Save();
    }

    public static int GetBestScore()
    {
        List<int> scores = GetHighScores();
        return scores.Count > 0 ? scores[0] : 0;
    }
}