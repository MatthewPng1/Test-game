using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    [Header("End Game UI")]
    [SerializeField] private GameObject endGameUI;

    [Header("Player Movement Script")]
    [SerializeField] private MonoBehaviour playerMovementScript; // assign your mover

    private bool gameEnded = false;

    public void TriggerEndGame()
    {
        if (gameEnded) return;
        gameEnded = true;

        if (endGameUI) endGameUI.SetActive(true);
        if (playerMovementScript) playerMovementScript.enabled = false;

        // --- HARD FREEZE the player ---
        if (playerMovementScript)
        {
            var go = playerMovementScript.gameObject;

            if (go.TryGetComponent<Rigidbody2D>(out var rb2d))
            {
                rb2d.linearVelocity = Vector2.zero;
                rb2d.angularVelocity = 0f;
                rb2d.gravityScale = 0f; // prevents sliding
                rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
            }

        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            TriggerEndGame();
    }
}
