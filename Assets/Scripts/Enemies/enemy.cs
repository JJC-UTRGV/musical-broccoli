using UnityEngine;
using UnityEngine.Tilemaps;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float chaseRange = 5f;

    [Header("Health")]
    [SerializeField] private int maxHealth = 5;
    private int currentHealth;

    [Header("Directional Sprites")]
    [SerializeField] private Sprite idleUp;
    [SerializeField] private Sprite idleRight;
    [SerializeField] private Sprite idleDown;
    [SerializeField] private Sprite idleLeft;

    [Header("Knockback")]
    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private float knockbackTime = 0.10f;

    [Header("Hit Flash")]
    [SerializeField] private Color hitFlashColor = new Color(1f, 0.35f, 0.35f, 1f);
    [SerializeField] private float flashTime = 0.08f;

    private Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private float knockTimer;
    [Header("Wall Interaction")]
    [SerializeField] private int wallDamageOnPath = 1;
    [SerializeField] private float wallDamageInterval = 0.5f;
    private float wallDamageTimer = 0f;
    private TilemapWallManager wallManager;

    private Color originalColor;
    private float flashTimer;

    void Awake()
    {
        currentHealth = maxHealth;

        rb = GetComponent<Rigidbody2D>();

        sr = transform.Find("Visual/Sprite")?.GetComponent<SpriteRenderer>();
        if (sr == null) sr = GetComponentInChildren<SpriteRenderer>();

        if (sr != null) originalColor = sr.color;

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        wallManager = FindObjectOfType<TilemapWallManager>();
    }

    void FixedUpdate()
    {
        
        if (sr != null && flashTimer > 0f)
        {
            flashTimer -= Time.fixedDeltaTime;
            if (flashTimer <= 0f)
                sr.color = originalColor;
        }

        
        if (knockTimer > 0f)
        {
            knockTimer -= Time.fixedDeltaTime;
            if (knockTimer <= 0f)
                rb.linearVelocity = Vector2.zero;
            return;
        }

        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(rb.position, player.position);

        if (distanceToPlayer <= chaseRange)
        {
            Vector2 direction = ((Vector2)player.position - rb.position).normalized;

            // Check for walls between enemy and player
            if (wallManager != null)
            {
                RaycastHit2D[] hits = Physics2D.RaycastAll(rb.position, direction, distanceToPlayer);
                foreach (var hit in hits)
                {
                    if (hit.collider == null) continue;
                    var tilemapCollider = hit.collider.GetComponentInParent<TilemapCollider2D>();
                    if (tilemapCollider != null)
                    {
                        Tilemap tilemap = tilemapCollider.GetComponent<Tilemap>();
                        if (tilemap != null)
                        {
                            // damage on interval
                            wallDamageTimer -= Time.fixedDeltaTime;
                            if (wallDamageTimer <= 0f)
                            {
                                Vector3Int tilePos = tilemap.WorldToCell(hit.point);
                                wallManager.DamageWallAt(tilePos, wallDamageOnPath);
                                wallDamageTimer = wallDamageInterval;
                            }

                            // stop at first tilemap hit
                            break;
                        }
                    }
                }
            }

            rb.linearVelocity = direction * moveSpeed;
            UpdateFacing(direction);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void UpdateFacing(Vector2 d)
    {
        if (sr == null) return;

        if (Mathf.Abs(d.x) > Mathf.Abs(d.y))
            sr.sprite = (d.x >= 0f) ? idleRight : idleLeft;
        else
            sr.sprite = (d.y >= 0f) ? idleUp : idleDown;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        
        if (sr != null)
        {
            sr.color = hitFlashColor;
            flashTimer = flashTime;
        }

        if (currentHealth <= 0) Die();
    }

    public void TakeDamage(int damage, Vector2 knockDir)
    {
        TakeDamage(damage);

        if (currentHealth <= 0) return;

        knockTimer = knockbackTime;
        rb.linearVelocity = knockDir.normalized * knockbackForce;
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
