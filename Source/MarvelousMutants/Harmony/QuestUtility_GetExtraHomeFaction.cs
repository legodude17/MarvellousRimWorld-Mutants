using HarmonyLib;
using MarvelousMutants.Abilities.Comps;
using RimWorld;
using Verse;

namespace MarvelousMutants.Harmony
{
        [HarmonyPatch(typeof(QuestUtility), "GetExtraHomeFaction")]
        public class QuestUtility_GetExtraHomeFaction
        {
            public static void Postfix(ref Faction __result, Pawn p)
            {
                foreach (var instance in CompAbilityEffect_MindControl.instances)
                {
                    var info = instance.Info;
                    if (info.pawn != p) continue;
                    __result = info.oldFaction;
                    return;
                }
            }
        }
    
}