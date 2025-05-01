using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using ModKit;
using PavonisInteractive.TerraInvicta;
using UnityEngine;
using UnityModManagerNet;
using static UnityModManagerNet.UnityModManager;
using UI = ModKit.UI;

namespace ResearchPlanner
{
    // [EnableReloading]
    // ReSharper disable InconsistentNaming
    public class Main
    {
        public static Settings ModSettings = new Settings();
        private static ModEntry? Entry;

        private static bool FirstOnGUI = true;

        public static List<TIGenericTechTemplate> GlobalTechs = new List<TIGenericTechTemplate>();
        public static List<TIGenericTechTemplate> Projects = new List<TIGenericTechTemplate>();

        private static readonly TabDefinition[] Tabs =
        {
            new TabDefinition("GLOBAL TECHS", ResearchType.GlobalTech, () => ResearchPlanningUI.OnGUI(ResearchType.GlobalTech)),
            new TabDefinition("PROJECTS", ResearchType.Projects, () => ResearchPlanningUI.OnGUI(ResearchType.Projects))
        };

        public static bool Load(ModEntry entry)
        {
            Entry = entry;
            var harmony = new Harmony(entry.Info.Id);
            harmony.PatchAll();

            ModSettings = UnityModManager.ModSettings.Load<Settings>(entry);

            entry.OnSaveGUI = OnSaveGUI;
            entry.OnGUI = OnGUI;
            entry.OnToggle = OnToggle;

            entry.Hotkey = new KeyBinding();
            entry.Hotkey.Change(KeyCode.R, true, false, false);
            return true;
        }

        public static bool OnToggle(ModEntry entry, bool value)
        {
            ModSettings.Enabled = value;
            return true;
        }

        public static void AddTrackedResearch(TIGenericTechTemplate template)
        {
            if (template.isProject())
            {
                Projects.AddUnique(template);
                ModSettings.Projects.AddUnique(template.dataName);
            }
            else
            {
                GlobalTechs.AddUnique(template);
                ModSettings.GlobalTechs.AddUnique(template.dataName);
            }

            ModSettings.Save(Entry!);
        }

        public static void RemoveTrackedResearch(TIGenericTechTemplate template)
        {
            if (template.isProject())
            {
                Projects.Remove(template);
                ModSettings.Projects.Remove(template.dataName);
            }
            else
            {
                GlobalTechs.Remove(template);
                ModSettings.GlobalTechs.Remove(template.dataName);
            }

            ModSettings.Save(Entry!);
        }

        public static void OnGUI(ModEntry modEntry)
        {
            if (!ModSettings.Enabled || !IsInGame) return;

            if (FirstOnGUI && GameStateManager.IsValid())
            {
                ResearchPlanningUI.OnUpdate();
                CurrentResearchUI.OnUpdate();
                FirstOnGUI = false;
            }
            
            using (UI.VerticalScope(UIStyles.MainRegion))
            {
                var selected = ModSettings.SelectedTab;
                if (selected >= Tabs.Length)
                    selected = 1;

                CurrentResearchUI.OnGUI();

                UI.Space(20);
                UI.Div();
                UI.Space(10);

                using (UI.HorizontalScope())
                {
                    for (var index = 0; index < Tabs.Length; index++) DrawTab(index);
                }
                UI.Space(10);

                GUILayout.BeginVertical(UIStyles.TabContent);
                UI.Space(10);
                Tabs[selected].action();
                GUILayout.EndVertical();
            }
        }

        public static void OnSaveGUI(ModEntry entry)
        {
            ModSettings.Save(entry);
        }

        private static void DrawTab(int index)
        {
            var tab = Tabs[index];
            var icon = tab.Icon;

            var isSelected = index == ModSettings.SelectedTab;
            var label = isSelected ? tab.name.orange().bold() : tab.name.bold();

            UIElements.TabButton(label, icon, () => { ModSettings.SelectedTab = index; });
        }

        private static bool IsInGame => GameStateManager.IsValid();

        private class TabDefinition
        {
            public readonly string name;
            private readonly ResearchType type;
            public readonly Action action;

            public Sprite Icon => type == ResearchType.Projects ? UIImages.ProjectIcon : UIImages.TechIcon;

            public TabDefinition(string name, ResearchType type, Action action)
            {
                this.name = name;
                this.type = type;
                this.action = action;
            }
        }
    }
}
