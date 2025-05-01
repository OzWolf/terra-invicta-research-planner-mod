using System.Linq;
using HarmonyLib;
using PavonisInteractive.TerraInvicta;

namespace ResearchPlanner
{
    [HarmonyPatch(typeof(GameStateManager))]
    public static class GameStateManagerPatch
    {
        [HarmonyPatch(nameof(GameStateManager.LoadAllGameStates))]
        [HarmonyPostfix]
        public static void LoadAllGameStatesPatch()
        {
            Main.GlobalTechs = Main.ModSettings.GlobalTechs
                .Select(name => (TIGenericTechTemplate)TemplateManager.Find<TITechTemplate>(name))
                .ToList();

            Main.Projects = Main.ModSettings.Projects
                .Select(name => (TIGenericTechTemplate)TemplateManager.Find<TIProjectTemplate>(name))
                .ToList();
        }
    }
}