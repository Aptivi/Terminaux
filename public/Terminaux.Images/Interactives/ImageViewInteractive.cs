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
using Terminaux.Base;
using Terminaux.Inputs.Interactive;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters.Simple;
using Terminaux.Writer.CyclicWriters.Renderer;
using ImageMagick;
using Terminaux.Images.Writers;

namespace Terminaux.Images.Interactives
{
    /// <summary>
    /// Interactive image viewer
    /// </summary>
    public class ImageViewInteractive : TextualUI
    {
        private static Keybinding[] bindings = [];
        private bool fit = true;
        private InteractiveTuiSettings settings = InteractiveTuiSettings.GlobalSettings;
        private Color[,] pixels = new Color[0, 0];
        private int lineIdx = 0;
        private int lineColIdx = 0;

        /// <summary>
        /// Opens an interactive image viewer
        /// </summary>
        /// <param name="image">Target image to show</param>
        /// <param name="settings">TUI settings</param>
        public static void OpenInteractive(MagickImage image, InteractiveTuiSettings? settings = null)
        {
            // Make a new TUI
            var finalSettings = settings ?? InteractiveTuiSettings.GlobalSettings;
            var pixels = ImageProcessor.GetColorsFromImage(image);
            var imageViewer = new ImageViewInteractive()
            {
                settings = finalSettings,
                pixels = pixels,
            };

            // Assign keybindings
            bindings =
            [
                new Keybinding(LanguageTools.GetLocalized("TI_IMAGEVIEWINTERACTIVE_BINDINGS_EXIT"), ConsoleKey.Escape),
                new Keybinding(LanguageTools.GetLocalized("TI_IMAGEVIEWINTERACTIVE_BINDINGS_FITORSCALED"), ConsoleKey.F),
                new Keybinding(LanguageTools.GetLocalized("TI_IMAGEVIEWINTERACTIVE_BINDINGS_MOVEUP"), ConsoleKey.UpArrow),
                new Keybinding(LanguageTools.GetLocalized("TI_IMAGEVIEWINTERACTIVE_BINDINGS_MOVEDOWN"), ConsoleKey.DownArrow),
                new Keybinding(LanguageTools.GetLocalized("TI_IMAGEVIEWINTERACTIVE_BINDINGS_MOVELEFT"), ConsoleKey.LeftArrow),
                new Keybinding(LanguageTools.GetLocalized("TI_IMAGEVIEWINTERACTIVE_BINDINGS_MOVERIGHT"), ConsoleKey.RightArrow),
                new Keybinding(LanguageTools.GetLocalized("TI_IMAGEVIEWINTERACTIVE_BINDINGS_PREVIOUSPAGE"), ConsoleKey.PageUp),
                new Keybinding(LanguageTools.GetLocalized("TI_IMAGEVIEWINTERACTIVE_BINDINGS_NEXTPAGE"), ConsoleKey.PageDown),
                new Keybinding(LanguageTools.GetLocalized("TI_IMAGEVIEWINTERACTIVE_BINDINGS_GOTOBEGINNING"), ConsoleKey.Home),
                new Keybinding(LanguageTools.GetLocalized("TI_IMAGEVIEWINTERACTIVE_BINDINGS_GOTOENDING"), ConsoleKey.End),
            ];
            imageViewer.Keybindings.Add((bindings[0], (ui, _, _) => TextualUITools.ExitTui(ui)));
            imageViewer.Keybindings.Add((bindings[1], (ui, _, _) => ((ImageViewInteractive)ui).FitScale()));
            imageViewer.Keybindings.Add((bindings[2], (ui, _, _) => ((ImageViewInteractive)ui).MoveUp()));
            imageViewer.Keybindings.Add((bindings[3], (ui, _, _) => ((ImageViewInteractive)ui).MoveDown()));
            imageViewer.Keybindings.Add((bindings[4], (ui, _, _) => ((ImageViewInteractive)ui).MoveBackward()));
            imageViewer.Keybindings.Add((bindings[5], (ui, _, _) => ((ImageViewInteractive)ui).MoveForward()));
            imageViewer.Keybindings.Add((bindings[6], (ui, _, _) => ((ImageViewInteractive)ui).PreviousPage()));
            imageViewer.Keybindings.Add((bindings[7], (ui, _, _) => ((ImageViewInteractive)ui).NextPage()));
            imageViewer.Keybindings.Add((bindings[8], (ui, _, _) => ((ImageViewInteractive)ui).Beginning()));
            imageViewer.Keybindings.Add((bindings[9], (ui, _, _) => ((ImageViewInteractive)ui).End()));

            // Run the TUI
            TextualUITools.RunTui(imageViewer);
        }

        /// <inheritdoc/>
        public override string Render()
        {
            var builder = new StringBuilder();

            // Now, render the keybindings
            builder.Append(RenderKeybindings());

            // Now, render the image with the current selection
            builder.Append(RenderContentsWithSelection());
            return builder.ToString();
        }

        private string RenderKeybindings()
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
        }

        private string RenderContentsWithSelection()
        {
            var renderer = new ImageView(pixels)
            {
                Width = ConsoleWrapper.WindowWidth,
                Height = ConsoleWrapper.WindowHeight - 1,
                ColumnOffset = lineColIdx,
                RowOffset = lineIdx,
                Fit = fit,
            };
            return renderer.Render();
        }

        private void FitScale()
        {
            if (!fit)
            {
                UpdateColumnIndex(0);
                UpdateLineIndex(0);
            }
            fit = !fit;
        }

        private void MoveBackward() =>
            UpdateColumnIndex(lineColIdx - 1);

        private void MoveForward() =>
            UpdateColumnIndex(lineColIdx + 1);

        private void MoveUp() =>
            UpdateLineIndex(lineIdx - 1);

        private void MoveDown() =>
            UpdateLineIndex(lineIdx + 1);

        private void PreviousPage()
        {
            int imageHeight = pixels.GetLength(1);
            int lineLinesPerPage = ConsoleWrapper.WindowHeight - 1;
            int currentPage = lineIdx / lineLinesPerPage;
            int startIndex = lineLinesPerPage * currentPage;
            if (startIndex > imageHeight)
                startIndex = imageHeight;
            UpdateLineIndex(startIndex - 1 < 0 ? 0 : startIndex - 1);
        }

        private void NextPage()
        {
            int imageHeight = pixels.GetLength(1);
            int lineLinesPerPage = ConsoleWrapper.WindowHeight - 1;
            int currentPage = lineIdx / lineLinesPerPage;
            int endIndex = lineLinesPerPage * (currentPage + 1);
            if (endIndex > imageHeight - 1)
                endIndex = imageHeight - 1;
            UpdateLineIndex(endIndex);
        }

        private void Beginning() =>
            UpdateLineIndex(0);

        private void End()
        {
            int imageHeight = pixels.GetLength(1);
            UpdateLineIndex(imageHeight - 1);
        }

        private void UpdateLineIndex(int lnIdx)
        {
            if (fit)
                return;
            int imageHeight = pixels.GetLength(1);
            lineIdx = lnIdx;
            if (lineIdx + ConsoleWrapper.WindowHeight - 1 > imageHeight - 1)
                lineIdx = imageHeight - ConsoleWrapper.WindowHeight + 1;
            if (lineIdx < 0)
                lineIdx = 0;
            UpdateColumnIndex(lineColIdx);
        }

        private void UpdateColumnIndex(int clIdx)
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
            if (lineColIdx + ConsoleWrapper.WindowWidth > imageWidth - 1)
                lineColIdx = imageWidth - ConsoleWrapper.WindowWidth;
            if (lineColIdx < 0)
                lineColIdx = 0;
            RequireRefresh();
        }
    }
}
