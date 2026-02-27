using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;

    [Header("Invulnerability (I-frames)")]
    [SerializeField] private float invulnTime = 0.75f;
    [SerializeField] private float blinkInterval = 0.08f; 
    private int currentHealth;

    private float invulnTimer;
    private float blinkTimer;
    private bool blinkState = true;
    private bool isDead = false;

    public GameOverController gameOverController;

    private SpriteRenderer sr;

    void Awake()
    {
        currentHealth = maxHealth;

    
        sr = transform.Find("Visual/Sprite")?.GetComponent<SpriteRenderer>();
        if (sr == null) sr = GetComponentInChildren<SpriteRenderer>();

        if (sr != null) sr.enabled = true;
    }

    void Update()
    {
        if (invulnTimer > 0f)
        {
            invulnTimer -= Time.deltaTime;

           
            blinkTimer -= Time.deltaTime;
            if (blinkTimer <= 0f)
            {
                blinkTimer = blinkInterval;
                blinkState = !blinkState;

                if (sr != null)
                    sr.enabled = blinkState;
            }

            
            if (invulnTimer <= 0f && sr != null)
            {
                sr.enabled = true;
                blinkState = true;
            }
        }
    }

    public void TakeDamage(int amount)
    {
        if (invulnTimer > 0f) return;

        currentHealth -= amount;
        Debug.Log("Player HP: " + currentHealth);

        
        invulnTimer = invulnTime;
        blinkTimer = 0f; 

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true; 

        Debug.Log("Player died!");
        if (gameOverController != null)
            gameOverController.ShowGameOver();

        gameObject.SetActive(false);
    }
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
}
