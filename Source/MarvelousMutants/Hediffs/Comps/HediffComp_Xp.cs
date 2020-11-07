using UnityEngine;
using Verse;

namespace MarvelousMutants.Hediffs.Comps
{
    public class HediffComp_Xp : HediffComp
    {
        private int xp;
        public int Level => Mathf.FloorToInt(xp / 100f);
        public void Notify_Used()
        {
            var lastLevel = Level;
            xp += Rand.Range(5, 30);
            if (Level > lastLevel && parent is IXpUser user)
            {
                user.Notify_LevelUp();
            }
        }

        public override string CompLabelInBracketsExtra => "Level " + Level + " (" + (xp % 100) + "xp/100)";

        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_Values.Look(ref xp, "xp");
        }

        public void SetLevel(int level)
        {
            int lastLevel = Level;
            xp = level * 100 + 1;
            if (lastLevel != level && parent is IXpUser user)
            {
                user.Notify_LevelUp();
            }
        }
    }
}