using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using ClearScope.Patches;
using ClearScope.Data;

namespace ClearScope
{
    [BepInPlugin("com.pein.clearscope", "Clear Scope", "1.0.2")]
    public class Plugin : BaseUnityPlugin
    {
        internal new static ManualLogSource Logger { get; private set; }

        internal static ConfigEntry<bool> ModEnabled { get; private set; }
        internal static ConfigEntry<float> ScaleXMultiplier { get; private set; }
        internal static ConfigEntry<float> ScaleYMultiplier { get; private set; }
        internal static ConfigEntry<bool> DiagnosticLogging { get; private set; }

        private void Awake()
        {
            Logger = base.Logger;

            ModEnabled = Config.Bind(
                "General",
                "Enabled",
                true,
                "Enable scope eye relief changes.");

            ScaleXMultiplier = Config.Bind(
                "General",
                "Scale X Multiplier",
                1.4f,
                new ConfigDescription(
                    "Multiplies _Scales.x on optic lens materials. Try increasing this if black shadow remains on the sides.",
                    new AcceptableValueRange<float>(0.1f, 5f)));

            ScaleYMultiplier = Config.Bind(
                "General",
                "Scale Y Multiplier",
                1.4f,
                new ConfigDescription(
                    "Multiplies _Scales.y on optic lens materials. This matches the original mod behavior.",
                    new AcceptableValueRange<float>(0.1f, 5f)));

            DiagnosticLogging = Config.Bind(
                "Debug",
                "Diagnostic Logging",
                false,
                "Write optic lens material values to BepInEx logs. Disable after testing.");

            ScaleXMultiplier.SettingChanged += (_, __) => ScopeRegistry.UpdateAll();
            ScaleYMultiplier.SettingChanged += (_, __) => ScopeRegistry.UpdateAll();
            ModEnabled.SettingChanged += (_, __) => ScopeRegistry.UpdateAll();

            new OpticSightAwakePatch().Enable();
            new OpticSightOnEnablePatch().Enable();
            new OnGameEndedPatch().Enable();

            Logger.LogInfo("Clear Scope loaded.");
        }
    }
}
