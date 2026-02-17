using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform player;

    [Header("Spawn Timing")]
    [SerializeField] private float startInterval = 1.5f;   
    [SerializeField] private float minInterval = 0.45f;     
    [SerializeField] private float timeToMaxDifficulty = 90f; 

    [Header("Alive Cap")]
    [SerializeField] private int startMaxAlive = 10;
    [SerializeField] private int endMaxAlive = 25;        
    [Header("Spawn Placement")]
    [SerializeField] private float spawnPadding = 1.5f;     

    [Header("Despawn")]
    [SerializeField] private float despawnExtra = 8f;      
    [SerializeField] private float cleanupInterval = 1.0f;  

    private Camera cam;
    private float spawnTimer;
    private float elapsed;

    private float cleanupTimer;

    
    private readonly List<GameObject> alive = new List<GameObject>();

    void Awake()
    {
        cam = Camera.main;

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        spawnTimer = startInterval;
        cleanupTimer = cleanupInterval;
    }

    void Update()
    {
        if (enemyPrefab == null || player == null || cam == null) return;

        elapsed += Time.deltaTime;

        
        float t = (timeToMaxDifficulty <= 0f) ? 1f : Mathf.Clamp01(elapsed / timeToMaxDifficulty);

        float currentInterval = Mathf.Lerp(startInterval, minInterval, t);
        int currentMaxAlive = Mathf.RoundToInt(Mathf.Lerp(startMaxAlive, endMaxAlive, t));

        
        cleanupTimer -= Time.deltaTime;
        if (cleanupTimer <= 0f)
        {
            cleanupTimer = cleanupInterval;
            CleanupAndDespawn();
        }

        
        if (alive.Count >= currentMaxAlive) return;

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            spawnTimer = currentInterval;
            SpawnOne();
        }
    }

    void SpawnOne()
    {
        Vector2 spawnPos = GetRandomOffscreenPositionAroundPlayer();
        GameObject e = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

        
        alive.Add(e);
    }

    void CleanupAndDespawn()
    {
        
        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        float despawnDist = Mathf.Max(camWidth, camHeight) + spawnPadding + despawnExtra;

        
        for (int i = alive.Count - 1; i >= 0; i--)
        {
            GameObject e = alive[i];

            if (e == null)
            {
                alive.RemoveAt(i);
                continue;
            }

            float d = Vector2.Distance(e.transform.position, player.position);
            if (d > despawnDist)
            {
                Destroy(e);
                alive.RemoveAt(i);
            }
        }
    }

    Vector2 GetRandomOffscreenPositionAroundPlayer()
    {
        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        Vector2 center = player.position;

        int side = Random.Range(0, 4); 
        float x, y;

        switch (side)
        {
            case 0: // top
                x = Random.Range(center.x - camWidth, center.x + camWidth);
                y = center.y + camHeight + spawnPadding;
                break;

            case 1: // right
                x = center.x + camWidth + spawnPadding;
                y = Random.Range(center.y - camHeight, center.y + camHeight);
                break;

            case 2: // bottom
                x = Random.Range(center.x - camWidth, center.x + camWidth);
                y = center.y - camHeight - spawnPadding;
                break;

            default: // left
                x = center.x - camWidth - spawnPadding;
                y = Random.Range(center.y - camHeight, center.y + camHeight);
                break;
        }

        return new Vector2(x, y);
    }
}
