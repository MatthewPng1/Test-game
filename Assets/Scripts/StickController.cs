using UnityEngine;

public class StickController : MonoBehaviour
{
    [SerializeField] private float stickLength = 2f; // Length of the stick
    private Vector3 anchorPoint; // The fixed point where the stick is attached
    private Transform anchorTransform; // The transform to follow (player's transform)

    private void Start()
    {
        // Get the initial anchor point
        anchorTransform = transform.parent;
        if (anchorTransform != null)
        {
            anchorPoint = anchorTransform.position;
        }
    }

    private void Update()
    {
        if (anchorTransform != null)
        {
            // Update anchor point to follow the parent (player)
            anchorPoint = anchorTransform.position;

            // Get mouse position in world space
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0; // Ensure we're in 2D space

            // Calculate direction from anchor to mouse
            Vector2 direction = (mousePos - anchorPoint).normalized;

            // Set position at fixed distance from anchor point towards mouse
            transform.position = anchorPoint + (Vector3)(direction * stickLength);

            // Calculate rotation to point towards mouse
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}