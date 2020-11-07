using RimWorld;

namespace MarvelousMutants.Abilities.Comps
{
    public class AbilityCompProperties_TireSelf : CompProperties_AbilityEffect
    {
        public float TireUsage;
        public AbilityCompProperties_TireSelf()
        {
            compClass = typeof(CompAbilityEffect_TireSelf);
        }
    }
}