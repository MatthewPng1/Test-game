using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameObject movingObject; // The object that will move left
    public float moveSpeed = 5f; // Speed at which the object moves

    private bool isTriggered = false;

    // Reference to the AudioSource component
    private AudioSource audioSource;

    private void Start()
    {
        // Get the AudioSource component attached to the same object
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !isTriggered)
        {
            isTriggered = true;

            // Play the sound effect
            audioSource.Play();

            StartCoroutine(MoveObjectLeft());
        }
    }

    IEnumerator MoveObjectLeft()
    {
        while (true) // Move indefinitely to the left
        {
            movingObject.transform.position += Vector3.left * moveSpeed * Time.deltaTime;
            yield return null;
        }
    }
}