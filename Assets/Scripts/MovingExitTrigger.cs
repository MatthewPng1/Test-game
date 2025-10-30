using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovingExitTrigger : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed at which the object moves

    private void Update()
    {
        // Move the object horizontally to the left
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine("LevelExit");
        }
    }

    IEnumerator LevelExit()
    {
        yield return new WaitForSeconds(0.1f);

        UIManager.instance.fadeToBlack = true;

        yield return new WaitForSeconds(2f);
        GameManager.instance.LevelComplete();
    }
}