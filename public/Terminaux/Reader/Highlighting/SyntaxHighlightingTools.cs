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

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Terminaux.Base;
using Colorimetry;
using Colorimetry.Data;

namespace Terminaux.Reader.Highlighting
{
    /// <summary>
    /// Syntax highlighting tools
    /// </summary>
    public static class SyntaxHighlightingTools
    {
        private static readonly SyntaxHighlighting[] baseHighlighters =
        [
            new("Command", new Dictionary<string, SyntaxHighlightingComponent>()
            {
                { "CommandName", new(@"^[^ ]+", ConsoleColors.Yellow, Color.Empty, false, true) }
            })
        ];
        private static readonly List<SyntaxHighlighting> customHighlighters = [];

        /// <summary>
        /// All installed highlighters
        /// </summary>
        public static SyntaxHighlighting[] Highlighters =>
            baseHighlighters.Union(customHighlighters).ToArray();

        /// <summary>
        /// All installed highlighter names
        /// </summary>
        public static string[] HighlighterNames =>
            Highlighters.Select((ti) => ti.Name).ToArray();

        /// <summary>
        /// Checks to see if a highlighter exists or not
        /// </summary>
        /// <param name="highlighter">A specific highlighter to check</param>
        /// <returns>True if found; false otherwise.</returns>
        public static bool Exists(string highlighter)
        {
            if (string.IsNullOrWhiteSpace(highlighter))
                throw new TerminauxException(LanguageTools.GetLocalized("T_READER_HIGHLIGHT_TOOLS_EXCEPTION_NEEDSHIGHLIGHTERNAME"));
            return HighlighterNames.Contains(highlighter);
        }

        /// <summary>
        /// Checks to see if a highlighter is a built-in or not
        /// </summary>
        /// <param name="highlighter">A specific highlighter to check</param>
        /// <returns>True if found; false otherwise.</returns>
        public static bool ExistsBuiltin(string highlighter)
        {
            if (string.IsNullOrWhiteSpace(highlighter))
                throw new TerminauxException(LanguageTools.GetLocalized("T_READER_HIGHLIGHT_TOOLS_EXCEPTION_NEEDSBASEHIGHLIGHTERNAME"));
            return baseHighlighters.Any((highlight) => highlight.Name == highlighter);
        }

        /// <summary>
        /// Gets the default highlighter for the current read. Returns <see langword="null"/> if no reader is present.
        /// </summary>
        /// <exception cref="TerminauxException"></exception>
        public static SyntaxHighlighting? GetHighlighter() =>
            TermReaderState.CurrentState?.Settings.SyntaxHighlighter;

        /// <summary>
        /// Gets the highlighter
        /// </summary>
        /// <param name="highlighter">Highlighter name</param>
        /// <exception cref="TerminauxException"></exception>
        public static SyntaxHighlighting GetHighlighter(string highlighter)
        {
            // Check to see if we have this highlighter
            if (!Exists(highlighter))
                throw new TerminauxException(LanguageTools.GetLocalized("T_READER_HIGHLIGHT_TOOLS_EXCEPTION_HIGHLIGHTERNOTFOUND1"), highlighter);

            // Now, get the highlighter
            int idx = GetHighlighterIndexFrom(highlighter);
            return Highlighters[idx];
        }

        /// <summary>
        /// Gets the default highlighter
        /// </summary>
        /// <param name="componentName">Component name</param>
        /// <exception cref="TerminauxException"></exception>
        public static SyntaxHighlightingComponent GetComponent(string componentName) =>
            GetComponent(GetHighlighter(), componentName);

        /// <summary>
        /// Gets the default highlighter
        /// </summary>
        /// <param name="highlighter">Highlighter name</param>
        /// <param name="componentName">Component name</param>
        /// <exception cref="TerminauxException"></exception>
        public static SyntaxHighlightingComponent GetComponent(string highlighter, string componentName) =>
            GetComponent(GetHighlighter(highlighter), componentName);

        /// <summary>
        /// Gets the highlighter
        /// </summary>
        /// <param name="highlighter">Highlighter</param>
        /// <param name="componentName">Component name</param>
        /// <exception cref="TerminauxException"></exception>
        public static SyntaxHighlightingComponent GetComponent(SyntaxHighlighting? highlighter, string componentName)
        {
            // Check to see if we have this highlighter
            if (highlighter is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_READER_HIGHLIGHT_TOOLS_EXCEPTION_HIGHLIGHTERNOTFOUND2"));

            // Now, get the highlighter
            if (!highlighter.Components.TryGetValue(componentName, out SyntaxHighlightingComponent component))
                throw new TerminauxException(LanguageTools.GetLocalized("T_READER_HIGHLIGHT_TOOLS_EXCEPTION_COMPONENTNOTFOUND"), componentName, highlighter);
            return component;
        }

        /// <summary>
        /// Registers a highlighter
        /// </summary>
        /// <param name="highlighter">Highlighter information</param>
        public static void RegisterHighlighter(SyntaxHighlighting highlighter)
        {
            // Check the highlighter
            if (highlighter is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_READER_HIGHLIGHT_TOOLS_EXCEPTION_HIGHLIGHTERINVALID"));
            if (string.IsNullOrWhiteSpace(highlighter.Name))
                throw new TerminauxException(LanguageTools.GetLocalized("T_READER_HIGHLIGHT_TOOLS_EXCEPTION_HIGHLIGHTERNONAME"));
            if (Exists(highlighter.Name))
                throw new TerminauxException(LanguageTools.GetLocalized("T_READER_HIGHLIGHT_TOOLS_EXCEPTION_HIGHLIGHTEREXISTS"));
            if (highlighter.Components is null || highlighter.Components.Count == 0)
                throw new TerminauxException(LanguageTools.GetLocalized("T_READER_HIGHLIGHT_TOOLS_EXCEPTION_HIGHLIGHTERNOCOMPONENTS"));

            // Now, actually register the highlighter
            customHighlighters.Add(highlighter);
        }

        /// <summary>
        /// Unregisters a highlighter
        /// </summary>
        /// <param name="highlighter">Highlighter information</param>
        public static void UnregisterHighlighter(string highlighter)
        {
            // Check to see if we have this highlighter
            if (!Exists(highlighter))
                throw new TerminauxException(LanguageTools.GetLocalized("T_READER_HIGHLIGHT_TOOLS_EXCEPTION_HIGHLIGHTERNOTFOUND1"), highlighter);
            if (ExistsBuiltin(highlighter))
                throw new TerminauxException(LanguageTools.GetLocalized("T_READER_HIGHLIGHT_TOOLS_EXCEPTION_REMOVEBUILTINHIGHLIGHTER"), highlighter);

            // Now, remove the highlighter
            var SyntaxHighlighting = GetHighlighter(highlighter);
            customHighlighters.Remove(SyntaxHighlighting);
        }

        /// <summary>
        /// Gets a JSON representation of the highlighter
        /// </summary>
        /// <returns>A string containing the JSON representation of a highlighter</returns>
        /// <exception cref="TerminauxException"></exception>
        public static string GetHighlighterToJson() =>
            GetHighlighterToJson(GetHighlighter());

        /// <summary>
        /// Gets a JSON representation of the highlighter
        /// </summary>
        /// <param name="highlighter">Highlighter name to save to JSON</param>
        /// <returns>A string containing the JSON representation of a highlighter</returns>
        /// <exception cref="TerminauxException"></exception>
        public static string GetHighlighterToJson(string highlighter) =>
            GetHighlighterToJson(GetHighlighter(highlighter));

        /// <summary>
        /// Gets a JSON representation of the highlighter
        /// </summary>
        /// <param name="highlighter">Highlighter to save to JSON</param>
        /// <returns>A string containing the JSON representation of a highlighter</returns>
        /// <exception cref="TerminauxException"></exception>
        public static string GetHighlighterToJson(SyntaxHighlighting? highlighter)
        {
            // Check to see if we have this highlighter
            if (highlighter is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_READER_HIGHLIGHT_TOOLS_EXCEPTION_HIGHLIGHTERNOTFOUND2"));

            // Now, get the highlighter JSON
            return JsonConvert.SerializeObject(highlighter, Formatting.Indented);
        }

        /// <summary>
        /// Gets a highlighter from its JSON representation
        /// </summary>
        /// <param name="json">Highlighter JSON contents</param>
        /// <returns>A highlighter info containing JSON representation</returns>
        /// <exception cref="TerminauxException"></exception>
        public static SyntaxHighlighting? GetHighlighterFromJson(string json) =>
            JsonConvert.DeserializeObject<SyntaxHighlighting>(json);

        private static int GetHighlighterIndexFrom(string highlighter) =>
            HighlighterNames.Select((name, idx) => (name, idx)).Where((tuple) => tuple.name == highlighter).First().idx;
    }
}
