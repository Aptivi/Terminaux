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
using System;
using System.Threading;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Colorimetry;
using Terminaux.Writer.ConsoleWriters;

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

        /// <summary>
        /// Plays the Asciinema recording in full-screen mode
        /// </summary>
        /// <param name="representation">Asciinema representation containing a valid instance of <see cref="Asciicast"/></param>
        /// <param name="resizeWindow">Whether to resize the window when playing</param>
        public static void PlayAsciinema(AsciinemaRepresentation representation, bool resizeWindow = true)
        {
            var asciicast = representation.Asciicast;
            PlayAsciinema(asciicast, resizeWindow);
        }

        /// <summary>
        /// Plays the Asciinema recording in full-screen mode
        /// </summary>
        /// <param name="asciicast">Asciicast instance containing recorded data</param>
        /// <param name="resizeWindow">Whether to resize the window when playing</param>
        public static void PlayAsciinema(Asciicast asciicast, bool resizeWindow = true)
        {
            // Get the initial window size
            int oldWidth = ConsoleWrapper.WindowWidth;
            int oldHeight = ConsoleWrapper.WindowHeight;

            // Try to set the window size
            if (resizeWindow)
                ConsoleWrapper.SetWindowDimensions(asciicast.Width, asciicast.Height);

            // Clear the screen
            ConsoleColoring.LoadBack();

            // Process the stdout data
            ConsoleLogger.Info("Playing {0} frames", asciicast.StdOutData.Count);
            for (int i = 0; i < asciicast.StdOutData.Count; i++)
            {
                // Get the standard output data
                (double timePoint, string eventType, string data) = asciicast.StdOutData[i];
                (double nextTimePoint, _, _) = i + 1 < asciicast.StdOutData.Count ? asciicast.StdOutData[i + 1] : (timePoint, "", "");

                // Get the time point difference in time span
                double timePointDiff = nextTimePoint - timePoint;
                if (asciicast is AsciicastV2 asciicastV2 && timePointDiff > asciicastV2.IdleTimeLimit && asciicastV2.IdleTimeLimit > 0)
                    timePointDiff = asciicastV2.IdleTimeLimit;
                var timePointSpan = TimeSpan.FromSeconds(timePointDiff);
                ConsoleLogger.Debug("Playing from {0} to {1} in {2} ms with event {3}", i, i + 1, timePointSpan.TotalMilliseconds, eventType);

                // Determine the event type
                switch (eventType)
                {
                    case "i":
                    case "o":
                        // Output or input event
                        TextWriterRaw.WriteRaw(data);
                        break;
                    case "r":
                        // Resize events in this format: {Columns}x{Rows}
                        if (resizeWindow)
                        {
                            int newWidth = int.Parse(data.Substring(0, data.IndexOf('x')));
                            int newHeight = int.Parse(data.Substring(data.IndexOf('x') + 1));
                            ConsoleLogger.Debug("Resizing to {0}, {1}", newWidth, newHeight);
                            ConsoleWrapper.SetWindowDimensions(newWidth, newHeight);
                        }
                        break;
                }

                // Now, wait between two events
                Thread.Sleep((int)timePointSpan.TotalMilliseconds);
            }

            // Try to restore the window size
            if (resizeWindow)
            {
                ConsoleLogger.Debug("Resizing to initial size {0}, {1}", oldWidth, oldHeight);
                ConsoleWrapper.SetWindowDimensions(oldWidth, oldHeight);
            }
        }
    }
}
