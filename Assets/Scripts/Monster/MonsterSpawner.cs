using UnityEngine;
using System.Collections.Generic;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab; // Reference to Monster.prefab
    public MapBoundaries mapBoundaries; // Reference to map boundaries
    [System.Serializable]
    public struct SpawnPoint
    {
        public Vector2 position; // Spawn position
        public MonsterBehavior.MovementType movementType; // Movement type
    }
    public SpawnPoint[] manualSpawnPoints; // Manual spawn points with movement type
    public bool useRandomSpawning = false; // Toggle random spawning
    public int randomSpawnCount = 3; // Number of monsters to spawn randomly
    public float randomVerticalChance = 0.3f; // Chance for Vertical movement
    public float randomCombinedChance = 0.3f; // Chance for Combined movement
    public float minSpawnDistance = 1f; // Minimum distance between spawns

    private List<Vector2> spawnedPositions = new List<Vector2>(); // Track spawn positions

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

        if (!mapBoundaries.IsInitialized())
        {
            Debug.LogWarning("MapBoundaries not initialized. Forcing initialization.");
            mapBoundaries.Initialize();
        }

        spawnedPositions.Clear();
        if (useRandomSpawning)
        {
            SpawnMonstersRandomly();
        }
        else
        {
            SpawnMonstersManually();
        }
    }

    bool IsPositionValid(Vector2 position)
    {
        foreach (Vector2 spawnedPos in spawnedPositions)
        {
            if (Vector2.Distance(position, spawnedPos) < minSpawnDistance)
                return false;
        }
        return true;
    }

    void SpawnMonstersManually()
    {
        if (manualSpawnPoints.Length == 0)
        {
            Debug.LogWarning("No manual spawn points defined in MonsterSpawner!");
            return;
        }

        foreach (SpawnPoint spawnPoint in manualSpawnPoints)
        {
            Vector2 pos = spawnPoint.position;
            if (!IsPositionValid(pos))
            {
                Debug.LogWarning($"Spawn point {pos} too close to another monster. Skipping.");
                continue;
            }

            Vector3 clampedPosition = mapBoundaries.ClampPosition(new Vector3(pos.x, pos.y, 0));
            GameObject monster = Instantiate(monsterPrefab, clampedPosition, Quaternion.identity);
            MonsterBehavior behavior = monster.GetComponent<MonsterBehavior>();
            if (behavior != null)
            {
                behavior.mapBoundaries = mapBoundaries;
                behavior.movementType = spawnPoint.movementType;
            }
            spawnedPositions.Add(pos);
            Debug.Log($"Spawned monster at {clampedPosition}, moving {spawnPoint.movementType}");
        }
    }

    void SpawnMonstersRandomly()
    {
        int attempts = 0;
        int maxAttempts = 10 * randomSpawnCount; // Prevent infinite loops

        for (int i = 0; i < randomSpawnCount && attempts < maxAttempts; i++)
        {
            Vector2 randomPos = new Vector2(
                Random.Range(mapBoundaries.GetMinBounds().x, mapBoundaries.GetMaxBounds().x),
                Random.Range(mapBoundaries.GetMinBounds().y, mapBoundaries.GetMaxBounds().y)
            );

            if (!IsPositionValid(randomPos))
            {
                i--; // Retry this spawn
                attempts++;
                continue;
            }

            Vector3 clampedPosition = mapBoundaries.ClampPosition(new Vector3(randomPos.x, randomPos.y, 0));
            GameObject monster = Instantiate(monsterPrefab, clampedPosition, Quaternion.identity);
            MonsterBehavior behavior = monster.GetComponent<MonsterBehavior>();
            if (behavior != null)
            {
                behavior.mapBoundaries = mapBoundaries;
                float rand = Random.value;
                if (rand < randomCombinedChance)
                    behavior.movementType = MonsterBehavior.MovementType.Combined;
                else if (rand < randomCombinedChance + randomVerticalChance)
                    behavior.movementType = MonsterBehavior.MovementType.Vertical;
                else
                    behavior.movementType = MonsterBehavior.MovementType.Horizontal;
            }
            spawnedPositions.Add(randomPos);
            Debug.Log($"Spawned monster randomly at {clampedPosition}, moving {behavior.movementType}");
            attempts = 0; // Reset attempts after successful spawn
        }

        if (attempts >= maxAttempts)
        {
            Debug.LogWarning("Could not spawn all monsters: too many attempts to find valid positions.");
        }
    }
}