using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using UnityEngine;

// A simple data structure to hold the progress we want to save
[Serializable]
public struct PlayerProgressData
{
    public string PlayerName;
    public int CurrentHP;
    public int MaxHP;
    public int CollectedGems;
}

public class CloudSaveManager : MonoBehaviour
{
    public static CloudSaveManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async Task SaveData(PlayerProgressData data)
    {
        try
        {
            // We create a dictionary of the data we want to save
            var dataToSave = new Dictionary<string, object>
            {
                { "PlayerProgress", data }
            };

            // The Cloud Save service automatically uses the logged-in player's ID
            await CloudSaveService.Instance.Data.Player.SaveAsync(dataToSave);
            Debug.Log($"<color=cyan>CloudSave:</color> Successfully saved player progress!");
        }
        catch (Exception e)
        {
            Debug.LogError($"<color=cyan>CloudSave:</color> Error saving data: {e.Message}");
        }
    }

    public async Task<PlayerProgressData> LoadData()
    {
        try
        {
            // Ask Cloud Save for the key "PlayerProgress"
            var loadedData = await CloudSaveService.Instance.Data.Player.LoadAsync(
                new HashSet<string> { "PlayerProgress" }
            );

            if (loadedData.TryGetValue("PlayerProgress", out var data))
            {
                // If we found it, convert it from a raw string (JSON) into our data structure
                var progressData = data.Value.GetAs<PlayerProgressData>();
                Debug.Log($"<color=cyan>CloudSave:</color> Successfully loaded player progress!");
                return progressData;
            }
            else
            {
                Debug.Log($"<color=cyan>CloudSave:</color> No cloud data found for this player. Starting fresh.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"<color=cyan>CloudSave:</color> Error loading data: {e.Message}");
        }

        // Return default/empty data if nothing was found or an error occurred
        return default;
    }
}