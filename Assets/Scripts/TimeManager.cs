using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Tracks cumulative play time across scenes. Persists via DontDestroyOnLoad.
/// The timer can be paused for specific scene names (configured in the inspector)
/// or via the PauseTimer/ResumeTimer API.
/// </summary>
public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    [Tooltip("Total play time in seconds (read-only at runtime).")]
    public float totalPlayTimeSeconds = 0f;

    [Tooltip("Scene names in which the timer should be paused automatically.")]
    public List<string> pausedScenes = new List<string>();

    [Header("Options")]
    [Tooltip("If true the timer will start paused until resumed by code or scene change.")]
    public bool startPaused = false;

    public bool IsPaused { get; private set; } = false;

    // Event invoked whenever the time value changes (subscribers usually update UI)
    public event Action<float> OnTimeChanged;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        IsPaused = startPaused;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        // Ensure correct pause state for the initial scene
        CheckScenePause(SceneManager.GetActiveScene().name);
        // Notify listeners of initial value
        OnTimeChanged?.Invoke(totalPlayTimeSeconds);
    }

    void Update()
    {
        if (IsPaused) return;

        // Use unscaledDeltaTime so the timer isn't affected by timescale changes
        totalPlayTimeSeconds += Time.unscaledDeltaTime;
        OnTimeChanged?.Invoke(totalPlayTimeSeconds);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckScenePause(scene.name);
    }

    void CheckScenePause(string sceneName)
    {
        bool shouldPause = pausedScenes != null && pausedScenes.Contains(sceneName);
        SetPaused(shouldPause);
    }

    public void SetPaused(bool pause)
    {
        if (IsPaused == pause) return;
        IsPaused = pause;
    }

    public void PauseTimer() => SetPaused(true);
    public void ResumeTimer() => SetPaused(false);
    public void TogglePause() => SetPaused(!IsPaused);

    public float GetTotalSeconds() => totalPlayTimeSeconds;

    public string GetFormattedTime()
    {
        int total = Mathf.FloorToInt(totalPlayTimeSeconds);
        int hours = total / 3600;
        int minutes = (total % 3600) / 60;
        int seconds = total % 60;
        if (hours > 0)
            return string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);
        return string.Format("{0:D2}:{1:D2}", minutes, seconds);
    }

    public void ResetTimer()
    {
        totalPlayTimeSeconds = 0f;
        OnTimeChanged?.Invoke(totalPlayTimeSeconds);
    }
}
