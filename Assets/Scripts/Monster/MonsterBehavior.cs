// Assets/Scripts/MonsterBehavior.cs
using UnityEngine;

public class MonsterBehavior : MonoBehaviour
{
    public float moveDistance = 2f; // Distance to move back and forth
    public float moveSpeed = 2f; // Speed of movement
    public bool moveHorizontally = true; // True for back-and-forth, false for up-and-down
    public MapBoundaries mapBoundaries; // Reference to map boundaries
    private Vector3 startPosition;
    private float damage = 5f; // Damage dealt to player

    void Start()
    {
        startPosition = transform.position; // Store initial position
        if (mapBoundaries == null)
        {
            Debug.LogError("MapBoundaries not assigned in MonsterBehavior!");
        }
    }

    void Update()
    {
        if (mapBoundaries == null) return;

        // Calculate movement using ping-pong for back-and-forth or up-and-down
        float offset = Mathf.PingPong(Time.time * moveSpeed, moveDistance) - moveDistance / 2f;
        Vector3 moveOffset = moveHorizontally ? new Vector3(offset, 0, 0) : new Vector3(0, offset, 0);
        Vector3 targetPosition = startPosition + moveOffset;

        // Clamp position to map boundaries
        targetPosition = mapBoundaries.ClampPosition(targetPosition);
        transform.position = targetPosition;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage((int)damage);
                Debug.Log($"Monster dealt {damage} damage to player!");
            }
        }
    }
}
