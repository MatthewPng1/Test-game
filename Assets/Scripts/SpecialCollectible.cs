using UnityEngine;

public class SpecialCollectible : MonoBehaviour
{
    [SerializeField] private GameObject SpecialObject; // Reference to the exit trigger object
    [SerializeField] private GameObject pickupEffect; // Reference to the pickup effect prefab

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Stick"))
        {
            // Enable the exit trigger
            if (SpecialObject != null)
            {
                SpecialObject.SetActive(true);
            }

            // Spawn pickup effect
            if (pickupEffect != null)
            {
                Instantiate(pickupEffect, transform.position, Quaternion.identity);
            }

            // Destroy the object with a slight delay to allow the effect to play
            Destroy(gameObject, 0.2f);
        }
    }
}