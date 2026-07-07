using System.Collections.Generic;
using EFT.CameraControl;
using UnityEngine;

namespace ClearScope.Data
{
    internal static class ScopeRegistry
    {
        internal const string ScalesProperty = "_Scales";

        private static readonly Dictionary<OpticSight, Vector4> OriginalScales = new Dictionary<OpticSight, Vector4>();
        private static int _diagnosticLinesWritten;

        internal static void RegisterAndUpdate(OpticSight opticSight, string source)
        {
            if (opticSight == null)
            {
                return;
            }

            Renderer opticRenderer = opticSight.LensRenderer;
            if (opticRenderer == null)
            {
                WriteDiagnostic($"{source}: OpticSight has no LensRenderer.");
                return;
            }

            Material opticMaterial = opticRenderer.material;
            if (opticMaterial == null)
            {
                WriteDiagnostic($"{source}: OpticSight LensRenderer has no material.");
                return;
            }

            if (!opticMaterial.HasProperty(ScalesProperty))
            {
                WriteDiagnostic($"{source}: material '{opticMaterial.name}' shader '{opticMaterial.shader?.name}' has no {ScalesProperty} property.");
                return;
            }

            Vector4 scales = opticMaterial.GetVector(ScalesProperty);
            if (!OriginalScales.ContainsKey(opticSight))
            {
                OriginalScales.Add(opticSight, scales);
            }

            WriteDiagnostic($"{source}: material '{opticMaterial.name}' shader '{opticMaterial.shader?.name}' original {FormatVector(OriginalScales[opticSight])}, current {FormatVector(scales)}.");
            Update(opticSight);
        }

        internal static void UpdateAll()
        {
            foreach (OpticSight opticSight in new List<OpticSight>(OriginalScales.Keys))
            {
                Update(opticSight);
            }
        }

        internal static void Update(OpticSight opticSight)
        {
            if (opticSight == null || !OriginalScales.TryGetValue(opticSight, out Vector4 originalScales))
            {
                return;
            }

            Renderer opticRenderer = opticSight.LensRenderer;
            Material opticMaterial = opticRenderer != null ? opticRenderer.material : null;
            if (opticMaterial == null || !opticMaterial.HasProperty(ScalesProperty))
            {
                return;
            }

            Vector4 scales = opticMaterial.GetVector(ScalesProperty);
            if (Plugin.ModEnabled.Value)
            {
                scales.x = originalScales.x * Plugin.ScaleXMultiplier.Value;
                scales.y = originalScales.y * Plugin.ScaleYMultiplier.Value;
            }
            else
            {
                scales.x = originalScales.x;
                scales.y = originalScales.y;
            }

            opticMaterial.SetVector(ScalesProperty, scales);
            WriteDiagnostic($"Update: applied {FormatVector(scales)} to material '{opticMaterial.name}'.");
        }

        internal static void Clear()
        {
            OriginalScales.Clear();
            _diagnosticLinesWritten = 0;
        }

        private static void WriteDiagnostic(string message)
        {
            if (!Plugin.DiagnosticLogging.Value || _diagnosticLinesWritten >= 80)
            {
                return;
            }

            _diagnosticLinesWritten++;
            Plugin.Logger.LogInfo($"[Diagnostic] {message}");
        }

        private static string FormatVector(Vector4 vector)
        {
            return $"({vector.x:0.###}, {vector.y:0.###}, {vector.z:0.###}, {vector.w:0.###})";
        }
    }
}
