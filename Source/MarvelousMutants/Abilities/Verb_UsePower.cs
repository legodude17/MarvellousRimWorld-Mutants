using MarvelousMutants.Abilities.Comps;
using RimWorld;
using Verse;
using Verse.AI;

namespace MarvelousMutants.Abilities
{
    public class Verb_UsePower : Verb_CastAbility
    {
        public Ability_Power Power => ability as Ability_Power;

        public override bool IsApplicableTo(LocalTargetInfo target, bool showMessages = false)
        {
            if (!base.IsApplicableTo(target, showMessages))
            {
                return false;
            }

            if (Power.Cooldown > 0)
            {
                if (showMessages) Messages.Message("Ability still on cooldown", caster, MessageTypeDefOf.RejectInput);
                return false;
            }

            return true;
        }

        public override void OrderForceTarget(LocalTargetInfo target)
        {
            base.OrderForceTarget(target);
        }
    }
}