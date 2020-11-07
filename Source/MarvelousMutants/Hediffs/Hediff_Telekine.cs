using System.Linq;
using MarvelousMutants.Abilities;
using MarvelousMutants.Hediffs.Comps;
using RimWorld;
using Verse;

namespace MarvelousMutants.Hediffs
{
    public class Hediff_Telekine : Hediff_Power, IXpUser
    {
        public HediffComp_Xp CompXp => this.TryGetComp<HediffComp_Xp>();
        
        public void Notify_LevelUp()
        {
            AddAbilities();
        }

        public override void PostAdd(DamageInfo? dinfo)
        {
            base.PostAdd(dinfo);
            AddAbilities();
        }

        public override void PostRemoved()
        {
            base.PostRemoved();
            RemoveAbilities();
        }

        private void AddAbilities()
        {
            foreach (var ability in DefDatabase<AbilityDef>.AllDefs.Where(ab =>
                (ab.abilityClass == typeof(Power_Telekinetic) ||
                 ab.abilityClass.IsSubclassOf(typeof(Power_Telekinetic))) &&
                ab.level <= CompXp.Level))
            {
                if (pawn.abilities.abilities.Any((ab => ab.def == ability))) continue;
                pawn.abilities.GainAbility(ability);
            }
        }

        private void RemoveAbilities()
        {
            var toRemove = pawn.abilities.abilities.Where(ab => ab is Power_Telekinetic).ToList();
            foreach (var ability in toRemove)
            {
                pawn.abilities.abilities.Remove(ability);
            }
        }
    }
}