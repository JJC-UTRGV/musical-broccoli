using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverController : MonoBehaviour
{
    public GameObject gameOverPanel;

    public TMP_Text scoreText;
    public TMP_Text bestScoreText;

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;

        int finalScore = GameSession.Instance.Score;

        HighScoreManager.AddScore(finalScore);

        scoreText.text = "Score: " + finalScore;
        bestScoreText.text = "High Score: " + HighScoreManager.GetBestScore();
    }

    public void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}