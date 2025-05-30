using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Reference to the player's transform
    public Vector3 offset = new Vector3(0, 0, -10); // Camera offset (z = -10 for 2D)

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }
}