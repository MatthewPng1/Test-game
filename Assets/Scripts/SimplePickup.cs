using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePickup : MonoBehaviour
{
    public enum pickupType { coin, gem, health }

    public pickupType pt;
    [SerializeField] GameObject PickupEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (pt == pickupType.coin)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                // Increment coin count on the simplified manager
                if (SimpleGameManager.instance != null)
                {
                    SimpleGameManager.instance.IncrementCoinCount();
                }

                if (PickupEffect != null)
                    Instantiate(PickupEffect, transform.position, Quaternion.identity);

                Destroy(this.gameObject, 0.2f);
            }
        }
    }
}
