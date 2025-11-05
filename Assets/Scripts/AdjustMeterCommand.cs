// This code is a custom Fungus command that integrates with your MeterManager.
// Free for use under the MIT license.

using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// Add (or subtract) a value to a selected meter (Happiness / Stress).
    /// </summary>
    [CommandInfo("Stats",
                 "Adjust Meter",
                 "Add (or subtract) a value to a meter.")]
    [AddComponentMenu("")]
    public class AdjustMeterCommand : Command
    {
        [SerializeField] protected MeterType meter = MeterType.Happiness;

        [Tooltip("Positive increases; negative decreases.")]
        [SerializeField] protected FloatData delta;

        public override void OnEnter()
        {
            if (MeterManager.Instance != null)
            {
                MeterManager.Instance.Add(meter, delta.Value);
            }
            else
            {
                Debug.LogWarning("[AdjustMeterCommand] No MeterManager.Instance in scene.");
            }

            Continue();
        }

        public override string GetSummary()
        {
            string sign = delta.Value >= 0f ? "+" : "";
            return $"{meter} {sign}{delta.Value}";
        }

        public override Color GetButtonColor()
        {
            // light teal-ish
            return new Color32(176, 242, 230, 255);
        }

        public override bool HasReference(Variable variable)
        {
            return delta.floatRef == variable || base.HasReference(variable);
        }
    }
}
