using UnityEngine;

public class EnemyContactDamage : MonoBehaviour
{
    [SerializeField] private int contactDamage = 1;

    void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerHealth ph = collision.collider.GetComponent<PlayerHealth>();
        if (ph != null)
            ph.TakeDamage(contactDamage);
    }
}

