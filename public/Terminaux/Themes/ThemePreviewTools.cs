//
// Terminaux  Copyright (C) 2023-2026  Aptivi
//
// This file is part of Terminaux
//
// Terminaux is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Terminaux is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System.Collections.Generic;
using System.Linq;
using Terminaux.Inputs.Styles.Selection;
using Terminaux.Inputs.Styles;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Colorimetry;

namespace Terminaux.Themes
{
    /// <summary>
    /// Theme preview tools (simple preview and wheel-based preview)
    /// </summary>
    public static class ThemePreviewTools
    {
        /// <summary>
        /// Prepares the preview of the theme (wheel-based)
        /// </summary>
        /// <param name="theme">Theme name</param>
        public static void PreviewTheme(string theme) =>
            PreviewTheme(ThemeTools.GetThemeInfo(theme));

        /// <summary>
        /// Prepares the preview of the theme (wheel-based)
        /// </summary>
        /// <param name="theme">Theme instance</param>
        public static void PreviewTheme(ThemeInfo theme) =>
            PreviewTheme(ThemeTools.GetColorsFromTheme(theme), theme);

        /// <summary>
        /// Prepares the preview of the theme (wheel-based)
        /// </summary>
        /// <param name="colors">Dictionary of colors</param>
        public static void PreviewTheme(Dictionary<string, Color> colors) =>
            PreviewTheme(colors, null);

        /// <summary>
        /// Prepares the preview of the theme (wheel-based)
        /// </summary>
        /// <param name="colors">Dictionary of colors</param>
        /// <param name="theme">Theme instance</param>
        internal static void PreviewTheme(Dictionary<string, Color> colors, ThemeInfo? theme)
        {
            // Check to see if we're trying to preview theme on non-true color console
            if (ThemeTools.MinimumTypeRequired(colors, ColorType.TrueColor) && !ConsoleColoring.ConsoleSupportsTrueColor)
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLORS_THEMES_EXCEPTION_NEEDSTRUECOLOR"));

            // Render the choices
            var choices = new List<InputChoiceInfo>();
            for (int key = 0; key < colors.Count; key++)
            {
                var type = colors.Keys.ElementAt(key);
                var color = colors.Values.ElementAt(key);
                choices.Add(
                    new(type.ToString(), $"[{color.PlainSequence}]{color.VTSequenceForeground()} Lorem ipsum dolor sit amet, consectetur adipiscing elit.")
                );
            }

            // Alt choices for exiting
            var altChoices = new List<InputChoiceInfo>
            {
                new(LanguageTools.GetLocalized("T_INPUT_COMMON_KEYBINDING_EXIT"), LanguageTools.GetLocalized("T_COLORS_THEMES_EXITPREVIEW_DESC"))
            };

            // Give a prompt for theme preview
            while (true)
            {
                int prev = SelectionStyle.PromptSelection((theme is not null ? $"{theme.Name}: {LanguageTools.GetLocalized(theme.Description)}\n\n" : "") + LanguageTools.GetLocalized("T_COLORS_THEMES_THEMESHOWCASE"), [.. choices], [.. altChoices], true);
                if (prev == choices.Count)
                    break;
                else
                    ColorSelector.OpenColorSelector(colors.Values.ElementAt(prev), readOnly: true);
            }
        }
    }
}
