using System;
using MarvelousMutants.Hediffs;
using MarvelousMutants.Hediffs.Comps;
using RimWorld;
using Verse;

namespace MarvelousMutants.Abilities
{
    public class Power_Telekinetic : Ability_Power
    {
        public Power_Telekinetic(Pawn pawn) : base(pawn)
        {
            
        }

        public Power_Telekinetic(Pawn pawn, AbilityDef def) : base(pawn, def)
        {
            
        }

        private Hediff_Telekine Telekine => pawn.health.hediffSet.GetHediffs<Hediff_Telekine>().FirstOrFallback();
        private int Level => Telekine?.TryGetComp<HediffComp_Xp>().Level ?? 0;

        public override bool Activate(LocalTargetInfo target, LocalTargetInfo dest)
        {
            Telekine?.TryGetComp<HediffComp_Xp>()?.Notify_Used();
            Cooldown = (int) (2 * Math.Max(1, 5 - Level) * ((Telekine?.Severity ?? 0.1) > 1 ? 0.1 : 10) * def.level * 60);
            return base.Activate(target, dest);
        }
    }
}