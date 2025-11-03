using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Small utility to block user clicks by showing a full-screen transparent UI overlay.
/// Call ClickBlocker.Instance.Block(seconds) to enable the overlay for a short time.
/// This is useful when you have small transitions that can be skipped by fast clicks (e.g. portrait show/hide).
/// </summary>
public class ClickBlocker : MonoBehaviour
{
    public static ClickBlocker Instance { get; private set; }

    // Optional: assign a pre-made overlay GameObject in the inspector.
    // If left null, the script will create one at runtime.
    [Tooltip("Optional: full-screen GameObject (Image) used to block clicks. If null, one will be created.")]
    public GameObject overlay;

    Canvas overlayCanvas;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Ensure we have an overlay that covers the screen and receives raycasts
        if (overlay == null)
        {
            // Create a Canvas
            GameObject canvasGO = new GameObject("ClickBlocker_Canvas");
            canvasGO.transform.SetParent(transform, false);
            overlayCanvas = canvasGO.AddComponent<Canvas>();
            overlayCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();

            // Create a full-screen Image to block clicks
            overlay = new GameObject("ClickBlocker_Overlay");
            overlay.transform.SetParent(canvasGO.transform, false);
            var img = overlay.AddComponent<Image>();
            img.color = new Color(0f, 0f, 0f, 0f); // transparent but blocks raycasts

            var rt = overlay.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }

        // Ensure overlay has a CanvasGroup so we can easily toggle interactivity if needed
        if (overlay.GetComponent<CanvasGroup>() == null)
        {
            overlay.AddComponent<CanvasGroup>();
        }

        overlay.SetActive(false);
    }

    /// <summary>
    /// Show the overlay (which blocks clicks) for the given number of seconds.
    /// </summary>
    public void Block(float seconds)
    {
        if (overlay == null)
        {
            Debug.LogWarning("ClickBlocker: overlay is missing.");
            return;
        }

        StopAllCoroutines();
        overlay.SetActive(true);
        StartCoroutine(UnblockAfterDelay(seconds));
    }

    IEnumerator UnblockAfterDelay(float seconds)
    {
        // Edge-case: negative or zero seconds -> unblock next frame
        if (seconds <= 0f)
            yield return null;
        else
            yield return new WaitForSeconds(seconds);

        if (overlay != null)
            overlay.SetActive(false);
    }
}
