using System.Linq;
using RimWorld;
using Verse;

namespace MarvelousMutants.Hediffs
{
    public class Hediff_XGene : Hediff
    {
        private bool givenPowers = false;
        public override bool ShouldRemove => givenPowers;

        public override void Tick()
        {
            base.Tick();
            if (pawn.needs.mood.CurLevel <= pawn.mindState.mentalBreaker.BreakThresholdExtreme ||
                pawn.health.hediffSet.PainTotal >= 0.800000011920929)
            {
                GivePowers();
                givenPowers = true;
            }
        }

        private void GivePowers()
        {
            var powers = DefDatabase<HediffDef>.AllDefs.Where(def => def.hediffClass == typeof(Hediff_Power) || def.hediffClass.IsSubclassOf(typeof(Hediff_Power)))
                .ToList();
            var power = powers.RandomElement();
            if (!(HediffMaker.MakeHediff(power, pawn) is Hediff_Power hediff))
            {
                Log.Error("Failed to find any power to give to " + pawn.LabelCap);
                return;
            }

            hediff.Level = Rand.Range(0.1f, hediff.def.maxSeverity);
            pawn.health.AddHediff(hediff);
            if (PawnUtility.ShouldSendNotificationAbout(pawn))
            {
                Messages.Message(
                    pawn.LabelCap + " has had their latent x-gene activated due to extreme stress or pain.",
                    pawn, MessageTypeDefOf.PositiveEvent);
            }

            if (Rand.Chance(0.01f))
            {
                var secondPower = powers.RandomElement();
                while (secondPower == power) secondPower = powers.RandomElement();
                hediff = HediffMaker.MakeHediff(secondPower, pawn) as Hediff_Power;
                if (hediff == null)
                {
                    Log.Error("Failed to find any power to give to " + pawn.LabelCap);
                    return;
                }

                hediff.Level = Rand.Range(0f, hediff.def.maxSeverity);
                pawn.health.AddHediff(hediff);
                if (PawnUtility.ShouldSendNotificationAbout(pawn))
                {
                    Messages.Message(pawn.LabelCap + " has developed a secondary mutation!", pawn,
                        MessageTypeDefOf.PositiveEvent);
                }
            }
        }
    }
}