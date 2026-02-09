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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Terminaux.Base.Extensions;
using Colorimetry;
using Terminaux.Themes.Colors;

namespace Terminaux.Writer.CyclicWriters.Simple
{
    /// <summary>
    /// Listing renderable
    /// </summary>
    public class Listing : SimpleCyclicWriter
    {
        private Color keyColor = ThemeColorsTools.GetColor(ThemeColorType.ListEntry);
        private Color valueColor = ThemeColorsTools.GetColor(ThemeColorType.ListValue);
        private bool useColors = true;

        /// <summary>
        /// List key color
        /// </summary>
        public Color KeyColor
        {
            get => keyColor;
            set => keyColor = value;
        }

        /// <summary>
        /// List value color
        /// </summary>
        public Color ValueColor
        {
            get => valueColor;
            set => valueColor = value;
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
        /// A list or an array of objects
        /// </summary>
        public IEnumerable? Objects { get; set; }

        /// <summary>
        /// A stringifier function that converts the object based on its type to a string
        /// </summary>
        public Func<object, string>? Stringifier { get; set; }

        /// <summary>
        /// A stringifier function that converts the object (a key in the dictionary) based on its type to a string
        /// </summary>
        public Func<object, string>? KeyStringifier { get; set; }

        /// <summary>
        /// A stringifier function that converts the object (a value in the dictionary) based on its type to a string
        /// </summary>
        public Func<object, string>? ValueStringifier { get; set; }

        /// <summary>
        /// A stringifier function that converts the object based on an array or an enumerable to a string
        /// </summary>
        public Func<object, string>? RecursiveStringifier { get; set; }

        /// <summary>
        /// Renders a list of elements from an array of objects
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public override string Render()
        {
            if (Objects is null)
                return "";

            var listBuilder = new StringBuilder();
            int EntryNumber = 1;

            // Get the dictionary keys first in case we got a dictionary
            object[] keys = [];
            if (Objects is IDictionary dict)
            {
                keys = new object[dict.Count];
                dict.Keys.CopyTo(keys, 0);
            }

            // Process each list entry
            foreach (var ListEntry in Objects)
            {
                if (ListEntry is IEnumerable enums && ListEntry is not string)
                {
                    var strings = new List<object>();
                    foreach (var Value in enums)
                        strings.Add(RecursiveStringifier is not null ? RecursiveStringifier(Value) : Value);
                    string valuesString = string.Join(", ", strings);
                    listBuilder.AppendLine(
                        new ListEntry()
                        {
                            Entry = $"{EntryNumber}",
                            Value = valuesString,
                            KeyColor = KeyColor,
                            ValueColor = ValueColor,
                            UseColors = UseColors,
                        }.Render()
                    );
                }
                else if (Objects is IDictionary dictionary)
                {
                    var key = keys[EntryNumber - 1];
                    var value = dictionary[key];
                    listBuilder.AppendLine(
                        new ListEntry()
                        {
                            Entry = KeyStringifier is not null && key is not null ? new Func<object, string>((obj) => KeyStringifier(obj)).Invoke(key) : key?.ToString() ?? "<<null>>",
                            Value = ValueStringifier is not null && value is not null ? new Func<object, string>((obj) => ValueStringifier(obj)).Invoke(value) : value?.ToString() ?? "<<null>>",
                            KeyColor = KeyColor,
                            ValueColor = ValueColor,
                            UseColors = UseColors,
                        }.Render()
                    );
                }
                else
                    listBuilder.AppendLine(
                        new ListEntry()
                        {
                            Entry = $"{EntryNumber}",
                            Value = (Stringifier is not null ? Stringifier(ListEntry) : ListEntry).ToString(),
                            KeyColor = KeyColor,
                            ValueColor = ValueColor,
                            UseColors = UseColors,
                        }.Render()
                    );
                EntryNumber += 1;
            }
            if (UseColors)
                listBuilder.Append(ConsoleColoring.RenderRevertForeground());
            return listBuilder.ToString();
        }

        /// <summary>
        /// Makes a new instance of the array element list renderer
        /// </summary>
        public Listing()
        { }
    }
}
