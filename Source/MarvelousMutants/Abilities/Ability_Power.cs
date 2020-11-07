using MarvelousMutants.Abilities.Comps;
using RimWorld;
using Verse;

namespace MarvelousMutants.Abilities
{
    public class Ability_Power : Ability
    {
        public int Cooldown { get; protected set; }

        public Ability_Power(Pawn pawn) : base(pawn)
        {
            
        }

        public Ability_Power(Pawn pawn, AbilityDef def) : base(pawn, def)
        {
            
        }

        private AbilityComp_Cooldown CooldownComp => CompOfType<AbilityComp_Cooldown>();

        public override void AbilityTick()
        {
            base.AbilityTick();
            if (Cooldown > 0)
            {
                Cooldown--;
            }

            foreach (var comp in comps)
            {
                if (comp is IEffectTick tickable)
                {
                    tickable.Tick();
                }
            }
        }

        public override bool Activate(LocalTargetInfo target, LocalTargetInfo dest)
        {
            if (CooldownComp != null)
            {
                Cooldown = CooldownComp.Props.Cooldown;
            }
            
            
            return base.Activate(target, dest);
        }

        public override Command GetGizmo()
        {
            return gizmo ?? (gizmo = new Command_Power(this));
        }
    }
}