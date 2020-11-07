using HarmonyLib;
using Verse;

namespace MarvelousMutants.Harmony
{
    [HarmonyPatch(typeof(PawnGenerator), "GenerateInitialHediffs")]
    public class PawnGenerator_GenerateInitialHediffs
    {
        public static void Postfix(Pawn pawn)
        {
            if (!pawn.def.race.Humanlike || pawn.Dead) return;
            if (Rand.Chance(0.5f))
            {
                pawn.health.AddHediff(MM_DefOf.XGene);
                Log.Message("Adding xgene to: " + pawn.LabelCap);
            }
        }
    }
}