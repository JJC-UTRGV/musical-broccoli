using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyContactDamage : MonoBehaviour
{
    [SerializeField] private int contactDamage = 1;
    [SerializeField] private int wallDamage = 1;

    void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerHealth ph = collision.collider.GetComponent<PlayerHealth>();
        if (ph != null)
        {
            ph.TakeDamage(contactDamage);
        }

        // Try to damage tilemap walls at contact point
        Collider2D hitCollider = collision.collider;
        TilemapCollider2D tilemapCollider = hitCollider.GetComponentInParent<TilemapCollider2D>();
        if (tilemapCollider != null)
        {
            Tilemap tilemap = tilemapCollider.GetComponent<Tilemap>();
            TilemapWallManager wallManager = tilemapCollider.GetComponentInParent<TilemapWallManager>();

            if (tilemap != null && wallManager != null)
            {
                // Use the collision contact point to determine which tile was hit
                Vector3 contactPoint = collision.GetContact(0).point;
                Vector3Int tilePos = tilemap.WorldToCell(contactPoint);
                wallManager.DamageWallAt(tilePos, wallDamage);
            }
        }
    }
}
