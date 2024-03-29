using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI.Group;

namespace MarvelousMutants.Abilities.Comps
{
    public class CompAbilityEffect_MindControl : CompAbilityEffect_WithDuration, IEffectTick
    {
        public static List<CompAbilityEffect_MindControl> instances = new List<CompAbilityEffect_MindControl>();
        private Pawn currentTarget;
        private Faction oldFaction;
        private Lord oldLord;
        private int ticksLeft;

        public MindControlInfo Info =>
            new MindControlInfo
            {
                oldFaction = oldFaction,
                pawn = currentTarget
            };

        public void Tick()
        {
            if (currentTarget == null) return;
            ticksLeft--;

            if (currentTarget.Dead || !currentTarget.Spawned || currentTarget.Downed) ticksLeft = 0;
            if (ticksLeft > 0) return;
            ticksLeft = -1;
            currentTarget.SetFaction(oldFaction);
            if (!currentTarget.Dead && currentTarget.Spawned) oldLord.AddPawn(currentTarget);
            currentTarget = null;
        }

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            var toFaction = parent.pawn.Faction;
            var pawn = target.Pawn;
            oldFaction = pawn.Faction;
            oldLord = pawn.GetLord();
            pawn.SetFaction(toFaction);
            ticksLeft = GetDurationSeconds(pawn).SecondsToTicks();
            currentTarget = pawn;
            pawn.drafter.Drafted = true;
        }

        public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
        {
            if (!base.Valid(target, throwMessages)) return false;

            if (target.Pawn.Faction == parent.pawn.Faction)
            {
                if (throwMessages)
                    Messages.Message("Cannot mind control friend", parent.pawn, MessageTypeDefOf.RejectInput);
                return false;
            }

            if (!target.Pawn.RaceProps.Humanlike)
            {
                if (throwMessages) Messages.Message("Must target humanlike", parent.pawn, MessageTypeDefOf.RejectInput);
                return false;
            }

            if (currentTarget != null)
            {
                if (throwMessages)
                    Messages.Message("Already mind controlling " + currentTarget.LabelCap, parent.pawn,
                        MessageTypeDefOf.RejectInput);
                return false;
            }

            return true;
        }

        public override void Initialize(AbilityCompProperties props)
        {
            base.Initialize(props);
            instances.Add(this);
        }

        public struct MindControlInfo
        {
            public Faction oldFaction;
            public Pawn pawn;
        }
    }
}