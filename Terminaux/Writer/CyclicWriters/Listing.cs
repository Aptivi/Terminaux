//
// Terminaux  Copyright (C) 2023-2024  Aptivi
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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Terminaux.Base.Structures;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Graphics;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Writer.CyclicWriters
{
    /// <summary>
    /// Listing renderable
    /// </summary>
    public class Listing : IStaticRenderable
    {
        private Color keyColor = ConsoleColors.Yellow;
        private Color valueColor = ConsoleColors.Olive;
        private bool customColor = false;

        /// <summary>
        /// List key color
        /// </summary>
        public Color KeyColor
        {
            get => keyColor;
            set
            {
                keyColor = value;
                customColor = true;
            }
        }

        /// <summary>
        /// List value color
        /// </summary>
        public Color ValueColor
        {
            get => valueColor;
            set
            {
                valueColor = value;
                customColor = true;
            }
        }

        /// <summary>
        /// A list or an array of objects
        /// </summary>
        public IEnumerable? Objects { get; set; }

        /// <summary>
        /// A stringifier function that converts the object based on its type to a string
        /// </summary>
        public Func<object, string>? Stringifier { get; set; }

        /// <summary>
        /// A stringifier function that converts the object based on an array or an enumerable to a string
        /// </summary>
        public Func<object, string>? RecursiveStringifier { get; set; }

        /// <summary>
        /// Renders a Listing segment group
        /// </summary>
        /// <returns>Rendered Listing text that will be used by the renderer</returns>
        public string Render() =>
            RenderList(Objects, KeyColor, ValueColor, customColor, Stringifier, RecursiveStringifier);

        internal static string RenderList(IEnumerable? List, Color ListKeyColor, Color ListValueColor, bool useColor, Func<object, string>? stringifier = null, Func<object, string>? recursiveStringifier = null)
        {
            if (List is null)
                return "";

            var listBuilder = new StringBuilder();
            int EntryNumber = 1;
            foreach (var ListEntry in List)
            {
                if (ListEntry is IEnumerable enums && ListEntry is not string)
                {
                    var strings = new List<object>();
                    foreach (var Value in enums)
                        strings.Add(recursiveStringifier is not null ? recursiveStringifier(Value) : Value);
                    string valuesString = string.Join(", ", strings);
                    listBuilder.AppendLine(
                        new ListEntry()
                        {
                            Entry = $"{EntryNumber}",
                            Value = valuesString,
                            KeyColor = ListKeyColor,
                            ValueColor = ListValueColor,
                        }.Render()
                    );
                }
                else
                    listBuilder.AppendLine(
                        new ListEntry()
                        {
                            Entry = $"{EntryNumber}",
                            Value = (string)(stringifier is not null ? stringifier(ListEntry) : ListEntry),
                            KeyColor = ListKeyColor,
                            ValueColor = ListValueColor,
                        }.Render()
                    );
                EntryNumber += 1;
            }
            if (useColor)
                listBuilder.Append(ColorTools.RenderRevertForeground());
            return listBuilder.ToString();
        }

        /// <summary>
        /// Makes a new instance of the Listing renderer
        /// </summary>
        public Listing()
        { }
    }
}
