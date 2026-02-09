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
using System.Diagnostics;
using System.Text.RegularExpressions;
using Terminaux.Base;
using Colorimetry;
using Textify.Tools;

namespace Terminaux.Reader.Highlighting
{
    /// <summary>
    /// Syntax highlighting tools
    /// </summary>
    [DebuggerDisplay("FG: {ComponentForegroundColor} | BG: {ComponentBackgroundColor} | Match: {ComponentMatch}")]
    public class SyntaxHighlightingComponent
    {
        [JsonProperty(nameof(ComponentMatch), Required = Required.Always)]
        private readonly Regex? componentMatch;
        [JsonProperty(nameof(ComponentForegroundColor), Required = Required.Always)]
        private readonly Color? componentForegroundColor;
        [JsonProperty(nameof(ComponentBackgroundColor), Required = Required.Always)]
        private readonly Color? componentBackgroundColor;
        [JsonProperty(nameof(UseBackgroundColor))]
        private readonly bool useBackgroundColor;
        [JsonProperty(nameof(UseForegroundColor))]
        private readonly bool useForegroundColor = true;

        /// <summary>
        /// Regular expression to match a component
        /// </summary>
        [JsonIgnore]
        public Regex? ComponentMatch =>
            componentMatch;
        /// <summary>
        /// Component foreground color
        /// </summary>
        [JsonIgnore]
        public Color? ComponentForegroundColor =>
            componentForegroundColor;
        /// <summary>
        /// Component background color
        /// </summary>
        [JsonIgnore]
        public Color? ComponentBackgroundColor =>
            componentBackgroundColor;
        /// <summary>
        /// Component background color
        /// </summary>
        [JsonIgnore]
        public bool UseBackgroundColor =>
            useBackgroundColor;
        /// <summary>
        /// Component background color
        /// </summary>
        [JsonIgnore]
        public bool UseForegroundColor =>
            useForegroundColor;

        [JsonConstructor]
        private SyntaxHighlightingComponent()
        { }

        /// <summary>
        /// Makes an instance of the syntax highlighter component class
        /// </summary>
        /// <param name="componentMatch">Regular expression to match in input</param>
        /// <param name="componentForegroundColor">Foreground color of the component</param>
        /// <param name="componentBackgroundColor">Background color of the component</param>
        /// <param name="useBackgroundColor">Whether the syntax highlighter can use the background color or not</param>
        /// <param name="useForegroundColor">Whether the syntax highlighter can use the foreground color or not</param>
        /// <exception cref="TerminauxException"></exception>
        public SyntaxHighlightingComponent(string componentMatch, Color componentForegroundColor, Color componentBackgroundColor, bool useBackgroundColor, bool useForegroundColor) :
            this(new Regex(componentMatch, RegexOptions.Compiled), componentForegroundColor, componentBackgroundColor, useBackgroundColor, useForegroundColor)
        {
            if (!RegexTools.IsValidRegex(componentMatch))
                throw new TerminauxException(LanguageTools.GetLocalized("T_READER_HIGHLIGHT_EXCEPTION_COMPONENTREGEXINVALID"));
        }

        internal SyntaxHighlightingComponent(Regex componentMatch, Color componentForegroundColor, Color componentBackgroundColor, bool useBackgroundColor, bool useForegroundColor)
        {
            this.componentMatch = componentMatch ??
                throw new TerminauxException(LanguageTools.GetLocalized("T_READER_HIGHLIGHT_EXCEPTION_COMPONENTREGEXNOTPROVIDED"));
            this.componentForegroundColor = componentForegroundColor ??
                throw new TerminauxException(LanguageTools.GetLocalized("T_READER_HIGHLIGHT_EXCEPTION_COMPONENTFORECOLORNOTPROVIDED"));
            this.componentBackgroundColor = componentBackgroundColor ??
                throw new TerminauxException(LanguageTools.GetLocalized("T_READER_HIGHLIGHT_EXCEPTION_COMPONENTBACKCOLORNOTPROVIDED"));
            this.useBackgroundColor = useBackgroundColor;
            this.useForegroundColor = useForegroundColor;
        }
    }
}
