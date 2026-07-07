using System.Reflection;
using ClearScope.Data;
using EFT.CameraControl;
using HarmonyLib;
using SPT.Reflection.Patching;
using UnityEngine;

namespace ClearScope.Patches
{
    public class OpticSightAwakePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(OpticSight), nameof(OpticSight.Awake));
        }

        [PatchPostfix]
        private static void PatchPostfix(OpticSight __instance)
        {
            ScopeRegistry.RegisterAndUpdate(__instance, "Awake");
        }
    }
}
