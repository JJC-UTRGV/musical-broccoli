using UnityEngine;
using UnityEngine.Tilemaps;

public class WallDestructor : MonoBehaviour
{
    [SerializeField] private int wallDamagePerSecond = 2;
    [SerializeField] private float damageInterval = 0.5f;

    private Tilemap wallTilemap;
    private TilemapWallManager wallManager;
    private float damageTimer = 0f;
    private Vector3Int lastWallPosition = Vector3Int.zero;

    void Start()
    {
        // Find the wall tilemap (should be tagged or named appropriately)
        wallTilemap = FindObjectOfType<Tilemap>();
        wallManager = FindObjectOfType<TilemapWallManager>();
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (wallTilemap == null || wallManager == null) return;

        // Check if colliding with tilemap collider
        TilemapCollider2D tilemapCollider = collision.gameObject.GetComponent<TilemapCollider2D>();
        if (tilemapCollider == null) return;

        // Get the wall tile position based on contact point
        Vector3 contactPoint = collision.GetContact(0).point;
        Vector3Int tilePos = wallTilemap.WorldToCell(contactPoint);

        if (tilePos != lastWallPosition)
        {
            damageTimer = 0f;
            lastWallPosition = tilePos;
        }

        damageTimer += Time.fixedDeltaTime;

        if (damageTimer >= damageInterval)
        {
            wallManager.DamageWallAt(tilePos, wallDamagePerSecond);
            damageTimer = 0f;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        damageTimer = 0f;
        lastWallPosition = Vector3Int.zero;
    }
}
