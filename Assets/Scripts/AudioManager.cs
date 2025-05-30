// Assets/Scripts/AudioManager.cs
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource bgmSource;

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

    public void PlayBGM(AudioClip musicClip)
    {
        // Do not restart the music if it's already playing the correct clip
        if (bgmSource.clip == musicClip && bgmSource.isPlaying)
        {
            return;
        }

        bgmSource.clip = musicClip;
        bgmSource.loop = true;
        bgmSource.Play();
    }
}