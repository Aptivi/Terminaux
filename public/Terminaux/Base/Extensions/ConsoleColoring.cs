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

using System;
using System.Text;
using Colorimetry;
using Colorimetry.Data;
using Colorimetry.Transformation.Contrast;
using SpecProbe.Software.Platform;
using Terminaux.Base.Buffered;
using Terminaux.Base.Checks;
using Terminaux.Base.TermInfo;
using Terminaux.Inputs;
using Terminaux.Sequences.Builder;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Graphical;
using Textify.General;

namespace Terminaux.Base.Extensions
{
    /// <summary>
    /// Console coloring extensions
    /// </summary>
    public static class ConsoleColoring
    {
        internal static Color currentForegroundColor = new(ConsoleColors.Silver);
        internal static Color currentBackgroundColor = Color.Empty;
        private static bool allowBackground;

        /// <summary>
        /// Current foreground color
        /// </summary>
        public static Color CurrentForegroundColor =>
            currentForegroundColor;

        /// <summary>
        /// Current background color
        /// </summary>
        public static Color CurrentBackgroundColor =>
            currentBackgroundColor;

        /// <summary>
        /// If you are sure that the console supports true color, or if you want to change your terminal to a terminal that supports true color, change this value.
        /// </summary>
        public static bool ConsoleSupportsTrueColor { get; set; } = true;

        /// <summary>
        /// Whether applications are allowed to set the current background color or not
        /// </summary>
        public static bool AllowBackground
        {
            get => allowBackground;
            set => allowBackground = value;
        }
        /// <summary>
        /// Parsable VT sequence (Foreground)
        /// </summary>
        public static string VTSequenceForeground(this Color color) =>
            color.IsOriginal ? color.VTSequenceForegroundOriginal() : color.VTSequenceForegroundTrueColor();

        /// <summary>
        /// Parsable VT sequence (Foreground)
        /// </summary>
        public static string VTSequenceForegroundOriginal(this Color color)
        {
            if (color.Type == ColorType.TrueColor)
                return color.VTSequenceForegroundTrueColor();
            color.vtSeqForeground ??=
                TermInfoDesc.Current.SetAForeground?.ProcessSequence(color.PlainSequence) ??
                $"{VtSequenceBasicChars.EscapeChar}[38;5;{color.PlainSequence}m";
            return color.vtSeqForeground;
        }

        /// <summary>
        /// Parsable VT sequence (Foreground, true color)
        /// </summary>
        public static string VTSequenceForegroundTrueColor(this Color color)
        {
            color.vtSeqForegroundTrue ??= $"{VtSequenceBasicChars.EscapeChar}[38;2;{color.PlainSequenceTrueColor}m";
            return color.vtSeqForegroundTrue;
        }

        /// <summary>
        /// Parsable VT sequence (Background)
        /// </summary>
        public static string VTSequenceBackground(this Color color) =>
            color.IsOriginal ? color.VTSequenceBackgroundOriginal() : color.VTSequenceBackgroundTrueColor();

        /// <summary>
        /// Parsable VT sequence (Background)
        /// </summary>
        public static string VTSequenceBackgroundOriginal(this Color color)
        {
            if (color.Type == ColorType.TrueColor)
                return color.VTSequenceBackgroundTrueColor();
            color.vtSeqBackground ??=
                TermInfoDesc.Current.SetABackground?.ProcessSequence(color.PlainSequence) ??
                $"{VtSequenceBasicChars.EscapeChar}[38;5;{color.PlainSequence}m";
            return color.vtSeqBackground;
        }

        /// <summary>
        /// Parsable VT sequence (Background, true color)
        /// </summary>
        public static string VTSequenceBackgroundTrueColor(this Color color)
        {
            color.vtSeqBackgroundTrue ??= $"{VtSequenceBasicChars.EscapeChar}[48;2;{color.PlainSequenceTrueColor}m";
            return color.vtSeqBackgroundTrue;
        }

        /// <summary>
        /// Gets the gray color according to the brightness of the background color
        /// </summary>
        /// <param name="contrastType">Contrast type</param>
        public static Color GetGray(ColorContrastType contrastType = ColorContrastType.Light) =>
            ColorTools.GetGray(CurrentBackgroundColor, contrastType);

        /// <summary>
        /// Loads the background
        /// </summary>
        public static void LoadBack() =>
            LoadBack(CurrentBackgroundColor);

        /// <summary>
        /// Loads the background
        /// </summary>
        /// <param name="ColorSequence">Color sequence used to load background</param>
        /// <param name="Force">Force set background even if background setting is disabled</param>
        public static void LoadBack(Color ColorSequence, bool Force = false)
        {
            try
            {
                SetConsoleColor(CurrentForegroundColor, false, Force, true);
                SetConsoleColor(ColorSequence, true, Force, AllowBackground);
                ConsoleWrapper.Clear();
            }
            catch (Exception ex)
            {
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_EXCEPTION_SETBACKGROUND") + $": {ex.Message}");
            }
        }

        /// <summary>
        /// Loads the background dryly
        /// </summary>
        public static void LoadBackDry() =>
            LoadBackDry(CurrentBackgroundColor);

        /// <summary>
        /// Loads the background dryly
        /// </summary>
        /// <param name="ColorSequence">Color sequence used to load background</param>
        /// <param name="Force">Force set background even if background setting is disabled</param>
        public static void LoadBackDry(Color ColorSequence, bool Force = false)
        {
            try
            {
                SetConsoleColorDry(ColorSequence, true, Force, AllowBackground);
                ConsoleWrapper.Clear();
            }
            catch (Exception ex)
            {
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_EXCEPTION_SETBACKGROUND") + $": {ex.Message}");
            }
        }

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="ColorSequence">The color instance</param>
        /// <param name="Background">Whether to set background or not</param>
        public static void SetConsoleColor(Color ColorSequence, bool Background) =>
            SetConsoleColorInternal(ColorSequence, Background, ColorSequence != CurrentBackgroundColor && ColorSequence != ThemeColorsTools.GetColor(ThemeColorType.Background), !Background || AllowBackground, true);

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="ColorSequence">The color instance</param>
        /// <param name="Background">Whether to set background or not</param>
        /// <param name="ForceSet">Force set background even if background setting is disabled or if the current background color doesn't match the provided color</param>
        /// <param name="canSet">Can the console set this color?</param>
        public static void SetConsoleColor(Color ColorSequence, bool Background = false, bool ForceSet = false, bool canSet = true) =>
            SetConsoleColorInternal(ColorSequence, Background, ForceSet, canSet, true);

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="ColorSequence">The color instance</param>
        /// <param name="Background">Whether to set background or not</param>
        /// <param name="ForceSet">Force set background even if background setting is disabled or if the current background color doesn't match the provided color</param>
        /// <param name="canSet">Can the console set this color?</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TrySetConsoleColor(Color ColorSequence, bool Background = false, bool ForceSet = false, bool canSet = true)
        {
            try
            {
                SetConsoleColor(ColorSequence, Background, ForceSet, canSet);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Sets the console color dryly
        /// </summary>
        /// <param name="ColorSequence">The color instance</param>
        /// <param name="Background">Whether to set background or not</param>
        public static void SetConsoleColorDry(Color ColorSequence, bool Background) =>
            SetConsoleColorInternal(ColorSequence, Background, ColorSequence != CurrentBackgroundColor && ColorSequence != ThemeColorsTools.GetColor(ThemeColorType.Background), !Background || AllowBackground, false);

        /// <summary>
        /// Sets the console color dryly
        /// </summary>
        /// <param name="ColorSequence">The color instance</param>
        /// <param name="Background">Whether to set background or not</param>
        /// <param name="ForceSet">Force set background even if background setting is disabled or if the current background color doesn't match the provided color</param>
        /// <param name="canSet">Can the console set this color?</param>
        public static void SetConsoleColorDry(Color ColorSequence, bool Background = false, bool ForceSet = false, bool canSet = true) =>
            SetConsoleColorInternal(ColorSequence, Background, ForceSet, canSet, false);

        /// <summary>
        /// Sets the console color dryly
        /// </summary>
        /// <param name="ColorSequence">The color instance</param>
        /// <param name="Background">Whether to set background or not</param>
        /// <param name="ForceSet">Force set background even if background setting is disabled or if the current background color doesn't match the provided color</param>
        /// <param name="canSet">Can the console set this color?</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TrySetConsoleColorDry(Color ColorSequence, bool Background = false, bool ForceSet = false, bool canSet = true)
        {
            try
            {
                SetConsoleColorDry(ColorSequence, Background, ForceSet, canSet);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the console color setting sequence
        /// </summary>
        /// <param name="ColorSequence">The color instance</param>
        public static string RenderSetConsoleColor(Color ColorSequence) =>
            RenderSetConsoleColor(ColorSequence, false, ColorSequence != CurrentForegroundColor, true);

        /// <summary>
        /// Gets the console color setting sequence
        /// </summary>
        /// <param name="ColorSequence">The color instance</param>
        /// <param name="Background">Whether to set background or not</param>
        public static string RenderSetConsoleColor(Color ColorSequence, bool Background) =>
            RenderSetConsoleColor(ColorSequence, Background, ColorSequence != CurrentBackgroundColor && ColorSequence != ThemeColorsTools.GetColor(ThemeColorType.Background), !Background || AllowBackground);

        /// <summary>
        /// Gets the console color setting sequence
        /// </summary>
        /// <param name="ColorSequence">The color instance</param>
        /// <param name="Background">Whether to set background or not</param>
        /// <param name="ForceSet">Force set background even if background setting is disabled or if the current background color doesn't match the provided color</param>
        /// <param name="canSet">Can the console set this color?</param>
        public static string RenderSetConsoleColor(Color ColorSequence, bool Background = false, bool ForceSet = false, bool canSet = true)
        {
            if (ColorSequence is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_EXCEPTION_COLORISNULL"));
            if (ConsoleChecker.IsDumb)
                return "";

            // Define reset background sequence
            string resetBgSequence = RenderResetBackground();
            string resetFgSequence = RenderResetForeground();

            // Render the background being set
            var builder = new StringBuilder();
            if (Background)
            {
                if (canSet || ForceSet)
                    builder.Append(ColorSequence.VTSequenceBackground());
                else if (!canSet)
                    builder.Append(resetBgSequence);
            }
            else
            {
                if (canSet || ForceSet)
                    builder.Append(ColorSequence.VTSequenceForeground());
                else if (!canSet)
                    builder.Append(resetFgSequence);
            }
            return builder.ToString();
        }

        /// <summary>
        /// Resets the console colors without clearing screen
        /// </summary>
        public static void ResetColors()
        {
            ResetForeground();
            ResetBackground();
        }

        /// <summary>
        /// Resets the foreground color without clearing screen
        /// </summary>
        public static void ResetForeground()
        {
            TextWriterRaw.WriteRaw(RenderResetForeground());
            currentForegroundColor = ConsoleColor.Gray;
        }

        /// <summary>
        /// Resets the background color without clearing screen
        /// </summary>
        public static void ResetBackground()
        {
            TextWriterRaw.WriteRaw(RenderResetBackground());
            currentBackgroundColor = ConsoleColor.Black;
        }

        /// <summary>
        /// Gets a sequence that resets the console colors without clearing screen
        /// </summary>
        public static string RenderResetColors() =>
            RenderResetForeground() + RenderResetBackground();

        /// <summary>
        /// Gets a sequence that resets the foreground color without clearing screen
        /// </summary>
        public static string RenderResetForeground() =>
            $"{VtSequenceBasicChars.EscapeChar}[39m";

        /// <summary>
        /// Gets a sequence that resets the background color without clearing screen
        /// </summary>
        public static string RenderResetBackground() =>
            $"{VtSequenceBasicChars.EscapeChar}[49m";

        /// <summary>
        /// Reverts the console colors without clearing screen
        /// </summary>
        public static void RevertColors()
        {
            RevertForeground();
            RevertBackground();
        }

        /// <summary>
        /// Reverts the foreground color without clearing screen
        /// </summary>
        public static void RevertForeground() =>
            TextWriterRaw.WriteRaw(RenderRevertForeground());

        /// <summary>
        /// Reverts the background color without clearing screen
        /// </summary>
        public static void RevertBackground() =>
            TextWriterRaw.WriteRaw(RenderRevertBackground());

        /// <summary>
        /// Gets a sequence that reverts the console colors without clearing screen
        /// </summary>
        public static string RenderRevertColors() =>
            RenderRevertForeground() + RenderRevertBackground();

        /// <summary>
        /// Gets a sequence that reverts the foreground color without clearing screen
        /// </summary>
        public static string RenderRevertForeground() =>
            RenderSetConsoleColor(CurrentForegroundColor);

        /// <summary>
        /// Gets a sequence that reverts the background color without clearing screen
        /// </summary>
        public static string RenderRevertBackground() =>
            RenderSetConsoleColor(CurrentBackgroundColor, true);

        /// <summary>
        /// Asks the user to decide whether the terminal supports true color
        /// </summary>
        public static void DetermineTrueColorFromUser()
        {
            var screen = new Screen();
            var rampPart = new ScreenPart();
            ScreenTools.SetCurrent(screen);

            // Show a tip
            rampPart.AddDynamicText(() =>
            {
                string message =
                    PlatformHelper.IsOnWindows() ?
                    LanguageTools.GetLocalized("T_COLOR_COLORTEST_WARNING") + "\n" :
                    LanguageTools.GetLocalized("T_COLOR_COLORTEST_INFO") + "\n";
                return TextWriterWhereColor.RenderWhere(TextTools.FormatString(message, PlatformHelper.GetTerminalType(), PlatformHelper.GetTerminalEmulator()), 3, 1, ThemeColorType.Warning);
            });

            // Show three color bands
            rampPart.AddDynamicText(() =>
            {
                var band = new StringBuilder();

                // First, render a box
                int times = ConsoleWrapper.WindowWidth - 10;
                ConsoleLogger.Debug("Band length: {0} cells", times);
                var rgbBand = new BoxFrame()
                {
                    Left = 3,
                    Top = 3,
                    Width = times + 1,
                    Height = 3,
                };
                var hueBand = new BoxFrame()
                {
                    Left = 3,
                    Top = 8,
                    Width = times + 1,
                    Height = 1,
                };
                band.Append(
                    rgbBand.Render() +
                    hueBand.Render()
                );
                band.Append(VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.CsiCursorPosition, 5, 5));

                // Then, render the three bands, starting from the red color
                double threshold = 255 / (double)times;
                for (double i = 0; i <= times; i++)
                    band.Append($"{new Color(Convert.ToInt32(i * threshold), 0, 0, new()
                    {
                        UseTerminalPalette = false,
                    }).VTSequenceBackground()} ");
                band.Append(RenderResetBackground());
                band.Append(VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.CsiCursorPosition, 5, 6));

                // The green color
                for (double i = 0; i <= times; i++)
                    band.Append($"{new Color(0, Convert.ToInt32(i * threshold), 0, new()
                    {
                        UseTerminalPalette = false,
                    }).VTSequenceBackground()} ");
                band.Append(RenderResetBackground());
                band.Append(VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.CsiCursorPosition, 5, 7));

                // The blue color
                for (double i = 0; i <= times; i++)
                    band.Append($"{new Color(0, 0, Convert.ToInt32(i * threshold), new()
                    {
                        UseTerminalPalette = false,
                    }).VTSequenceBackground()} ");
                band.Append(RenderResetBackground());
                band.Append(VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.CsiCursorPosition, 5, 10));

                // Now, show the hue band
                double hueThreshold = 360 / (double)times;
                for (double h = 0; h <= times; h++)
                    band.Append($"{new Color($"hsl:{Convert.ToInt32(h * hueThreshold)};100;50", new()
                    {
                        UseTerminalPalette = false,
                    }).VTSequenceBackground()} ");
                band.AppendLine();
                band.Append(RenderResetBackground());

                // Render the result
                return
                    TextWriterWhereColor.RenderWhere(TextTools.FormatString(band.ToString(), PlatformHelper.GetTerminalType(), PlatformHelper.GetTerminalEmulator()), 3, 3) +
                    TextWriterWhereColor.RenderWhere(LanguageTools.GetLocalized("T_COLOR_COLORTEST_QUESTION") + " <y/n>", 3, ConsoleWrapper.WindowHeight - 2, ThemeColorType.Question);
            });
            screen.AddBufferedPart("Ramp screen part", rampPart);

            // Tell the user to select either Y or N
            ConsoleKey answer = default;
            ScreenTools.Render();
            while (answer != ConsoleKey.N && answer != ConsoleKey.Y)
                answer = Input.ReadKey().Key;

            // Set the appropriate config
            ConsoleSupportsTrueColor = answer == ConsoleKey.Y;

            // Clear the screen and remove the screen
            ScreenTools.UnsetCurrent(screen);
            ThemeColorsTools.LoadBackground();
        }

        private static void SetConsoleColorInternal(Color ColorSequence, bool Background, bool ForceSet, bool canSet, bool needsToSetCurrentColors)
        {
            if (ColorSequence is null)
                throw new ArgumentNullException(nameof(ColorSequence));

            // Get the appropriate color setting sequence
            string sequence = RenderSetConsoleColor(ColorSequence, Background, ForceSet, canSet);

            // Actually set the color
            TextWriterRaw.WriteRaw(sequence);

            // Set current color
            if (needsToSetCurrentColors)
            {
                if (Background)
                {
                    if (canSet | ForceSet)
                        currentBackgroundColor = ColorSequence;
                }
                else
                {
                    if (canSet | ForceSet)
                        currentForegroundColor = ColorSequence;
                }
            }
        }
    }
}
