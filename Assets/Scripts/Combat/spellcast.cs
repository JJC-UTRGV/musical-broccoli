using UnityEngine;

public class spellcast : MonoBehaviour
{
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float spriteAngleOffset = -90f; 
    private bool consumed;
    
    void Start()
    {
        Destroy(gameObject, lifetime);
        
    }

    void Update()
    {
        var rb = GetComponent<Rigidbody2D>();
        Vector2 v = rb.linearVelocity;
        float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
            if (other.CompareTag("Player"))
                return;

            Enemy e = other.GetComponent<Enemy>();
            if (e != null)
            {
                Vector2 knockDir = ((Vector2)other.transform.position - (Vector2)transform.position).normalized;
                e.TakeDamage(damage, knockDir);
                Destroy(gameObject);
            }


            if (other.CompareTag("Wall") || other.CompareTag("Obstacle"))
            {
                Destroy(gameObject);
            }
    }
}
