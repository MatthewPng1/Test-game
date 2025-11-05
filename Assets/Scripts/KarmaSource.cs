using UnityEngine;

public class KarmaSource : MonoBehaviour 
{
    public enum KarmaType 
    { 
        GoodDeed,    // +2 karma
        BadDeed,     // -2 karma
        Blessing,    // +5 karma
        Curse,       // -5 karma
        // etc
    }
    
    public KarmaType type;
    public bool isOneTime = true;  // Destroy after use?
    public float customKarmaValue;  // Optional override
    
    private void OnTriggerEnter2D(Collider2D col) 
    {
        if (!col.CompareTag("Player")) return;
        
        float karmaValue = type switch {
            KarmaType.GoodDeed => 2f,
            KarmaType.BadDeed => -2f,
            KarmaType.Blessing => 5f,
            KarmaType.Curse => -5f,
            _ => customKarmaValue
        };
        
        KarmaManager.Instance?.AddKarma(karmaValue);
        
        if (isOneTime) {
            Destroy(gameObject);
        }
    }
}
