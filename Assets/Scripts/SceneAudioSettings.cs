// Assets/Scripts/SceneAudioSettings.cs
using UnityEngine;

public class SceneAudioSettings : MonoBehaviour
{
    public AudioClip sceneBGM;

    void Start()
    {
        // Tell the AudioManager to play our specified BGM
        if (AudioManager.instance != null && sceneBGM != null)
        {
            AudioManager.instance.PlayBGM(sceneBGM);
        }
    }
}