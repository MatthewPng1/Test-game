using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Add new meters to this enum (e.g., Energy, Focus, etc.)
public enum MeterType { Happiness, Stress, /* , Energy, Focus, ... */ }

[System.Serializable]
public class FloatEvent : UnityEvent<float> {}

// Generic per-meter definition for extras configured in the Inspector
[System.Serializable]
public class ExtraMeter
{
    public MeterType type;
    [Range(0,100)] public float startValue = 0f;

    [HideInInspector] public float currentValue;
    public FloatEvent OnChanged;
}

public class MeterManager : MonoBehaviour
{
    public static MeterManager Instance { get; set; }

    [Header("Ranges")]
    public float minValue = 0f;
    public float maxValue = 100f;

    [Header("Initial Values (built-ins)")]
    [Range(0,100)] public float happiness = 50f;
    [Range(0,100)] public float stress = 0f;

    [Header("Events (built-ins)")]
    public FloatEvent OnHappinessChanged;   // sends raw value (0–100)
    public FloatEvent OnStressChanged;      // sends raw value (0–100)

    [Header("Additional Meters")]
    public List<ExtraMeter> additionalMeters = new List<ExtraMeter>();
    private Dictionary<MeterType, ExtraMeter> extraMap;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        // Build lookup for extras, clamp starting values
        extraMap = new Dictionary<MeterType, ExtraMeter>();
        foreach (var m in additionalMeters)
        {
            if (m == null) continue;
            if (m.type == MeterType.Happiness || m.type == MeterType.Stress)
            {
                Debug.LogWarning($"[MeterManager] '{m.type}' is a built-in meter; use the built-in fields instead.");
                continue;
            }
            if (!extraMap.ContainsKey(m.type))
            {
                m.currentValue = Mathf.Clamp(m.startValue, minValue, maxValue);
                extraMap.Add(m.type, m);
            }
            else
            {
                Debug.LogWarning($"[MeterManager] Duplicate extra meter '{m.type}' ignored.");
            }
        }
        // Optional: DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Push initial values to UI on play
        OnHappinessChanged?.Invoke(happiness);
        OnStressChanged?.Invoke(stress);

        foreach (var kv in extraMap)
            kv.Value.OnChanged?.Invoke(kv.Value.currentValue);
    }

    public float Get(MeterType type)
    {
        switch (type)
        {
            case MeterType.Happiness: return happiness;
            case MeterType.Stress:    return stress;
            default:
                if (extraMap != null && extraMap.TryGetValue(type, out var m))
                    return m.currentValue;
                Debug.LogWarning($"[MeterManager] Get: no meter '{type}'");
                return 0f;
        }
    }

    public void Set(MeterType type, float value)
    {
        value = Mathf.Clamp(value, minValue, maxValue);

        switch (type)
        {
            case MeterType.Happiness:
                if (!Mathf.Approximately(happiness, value))
                {
                    happiness = value;
                    OnHappinessChanged?.Invoke(happiness);
                }
                break;

            case MeterType.Stress:
                if (!Mathf.Approximately(stress, value))
                {
                    stress = value;
                    OnStressChanged?.Invoke(stress);
                }
                break;

            default:
                if (extraMap != null && extraMap.TryGetValue(type, out var m))
                {
                    if (!Mathf.Approximately(m.currentValue, value))
                    {
                        m.currentValue = value;
                        m.OnChanged?.Invoke(m.currentValue);
                    }
                }
                else
                {
                    Debug.LogWarning($"[MeterManager] Set: no meter '{type}'");
                }
                break;
        }
    }

    public void Add(MeterType type, float delta) => Set(type, Get(type) + delta);

    // Convenience wrappers (handy for Fungus Invoke Method from existing blocks)
    public void AddHappiness(float delta) => Add(MeterType.Happiness, delta);
    public void AddStress(float delta)     => Add(MeterType.Stress, delta);
    public void SetHappiness(float v)      => Set(MeterType.Happiness, v);
    public void SetStress(float v)         => Set(MeterType.Stress, v);

    // === Helper for UI scripts ===
    // Allows a UI script to subscribe to the correct event for any meter.
    public bool TryGetEvent(MeterType type, out FloatEvent ev)
    {
        switch (type)
        {
            case MeterType.Happiness: ev = OnHappinessChanged; return true;
            case MeterType.Stress:    ev = OnStressChanged;    return true;
            default:
                if (extraMap != null && extraMap.TryGetValue(type, out var m) && m.OnChanged != null)
                {
                    ev = m.OnChanged;
                    return true;
                }
                ev = null;
                return false;
        }
    }
}
