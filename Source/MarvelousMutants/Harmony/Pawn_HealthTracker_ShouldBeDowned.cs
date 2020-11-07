using System.Linq;
using HarmonyLib;
using Verse;

// ReSharper disable InconsistentNaming

namespace MarvelousMutants.Harmony
{
    [HarmonyPatch(typeof(Pawn_HealthTracker), "ShouldBeDowned")]
    public class Pawn_HealthTracker_ShouldBeDowned
    {
        public static void Postfix(ref bool __result, Pawn_HealthTracker __instance, Pawn ___pawn)
        {
            if (__result) return;
            if (Enumerable.Any(__instance.hediffSet.hediffs, t => t.CauseDeathNow()))
            {
                __result = true;
                return;
            }
            __result = __instance.ShouldBeDeadFromRequiredCapacity() != null || PawnCapacityUtility.CalculatePartEfficiency(__instance.hediffSet, ___pawn.RaceProps.body.corePart) <= 9.99999974737875E-05 || __instance.ShouldBeDeadFromLethalDamageThreshold();
        }
    }
}