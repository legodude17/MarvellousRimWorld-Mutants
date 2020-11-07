using HarmonyLib;
using MarvelousMutants.Hediffs;
using Verse;
// ReSharper disable InconsistentNaming

namespace MarvelousMutants.Harmony
{
    [HarmonyPatch(typeof(Pawn_HealthTracker), "ShouldBeDead")]
    public class Pawn_HealthTracker_ShouldBeDead
    {
        public static void Postfix(ref bool __result, Pawn_HealthTracker __instance)
        {
            foreach (var hediff in __instance.hediffSet.hediffs)
            {
                if (!(hediff is Hediff_Power power)) continue;
                if (!power.PreventsDeath) continue;
                __result = false;
                return;
            }
        }
    }
}