using UnityEngine;

public class MapBoundaries : MonoBehaviour
{
    public SpriteRenderer mapSprite; // Reference to the WorldMap sprite
    private Vector2 minBounds;
    private Vector2 maxBounds;
    private Vector2 playerSize; // To account for player's collider size
    private bool isInitialized = false;

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (mapSprite == null)
        {
            Debug.LogError("Map Sprite not assigned in MapBoundaries!");
            return;
        }

        // Calculate map bounds in world space
        Bounds spriteBounds = mapSprite.bounds;
        minBounds = spriteBounds.min;
        maxBounds = spriteBounds.max;

        // Get player's collider size to prevent clipping at edges
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            playerSize = collider.bounds.extents;
        }
        else
        {
            playerSize = Vector2.one * 0.5f; // Default size if no collider
        }

        isInitialized = true;
    }

    public Vector3 ClampPosition(Vector3 position)
    {
        if (!isInitialized)
        {
            Initialize();
        }
        // Clamp position to stay within map bounds, accounting for player size
        float clampedX = Mathf.Clamp(position.x, minBounds.x + playerSize.x, maxBounds.x - playerSize.x);
        float clampedY = Mathf.Clamp(position.y, minBounds.y + playerSize.y, maxBounds.y - playerSize.y);
        return new Vector3(clampedX, clampedY, position.z);
    }

    public Vector2 GetMinBounds() => isInitialized ? minBounds : Vector2.zero;
    public Vector2 GetMaxBounds() => isInitialized ? maxBounds : Vector2.zero;
    public bool IsInitialized() => isInitialized;
}