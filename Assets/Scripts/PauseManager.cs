// Assets/Scripts/PauseManager.cs
using UnityEngine;
using UnityEngine.UI; // Required for Slider
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public Slider volumeSlider;
    private bool isPaused = false;
    private float volumeBeforePause;
    public PlayerStats playerStats; // Assign this in the Inspector

    void Start()
    {
        // Ensure the pause menu is hidden at the start
        pauseMenuPanel.SetActive(false);

        // Set the slider's initial value to the current game volume and add a listener
        if (volumeSlider != null)
        {
            volumeSlider.value = AudioListener.volume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    void Update()
    {
        // Listen for the Escape key to toggle the pause menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f; // This freezes the game

        volumeBeforePause = AudioListener.volume; // Save the current volume
        AudioListener.volume = 0f; // Mute all game audio
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f; // This unfreezes the game

        AudioListener.volume = volumeBeforePause; // Restore the saved volume
    }

    public void SetVolume(float volume)
    {
        // Use AudioListener.volume to control all sound in the game
        AudioListener.volume = volume;
    }

    public void ToggleAudio()
    {
        // Toggle master volume between 0 and the slider's value
        if (AudioListener.volume > 0)
        {
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = volumeSlider.value;
        }
    }

    public async void OnSaveAndExit()
    {
        Debug.Log("Save and Exit button clicked!");

        if (CloudSaveManager.instance != null && playerStats != null)
        {
            // Create a data object from our current stats
            var progressData = new PlayerProgressData
            {
                PlayerName = playerStats.playerName,
                CurrentHP = playerStats.currentHP,
                MaxHP = playerStats.maxHP,
                CollectedGems = playerStats.collectedGems
            };

            // Tell the manager to save it to the cloud
            await CloudSaveManager.instance.SaveData(progressData);
        }

        // --- ADD THESE TWO LINES ---
        // 1. Unfreeze the game before changing scenes
        Time.timeScale = 1f;

        // 2. Load your main menu or login scene. Replace "LoginScene" with your actual scene name.
        SceneManager.LoadScene("LoginScene");
    }

}