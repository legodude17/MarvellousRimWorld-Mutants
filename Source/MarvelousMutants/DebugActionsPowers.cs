using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MarvelousMutants.Hediffs;
using MarvelousMutants.Hediffs.Comps;
using UnityEngine;
using Verse;

namespace MarvelousMutants
{
    public static class DebugActionsPowers
    {
        [DebugAction("General", "Make Super Pawn", allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void SuperPawn()
        { 
            Find.WindowStack.Add(new Dialog_DebugOptionListLister(new List<DebugMenuOption>{new DebugMenuOption("All powers", DebugMenuOptionMode.Tool, DoSuperPawn)}));
        }

        private static void DoSuperPawn()
        {
            var health = UI.MouseCell().GetFirstPawn(Find.CurrentMap)?.health;
            if (health == null) return;
            if (health.AddHediff(HediffDef.Named("HealingFactor")) is Hediff_HealingFactor hediff1)
            {
                hediff1.Level = 6;
            }

            if (health.AddHediff(HediffDef.Named("Telepath")) is Hediff_Telepath hediff2)
            {
                hediff2.Level = 1.1f;
                hediff2.TryGetComp<HediffComp_Xp>()?.SetLevel(10);
            }
            
            if (health.AddHediff(HediffDef.Named("Telekine")) is Hediff_Telekine hediff3)
            {
                hediff3.Level = 1.1f;
                hediff3.TryGetComp<HediffComp_Xp>()?.SetLevel(10);
            }

            health.AddHediff(HediffDef.Named("Teleporter"));
        }

        [DebugAction("General", "Give super power", allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void GivePower()
        {
            var debugMenuOptionList = DefDatabase<HediffDef>.AllDefs
                .Where(def => def.hediffClass == typeof(Hediff_Power) || def.hediffClass.IsSubclassOf(typeof(Hediff_Power)))
                .Select(def => new DebugMenuOption(def.LabelCap, DebugMenuOptionMode.Action, () => DoPowerLevel(def))).ToList();
            Find.WindowStack.Add(new Dialog_DebugOptionListLister(debugMenuOptionList));
        }

        private static void DoPowerLevel(HediffDef def)
        {
            var optionList = new List<DebugMenuOption>();
            var flag = def.comps.Any(comp => comp.compClass == typeof(HediffComp_Xp));
            var type = flag ? DebugMenuOptionMode.Action : DebugMenuOptionMode.Tool;
            for (float i = 0; i <= def.maxSeverity; i++)
            {
                var i1 = i;
                optionList.Add(new DebugMenuOption(i.ToString(CultureInfo.CurrentCulture), type,
                    () =>
                    {
                        if (flag)
                            DoXp(i1, def);
                        else
                            DoPower(def, i1, -1);
                    }));
            }

            if (Math.Abs(Mathf.Round(def.maxSeverity) - def.maxSeverity) > 0.0001)
            {
                optionList.Add(new DebugMenuOption(def.maxSeverity.ToString(CultureInfo.CurrentCulture), type, () => 
                {
                    if (flag)
                        DoXp(def.maxSeverity, def);
                    else
                        DoPower(def, def.maxSeverity, -1);
                }));
            }
            
            Find.WindowStack.Add(new Dialog_DebugOptionListLister(optionList));
        }

        private static void DoXp(float severity, HediffDef def)
        {
            var optionList = new List<DebugMenuOption>();
            for (var i = 0; i < 10; i++)
            {
                var i1 = i;
                optionList.Add(new DebugMenuOption(i.ToString(), DebugMenuOptionMode.Tool, () => DoPower(def, severity, i1)));
            }
            
            Find.WindowStack.Add(new Dialog_DebugOptionListLister(optionList));
        }

        private static void DoPower(HediffDef def, float severity, int level)
        {
            var health = UI.MouseCell().GetFirstPawn(Find.CurrentMap)?.health;
            if (health == null) return;
            if (!(health.AddHediff(def) is Hediff_Power power)) return;
            power.Level = severity;
            if (level >= 0)
            {
                power.TryGetComp<HediffComp_Xp>()?.SetLevel(level);
            }
        }
    }
}