using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MeterUI : MonoBehaviour
{
    public MeterType meterType;

    // OPTION A: Use a Slider (your current setup)
    public Slider slider;

    // OPTION B: Use a stand-alone Image with Filled type
    public Image fillImage;

    public TextMeshProUGUI valueLabel; // optional

    void OnEnable()
    {
        if (MeterManager.Instance != null && MeterManager.Instance.TryGetEvent(meterType, out var ev))
        {
            ev.AddListener(UpdateUI);
            UpdateUI(MeterManager.Instance.Get(meterType)); // initialize
        }
    }

    void OnDisable()
    {
        if (MeterManager.Instance != null && MeterManager.Instance.TryGetEvent(meterType, out var ev2))
        {
            ev2.RemoveListener(UpdateUI);
        }}

    void UpdateUI(float raw)
    {
        // If a Slider is assigned, drive it
        if (slider != null)
        {
            slider.minValue = MeterManager.Instance.minValue;
            slider.maxValue = MeterManager.Instance.maxValue;
            slider.value    = raw; // <-- this is what makes the bar move
        }
        // Otherwise fall back to Image.fillAmount
        else if (fillImage != null)
        {
            float normalized = Mathf.InverseLerp(
                MeterManager.Instance.minValue,
                MeterManager.Instance.maxValue,
                raw
            );
            fillImage.fillAmount = normalized;
        }

        if (valueLabel != null)
            valueLabel.text = Mathf.RoundToInt(raw).ToString();
    }
}

