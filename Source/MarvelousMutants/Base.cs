using Verse;

namespace MarvelousMutants
{
    public class Base : Mod
    {
        public Base(ModContentPack content) : base(content)
        {
            var harm = new HarmonyLib.Harmony("legodude17.marvelous.mutants");
            harm.PatchAll();
            Log.Message("Applied patches for " + harm.Id);
        }
    }
}