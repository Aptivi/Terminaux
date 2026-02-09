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

using Spectre.Console;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Colorimetry;
using Terminaux.Sequences;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer.Markup;
using Textify.General;
using Table = Spectre.Console.Table;
using SBarChart = Spectre.Console.BarChart;
using SBreakdownChart = Spectre.Console.BreakdownChart;
using SFigletText = Spectre.Console.FigletText;
using SCanvas = Spectre.Console.Canvas;
using STextPath = Spectre.Console.TextPath;
using SColor = Spectre.Console.Color;
using TColor = Colorimetry.Color;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Spectre
{
    /// <summary>
    /// Translation tools
    /// </summary>
	public static class TranslationTools
    {
        /// <summary>
        /// Returns a compatible Spectre.Console <see cref="SColor"/> from Terminaux's <see cref="TColor"/>
        /// </summary>
        /// <param name="color">Terminaux's color instance</param>
        /// <returns>Spectre.Console's color instance</returns>
        public static SColor GetColor(TColor color) =>
            new((byte)color.RGB.R, (byte)color.RGB.G, (byte)color.RGB.B);

        /// <summary>
        /// Returns a compatible Terminaux <see cref="TColor"/> from Spectre.Console's <see cref="SColor"/>
        /// </summary>
        /// <param name="color">Spectre.Console's color instance</param>
        /// <returns>Terminaux's color instance</returns>
        public static TColor GetColor(SColor color) =>
            new(color.R, color.G, color.B);

        /// <summary>
        /// Returns a compatible Spectre.Console <see cref="Markup"/> from Terminaux's <see cref="Mark"/>
        /// </summary>
        /// <param name="mark">Terminaux's markup instance</param>
        /// <returns>Spectre.Console's markup instance</returns>
        public static Markup GetMarkup(Mark mark)
        {
            StringBuilder originalMarkup = new(mark.Markup);

            // Check to see if there are any properties we need to replace
            var markupInfos = MarkupTools.GetMarkupInfo(mark);
            List<string> effectNames = [.. MarkupTools.effects.Select((effect) => effect.MarkupTag)];
            for (int i = markupInfos.Length - 1; i >= 0; i--)
            {
                // Split the representations
                MarkupInfo markupInfo = markupInfos[i];
                if (markupInfo.Escape)
                    continue;
                string representationGroup = markupInfo.Representation.RemovePrefix("[").RemoveSuffix("]");
                List<string> representations = [.. representationGroup.Split(' ')];

                // Parse the effect or the color
                for (int r = representations.Count - 1; r >= 0; r--)
                {
                    string representation = representations[r];
                    if (effectNames.Contains(representation))
                    {
                        // Spectre.Console doesn't have "standout"
                        if (representation == "standout")
                            representations.RemoveAt(r);
                    }
                    else if (ColorTools.TryParseColor(representation))
                    {
                        // Convert to RGB sequence
                        var parsedColor = new TColor(representation);
                        representations[r] = parsedColor.Hex;
                    }
                }

                // Apply the changes
                originalMarkup.Remove(markupInfo.EntranceIndex + 1, representationGroup.Length);
                originalMarkup.Insert(markupInfo.EntranceIndex + 1, string.Join(" ", representations));
            }
            return new(originalMarkup.ToString());
        }

        /// <summary>
        /// Returns a compatible Spectre.Console <see cref="Panel"/> from Terminaux's <see cref="BoxFrame"/>
        /// </summary>
        /// <param name="boxFrame">Terminaux's box frame instance</param>
        /// <returns>Spectre.Console's panel instance</returns>
        public static Panel GetPanel(BoxFrame boxFrame)
        {
            // Get all properties
            var width = boxFrame.Width;
            var height = boxFrame.Height;

            // Return the new panel
            string panelTitle = boxFrame.Text;
            panelTitle = VtSequenceTools.FilterVTSequences(panelTitle);
            var spectrePanel = new Panel("")
            {
                Header = new(panelTitle),
                Width = width + 2,
                Height = height + 2,
                Padding = new(boxFrame.Padding.Left, boxFrame.Padding.Top, boxFrame.Padding.Right, boxFrame.Padding.Bottom),
                Border = BoxBorder.Rounded,
            };
            return spectrePanel;
        }

        /// <summary>
        /// Returns a compatible Spectre.Console <see cref="Table"/> from Terminaux's <see cref="Writer.CyclicWriters.Graphical.Table"/>
        /// </summary>
        /// <param name="table">Terminaux's table instance</param>
        /// <returns>Spectre.Console's table instance</returns>
        public static Table GetTable(Writer.CyclicWriters.Graphical.Table table)
        {
            // Make a new table
            var spectreTable = new Table()
            {
                Width = table.Width,
                Title = new(table.Title),
                Border = TableBorder.Rounded,
                ShowHeaders = table.Header,
            };

            // Add rows and columns
            int columnsCount = table.Rows.GetLength(1);
            int rowsCount = table.Rows.GetLength(0);
            for (int x = 0; x < columnsCount; x++)
            {
                string column = table.Rows[0, x];
                spectreTable.AddColumn(column);
            }
            for (int y = table.Header ? 1 : 0; y < rowsCount; y++)
            {
                List<string> rows = [];
                for (int x = 0; x < columnsCount; x++)
                {
                    string row = table.Rows[y, x];
                    rows.Add(row);
                }
                spectreTable.AddRow([.. rows]);
            }

            // Return the new table
            return spectreTable;
        }

        /// <summary>
        /// Returns a compatible Spectre.Console <see cref="SBarChart"/> from Terminaux's <see cref="Writer.CyclicWriters.Graphical.BarChart"/>
        /// </summary>
        /// <param name="barChart">Terminaux's bar chart instance</param>
        /// <returns>Spectre.Console's bar chart instance</returns>
        public static SBarChart GetBarChart(Writer.CyclicWriters.Graphical.BarChart barChart)
        {
            // Make a new bar chart
            var spectreBarChart = new SBarChart()
            {
                Width = barChart.Width,
                ShowValues = barChart.Showcase,
            };

            // Add values
            foreach (var element in barChart.Elements)
            {
                var spectreColor = GetColor(element.Color);
                spectreBarChart.AddItem(element.Name, element.Value, spectreColor);
            }

            // Return the new bar chart
            return spectreBarChart;
        }

        /// <summary>
        /// Returns a compatible Spectre.Console <see cref="SBreakdownChart"/> from Terminaux's <see cref="Writer.CyclicWriters.Graphical.BreakdownChart"/>
        /// </summary>
        /// <param name="breakdownChart">Terminaux's breakdown chart instance</param>
        /// <returns>Spectre.Console's breakdown chart instance</returns>
        public static SBreakdownChart GetBreakdownChart(Writer.CyclicWriters.Graphical.BreakdownChart breakdownChart)
        {
            // Make a new breakdown chart
            var spectreBreakdownChart = new SBreakdownChart()
            {
                Width = breakdownChart.Width,
                Compact = !breakdownChart.Showcase,
                ShowTagValues = true,
                ShowTags = true,
            };

            // Add values
            foreach (var element in breakdownChart.Elements)
            {
                var spectreColor = GetColor(element.Color);
                spectreBreakdownChart.AddItem(element.Name, element.Value, spectreColor);
            }

            // Return the new breakdown chart
            return spectreBreakdownChart;
        }

        /// <summary>
        /// Returns a compatible Spectre.Console <see cref="Calendar"/> from Terminaux's <see cref="Calendars"/>
        /// </summary>
        /// <param name="calendars">Terminaux's calendar instance</param>
        /// <returns>Spectre.Console's calendar instance</returns>
        public static Calendar GetCalendar(Calendars calendars)
        {
            // Make a new calendar
            var spectreCalendar = new Calendar(calendars.Year, calendars.Month)
            {
                Border = TableBorder.Rounded,
                Culture = calendars.Culture,
            };

            // Return the new calendar
            return spectreCalendar;
        }

        /// <summary>
        /// Returns a compatible Spectre.Console <see cref="Justify"/> from Terminaux's <see cref="TextAlignment"/>
        /// </summary>
        /// <param name="alignment">Terminaux's text alignment</param>
        /// <returns>Spectre.Console's text alignment</returns>
        public static Justify GetAlignment(TextAlignment alignment) =>
            alignment switch
            {
                TextAlignment.Left => Justify.Left,
                TextAlignment.Middle => Justify.Center,
                TextAlignment.Right => Justify.Right,
                _ => Justify.Left
            };

        /// <summary>
        /// Returns a compatible Spectre.Console <see cref="SFigletText"/> from Terminaux's <see cref="AlignedFigletText"/>
        /// </summary>
        /// <param name="figletText">Terminaux's figlet text instance</param>
        /// <returns>Spectre.Console's figlet text instance</returns>
        public static SFigletText GetFigletText(AlignedFigletText figletText)
        {
            // Make a new figlet text
            var spectreFigletText = new SFigletText(figletText.Text)
            {
                Color = GetColor(figletText.ForegroundColor),
                Justification = GetAlignment(figletText.Settings.Alignment),
            };

            // Return the new figlet text
            return spectreFigletText;
        }

        /// <summary>
        /// Returns a compatible Spectre.Console <see cref="SCanvas"/> from Terminaux's <see cref="Writer.CyclicWriters.Graphical.Canvas"/>
        /// </summary>
        /// <param name="canvas">Terminaux's canvas instance</param>
        /// <returns>Spectre.Console's canvas instance</returns>
        public static SCanvas GetCanvas(Writer.CyclicWriters.Graphical.Canvas canvas)
        {
            // Make a new canvas
            var spectreCanvas = new SCanvas(canvas.Width, canvas.Height)
            {
                PixelWidth = canvas.DoubleWidth ? 2 : 1,
            };

            // Set the background pixels (if any)
            if (!canvas.Transparent)
                for (int x = 0; x < canvas.Width; x++)
                    for (int y = 0; y < canvas.Height; y++)
                        spectreCanvas.SetPixel(x, y, GetColor(canvas.Color));

            // Set the pixels
            foreach (var pixel in canvas.Pixels)
                spectreCanvas.SetPixel(pixel.ColumnIndex, pixel.RowIndex, GetColor(pixel.CellColor));

            // Return the new canvas
            return spectreCanvas;
        }

        /// <summary>
        /// Returns a compatible Spectre.Console <see cref="STextPath"/> from Terminaux's <see cref="Writer.CyclicWriters.Graphical.TextPath"/>
        /// </summary>
        /// <param name="textPath">Terminaux's text path instance</param>
        /// <returns>Spectre.Console's text path instance</returns>
        public static STextPath GetTextPath(Writer.CyclicWriters.Graphical.TextPath textPath)
        {
            // Make a new text path
            var spectreTextPath = new STextPath(textPath.PathText)
            {
                Justification = GetAlignment(textPath.Settings.Alignment),
                RootStyle = new(GetColor(textPath.RootDriveColor)),
                LeafStyle = new(GetColor(textPath.LastPathColor)),
                StemStyle = new(GetColor(textPath.SeparatorColor)),
                SeparatorStyle = new(GetColor(textPath.SeparatorColor)),
            };

            // Return the new text path
            return spectreTextPath;
        }
    }
}
