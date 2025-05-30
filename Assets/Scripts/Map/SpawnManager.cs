using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    public SpawnPointData spawnPointData; // Reference to spawn point data
    public GameObject player; // Reference to Player GameObject

    void Start()
    {
        if (spawnPointData == null || player == null)
        {
            Debug.LogError("SpawnPointData or Player not assigned in SpawnManager!");
            return;
        }

        // Get the current scene name
        string currentScene = SceneManager.GetActiveScene().name;

        // Find the spawn point for the current scene
        foreach (var spawnPoint in spawnPointData.spawnPoints)
        {
            if (spawnPoint.sceneName == currentScene)
            {
                // Set player position
                player.transform.position = new Vector3(spawnPoint.position.x, spawnPoint.position.y, 0f);
                Debug.Log($"Player spawned at {spawnPoint.position} in {currentScene}");
                return;
            }
        }

        Debug.LogWarning($"No spawn point found for scene {currentScene}. Player remains at default position.");
    }
}