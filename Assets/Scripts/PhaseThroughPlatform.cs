using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A moving platform that lets a player standing on it temporarily phase through floor tiles.
/// - Moves along waypoints (loop or back-and-forth)
/// - Does NOT parent the player (avoids parenting side-effects)
/// - When the player stands on top of the platform, the script will ignore collisions
///   between the player's collider and any floor colliders found in a configurable area above the platform.
/// - When the player leaves the platform the ignored collisions are restored.
///
/// Usage:
/// - Add a Collider2D and Rigidbody2D (set body type to Kinematic or leave none) to the platform GameObject
/// - Assign waypoints in the inspector
/// - Set the Floor LayerMask to the layer(s) used by your ground tiles
/// - The platform will apply its movement velocity to the player's Rigidbody2D while the player is riding
///   (so the player moves smoothly with the platform without parenting).
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class PhaseThroughPlatform : MonoBehaviour
{
    [Header("Movement")]
    public List<Transform> waypoints = new List<Transform>();
    public float moveSpeed = 3f;
    public float waitTime = 0.5f;
    public bool isLoop = true;
    public bool startAtFirstWaypoint = true;

    [Header("Floor Phasing")]
    [Tooltip("Layers considered 'floor' that the player can phase through when on this platform")]
    public LayerMask floorLayer;
    [Tooltip("Size of the area (local space) above the platform to search for floor colliders to ignore")]
    public Vector2 floorSearchSize = new Vector2(3f, 1.5f);
    [Tooltip("Vertical offset from the platform center where the floor search box is placed")]
    public float floorSearchVerticalOffset = 0.75f;

    // internal movement state
    int currentWaypointIndex = 0;
    int direction = 1;
    Vector3 velocity = Vector3.zero;
    bool isWaiting = false;
    float waitCounter = 0f;

    // riding player references
    Rigidbody2D ridingPlayerRb;
    Collider2D ridingPlayerCollider;

    // Keep track of which floor colliders we've ignored for the player so we can restore them
    HashSet<Collider2D> ignoredFloorColliders = new HashSet<Collider2D>();

    Vector3 lastPosition;

    void Start()
    {
        if (waypoints.Count > 0 && startAtFirstWaypoint)
            transform.position = waypoints[0].position;

        lastPosition = transform.position;
    }

    void FixedUpdate()
    {
        // Movement
        if (waypoints.Count >= 2)
        {
            if (isWaiting)
            {
                waitCounter += Time.fixedDeltaTime;
                if (waitCounter >= waitTime)
                {
                    isWaiting = false;
                    waitCounter = 0f;
                }
            }
            else
            {
                Vector3 target = waypoints[currentWaypointIndex].position;
                Vector3 newPos = Vector3.SmoothDamp(transform.position, target, ref velocity, 0.5f, moveSpeed);
                Vector3 platformDelta = newPos - transform.position;

                // Move platform
                transform.position = newPos;

                // If a player is riding, add platform movement to player's velocity so they move with it
                if (ridingPlayerRb != null)
                {
                    ridingPlayerRb.linearVelocity = ridingPlayerRb.linearVelocity + (Vector2)platformDelta / Time.fixedDeltaTime;
                }

                // Check arrival
                if (Vector3.Distance(transform.position, target) < 0.05f)
                {
                    isWaiting = true;
                    if (isLoop)
                    {
                        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
                    }
                    else
                    {
                        if (currentWaypointIndex >= waypoints.Count - 1) direction = -1;
                        else if (currentWaypointIndex <= 0) direction = 1;
                        currentWaypointIndex += direction;
                    }
                }
            }
        }

        lastPosition = transform.position;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // If player lands on top of the platform
        if (collision.gameObject.CompareTag("Player"))
        {
            // Find the contact normal that points upward relative to platform
            foreach (var contact in collision.contacts)
            {
                // normal.y < 0 means the other collider is above this platform surface (player landed on top)
                if (contact.normal.y < -0.5f)
                {
                    ridingPlayerRb = collision.rigidbody;
                    ridingPlayerCollider = collision.collider;
                    StartIgnoringFloorsForPlayer(ridingPlayerCollider);
                    break;
                }
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Clear riding references and restore collisions
            if (collision.collider == ridingPlayerCollider)
            {
                RestoreIgnoredFloors();
                ridingPlayerRb = null;
                ridingPlayerCollider = null;
            }
        }
    }

    void OnDisable()
    {
        RestoreIgnoredFloors();
    }

    void OnDestroy()
    {
        RestoreIgnoredFloors();
    }

    // Find floor colliders in an area above the platform and ignore collisions with the player's collider
    void StartIgnoringFloorsForPlayer(Collider2D playerCol)
    {
        if (playerCol == null) return;

        // Compute world-space center of search box
        Vector2 center = (Vector2)transform.position + Vector2.up * floorSearchVerticalOffset;
        Vector2 size = floorSearchSize;

        Collider2D[] hits = Physics2D.OverlapBoxAll(center, size, 0f, floorLayer);
        foreach (var col in hits)
        {
            if (col == null) continue;
            // ignore only non-trigger colliders
            if (col.isTrigger) continue;
            if (!ignoredFloorColliders.Contains(col))
            {
                Physics2D.IgnoreCollision(playerCol, col, true);
                ignoredFloorColliders.Add(col);
            }
        }
    }

    // Restore previously ignored collisions
    void RestoreIgnoredFloors()
    {
        if (ridingPlayerCollider == null && ignoredFloorColliders.Count == 0) return;
        foreach (var col in ignoredFloorColliders)
        {
            if (col == null) continue;
            Physics2D.IgnoreCollision(ridingPlayerCollider, col, false);
        }
        ignoredFloorColliders.Clear();
    }

    // Optional: draw search box in editor for easier tuning
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 0.7f, 1f, 0.3f);
        Vector3 center = transform.position + Vector3.up * floorSearchVerticalOffset;
        Gizmos.DrawCube(center, new Vector3(floorSearchSize.x, floorSearchSize.y, 0.1f));
    }
}
