using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace MarvelousMutants.Hediffs
{
    public class Hediff_HealingFactor : Hediff_Power
    {
        public override void Tick()
        {
            base.Tick();
            var toRemove = new List<Hediff>();
            var toAdd = new List<Hediff>();
            foreach (var hediff in pawn.health.hediffSet.hediffs)
            {
                if (hediff.TendableNow() && Find.TickManager.TicksGame % 100 == 0)
                {
                    hediff.Tended((Severity * 15.1594912957f + 9.30496195142f) / 100f);
                }

                if (hediff is Hediff_Injury && !hediff.IsPermanent())
                {
                    hediff.Heal(Severity / 1000);
                }

                if (hediff is Hediff_MissingPart && !pawn.health.hediffSet.PartIsMissing(hediff.Part.parent) &&
                    !pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(hediff.Part))
                {
                    var part = hediff.Part;
                    var flag = true;
                    while (part != null)
                    {
                        if (pawn.health.hediffSet.hediffs.Where(hd => hd.Part == part).Any(hd => hd.def == MM_DefOf.RegrowingPart))
                        {
                            flag = false;
                            part = null;
                        }
                        else
                        {
                            part = part.parent;
                        }
                    }

                    if (flag)
                    {
                        var newHediff = HediffMaker.MakeHediff(MM_DefOf.RegrowingPart, pawn, hediff.Part);
                        newHediff.Severity = 0.1f;
                        toAdd.Add(newHediff);
                        toRemove.Add(hediff);
                    }
                }

                if (hediff.def == MM_DefOf.RegrowingPart)
                {
                    hediff.Severity += Severity / 60000f;
                    if (hediff.Severity >= 1f)
                    {
                        toRemove.Add(hediff);
                    }
                }
            }

            foreach (var hediff in toRemove)
            {
                pawn.health.RemoveHediff(hediff);
            }

            foreach (var hediff in toAdd)
            {
                pawn.health.AddHediff(hediff);
            }

            var comp = this.TryGetComp<HediffComp_HealPermanentWounds>();

            if (comp != null)
            {
                float severityAdjustment = 0;
                for (var i = 0; i < 25 * Severity; i++)
                {
                    comp.CompPostTick(ref severityAdjustment);
                }
            }
        }

        public override bool PreventsDeath => Level >= 3f;
    }
}