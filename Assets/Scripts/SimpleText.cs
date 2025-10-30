using UnityEngine;

public class SimpleText : MonoBehaviour
{
    void Start()
    {
        // Get the text component
        TextMesh text = GetComponent<TextMesh>();
        if (text != null)
        {
            // Set up the text properties
            text.text = "new text";
            text.fontSize = 24;
            text.alignment = TextAlignment.Center;
            text.anchor = TextAnchor.MiddleCenter;
            
            // Make sure it renders in front
            MeshRenderer renderer = GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.sortingOrder = 2;  // Higher number = more in front
            }
        }

        // Position slightly in front of other objects
        Vector3 pos = transform.position;
        pos.z = -1;  // Adjust this value if needed
        transform.position = pos;
    }
}