using UnityEngine;

public class spellcast : MonoBehaviour
{
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private int damage = 1;
    
    void Start()
    {
        Destroy(gameObject, lifetime);
        
    }

    void OnTriggerEnter2D(Collider2D other)
{
    enemy enemy = other.GetComponent<enemy>();

    if (enemy != null)
    {
        enemy.TakeDamage(damage);
        Destroy(gameObject);
    }
}
}
