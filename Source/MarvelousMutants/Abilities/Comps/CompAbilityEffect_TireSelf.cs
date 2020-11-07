using RimWorld;
using Verse;

namespace MarvelousMutants.Abilities.Comps
{
    public class CompAbilityEffect_TireSelf : CompAbilityEffect
    {
        private new AbilityCompProperties_TireSelf Props => props as AbilityCompProperties_TireSelf;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            if (parent?.pawn?.needs?.rest != null)
                parent.pawn.needs.rest.CurLevel -= Props.TireUsage;
        }

        public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
        {
            if (!base.Valid(target, throwMessages))
            {
                return false;
            }

            if (parent?.pawn?.needs?.rest.CurLevel <= Need_Rest.ThreshVeryTired)
            {
                if (throwMessages)
                    Messages.Message("Too tired to teleport", parent?.pawn, MessageTypeDefOf.RejectInput);
                return false;
            }
            
            return true;
        }
    }
}