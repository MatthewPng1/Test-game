using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePickup : MonoBehaviour
{
    public enum pickupType { coin, gem, health }
    public int value = 1;
    public pickupType pt;
    [SerializeField] GameObject PickupEffect;
    [SerializeField] AudioSource pickupAudioSource;  // Assign the audio source with pickup sound

    private bool isCollected = false;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (pt == pickupType.coin && !isCollected)
        {
            if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Stick"))
            {
                // Extra safety: ensure SimpleGameManager hasn't marked level as ended
                if (SimpleGameManager.instance != null && SimpleGameManager.instance.levelEnded)
                    return;

                isCollected = true;  // Mark as collected immediately
                
                // Increment coin count on the simplified manager
                if (SimpleGameManager.instance != null)
                {
                    SimpleGameManager.instance.IncrementCoinCount();
                }
                if (PickupEffect != null)
                    Instantiate(PickupEffect, transform.position, Quaternion.identity);

                // Play pickup sound
                if (pickupAudioSource != null)
                    pickupAudioSource.PlayOneShot(pickupAudioSource.clip);

                // Disable the collider immediately to prevent double triggers
                GetComponent<Collider2D>().enabled = false;
                
                Destroy(this.gameObject, 0.2f);
            }
        }
    }
}
