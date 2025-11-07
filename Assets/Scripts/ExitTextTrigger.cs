using UnityEngine;
using TMPro;

public class ExitTextTrigger : MonoBehaviour
{
    [SerializeField] private GameObject textObject; // Reference to the text object

    private void Start()
    {
        // Ensure text is hidden at start
        if (textObject != null)
        {
            textObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ShowText();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HideText();
        }
    }

    private void ShowText()
    {
        if (textObject != null)
        {
            textObject.SetActive(true);
        }
    }

    private void HideText()
    {
        if (textObject != null)
        {
            textObject.SetActive(false);
        }
    }
}