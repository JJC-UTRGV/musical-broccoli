using UnityEngine;
using TMPro;

public class GameHUD : MonoBehaviour
{
    [Header("Text References")]
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text killsText;
    [SerializeField] private TMP_Text timeText;

    [Header("Player (optional, auto-find if empty)")]
    [SerializeField] private PlayerHealth playerHealth;

    private float playerSearchTimer;

    void Start()
    {
        TryFindPlayer();
        RefreshAll();
    }

    void Update()
    {
        // Re-find player if needed
        if (playerHealth == null)
        {
            playerSearchTimer -= Time.deltaTime;
            if (playerSearchTimer <= 0f)
            {
                playerSearchTimer = 0.5f;
                TryFindPlayer();
            }
        }

        RefreshAll();
    }

    void TryFindPlayer()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            playerHealth = p.GetComponent<PlayerHealth>();
    }

    void RefreshAll()
    {
        // HP
        if (hpText != null)
        {
            if (playerHealth != null)
                hpText.text = $"HP: {playerHealth.CurrentHealth}/{playerHealth.MaxHealth}";
            else
                hpText.text = "HP: --";
        }

        // Session stats
        if (GameSession.Instance != null)
        {
            if (scoreText != null)
                scoreText.text = $"Score: {GameSession.Instance.Score}";

            if (killsText != null)
                killsText.text = $"Kills: {GameSession.Instance.Kills}";

            if (timeText != null)
                timeText.text = $"Time: {FormatTime(GameSession.Instance.ElapsedTime)}";
        }
        else
        {
            if (scoreText != null) scoreText.text = "Score: 0";
            if (killsText != null) killsText.text = "Kills: 0";
            if (timeText != null) timeText.text = "Time: 00:00";
        }
    }

    string FormatTime(float seconds)
    {
        int total = Mathf.FloorToInt(seconds);
        int mins = total / 60;
        int secs = total % 60;
        return $"{mins:00}:{secs:00}";
    }
}
