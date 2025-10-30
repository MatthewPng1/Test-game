using UnityEngine;

public class SimpleExitTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            LevelExit();
        }
    }

    private void LevelExit()
    {
        SimpleGameManager.instance.LevelComplete();
    }
}