// Assets/Scripts/PlayerAudio.cs
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerAudio : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private AudioSource audioSource;
    private Animator animator;

    public AudioClip footstepSound;
    public AudioClip hurtSound;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        // IMPORTANT: Ensure this AudioSource does not play on awake.
        audioSource.playOnAwake = false;
    }

    void OnEnable()
    {
        PlayerController.OnPlayerTookDamage += PlayHurtSound;
    }

    void OnDisable()
    {
        PlayerController.OnPlayerTookDamage -= PlayHurtSound;
    }

    void Update()
    {
        HandleFootstepSound();
    }

    void HandleFootstepSound()
    {
        bool isMoving = animator.GetBool("IsMoving");

        if (isMoving && !audioSource.isPlaying)
        {
            // If we start moving, set the clip to footsteps and play it on a loop.
            audioSource.clip = footstepSound;
            audioSource.loop = true;
            audioSource.Play();
        }
        else if (!isMoving && audioSource.isPlaying && audioSource.clip == footstepSound)
        {
            // If we stop moving, stop the sound.
            audioSource.Stop();
        }
    }

    void PlayHurtSound()
    {
        // Play the hurt sound as a one-off sound that can overlap with other sounds.
        // This is better than stopping the footstep sound.
        audioSource.PlayOneShot(hurtSound);
    }
}