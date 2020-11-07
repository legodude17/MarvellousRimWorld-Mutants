using Verse;

namespace MarvelousMutants.Hediffs
{
    public class Hediff_Power : HediffWithComps
    {
        
        public float Level = -1f;
        public override void Tick()
        {
            base.Tick();
            if (Level > 0.0f)
            {
                Severity = Level;
            }
        }

        public virtual bool PreventsDeath => false;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref Level, "Level");
        }
    }
}