using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Add for scene management

public class UILogin : MonoBehaviour
{
    [SerializeField] private Button loginButton;
    [SerializeField] private TMP_Text userIdText;
    [SerializeField] private TMP_Text userNameText;
    [SerializeField] private Transform loginPanel, userPanel;
    [SerializeField] private LoginController loginController;
    [SerializeField] private PlayerStats playerStats;

    private PlayerProfile playerProfile;

    private void OnEnable()
    {
        loginButton.onClick.AddListener(LoginButtonPressed);
        loginController.OnSignedIn += LoginController_OnSignedIn;
        loginController.OnAvatarUpdate += LoginController_OnAvatarUpdate;
    }

    private void OnDisable()
    {
        loginButton.onClick.RemoveListener(LoginButtonPressed);
        loginController.OnSignedIn -= LoginController_OnSignedIn;
        loginController.OnAvatarUpdate -= LoginController_OnAvatarUpdate;
    }

    private async void LoginButtonPressed()
    {
        await loginController.InitSignIn();
    }

    private async void LoginController_OnSignedIn(PlayerProfile profile)
    {
        playerProfile = profile;
        loginPanel.gameObject.SetActive(false);
        userPanel.gameObject.SetActive(true);

        userIdText.text = $"id_{playerProfile.playerInfo.Id}";
        userNameText.text = profile.Name;

        // --- NEW: Load data from the cloud ---
        if (CloudSaveManager.instance != null)
        {
            var loadedProgress = await CloudSaveManager.instance.LoadData();

            // If data was loaded, apply it to our PlayerStats. Otherwise, use defaults.
            if (loadedProgress.PlayerName != null)
            {
                playerStats.playerName = loadedProgress.PlayerName;
                playerStats.currentHP = loadedProgress.CurrentHP;
                playerStats.maxHP = loadedProgress.MaxHP;
                playerStats.collectedGems = loadedProgress.CollectedGems;
            }
            else
            {
                // This is a new player, set default stats
                playerStats.playerName = profile.Name;
                playerStats.currentHP = 100;
                playerStats.maxHP = 100;
                playerStats.collectedGems = 0;
            }
        }

        SavePlayerData(playerProfile); // This saves the ID locally, which is fine

        await System.Threading.Tasks.Task.Delay(2000);
        SceneManager.LoadScene("SunlitDunes");
    }

    private void LoginController_OnAvatarUpdate(PlayerProfile profile)
    {
        playerProfile = profile;
    }

    // Example method to save player data
    private void SavePlayerData(PlayerProfile profile)
    {
        // Example: Save to PlayerPrefs (replace with your data storage method)
        PlayerPrefs.SetString("PlayerID", profile.playerInfo.Id);
        PlayerPrefs.SetString("PlayerName", profile.Name);
        PlayerPrefs.Save();
    }
}