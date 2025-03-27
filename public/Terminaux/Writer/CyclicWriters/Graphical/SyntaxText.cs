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

using SpecProbe.Software.Platform;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Terminaux.Colors;
using Terminaux.Writer.CyclicWriters.Renderer.Markup;
using Textify.General;

namespace Terminaux.Writer.CyclicWriters.Graphical
{
    /// <summary>
    /// Syntax text renderable
    /// </summary>
    public class SyntaxText : GraphicalCyclicWriter
    {
        private string syntax = "csharp";
        private string text = "";
        private string pathToHighlight = "";
        private int leftMargin = 0;
        private int rightMargin = 0;
        private Color foregroundColor = ColorTools.CurrentForegroundColor;
        private Color backgroundColor = ColorTools.CurrentBackgroundColor;
        private bool useColors = true;

        /// <summary>
        /// Language type
        /// </summary>
        public string Syntax
        {
            get => syntax;
            set => syntax = value;
        }

        /// <summary>
        /// Text to render
        /// </summary>
        public string Text
        {
            get => text;
            set => text = value;
        }

        /// <summary>
        /// Path to the Highlight executable (platform-dependent). You can download a copy from <see href="http://andre-simon.de/zip/download.php">here</see>.
        /// </summary>
        public string PathToHighlight
        {
            get => pathToHighlight;
            set => pathToHighlight = value;
        }

        /// <summary>
        /// Left margin of the syntax figlet text
        /// </summary>
        public int LeftMargin
        {
            get => leftMargin;
            set => leftMargin = value;
        }

        /// <summary>
        /// Right margin of the syntax figlet text
        /// </summary>
        public int RightMargin
        {
            get => rightMargin;
            set => rightMargin = value;
        }

        /// <summary>
        /// Foreground color of the text
        /// </summary>
        public Color ForegroundColor
        {
            get => foregroundColor;
            set => foregroundColor = value;
        }

        /// <summary>
        /// Background color of the text
        /// </summary>
        public Color BackgroundColor
        {
            get => backgroundColor;
            set => backgroundColor = value;
        }

        /// <summary>
        /// Whether to use colors or not
        /// </summary>
        public bool UseColors
        {
            get => useColors;
            set => useColors = value;
        }

        /// <summary>
        /// Renders a syntax text
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public override string Render()
        {
            try
            {
                // Try to find the highlight process
                var syntax = new StringBuilder();
                string finalHighlightProcess = string.IsNullOrWhiteSpace(PathToHighlight) ? "highlight" : PathToHighlight;
                bool canHighlight = UseColors;
                if (UseColors)
                {
                    if (!File.Exists(finalHighlightProcess))
                    {
                        if (PlatformHelper.IsOnWindows())
                        {
                            // It doesn't exist. It's possible that Windows users might have installed highlight using the
                            // Windows installer. Look at "%SYSTEMDRIVE%/Program Files/Highlight/highlight.exe".
                            finalHighlightProcess = Path.GetFullPath($"{Environment.GetEnvironmentVariable("SYSTEMDRIVE")}/Program Files/Highlight/highlight.exe");
                            if (!File.Exists(finalHighlightProcess))
                                canHighlight = false;
                        }
                        else
                            canHighlight = false;
                    }
                }

                // Now, determine whether we can highlight.
                string output = Text;
                if (canHighlight)
                {
                    try
                    {
                        // Make a temporary file and write data to it
                        string tempPath = Path.GetTempFileName();
                        var stream = new StreamWriter(tempPath);
                        stream.WriteLine(output);
                        stream.Close();

                        // Populate command and argument, depending on platform
                        string command = PlatformHelper.IsOnWindows() ? "cmd" : "cat";
                        string arguments = $"\"{tempPath}\" | \"{finalHighlightProcess}\" --out-format=ansi --syntax={Syntax}";
                        arguments = PlatformHelper.IsOnWindows() ? $"/C \"type {arguments}\"" : arguments;

                        // Formulate the final start info and start the highlighter process
                        var highlightProcessInfo = new ProcessStartInfo(command, arguments)
                        {
                            RedirectStandardOutput = true,
                        };
                        var highlightProcess = Process.Start(highlightProcessInfo);
                        highlightProcess.WaitForExit();
                        output = highlightProcess.StandardOutput.ReadToEnd();

                        // Delete the temporary file
                        File.Delete(tempPath);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Failed to render syntax text: " + ex.Message);
                        Debug.WriteLine(ex);
                    }
                }

                // Add the result
                syntax.Append(new AlignedText()
                {
                    Text = output,
                    ForegroundColor = ForegroundColor,
                    BackgroundColor = BackgroundColor,
                    Top = Top,
                    LeftMargin = LeftMargin,
                    RightMargin = RightMargin,
                    UseColors = UseColors,
                }.Render());

                // Write the resulting buffer
                if (UseColors)
                {
                    syntax.Append(
                        ColorTools.RenderRevertForeground() +
                        ColorTools.RenderRevertBackground()
                    );
                }
                return syntax.ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
            return "";
        }

        /// <summary>
        /// Makes a new instance of the syntax text renderer
        /// </summary>
        /// <param name="text">Text to use</param>
        /// <param name="vars">Variables to format the text with</param>
        public SyntaxText(Mark? text = null, params object[] vars)
        {
            // Install the values
            this.text = TextTools.FormatString(text ?? "", vars);
        }
    }
}
