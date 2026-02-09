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
using Terminaux.Base;
using Colorimetry.Data;
using Terminaux.Inputs.Interactive;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters.Simple;

namespace Terminaux.Console.Fixtures.Cases.CaseData
{
    internal class TextualUiRefreshTestData : TextualUI
    {
        private bool redBoxVisible = true;
        private bool greenBoxVisible = true;
        private bool rgbBoxVisible = true;
        private long renders = 0;
        private bool paused = false;
        private readonly Keybinding[] bindings =
        [
            new("Exit", ConsoleKey.Escape),
            new("Show/hide red box", ConsoleKey.R),
            new("Show/hide green box", ConsoleKey.G),
            new("Show/hide RGB box", ConsoleKey.B),
            new("Increase FPS", ConsoleKey.UpArrow),
            new("Decrease FPS", ConsoleKey.DownArrow),
            new("Pause/Resume", ConsoleKey.Spacebar),
        ];

        public override string Render()
        {
            StringBuilder builder = new();

            // Red box
            if (redBoxVisible)
            {
                int consoleHalf = ConsoleWrapper.WindowWidth / 2;
                int startX = consoleHalf - (consoleHalf * 3 / 4);
                int endX = consoleHalf + (consoleHalf / 4);
                int y = 2;
                int height = ConsoleWrapper.WindowHeight - y - 7;
                int width = endX - startX;
                var box = new Box()
                {
                    Left = startX,
                    Top = y,
                    Width = width,
                    Height = height,
                    Color = ConsoleColors.Red,
                };
                builder.Append(box.Render());
            }

            // Green box
            if (greenBoxVisible)
            {
                int consoleHalf = ConsoleWrapper.WindowWidth / 2;
                int startX = consoleHalf - (consoleHalf * 3 / 4) + 4;
                int endX = consoleHalf + (consoleHalf / 4) + 4;
                int y = 4;
                int height = ConsoleWrapper.WindowHeight - y - 5;
                int width = endX - startX;
                var box = new Box()
                {
                    Left = startX,
                    Top = y,
                    Width = width,
                    Height = height,
                    Color = ConsoleColors.Lime,
                };
                builder.Append(box.Render());
            }

            // RGB box
            if (rgbBoxVisible)
            {
                int consoleHalf = ConsoleWrapper.WindowWidth / 2;
                int startX = consoleHalf - (consoleHalf * 3 / 4) + 8;
                int endX = consoleHalf + (consoleHalf / 4) + 8;
                int y = 6;
                int height = ConsoleWrapper.WindowHeight - y - 3;
                int width = endX - startX;
                var box = new Box()
                {
                    Left = startX,
                    Top = y,
                    Width = width,
                    Height = height,
                    Color = new($"hsl:{renders % 360};50;50"),
                };
                builder.Append(box.Render());
            }

            // Keybindings
            var keybindings = new Keybindings()
            {
                KeybindingList = bindings,
                Width = ConsoleWrapper.WindowWidth - 1,
            };
            builder.Append(RendererTools.RenderRenderable(keybindings, new(0, ConsoleWrapper.WindowHeight - 1)));

            // Refresh rate and frame number count
            builder.Append(new AlignedText($"Delay: [Lime]{RefreshDelay}[/] ms, frame [Lime]{renders}[/]")
            {
                Top = 0,
                Settings = new() { Alignment = TextAlignment.Middle }
            }.Render());

            // Return the result
            renders++;
            return builder.ToString();
        }

        internal TextualUiRefreshTestData()
        {
            Keybindings.Add((bindings[0], (ui, _, _) => TextualUITools.ExitTui(ui)));
            Keybindings.Add((bindings[1], (ui, _, _) =>
            {
                redBoxVisible = !redBoxVisible;
                ui.RequireRefresh();
            }
            ));
            Keybindings.Add((bindings[2], (ui, _, _) =>
            {
                greenBoxVisible = !greenBoxVisible;
                ui.RequireRefresh();
            }
            ));
            Keybindings.Add((bindings[3], (ui, _, _) =>
            {
                rgbBoxVisible = !rgbBoxVisible;
                ui.RequireRefresh();
            }
            ));
            Keybindings.Add((bindings[4], (_, _, _) =>
            {
                if (paused)
                    return;
                RefreshDelay--;
                if (RefreshDelay <= 20)
                    RefreshDelay = 20;
            }
            ));
            Keybindings.Add((bindings[5], (_, _, _) =>
            {
                if (paused)
                    return;
                RefreshDelay++;
                if (RefreshDelay >= 1000)
                    RefreshDelay = 1000;
            }
            ));
            Keybindings.Add((bindings[6], (_, _, _) =>
            {
                paused = !paused;
                if (paused)
                    RefreshDelay = 0;
                else
                    RefreshDelay = 40;
            }
            ));
            RefreshDelay = 40;
        }
    }
}
