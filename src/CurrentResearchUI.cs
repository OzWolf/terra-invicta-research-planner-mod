using ModKit;
using PavonisInteractive.TerraInvicta;

namespace ResearchPlanner
{
    // ReSharper disable InconsistentNaming
    public static class CurrentResearchUI
    {
        private static TIFactionState? Faction;

        public static void OnUpdate()
        {
            Faction = GameControl.control.activePlayer;
        }

        public static void OnGUI()
        {
            UI.Space(10);
            using (UI.VerticalScope())
            {
                UIElements.Header("CURRENT RESEARCH".orange().bold(), 500);
                UI.Space(10);
                using (UI.HorizontalScope())
                {
                    UIElements.Label("RESEARCH TYPE".bold(), 208);
                    UI.Space(10);
                    UIElements.Label("SLOT 1".bold(), 400);
                    UI.Space(10);
                    UIElements.Label("SLOT 2".bold(), 400);
                    UI.Space(14);
                    UIElements.Label("SLOT 3".bold(), 400);
                    UI.Space(10);
                }

                UI.Space(10);
                UI.Div();
                UI.Space(10);

                using (UI.HorizontalScope())
                {
                    UIElements.Label("GLOBAL TECH".bold());
                    UI.Space(10);
                    DrawGlobalTechSlot(0);
                    UI.Space(10);
                    DrawGlobalTechSlot(1);
                    UI.Space(10);
                    DrawGlobalTechSlot(2);
                }

                UI.Space(10);

                using (UI.HorizontalScope())
                {
                    UIElements.Label("PROJECTS".bold());
                    UI.Space(10);
                    DrawProjectSlot(3);
                    UI.Space(10);
                    DrawProjectSlot(4);
                    UI.Space(10);
                    DrawProjectSlot(5);
                }
            }
        }

        private static void DrawGlobalTechSlot(int slot)
        {
            var project = TIGlobalResearchState.globalResearch.GetTechProgress(slot);

            if (TIGlobalResearchState.FinishedTechs().Contains(project.techTemplate))
            {
                UIElements.IconLabel(project.GetExpectedWinner().displayNameCapitalizedWithColor.bold().ToUpper(), project.GetExpectedWinner().factionIcon64UI, 400);
            }
            else
                UIElements.IconLabel(project.techTemplate.displayName.ToUpper(), UIImages.TechCategoryIcon(project.techTemplate.techCategory), 400);
        }

        private static void DrawProjectSlot(int slot)
        {
            var project = Faction!.GetProjectInSlot(slot);
            if (project == null)
                UIElements.Label("", 400);
            else
                UIElements.IconLabel(project.displayName.ToUpper(), UIImages.TechCategoryIcon(project.techCategory), 400);
        }
    }
}