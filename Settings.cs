using System.Collections.Generic;
using UnityModManagerNet;

namespace ResearchPlanner
{
    // ReSharper disable InconsistentNaming
    public class Settings : UnityModManager.ModSettings
    {
        public bool Enabled = true;
        public int SelectedTab = 0;
        public List<string> Projects = new List<string>();
        public List<string> GlobalTechs = new List<string>();

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }
}
