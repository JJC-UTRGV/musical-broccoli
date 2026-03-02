using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class HighScoresUI : MonoBehaviour
{
    public TMP_Text scoresText;

    void OnEnable()
    {
        List<int> scores = HighScoreManager.GetHighScores();

        if (scores.Count == 0)
        {
            scoresText.text = "No scores yet!";
            return;
        }

        scoresText.text = "";

        for (int i = 0; i < scores.Count; i++)
        {
            scoresText.text += $"{i + 1}. {scores[i]}\n";
        }
    }
}