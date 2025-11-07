using UnityEngine;

public class StickPickup : MonoBehaviour
{
    [SerializeField] private GameObject stickObject; // Reference to the stick object
    [SerializeField] private GameObject pickupEffect; // Reference to the pickup effect prefab

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Enable the stick object
            if (stickObject != null)
            {
                stickObject.SetActive(true);
            }

            // Spawn pickup effect
            if (pickupEffect != null)
            {
                Instantiate(pickupEffect, transform.position, Quaternion.identity);
            }

            // Destroy the pickup with a slight delay to allow the effect to play
            Destroy(gameObject, 0.2f);
        }
    }
}