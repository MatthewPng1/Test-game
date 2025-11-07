using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePickup : MonoBehaviour
{
    public enum pickupType { coin, gem, health }
    public int value = 1;
    public pickupType pt;
    [SerializeField] GameObject PickupEffect;

    private bool isCollected = false;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (pt == pickupType.coin && !isCollected)
        {
            if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Stick"))
            {
                isCollected = true;  // Mark as collected immediately
                
                // Increment coin count on the simplified manager
                if (SimpleGameManager.instance != null)
                {
                    SimpleGameManager.instance.IncrementCoinCount();
                }
                if (PickupEffect != null)
                    Instantiate(PickupEffect, transform.position, Quaternion.identity);

                // Disable the collider immediately to prevent double triggers
                GetComponent<Collider2D>().enabled = false;
                
                Destroy(this.gameObject, 0.2f);
            }
        }
    }
}
