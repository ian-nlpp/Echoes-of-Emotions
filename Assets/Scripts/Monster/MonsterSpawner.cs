using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab; // Reference to Monster.prefab
    public MapBoundaries mapBoundaries; // Reference to map boundaries
    public Vector2[] manualSpawnPoints; // Manual spawn positions
    public bool useRandomSpawning = false; // Toggle random spawning
    public int randomSpawnCount = 3; // Number of monsters to spawn randomly

    void Start()
    {
        if (monsterPrefab == null)
        {
            Debug.LogError("Monster Prefab not assigned in MonsterSpawner!");
            return;
        }
        if (mapBoundaries == null)
        {
            Debug.LogError("Map Boundaries not assigned in MonsterSpawner!");
            return;
        }

        // Ensure map boundaries are initialized
        if (!mapBoundaries.IsInitialized())
        {
            Debug.LogWarning("MapBoundaries not initialized. Forcing initialization.");
            mapBoundaries.Initialize();
        }

        if (useRandomSpawning)
        {
            SpawnMonstersRandomly();
        }
        else
        {
            SpawnMonstersManually();
        }
    }

    void SpawnMonstersManually()
    {
        if (manualSpawnPoints.Length == 0)
        {
            Debug.LogWarning("No manual spawn points defined in MonsterSpawner!");
            return;
        }

        foreach (Vector2 spawnPoint in manualSpawnPoints)
        {
            Vector3 clampedPosition = mapBoundaries.ClampPosition(new Vector3(spawnPoint.x, spawnPoint.y, 0));
            GameObject monster = Instantiate(monsterPrefab, clampedPosition, Quaternion.identity);
            MonsterBehavior behavior = monster.GetComponent<MonsterBehavior>();
            if (behavior != null)
            {
                behavior.mapBoundaries = mapBoundaries; // Assign MapBoundaries
            }
            Debug.Log($"Spawned monster at {clampedPosition}");
        }
    }

    void SpawnMonstersRandomly()
    {
        for (int i = 0; i < randomSpawnCount; i++)
        {
            Vector2 randomPos = new Vector2(
                Random.Range(mapBoundaries.GetMinBounds().x, mapBoundaries.GetMaxBounds().x),
                Random.Range(mapBoundaries.GetMinBounds().y, mapBoundaries.GetMaxBounds().y)
            );
            Vector3 clampedPosition = mapBoundaries.ClampPosition(new Vector3(randomPos.x, randomPos.y, 0));
            GameObject monster = Instantiate(monsterPrefab, clampedPosition, Quaternion.identity);
            MonsterBehavior behavior = monster.GetComponent<MonsterBehavior>();
            if (behavior != null)
            {
                behavior.mapBoundaries = mapBoundaries; // Assign MapBoundaries
            }
            Debug.Log($"Spawned monster randomly at {clampedPosition}");
        }
    }
}