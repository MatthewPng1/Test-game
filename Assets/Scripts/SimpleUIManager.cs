using UnityEngine;
using UnityEngine.UI;

public class SimpleUIManager : MonoBehaviour
{
    public static SimpleUIManager instance;

    public bool fadeToBlack, fadeFromBlack;
    public Image blackScreen;
    public float fadeSpeed = 2f;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (fadeToBlack)
        {
            FadeToBlack();
        }
        else if (fadeFromBlack)
        {
            FadeFromBlack();
        }
    }

    private void FadeToBlack()
    {
        FadeScreen(1f);
        if (blackScreen.color.a >= 1f)
        {
            fadeToBlack = false;
        }
    }

    private void FadeFromBlack()
    {
        FadeScreen(0f);
        if (blackScreen.color.a <= 0f)
        {
            fadeFromBlack = false;
        }
    }

    private void FadeScreen(float targetAlpha)
    {
        Color currentColor = blackScreen.color;
        float newAlpha = Mathf.MoveTowards(currentColor.a, targetAlpha, fadeSpeed * Time.deltaTime);
        blackScreen.color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
    }
}