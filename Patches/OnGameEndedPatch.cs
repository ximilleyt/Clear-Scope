using System.Reflection;
using ClearScope.Data;
using EFT;
using HarmonyLib;
using SPT.Reflection.Patching;

namespace ClearScope.Patches
{
    public class OnGameEndedPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(Player), nameof(Player.OnGameSessionEnd));
        }

        [PatchPostfix]
        private static void PatchPostfix()
        {
            ScopeRegistry.Clear();
        }
    }
}
