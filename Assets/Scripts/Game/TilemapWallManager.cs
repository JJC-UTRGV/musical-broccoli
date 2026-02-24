using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TilemapWallManager : MonoBehaviour
{
    [SerializeField] private Tilemap wallTilemap;
    [SerializeField] private int wallMaxHealth = 3;
    [SerializeField] private GameObject destructionEffectPrefab;
    [SerializeField] private AudioClip wallBreakSound;
    
    private Dictionary<Vector3Int, int> wallHealth = new Dictionary<Vector3Int, int>();
    private AudioSource audioSource;

    void Start()
    {
        if (wallTilemap == null)
            wallTilemap = GetComponent<Tilemap>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        InitializeWalls();
    }

    void InitializeWalls()
    {
        BoundsInt bounds = wallTilemap.cellBounds;

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            TileBase tile = wallTilemap.GetTile(pos);
            
            // Only track wall tiles (tile index 3, which is the dark gray wall)
            // Adjust the check based on your tileset
            if (tile != null && IsWallTile(tile))
            {
                wallHealth[pos] = wallMaxHealth;
            }
        }
    }

    bool IsWallTile(TileBase tile)
    {
        // Check if tile name contains "wall" or matches your wall tile
        // Adjust this based on your tile naming convention
        return tile.name.Contains("tileset_3") || tile.name.Contains("Wall");
    }

    public void DamageWallAt(Vector3Int tilePos, int damage)
    {
        if (!wallHealth.ContainsKey(tilePos)) return;

        wallHealth[tilePos] -= damage;

        if (wallHealth[tilePos] <= 0)
        {
            DestroyWallAt(tilePos);
        }
    }

    void DestroyWallAt(Vector3Int tilePos)
    {
        wallTilemap.SetTile(tilePos, null);
        wallHealth.Remove(tilePos);

        // Optional: Add visual/audio effects here
        // Instantiate a destruction effect, play sound, etc.
    }

    public int GetWallHealthAt(Vector3Int tilePos)
    {
        if (wallHealth.ContainsKey(tilePos))
            return wallHealth[tilePos];
        return 0;
    }
}
