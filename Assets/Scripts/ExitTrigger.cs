using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitTrigger : MonoBehaviour
{
    // Reference to the AudioSource component
    //private AudioSource audioSource;

    private void Start()
    {
        // Get the AudioSource component attached to the same object
        //audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Play the sound effect
            //audioSource.Play();

            // Complete the level
            LevelExit();
        }
    }

    private void LevelExit()
    {
        GameManager.instance.LevelComplete();
    }
}
