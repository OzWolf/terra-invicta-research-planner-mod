using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using ModKit;
using PavonisInteractive.TerraInvicta;

namespace ResearchPlanner
{
    // ReSharper disable InconsistentNaming
    public static class ResearchPlanningUI
    {
        private static TIFactionState? Faction;
        private static ResearchType Type = ResearchType.GlobalTech;

        private static bool ShowSearch;
        private static string SearchTerm = "";
        private static List<TIGenericTechTemplate>? SearchResults;

        private static List<TIGenericTechTemplate> Items = new List<TIGenericTechTemplate>();

        public static void OnUpdate()
        {
            Faction = GameControl.control.activePlayer;
        }

        public static void OnGUI(ResearchType type)
        {
            if (Faction == null) return;

            Type = type;

            Items = type == ResearchType.GlobalTech ? Main.GlobalTechs : Main.Projects;

            if (ShowSearch)
            {
                DrawSearch();
                return;
            }

            DrawPlanning();
        }

        private static void DrawPlanning()
        {
            using (UI.VerticalScope())
            {
                UI.Space(20);

                using (UI.HorizontalScope())
                {
                    UIElements.IconTextActionButton("ADD", UIImages.CheckIcon, () => { ShowSearch = true; });
                    UI.Space(40);
                    UIElements.IconLabel("= Unlocked", UIImages.CheckIcon, width: 150, iconSize: 24);
                    UI.Space(10);
                    UIElements.IconLabel("= Locked", UIImages.XIcon, width: 150, iconSize: 24);
                    UI.Space(10);
                    UIElements.IconLabel("= Researching", UIImages.HourGlassIcon, width: 170, iconSize: 24);
                }

                UI.Space(20);
                UI.Div();
                UI.Space(10);

                using (UI.HorizontalScope())
                {
                    UIElements.Label("RESEARCH".bold(), 510);
                    UI.Space(10);
                    UIElements.Label("TYPE".bold(), 160);
                    UI.Space(10);
                    UIElements.Icon(UIImages.PadlockIcon, 32);
                }

                UI.Space(10);
                UI.Div();
                UI.Space(10);

                if (Items.IsEmpty())
                {
                    UI.Label("No research being planned and tracked.");
                    UI.Space(10);
                    return;
                }

                DrawProjects();
            }
        }

        private static void DrawSearch()
        {
            using (UI.VerticalScope())
            {
                UIElements.Label("SEARCH AND ADD RESEARCH".bold().orange(), 400);
                UI.Space(20);
                using (UI.HorizontalScope())
                {
                    UIElements.IconTextField(ref SearchTerm, UIImages.ProjectIcon, 300);
                    UI.Space(10);
                    UIElements.IconTextActionButton("SEARCH".bold(), UIImages.FinderIcon, OnSearch, 150);
                    UI.Space(10);
                    UIElements.IconTextActionButton("CLOSE".bold(), UIImages.XIcon, OnClose, 150);
                }

                UI.Space(10);

                if (SearchResults == null) return;
                UI.Div();
                UI.Space(10);
                DrawSearchResults();
            }

            return;

            void OnSearch()
            {
                if (SearchTerm.IsEmpty()) return;

                if (SearchTerm.Length < 3) return;

                SearchResults = Type switch
                {
                    ResearchType.GlobalTech => TemplateManager.GetAllTemplates<TITechTemplate>()
                        .ToList()
                        .FindAll(t => t.displayName.IndexOf(SearchTerm, StringComparison.OrdinalIgnoreCase) > -1)
                        .FindAll(t => !TIGlobalResearchState.FinishedTechs().Contains(t))
                        .Select(t => (TIGenericTechTemplate)t)
                        .ToList(),
                    ResearchType.Projects => TemplateManager.GetAllTemplates<TIProjectTemplate>()
                        .ToList()
                        .FindAll(t => t.displayName.IndexOf(SearchTerm, StringComparison.OrdinalIgnoreCase) > -1)
                        .FindAll(t => !Faction!.completedProjects.Contains(t))
                        .Select(t => (TIGenericTechTemplate)t)
                        .ToList(),
                    _ => new List<TIGenericTechTemplate>()
                };
            }

            void OnClose()
            {
                SearchTerm = "";
                SearchResults = null;
                ShowSearch = false;
            }
        }

        private static void DrawSearchResults()
        {
            if (SearchResults == null) return;

            using (UI.VerticalScope())
            {
                using (UI.HorizontalScope())
                {
                    UIElements.IconActionButton(UIImages.XIcon, OnClose, 32);
                    UI.Space(10);
                    UIElements.Label("SEARCH RESULTS ...".orange().bold(), 300);
                }

                UI.Space(20);
                if (SearchResults == null || SearchResults.IsEmpty())
                {
                    UI.Space(10);
                    UI.Label("No search results found...".bold());
                    UI.Space(10);
                    return;
                }

                var chunks = SearchResults.Sort(t => t.displayName).Chunk(2);

                foreach (var chunk in chunks)
                {
                    using (UI.HorizontalScope())
                    {
                        foreach (var item in chunk)
                        {
                            var item1 = item;
                            UIElements.IconTextActionButton(item.displayName.ToUpper(),
                                UIImages.TechCategoryIcon(item.techCategory), () => { OnChooseResearch(item1); }, 700);
                            UI.Space(10);
                        }
                    }

                    UI.Space(10);
                }
            }

            return;

            void OnChooseResearch(TIGenericTechTemplate template)
            {
                Main.AddTrackedResearch(template);
                SearchResults = null;
                ShowSearch = false;
            }

            void OnClose()
            {
                SearchResults = null;
            }
        }

        private static void DrawProjects()
        {
            using (UI.VerticalScope())
            {
                foreach (var item in Items.ToList())
                {
                    using (UI.HorizontalScope())
                    {
                        UIElements.IconLabel(item.displayName.ToUpper().bold().orange(),
                            UIImages.TechCategoryIcon(item.techCategory), 720);
                        UI.Space(720);
                        UIElements.IconActionButton(UIImages.XIcon, () => OnStopTrackingProject(item), 32);
                    }

                    UI.Space(10);
                    DrawChildren(item, 1);
                    UI.Space(10);
                    UI.Div();
                    UI.Space(10);
                }
            }
        }

        private static void DrawChildren(TIGenericTechTemplate child, int level)
        {
            var spaces = level * 16;

            var unlocked = child.isProject()
                ? Faction!.availableProjects.Contains(child)
                : TIGlobalResearchState.UnlockedTechs.Contains(child);
            var researched = child.isProject()
                ? Faction!.completedProjects.Contains(child)
                : TIGlobalResearchState.FinishedTechs().Contains(child);
            var researching = child.isProject()
                ? Faction!.currentProjectProgress.Any(p => p.projectTemplate == child)
                : TIGlobalResearchState.CurrentResearchingTechs.Contains(child);

            if (researched) return;

            var unlockedGlyph = UIImages.XIcon;
            if (researching)
                unlockedGlyph = UIImages.HourGlassIcon;
            else if (unlocked)
                unlockedGlyph = UIImages.CheckIcon;

            var unlockChance = child.isProject() ? ((TIProjectTemplate)child).factionAvailableChance / 100.0f : 1.0f;
            var unlockChanceText = unlocked || child.isGlobalTech() ? "" : unlockChance.ToString("P0");

            var labelWidth = 522 - spaces;

            using (UI.HorizontalScope())
            {
                UI.Space(spaces);

                UIElements.IconLabel(child.displayName.ToUpper().bold(), UIImages.TechCategoryIcon(child.techCategory),
                    labelWidth);
                UI.Space(10);
                UIElements.Icon(child.isProject() ? UIImages.ProjectIcon : UIImages.TechIcon, 32);
                UI.Space(118);
                UIElements.Icon(unlockedGlyph);
                UI.Space(5);
                UIElements.Label(unlockChanceText, 105);

                if (child.isProject() && unlocked && !researching) DrawProjectOptions(child);

                if (child.isGlobalTech() && unlocked && !researching) DrawGlobalTechOptions(child);
            }

            UI.Space(15);

            foreach (var p in child.TechPrereqs) DrawChildren(p, level + 1);
        }

        private static void DrawProjectOptions(TIGenericTechTemplate child)
        {
            UI.Space(10);
            var freeSlot = ResearchPlannerUtils.GetOpenProjectSlotFor(Faction!);
            if (freeSlot > 0)
            {
                UI.Space(460);
                UIElements.IconTextActionButton("Research", UIImages.ProjectIcon, () => StartResearchingProject(child, freeSlot));
                return;
            }

            if (Faction!.ProjectAllowedInSlot(3))
                UIElements.IconTextActionButton("Slot 1", UIImages.ProjectIcon, () => StartResearchingProject(child, 3));
            else
                UIElements.Label(" ");

            UI.Space(10);

            if (Faction.ProjectAllowedInSlot(4))
                UIElements.IconTextActionButton("Slot 2", UIImages.ProjectIcon, () => StartResearchingProject(child, 4));
            else
                UIElements.Label(" ");

            UI.Space(10);

            if (Faction.ProjectAllowedInSlot(5))
                UIElements.IconTextActionButton("Slot 3", UIImages.ProjectIcon, () => StartResearchingProject(child, 5));
            else
                UIElements.Label(" ");
        }

        private static void DrawGlobalTechOptions(TIGenericTechTemplate child)
        {
            var slot = ResearchPlannerUtils.GetOpenGlobalTechSlotFor(Faction!);

            if (slot < 0) return;

            UI.Space(460);
            UIElements.IconTextActionButton("Research", UIImages.TechIcon, () => StartResearchGlobalTech(child, slot));
        }

        private static void StartResearchingProject(TIGenericTechTemplate project, int slot)
        {
            if (!project.isProject()) return;

            var p = (TIProjectTemplate)project;
            Faction!.SetProjectInSlot(slot, p);
            TIPromptQueueState.RemovePromptStatic(Faction, Faction, null, "PromptSelectProject", slot);
        }

        private static void StartResearchGlobalTech(TIGenericTechTemplate tech, int slot)
        {
            if (!tech.isGlobalTech()) return;

            var t = (TITechTemplate)tech;

            var gameState = GameStateManager.FindGameState<TIGlobalResearchState>();
            gameState.AssignNewTechToSlot(t, slot);
            TIPromptQueueState.RemovePromptStatic(Faction!, gameState, null, "PromptSelectTech", slot);
        }

        private static void OnStopTrackingProject(TIGenericTechTemplate template)
        {
            Main.RemoveTrackedResearch(template);
        }
    }
}