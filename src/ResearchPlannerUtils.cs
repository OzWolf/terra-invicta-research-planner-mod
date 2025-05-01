using System;
using System.Collections.Generic;
using System.Text;
using PavonisInteractive.TerraInvicta;

namespace ResearchPlanner
{
    public static class ResearchPlannerUtils
    {
        public static int GetOpenGlobalTechSlotFor(TIFactionState faction)
        {
            for (var i = 0; i <= 2; i++)
            {
                var project = TIGlobalResearchState.globalResearch.GetTechProgress(i);
                if (TIGlobalResearchState.FinishedTechs().Contains(project.techTemplate) &&
                    project.GetExpectedWinner() == faction) return i;
            }

            return -1;
        }
    }
}
