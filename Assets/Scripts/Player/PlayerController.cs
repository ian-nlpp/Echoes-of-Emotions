using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerStats stats; // Reference to ScriptableObject
    private float invincibilityDuration = 1f; // 1 second invincibility
    private float invincibilityTimer = 0f;
    private bool isInvincible = false;
    public static event System.Action OnPlayerTookDamage;

    void Start()
    {
        // Ensure stats are initialized
        if (stats == null)
        {
            Debug.LogError("PlayerStats not assigned!");
        }
    }

    void Update()
    {
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0)
            {
                isInvincible = false;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible)
        {
            return;
        }

        stats.currentHP = Mathf.Max(0, stats.currentHP - damage);
        OnPlayerTookDamage?.Invoke(); // <-- ADD THIS LINE

        Debug.Log($"{stats.playerName} took {damage} damage. Current HP: {stats.currentHP}");
        if (stats.currentHP <= 0)
        {
            Debug.Log("Player defeated!");
            // Handle player death
        }
        else
        {
            isInvincible = true;
            invincibilityTimer = invincibilityDuration;
        }
    }

    public void CollectGem()
    {
        stats.collectedGems++;
        Debug.Log($"{stats.playerName} collected a gem! Total: {stats.collectedGems}");
    }
}