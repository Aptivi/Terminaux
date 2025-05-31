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

using System;
using System.Text;
using Terminaux.Sequences;
using Terminaux.Sequences.Builder.Types;
using System.Collections.Generic;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Base.Buffered;
using Terminaux.Colors;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using System.Text.RegularExpressions;
using Terminaux.Inputs.Interactive;
using Textify.General;
using Textify.Tools;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters.Simple;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Inputs;
using ImageMagick;
using Terminaux.Images.Writers;

namespace Terminaux.Images.Interactives
{
    /// <summary>
    /// Interactive image viewer
    /// </summary>
    public static class ImageViewInteractive
    {
        private static bool bail;
        private static bool fit = true;
        private static int lineIdx = 0;
        private static int lineColIdx = 0;
        private static readonly Keybinding[] bindings =
        [
            new Keybinding("Exit", ConsoleKey.Escape),
            new Keybinding("Keybindings", ConsoleKey.K),
        ];

        /// <summary>
        /// Opens an interactive image viewer
        /// </summary>
        /// <param name="image">Target image to show</param>
        /// <param name="settings">TUI settings</param>
        public static void OpenInteractive(MagickImage image, InteractiveTuiSettings? settings = null)
        {
            // Set status
            bail = false;
            fit = true;
            settings ??= InteractiveTuiSettings.GlobalSettings;
            var pixels = ImageProcessor.GetColorsFromImage(image);

            // Main loop
            lineIdx = 0;
            lineColIdx = 0;
            var screen = new Screen();
            ScreenTools.SetCurrent(screen);
            ConsoleWrapper.CursorVisible = false;
            try
            {
                while (!bail)
                {
                    // Now, render the keybindings
                    RenderKeybindings(ref screen, settings);

                    // Now, render the image with the current selection
                    RenderContentsWithSelection(lineIdx, lineColIdx, ref screen, pixels);

                    // Wait for a keypress
                    ScreenTools.Render(screen);
                    var keypress = Input.ReadKey();
                    HandleKeypress(ref screen, keypress, pixels, settings);

                    // Reset, in case selection changed
                    screen.RemoveBufferedParts();
                }
            }
            catch (Exception ex)
            {
                InfoBoxModalColor.WriteInfoBoxModal($"The text viewer failed: {ex.Message}");
            }
            bail = false;
            ScreenTools.UnsetCurrent(screen);

            // Close the file and clean up
            ColorTools.LoadBack();
        }

        private static void RenderKeybindings(ref Screen screen, InteractiveTuiSettings settings)
        {
            // Make a screen part
            var part = new ScreenPart();
            part.AddDynamicText(() =>
            {
                var keybindingsRenderable = new Keybindings()
                {
                    KeybindingList = bindings,
                    BuiltinColor = settings.KeyBindingBuiltinColor,
                    BuiltinForegroundColor = settings.KeyBindingBuiltinForegroundColor,
                    BuiltinBackgroundColor = settings.KeyBindingBuiltinBackgroundColor,
                    OptionColor = settings.KeyBindingOptionColor,
                    OptionForegroundColor = settings.OptionForegroundColor,
                    OptionBackgroundColor = settings.OptionBackgroundColor,
                    BackgroundColor = settings.BackgroundColor,
                    Width = ConsoleWrapper.WindowWidth - 1,
                };
                return RendererTools.RenderRenderable(keybindingsRenderable, new(0, ConsoleWrapper.WindowHeight - 1));
            });
            screen.AddBufferedPart("Image viewer interactive - Keybindings", part);
        }

        private static void RenderContentsWithSelection(int lineIdx, int columnIdx, ref Screen screen, Color[,] pixels)
        {
            // Render the contents using the image viewer
            var part = new ScreenPart();
            part.AddDynamicText(() =>
            {
                var renderer = new ImageView(pixels)
                {
                    Width = ConsoleWrapper.WindowWidth - 1,
                    Height = ConsoleWrapper.WindowHeight - 1,
                    ColumnOffset = columnIdx,
                    RowOffset = lineIdx,
                    Fit = fit,
                };
                return renderer.Render();
            });
            screen.AddBufferedPart("Image viewer interactive - Contents", part);
        }

        private static void HandleKeypress(ref Screen screen, ConsoleKeyInfo key, Color[,] pixels, InteractiveTuiSettings settings)
        {
            switch (key.Key)
            {
                case ConsoleKey.LeftArrow:
                    MoveBackward(pixels, ref screen);
                    return;
                case ConsoleKey.RightArrow:
                    MoveForward(pixels, ref screen);
                    return;
                case ConsoleKey.UpArrow:
                    MoveUp(pixels, ref screen);
                    return;
                case ConsoleKey.DownArrow:
                    MoveDown(pixels, ref screen);
                    return;
                case ConsoleKey.PageUp:
                    PreviousPage(pixels, ref screen);
                    return;
                case ConsoleKey.PageDown:
                    NextPage(pixels, ref screen);
                    return;
                case ConsoleKey.Home:
                    Beginning(pixels, ref screen);
                    return;
                case ConsoleKey.End:
                    End(pixels, ref screen);
                    return;
                case ConsoleKey.F:
                    fit = !fit;
                    UpdateColumnIndex(0, pixels, ref screen);
                    UpdateLineIndex(0, pixels, ref screen);
                    return;
                case ConsoleKey.Escape:
                    bail = true;
                    return;
                case ConsoleKey.K:
                    RenderKeybindingsBox(settings);
                    screen.RequireRefresh();
                    return;
            }
        }

        private static void RenderKeybindingsBox(InteractiveTuiSettings settings)
        {
            // Show the available keys list
            if (bindings.Length == 0)
                return;

            // User needs an infobox that shows all available keys
            string bindingsHelp = KeybindingTools.RenderKeybindingHelpText(bindings);
            InfoBoxModalColor.WriteInfoBoxModal(bindingsHelp, new InfoBoxSettings(settings.InfoBoxSettings)
            {
                Title = "Available keybindings"
            });
        }

        private static void MoveBackward(Color[,] pixels, ref Screen screen) =>
            UpdateColumnIndex(lineColIdx - 1, pixels, ref screen);

        private static void MoveForward(Color[,] pixels, ref Screen screen) =>
            UpdateColumnIndex(lineColIdx + 1, pixels, ref screen);

        private static void MoveUp(Color[,] pixels, ref Screen screen) =>
            UpdateLineIndex(lineIdx - 1, pixels, ref screen);

        private static void MoveDown(Color[,] pixels, ref Screen screen) =>
            UpdateLineIndex(lineIdx + 1, pixels, ref screen);

        private static void PreviousPage(Color[,] pixels, ref Screen screen)
        {
            int imageHeight = pixels.GetLength(1);
            int lineLinesPerPage = ConsoleWrapper.WindowHeight - 2;
            int currentPage = lineIdx / lineLinesPerPage;
            int startIndex = lineLinesPerPage * currentPage;
            if (startIndex > imageHeight)
                startIndex = imageHeight;
            UpdateLineIndex(startIndex - 1 < 0 ? 0 : startIndex - 1, pixels, ref screen);
        }

        private static void NextPage(Color[,] pixels, ref Screen screen)
        {
            int imageHeight = pixels.GetLength(1);
            int lineLinesPerPage = ConsoleWrapper.WindowHeight - 2;
            int currentPage = lineIdx / lineLinesPerPage;
            int endIndex = lineLinesPerPage * (currentPage + 1);
            if (endIndex > imageHeight - 1)
                endIndex = imageHeight - 1;
            UpdateLineIndex(endIndex, pixels, ref screen);
        }

        private static void Beginning(Color[,] pixels, ref Screen screen) =>
            UpdateLineIndex(0, pixels, ref screen);

        private static void End(Color[,] pixels, ref Screen screen)
        {
            int imageHeight = pixels.GetLength(1);
            UpdateLineIndex(imageHeight - 1, pixels, ref screen);
        }

        private static void UpdateLineIndex(int lnIdx, Color[,] pixels, ref Screen screen)
        {
            if (fit)
                return;
            int imageHeight = pixels.GetLength(1);
            lineIdx = lnIdx;
            if (lineIdx + ConsoleWrapper.WindowHeight - 1 > imageHeight - 1)
                lineIdx = imageHeight - ConsoleWrapper.WindowHeight + 1;
            if (lineIdx < 0)
                lineIdx = 0;
            UpdateColumnIndex(lineColIdx, pixels, ref screen);
        }

        private static void UpdateColumnIndex(int clIdx, Color[,] pixels, ref Screen screen)
        {
            if (fit)
                return;
            int imageWidth = pixels.GetLength(0);
            int imageHeight = pixels.GetLength(1);
            lineColIdx = clIdx;
            if (imageHeight == 0)
            {
                lineColIdx = 0;
                return;
            }
            if (lineColIdx + ConsoleWrapper.WindowWidth - 1 > imageWidth - 1)
                lineColIdx = imageWidth - ConsoleWrapper.WindowWidth + 1;
            if (lineColIdx < 0)
                lineColIdx = 0;
            screen.RequireRefresh();
        }
    }
}
