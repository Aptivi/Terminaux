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

using System.Linq;
using Terminaux.Base.Extensions;
using Textify.Data.Cowsay;
using Textify.General;

namespace Terminaux.Writer.CyclicWriters.Renderer.Tools
{
    /// <summary>
    /// Cowsay tools
    /// </summary>
    public static class CowsayTools
    {
        /// <summary>
        /// Gets the cowsay lines
        /// </summary>
        /// <param name="Text">Text</param>
        /// <param name="CowsayFont">Target cowsay font</param>
        /// <param name="think">Thinking mode</param>
        /// <param name="width">Max width of the resultant cowsay rendered text. Pass 0 for single-line.</param>
        public static string[] GetCowsayLines(string Text, CowName CowsayFont, bool think = false, int width = 0)
        {
            ICow cowsay = DefaultCattleFarmer.RearCowWithDefaults(CowNameMapping.GetCowNameFrom(CowsayFont)).Result;
            string spoken = think ? cowsay.Think(Text) : cowsay.Speak(Text);
            return spoken.GetWrappedSentences(width);
        }

        /// <summary>
        /// Gets the cowsay text height
        /// </summary>
        /// <param name="Text">Text</param>
        /// <param name="CowsayFont">Target cowsay font</param>
        /// <param name="think">Thinking mode</param>
        /// <param name="width">Max width of the resultant cowsay rendered text. Pass 0 for single-line.</param>
        public static int GetCowsayHeight(string Text, CowName CowsayFont, bool think = false, int width = 0) =>
            GetCowsayLines(Text, CowsayFont, think, width).Length;

        /// <summary>
        /// Gets the cowsay text width
        /// </summary>
        /// <param name="Text">Text</param>
        /// <param name="CowsayFont">Target cowsay font</param>
        /// <param name="think">Thinking mode</param>
        /// <param name="width">Max width of the resultant cowsay rendered text. Pass 0 for single-line.</param>
        public static int GetCowsayWidth(string Text, CowName CowsayFont, bool think = false, int width = 0) =>
            GetCowsayLines(Text, CowsayFont, think, width).Max(ConsoleChar.EstimateCellWidth);
    }
}
