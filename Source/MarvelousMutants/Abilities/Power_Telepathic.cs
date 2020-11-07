using System;
using MarvelousMutants.Hediffs;
using MarvelousMutants.Hediffs.Comps;
using RimWorld;
using Verse;

namespace MarvelousMutants.Abilities
{
    public class Power_Telepathic : Ability_Power
    {
        public Power_Telepathic(Pawn pawn) : base(pawn)
        {
            
        }

        public Power_Telepathic(Pawn pawn, AbilityDef def) : base(pawn, def)
        {
            
        }

        private Hediff_Telepath Telepath => pawn.health.hediffSet.GetHediffs<Hediff_Telepath>().FirstOrFallback();
        private int Level => Telepath?.TryGetComp<HediffComp_Xp>().Level ?? 0;

        public override bool Activate(LocalTargetInfo target, LocalTargetInfo dest)
        {
            Telepath?.TryGetComp<HediffComp_Xp>()?.Notify_Used();
            Cooldown = (int) (2 * Math.Max(1, 5 - Level) * ((Telepath?.Severity ?? 0.1) > 1 ? 0.1 : 10) * def.level * 60);
            return base.Activate(target, dest);
        }
    }
}