using UnityEngine;
using UnityModManagerNet;

namespace ResearchPlanner
{
    // ReSharper disable InconsistentNaming
    public static class UIStyles
    {

        public static GUIStyle MainRegion = new GUIStyle(GUI.skin.label)
        {
            padding = new RectOffset(20, 20, 20, 20)
        };

        public static GUIStyle TabContent = new GUIStyle(GUI.skin.box)
        {
            padding = new RectOffset(10, 10, 10, 10)
        };
    }
}