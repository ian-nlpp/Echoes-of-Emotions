using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Movement speed
    private Rigidbody2D rb;
    private MapBoundaries mapBoundaries; // Reference to boundary script
    private Animator animator; // Reference to Animator

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f; // Disable gravity
        mapBoundaries = GetComponent<MapBoundaries>(); // Get boundary component
        animator = GetComponent<Animator>(); // Get Animator component
    }

    void Update()
    {
        // Get input from WASD or arrow keys
        float moveX = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
        float moveY = Input.GetAxisRaw("Vertical");   // W/S or Up/Down

        // Calculate movement vector
        Vector2 movement = new Vector2(moveX, moveY).normalized * moveSpeed;

        // Apply movement to Rigidbody2D
        rb.linearVelocity = movement;

        // Update animation state
        bool isMoving = movement.magnitude > 0;
        animator.SetBool("IsMoving", isMoving);

        // Clamp position to map boundaries
        if (mapBoundaries != null)
        {
            transform.position = mapBoundaries.ClampPosition(transform.position);
        }
    }
}