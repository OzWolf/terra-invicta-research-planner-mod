using System.Collections.Generic;
using UnityEngine;

namespace ResearchPlanner
{
    // ReSharper disable InconsistentNaming
    public static class UIImages
    {
        private static Sprite? _FinderIcon;

        public static Sprite FinderIcon
        {
            get
            {
                _FinderIcon ??= GameControl.assetLoader.LoadAssetForSpriteAssignment("icons_2d/ICO_finder");
                return _FinderIcon;
            }
        }

        private static Sprite? _ProjectIcon;

        public static Sprite ProjectIcon
        {
            get
            {
                _ProjectIcon ??= GameControl.assetLoader.LoadAssetForSpriteAssignment("icons_2d/ICO_Projects");
                return _ProjectIcon;
            }
        }

        private static Sprite? _TechIcon;

        public static Sprite TechIcon
        {
            get
            {
                _TechIcon ??= GameControl.assetLoader.LoadAssetForSpriteAssignment("icons_2d/ICO_Research");
                return _TechIcon;
            }
        }

        private static Sprite? _CheckIcon;

        public static Sprite CheckIcon
        {
            get
            {
                _CheckIcon ??= GameControl.assetLoader.LoadAssetForSpriteAssignment("research_planner/CheckMark");
                return _CheckIcon;
            }
        }

        private static Sprite? _XIcon;

        public static Sprite XIcon
        {
            get
            {
                _XIcon ??= GameControl.assetLoader.LoadAssetForSpriteAssignment("research_planner/Cross");
                return _XIcon;
            }
        }

        private static Sprite? _PadlockIcon;

        public static Sprite PadlockIcon
        {
            get
            {
                _PadlockIcon ??= GameControl.assetLoader.LoadAssetForSpriteAssignment("research_planner/Padlock");
                return _PadlockIcon;
            }
        }

        private static Sprite? _HourGlassIcon;

        public static Sprite HourGlassIcon
        {
            get
            {
                _HourGlassIcon ??= GameControl.assetLoader.LoadAssetForSpriteAssignment("research_planner/HourGlass");
                return _HourGlassIcon;
            }
        }

        private static readonly Dictionary<TechCategory, string> TechCategoryResources =
            new Dictionary<TechCategory, string>()
            {
                { TechCategory.Energy, "icons_2d/tech_energy_icon" },
                { TechCategory.InformationScience, "icons_2d/tech_info_icon" },
                { TechCategory.LifeScience, "icons_2d/tech_life_icon" },
                { TechCategory.Materials, "icons_2d/tech_material_icon" },
                { TechCategory.MilitaryScience, "icons_2d/tech_military_icon" },
                { TechCategory.SocialScience, "icons_2d/tech_social_icon" },
                { TechCategory.SpaceScience, "icons_2d/tech_space_icon" },
                { TechCategory.Xenology, "icons_2d/tech_xeno_icon" }
            };

        private static readonly Dictionary<TechCategory, Sprite> TechCategorySprites = new Dictionary<TechCategory, Sprite>();

        public static Sprite TechCategoryIcon(TechCategory category)
        {
            if (!TechCategorySprites.ContainsKey(category))
                TechCategorySprites[category] = GameControl.assetLoader.LoadAssetForSpriteAssignment(TechCategoryResources[category]);

            return TechCategorySprites[category];
        }

        public static string HourGlassGlyph = "◴";
    }
}
