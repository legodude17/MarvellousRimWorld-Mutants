using Verse;

namespace MarvelousMutants.Abilities.Comps
{
    public class AbilityCompProperties_Cooldown : AbilityCompProperties
    {
        public int Cooldown;

        public AbilityCompProperties_Cooldown()
        {
            compClass = typeof(AbilityComp_Cooldown);
        }
    }
}