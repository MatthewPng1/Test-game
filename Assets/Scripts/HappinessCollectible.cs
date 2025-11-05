using UnityEngine;

public class HappinessCollectible : MonoBehaviour
{
    [Tooltip("Amount to add to happiness when collected. Use negative values to decrease happiness.")]
    public float happinessChange = 10f;

    [Tooltip("Should the object be destroyed after collection?")]
    public bool destroyOnCollect = true;

    [Tooltip("Optional particle system to play on collection")]
    public ParticleSystem collectionEffect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if it's the player
        if (other.CompareTag("Player"))
        {
            // Modify happiness through MeterManager
            if (MeterManager.Instance != null)
            {
                MeterManager.Instance.AddHappiness(happinessChange);
            }

            // Play effect if assigned
            if (collectionEffect != null)
            {
                collectionEffect.Play();
            }

            // Destroy the collectible if specified
            if (destroyOnCollect)
            {
                Destroy(gameObject);
            }
        }
    }
}