using System.Collections;
using UnityEngine;

/// <summary>
/// When the configured trigger (default: Player) collides with this object,
/// the object will wait for a short delay, switch its Rigidbody2D to Dynamic
/// so it falls, and then disable its Collider2D so it no longer blocks the player.
/// Attach to platforms or objects that should collapse when the player steps on them.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class FallingPlatform : MonoBehaviour
{
    [Tooltip("Tag of the object that triggers the platform (default: Player)")]
    public string triggerTag = "Player";

    [Tooltip("Delay (in seconds) after trigger before the platform starts to fall")] 
    public float fallDelay = 0.5f;

    [Tooltip("Delay (in seconds) after fall begins before disabling the collider")]
    public float disableColliderDelay = 0.1f;

    [Tooltip("Gravity scale to apply once platform becomes dynamic")]
    public float gravityScale = 1f;

    [Tooltip("Optional additional downward velocity applied when the platform starts to fall")]
    public float additionalDownwardVelocity = 0f;

    [Tooltip("If true, the platform will be destroyed after falling (destroyDelay seconds)")]
    public bool destroyAfterFall = false;
    public float destroyDelay = 5f;
    
    [Header("Audio")]
    [Tooltip("Sound to play when the platform is triggered by the player")]
    public AudioClip fallSound;
    [Range(0f,1f)]
    public float soundVolume = 1f;

    Rigidbody2D rb;
    Collider2D col;
    bool hasFallen = false;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();

        // If no Rigidbody2D exists, add one and keep it kinematic so the platform is stationary
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.simulated = true;
        }
        else
        {
            // Ensure initial state is kinematic so it doesn't respond to physics until triggered
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        // Ensure no gravity until activated
        rb.gravityScale = 0f;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasFallen) return;
        if (collision.gameObject.CompareTag(triggerTag))
        {
            PlayFallSound();
            StartCoroutine(FallRoutine());
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasFallen) return;
        if (other.gameObject.CompareTag(triggerTag))
        {
            PlayFallSound();
            StartCoroutine(FallRoutine());
        }
    }

    void PlayFallSound()
    {
        if (fallSound == null) return;
        // Play one-shot at the platform position so it is audible in 2D/3D space
        AudioSource.PlayClipAtPoint(fallSound, transform.position, soundVolume);
    }

    IEnumerator FallRoutine()
    {
        hasFallen = true;

        // Optional delay before falling (gives a little warning or animation time)
        if (fallDelay > 0f)
            yield return new WaitForSeconds(fallDelay);

        // Switch to dynamic so physics makes it fall
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = gravityScale;

        if (additionalDownwardVelocity != 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y - Mathf.Abs(additionalDownwardVelocity));
        }

        // Disable collider after a short delay so the player can fall through the platform
        if (disableColliderDelay <= 0f)
        {
            col.enabled = false;
        }
        else
        {
            yield return new WaitForSeconds(disableColliderDelay);
            if (col != null)
                col.enabled = false;
        }

        if (destroyAfterFall)
        {
            Destroy(gameObject, destroyDelay);
        }
    }
}
