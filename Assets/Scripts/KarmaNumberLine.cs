using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class KarmaNumberLine : MonoBehaviour 
{
    [Header("Visual Settings")]
    public float lineLength = 500f;  // Width of number line in pixels
    public float markerSize = 10f;   // Size of karma position marker
    public float maxKarma = 100f;    // Value at right end of line
    public float minKarma = -100f;   // Value at left end of line
    
    [Header("References")]
    public RectTransform numberLine;  // The actual line image
    public RectTransform marker;      // Marker showing karma position
    public TMP_Text karmaText;        // Numerical display
    
    private void OnEnable() 
    {
        KarmaManager.Instance.OnKarmaChanged += UpdateKarmaDisplay;
    }
    
    private void UpdateKarmaDisplay(float globalKarma, float levelKarma) 
    {
        // Position marker on number line
        float t = Mathf.InverseLerp(minKarma, maxKarma, globalKarma);
        float xPos = Mathf.Lerp(-lineLength/2f, lineLength/2f, t);
        marker.anchoredPosition = new Vector2(xPos, 0);
        
        // Update text
        karmaText.text = $"Karma: {globalKarma:+#;-#;0}\nLevel: {levelKarma:+#;-#;0}";
    }
}