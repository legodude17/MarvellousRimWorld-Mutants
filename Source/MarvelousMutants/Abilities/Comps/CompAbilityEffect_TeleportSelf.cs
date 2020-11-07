using RimWorld;
using Verse;

namespace MarvelousMutants.Abilities.Comps
{
    public class CompAbilityEffect_TeleportSelf : CompAbilityEffect
    {
        private new CompProperties_AbilityTeleport Props => props as CompProperties_AbilityTeleport;
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            if (!target.IsValid)
                return;
            var pawn = parent.pawn;
            var drawPos = pawn.DrawPos;
            pawn.Position = target.Cell;
            pawn.stances.stunner.StunFor(Props.stunTicks.RandomInRange, parent.pawn, false);
            pawn.Notify_Teleported();
            if (Props.destClamorType != null)
                GenClamor.DoClamor(pawn, target.Cell, Props.destClamorRadius, Props.destClamorType);
            MoteMaker.MakeConnectingLine(drawPos, target.Cell.ToVector3(), ThingDefOf.Mote_PsycastSkipLine, pawn.Map);
            MoteMaker.MakeStaticMote(drawPos, pawn.Map, ThingDefOf.Mote_PsycastSkipEffectSource);
        }
    }
}