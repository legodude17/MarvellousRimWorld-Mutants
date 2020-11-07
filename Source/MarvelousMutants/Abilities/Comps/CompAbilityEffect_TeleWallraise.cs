using System.Collections.Generic;
using RimWorld;
using Verse;

namespace MarvelousMutants.Abilities.Comps
{
    public class CompAbilityEffect_TeleWallraise : CompAbilityEffect_Wallraise, IEffectTick
    {
        private int wallsLeft = 50;
        
        private readonly List<int> walls = new List<int>();
        
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            wallsLeft--;
            walls.Add(10000);
        }

        public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
        {
            if (!base.Valid(target, throwMessages))
            {
                return false;
            }

            if (wallsLeft <= 0)
            {
                if (throwMessages)
                    Messages.Message("No walls left.", parent.pawn, MessageTypeDefOf.RejectInput);
                return false;
            }

            return true;
        }

        public void Tick()
        {
            for (var i = 0; i < walls.Count; i++)
            {
                walls[i]--;
                if (walls[i] <= 0)
                {
                    wallsLeft++;
                    walls.RemoveAt(i);
                }
            }
        }
    }
}