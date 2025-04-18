﻿//
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
using System.Collections.Generic;
using static Terminaux.Writer.CyclicWriters.Renderer.Tools.AsciinemaPlayer;

#pragma warning disable CS0649

namespace Terminaux.Writer.CyclicWriters.Renderer.Tools
{
    /// <summary>
    /// Asciicast v2 class containing info for recorded terminal screencast
    /// </summary>
    public class AsciicastV2 : Asciicast
    {
        [JsonProperty("timestamp")]
        private readonly int timestamp;
        [JsonProperty("duration")]
        private readonly float duration;
        [JsonProperty("idle_time_limit")]
        private readonly float idleTimeLimit;
        [JsonProperty("command")]
        private readonly string command = "";
        [JsonProperty("title")]
        private readonly string title = "";
        [JsonProperty("env")]
        private readonly EnvironmentInfo? environment;
        [JsonProperty("theme")]
        private readonly ThemeInfo? theme;
        [JsonIgnore]
        internal List<(double, string, string)> stdOutData = [];

        /// <summary>
        /// Duration of the Asciicast recording
        /// </summary>
        [JsonIgnore]
        public int Timestamp =>
            timestamp;

        /// <summary>
        /// Duration of the Asciicast recording
        /// </summary>
        [JsonIgnore]
        public float Duration =>
            duration;

        /// <summary>
        /// Maximum idle limit
        /// </summary>
        [JsonIgnore]
        public float IdleTimeLimit =>
            idleTimeLimit;

        /// <summary>
        /// Command to be recorded
        /// </summary>
        [JsonIgnore]
        public string Command =>
            command;

        /// <summary>
        /// Duration of the Asciicast recording
        /// </summary>
        [JsonIgnore]
        public string Title =>
            title;

        /// <summary>
        /// Environment of the recorded terminal
        /// </summary>
        [JsonIgnore]
        public EnvironmentInfo? Environment =>
            environment;

        /// <summary>
        /// Theme info for the recorded terminal
        /// </summary>
        [JsonIgnore]
        public ThemeInfo? Theme =>
            theme;

        /// <summary>
        /// Delay and Data
        /// </summary>
        [JsonIgnore]
        public override List<(double, string, string)> StdOutData =>
            [.. stdOutData];
    }
}
