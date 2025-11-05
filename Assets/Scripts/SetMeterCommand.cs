// This code is a custom Fungus command that integrates with your MeterManager.
// Free for use under the MIT license.

using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// Set a selected meter (Happiness / Stress) to an absolute value.
    /// </summary>
    [CommandInfo("Stats",
                 "Set Meter",
                 "Set a meter to an absolute value.")]
    [AddComponentMenu("")]
    public class SetMeterCommand : Command
    {
        [SerializeField] protected MeterType meter = MeterType.Happiness;

        [Tooltip("Absolute value to set (will be clamped by MeterManager).")]
        [SerializeField] protected FloatData value;

        public override void OnEnter()
        {
            if (MeterManager.Instance != null)
            {
                MeterManager.Instance.Set(meter, value.Value);
            }
            else
            {
                Debug.LogWarning("[SetMeterCommand] No MeterManager.Instance in scene.");
            }

            Continue();
        }

        public override string GetSummary()
        {
            return $"{meter} = {value.Value}";
        }

        public override Color GetButtonColor()
        {
            // light green-ish
            return new Color32(209, 242, 176, 255);
        }

        public override bool HasReference(Variable variable)
        {
            return value.floatRef == variable || base.HasReference(variable);
        }
    }
}
