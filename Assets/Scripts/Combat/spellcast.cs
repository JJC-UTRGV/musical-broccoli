using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class spellcast : MonoBehaviour
{
    [SerializeField] private float lifetime = 10f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float spriteAngleOffset = -90f;
    [SerializeField] private int wallDamage = 1;
    [SerializeField] private float wallDamageRadius = 1.5f;
    
    private bool consumed;
    private HashSet<Vector3Int> hitWallTiles = new HashSet<Vector3Int>();
    
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

            // Damage tilemap walls - destroy on impact
            TilemapCollider2D tilemapCollider = other.GetComponentInParent<TilemapCollider2D>();
            if (tilemapCollider != null)
            {
                Tilemap tilemap = tilemapCollider.GetComponent<Tilemap>();
                TilemapWallManager wallManager = tilemapCollider.GetComponentInParent<TilemapWallManager>();

                if (wallManager != null && tilemap != null)
                {
                    // Use closest contact point to reliably choose the tile touched
                    Vector3 worldPoint = other.ClosestPoint(transform.position);
                    Vector3Int centerTilePos = tilemap.WorldToCell(worldPoint);

                    // Damage all tiles within radius of the impact
                    int radiusTiles = Mathf.CeilToInt(wallDamageRadius);
                    for (int x = -radiusTiles; x <= radiusTiles; x++)
                    {
                        for (int y = -radiusTiles; y <= radiusTiles; y++)
                        {
                            Vector3Int tilePos = centerTilePos + new Vector3Int(x, y, 0);
                            float distToTile = Vector2.Distance(tilemap.GetCellCenterWorld(tilePos), worldPoint);

                            if (distToTile <= wallDamageRadius && !hitWallTiles.Contains(tilePos))
                            {
                                wallManager.DestroyWallAtInstant(tilePos);
                                hitWallTiles.Add(tilePos);
                            }
                        }
                    }
                }

                // Destroy the spell on wall impact
                Destroy(gameObject);
                return;
            }

            if (other.CompareTag("Wall") || other.CompareTag("Obstacle"))
            {
                Destroy(gameObject);
            }
    }
}

