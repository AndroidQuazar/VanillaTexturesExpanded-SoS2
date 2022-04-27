using HarmonyLib;
using RimWorld;
using Verse;

namespace VTESoS
{
    public class VTESoS2Mod : Mod
    {
        public static Harmony Harm;

        public VTESoS2Mod(ModContentPack content) : base(content)
        {
            Harm = new Harmony("vanillaexpanded.vtexe.sos2");
            Harm.Patch(AccessTools.Method(typeof(CompShuttleCosmetics), nameof(CompShuttleCosmetics.ChangeShipGraphics)),
                new HarmonyMethod(GetType(), nameof(ChangeShipGraphics_Prefix)));
        }

        public static bool ChangeShipGraphics_Prefix(ThingWithComps parent, CompProperties_ShuttleCosmetics Props, bool triggeredByChange = false)
        {
            if (triggeredByChange) parent.GetComp<CompShuttleCosmetics>().whichVersion = FloatMenuWithCallback.whichOptionWasChosen;

            var whichVersion = parent.GetComp<CompShuttleCosmetics>().whichVersion;
            if (parent is Pawn pawn)
            {
                if (!pawn.Drawer.renderer.graphics.AllResolved) pawn.Drawer.renderer.graphics.ResolveAllGraphics();
                pawn.Drawer.renderer.graphics.nakedGraphic = Props.graphicsHover[whichVersion].Graphic;
                pawn.Drawer.renderer.graphics.ClearCache();
            }
            else
            {
                parent.graphicInt = Props.graphics[whichVersion].Graphic;
                parent.DirtyMapMesh(parent.Map);
            }

            return false;
        }
    }
}