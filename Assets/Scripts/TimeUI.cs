using UnityEngine;
using TMPro;

/// <summary>
/// Simple UI component that displays the play time from TimeManager.
/// Assign a TextMeshProUGUI (or TMP_Text) component in the inspector; if none is assigned
/// the script will attempt to get one from the same GameObject.
/// </summary>
public class TimeUI : MonoBehaviour
{
    public TMP_Text timeText;

    [Tooltip("If true the UI will always show hours even when 0 (HH:MM:SS).")]
    public bool alwaysShowHours = false;

    void OnEnable()
    {
        if (timeText == null)
            timeText = GetComponent<TMP_Text>();

        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.OnTimeChanged += UpdateTime;
            // Initialize immediately
            UpdateTime(TimeManager.Instance.GetTotalSeconds());
        }
    }

    void OnDisable()
    {
        if (TimeManager.Instance != null)
            TimeManager.Instance.OnTimeChanged -= UpdateTime;
    }

    void UpdateTime(float seconds)
    {
        if (timeText == null) return;

        int total = Mathf.FloorToInt(seconds);
        int hours = total / 3600;
        int minutes = (total % 3600) / 60;
        int secs = total % 60;

        if (hours > 0 || alwaysShowHours)
        {
            timeText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, secs);
        }
        else
        {
            timeText.text = string.Format("{0:D2}:{1:D2}", minutes, secs);
        }
    }
}
