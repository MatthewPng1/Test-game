using UnityEngine;
using System;
using System.Collections.Generic;

public class KarmaManager : MonoBehaviour 
{
    public static KarmaManager Instance { get; private set; }
    public float GlobalKarma { get; private set; }  // Total across all levels
    public float CurrentLevelKarma { get; private set; }  // Reset on death
    
    // Event that UI can listen to
    public event Action<float, float> OnKarmaChanged;  // (globalKarma, levelKarma)
    
    private Dictionary<string, float> levelKarmaHighScores;  // Best karma per level

    public void AddKarma(float amount) 
    {
        CurrentLevelKarma += amount;
        GlobalKarma += amount;
        OnKarmaChanged?.Invoke(GlobalKarma, CurrentLevelKarma);
    }

    public void ResetLevelKarma() 
    {
        // On death, subtract level karma from global
        GlobalKarma -= CurrentLevelKarma;
        CurrentLevelKarma = 0;
        OnKarmaChanged?.Invoke(GlobalKarma, CurrentLevelKarma);
    }
}