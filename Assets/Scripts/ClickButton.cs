using UnityEngine;

public class ClickButton : MonoBehaviour
{
    [Tooltip("Assign the MovingPlatform component or GameObject that should be activated")]
    public MovingPlatform targetPlatform;

    [Header("Sound Effect")]
    public AudioSource audioSource; // Assign in inspector, set up with desired sound/loop/volume

    private bool hasBeenClicked = false;

    void OnMouseDown()
    {
        if (hasBeenClicked) return;
        hasBeenClicked = true;

        if (targetPlatform != null)
            targetPlatform.gameObject.SetActive(true);

        // Play sound from assigned AudioSource (e.g., Play, PlayOneShot, etc.)
        if (audioSource != null)
        {
            audioSource.Play(); // Plays the AudioSource's assigned clip with its settings
        }
    }
}