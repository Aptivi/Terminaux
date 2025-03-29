//
// Terminaux  Copyright (C) 2023-2025  Aptivi
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
using Terminaux.Colors;

namespace Terminaux.Writer.CyclicWriters.Renderer.Tools
{
    /// <summary>
    /// Representation class for the Asciinema specification
    /// </summary>
    public static class AsciinemaPlayer
    {
        /// <summary>
        /// Environment information
        /// </summary>
        public class EnvironmentInfo
        {
            [JsonProperty("TERM")]
            private readonly string terminal = "";
            [JsonProperty("SHELL")]
            private readonly string shell = "";

            /// <summary>
            /// Terminal emulator name
            /// </summary>
            [JsonIgnore]
            public string Terminal =>
                terminal;

            /// <summary>
            /// Path to shell used
            /// </summary>
            [JsonIgnore]
            public string Shell =>
                shell;
        }
        
        /// <summary>
        /// Theme information
        /// </summary>
        public class ThemeInfo
        {
            [JsonProperty("fg")]
            private readonly string foregroundString = "";
            [JsonProperty("bg")]
            private readonly string backgroundString = "";
            [JsonProperty("palette")]
            private readonly string palette = "";

            /// <summary>
            /// Foreground color as string
            /// </summary>
            [JsonIgnore]
            public string ForegroundString =>
                foregroundString;

            /// <summary>
            /// Foreground color
            /// </summary>
            [JsonIgnore]
            public Color Foreground =>
                new(ForegroundString);

            /// <summary>
            /// Background color as string
            /// </summary>
            [JsonIgnore]
            public string BackgroundString =>
                backgroundString;

            /// <summary>
            /// Background color
            /// </summary>
            [JsonIgnore]
            public Color Background =>
                new(BackgroundString);

            /// <summary>
            /// Palette representation
            /// </summary>
            [JsonIgnore]
            public string Palette =>
                palette;
        }

        // TODO: Populate AsciiCast player code
    }
}
