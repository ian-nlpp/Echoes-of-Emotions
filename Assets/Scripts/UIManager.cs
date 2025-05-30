// Assets/Scripts/UIManager.cs
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public PlayerController playerController; // Reference to PlayerController
    public TextMeshProUGUI statsText; // Reference to UI text

    void Update()
    {
        if (playerController != null && playerController.stats != null)
        {
            statsText.text = $"Name: {playerController.stats.playerName}\n" +
                             $"HP: {playerController.stats.currentHP}/{playerController.stats.maxHP}\n" +
                             $"Gems: {playerController.stats.collectedGems}";
        }
    }
}