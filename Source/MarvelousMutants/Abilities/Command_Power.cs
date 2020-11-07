using RimWorld;
using UnityEngine;
using Verse;
using Verse.Noise;
using Verse.Sound;

namespace MarvelousMutants.Abilities
{
    public class Command_Power : Command
    {
        public static readonly Texture2D BgTex = ContentFinder<Texture2D>.Get("UI/Widgets/AbilityButBG");
        private readonly Ability ability;

        public override Texture2D BGTexture => Command_Ability.BGTex;

        public Command_Power(Ability ability)
        {
            this.ability = ability;
            order = 5f;
            defaultLabel = ability.def.LabelCap;
            hotKey = ability.def.hotKey;
            icon = ability.def.uiIcon;
        }
        
        public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth)
        {
            defaultDesc = ability.def.GetTooltip();
            if (ability is Ability_Power power)
            {
                if (power.Cooldown > 0)
                {
                    disabled = true;
                    disabledReason = "Still on cooldown: " + power.Cooldown / 60f + "s";
                }
                else
                {
                    disabled = false;
                }
            }
            var gizmoResult = base.GizmoOnGUI(topLeft, maxWidth);
            if (gizmoResult.State == GizmoState.Interacted && ability.CanCast)
                return gizmoResult;
            return new GizmoResult(gizmoResult.State);
        }

        public override void ProcessInput(Event ev)
        {
            base.ProcessInput(ev);
            SoundDefOf.Tick_Tiny.PlayOneShotOnCamera();
            Find.Targeter.BeginTargeting(ability.verb);
        }

        public override void GizmoUpdateOnMouseover()
        {
            if (!(ability.verb is Verb_CastAbility verb))
                return;
            verb.DrawRadius();
        }
    }
}