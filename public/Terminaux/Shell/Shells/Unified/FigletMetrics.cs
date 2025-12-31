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

using Terminaux.Shell.Commands;
using Terminaux.Base;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Colors.Themes.Colors;
using Textify.Data.Figlet;
using System.Text;

namespace Terminaux.Shell.Shells.Unified
{
    /// <summary>
    /// Gets figlet metrics
    /// </summary>
    class FigletMetricsCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Flags
            bool detailed = !parameters.ContainsSwitch("-detailed");
            bool print = !parameters.ContainsSwitch("-quiet");

            // Text to process
            string text = parameters.ArgumentsList[0];
            string font = parameters.ArgumentsList.Length > 1 ? parameters.ArgumentsList[1] : "small";
            string widthStr = parameters.ArgumentsList.Length > 2 ? parameters.ArgumentsList[2] : "0";
            if (!int.TryParse(widthStr, out var width))
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_UNIFIED_TOFIGLET_WIDTHINVALID"), ThemeColorType.Error);
                return 1;
            }

            // Make a figlet font instance and get its metrics
            var metricsBuilder = new StringBuilder("[");
            var figletFont = FigletTools.GetFigletFont(font);
            var figletHeight = FigletTools.GetFigletHeight(text, figletFont, width);
            var figletWidth = FigletTools.GetFigletWidth(text, figletFont, width);
            metricsBuilder.Append($"{figletHeight}, {figletWidth}");

            // Get some additional metrics
            if (detailed)
            {
                metricsBuilder.Append(", ");
                var figletName = figletFont.Name;
                var figletBaseline = figletFont.Baseline;
                var figletDirection = figletFont.Direction;
                var figletBaseHeight = figletFont.Height;
                var figletComments = figletFont.Comments;
                metricsBuilder.Append($"{figletName}, {figletBaseline}, {figletDirection}, {figletBaseHeight}, {string.Join(", ", figletComments)}");
            }
            metricsBuilder.Append("]");

            // Set the MESH variable to contain the result
            string processed = metricsBuilder.ToString();
            variableValue = processed;
            if (print)
                TextWriterRaw.WritePlain(variableValue);
            return 0;
        }
    }
}
