// Assets/Scripts/SpawnPointData.cs
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnPointData", menuName = "Game/Spawn Point Data")]
public class SpawnPointData : ScriptableObject
{
    [System.Serializable]
    public struct SpawnPoint
    {
        public string sceneName; // Name of the scene (e.g., "SkyGardens")
        public Vector2 position; // Spawn position in the scene
    }

    public SpawnPoint[] spawnPoints; // Array of spawn points for all scenes
}
