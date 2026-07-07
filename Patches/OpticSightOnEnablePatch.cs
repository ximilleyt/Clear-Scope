using System.Reflection;
using ClearScope.Data;
using EFT.CameraControl;
using HarmonyLib;
using SPT.Reflection.Patching;

namespace ClearScope.Patches
{
    public class OpticSightOnEnablePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(OpticSight), "OnEnable");
        }

        [PatchPostfix]
        private static void PatchPostfix(OpticSight __instance)
        {
            ScopeRegistry.RegisterAndUpdate(__instance, "OnEnable");
        }
    }
}
