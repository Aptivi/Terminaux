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
using System.Collections.Generic;
using static Terminaux.Writer.CyclicWriters.Renderer.Tools.AsciinemaPlayer;

namespace Terminaux.Writer.CyclicWriters.Renderer.Tools
{
    /// <summary>
    /// Asciicast v1 class containing info for recorded terminal screencast
    /// </summary>
    public class AsciicastV1 : Asciicast
    {
        [JsonProperty("duration")]
        private readonly float duration;
        [JsonProperty("command")]
        private readonly string command = "";
        [JsonProperty("title")]
        private readonly string title = "";
        [JsonProperty("env")]
        private readonly EnvironmentInfo? environment;
        [JsonProperty("stdout")]
        private readonly object[][] stdOutData = [];

        /// <summary>
        /// Duration of the Asciicast recording
        /// </summary>
        [JsonIgnore]
        public float Duration =>
            duration;

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
        /// Delay and Data
        /// </summary>
        [JsonIgnore]
        public override List<(double, string, string)> StdOutData
        {
            get
            {
                var dataList = new List<(double, string, string)>();
                if (stdOutData.Length == 0)
                    return [];
                foreach (var dataArray in stdOutData)
                {
                    // Get the delay and the printed data
                    double delay = (double)dataArray[0];
                    string printed = (string)dataArray[1];

                    // Add them to the data list
                    dataList.Add((delay, "o", printed));
                }
                return dataList;
            }
        }
    }
}
