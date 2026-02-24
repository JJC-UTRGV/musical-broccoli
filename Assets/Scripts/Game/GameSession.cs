using UnityEngine;

public class GameSession : MonoBehaviour
{
    public static GameSession Instance { get; private set; }

    public int Score { get; private set; }
    public int Kills { get; private set; }
    public float ElapsedTime { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Update()
    {
        ElapsedTime += Time.deltaTime;
    }

    public void AddScore(int amount)
    {
        Score += Mathf.Max(0, amount);
    }

    public void RegisterEnemyKill(int scoreValue)
    {
        Kills++;
        AddScore(scoreValue);
    }

    void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
