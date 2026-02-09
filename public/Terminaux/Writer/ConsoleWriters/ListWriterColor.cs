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
using System.Collections.Generic;
using Terminaux.Base;
using Colorimetry;
using Terminaux.Themes.Colors;
using Terminaux.Writer.CyclicWriters.Simple;

namespace Terminaux.Writer.ConsoleWriters
{
    /// <summary>
    /// List writer with color support
    /// </summary>
    public static class ListWriterColor
    {
        /// <summary>
        /// Outputs a list and value into the terminal prompt plainly.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        public static void WriteListPlain<TKey, TValue>(Dictionary<TKey, TValue> List)
        {
            lock (TextWriterRaw.WriteLock)
            {
                try
                {
                    // Write the list
                    string buffered = RenderListPlain(List);
                    TextWriterRaw.WritePlain(buffered);
                }
                catch (Exception ex)
                {
                    ConsoleLogger.Error(ex, $"There is a serious error when printing text. {ex.Message}");
                }
            }
        }
        
        /// <summary>
        /// Outputs a list and value into the terminal prompt plainly.
        /// </summary>
        /// <param name="List">An enumerable that will be listed to the terminal prompt.</param>
        public static void WriteListPlain<T>(IEnumerable<T> List)
        {
            lock (TextWriterRaw.WriteLock)
            {
                try
                {
                    // Write the list
                    string buffered = RenderListPlain(List);
                    TextWriterRaw.WritePlain(buffered);
                }
                catch (Exception ex)
                {
                    ConsoleLogger.Error(ex, $"There is a serious error when printing text. {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Outputs a list and value into the terminal prompt.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        public static void WriteList<TKey, TValue>(Dictionary<TKey, TValue> List, ThemeColorType ListKeyColor = ThemeColorType.ListEntry, ThemeColorType ListValueColor = ThemeColorType.ListValue)
        {
            // Get the colors
            var keyColor = ThemeColorsTools.GetColor(ListKeyColor);
            var valueColor = ThemeColorsTools.GetColor(ListValueColor);

            WriteList(List, keyColor, valueColor);
        }

        /// <summary>
        /// Outputs a list and value into the terminal prompt.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        public static void WriteList<TKey, TValue>(Dictionary<TKey, TValue> List, Color ListKeyColor, Color ListValueColor)
        {
            lock (TextWriterRaw.WriteLock)
            {
                try
                {
                    // Write the list
                    string buffered = RenderList(List, ListKeyColor, ListValueColor);
                    TextWriterRaw.WritePlain(buffered);
                }
                catch (Exception ex)
                {
                    ConsoleLogger.Error(ex, $"There is a serious error when printing text. {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Outputs a list and value into the terminal prompt.
        /// </summary>
        /// <param name="List">An enumerable that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        public static void WriteList<T>(IEnumerable<T> List, ThemeColorType ListKeyColor = ThemeColorType.ListEntry, ThemeColorType ListValueColor = ThemeColorType.ListValue)
        {
            // Get the colors
            var keyColor = ThemeColorsTools.GetColor(ListKeyColor);
            var valueColor = ThemeColorsTools.GetColor(ListValueColor);

            WriteList(List, keyColor, valueColor);
        }

        /// <summary>
        /// Outputs a list and value into the terminal prompt.
        /// </summary>
        /// <param name="List">An enumerable that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        public static void WriteList<T>(IEnumerable<T> List, Color ListKeyColor, Color ListValueColor)
        {
            lock (TextWriterRaw.WriteLock)
            {
                try
                {
                    // Write the list
                    string buffered = RenderList(List, ListKeyColor, ListValueColor);
                    TextWriterRaw.WritePlain(buffered);
                }
                catch (Exception ex)
                {
                    ConsoleLogger.Error(ex, $"There is a serious error when printing text. {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Renders a list and value.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <returns>A list without the new line at the end</returns>
        public static string RenderListPlain<TKey, TValue>(Dictionary<TKey, TValue> List)
        {
            var list = new Listing()
            {
                Objects = List,
                UseColors = false
            };
            return list.Render();
        }

        /// <summary>
        /// Renders a list and value.
        /// </summary>
        /// <param name="List">An enumerable that will be listed to the terminal prompt.</param>
        /// <returns>A list without the new line at the end</returns>
        public static string RenderListPlain<T>(IEnumerable<T> List)
        {
            var list = new Listing()
            {
                Objects = List,
                UseColors = false
            };
            return list.Render();
        }

        /// <summary>
        /// Renders a list and value.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        /// <returns>A list without the new line at the end</returns>
        public static string RenderList<TKey, TValue>(Dictionary<TKey, TValue> List, ThemeColorType ListKeyColor = ThemeColorType.ListEntry, ThemeColorType ListValueColor = ThemeColorType.ListValue)
        {
            // Get the colors
            var keyColor = ThemeColorsTools.GetColor(ListKeyColor);
            var valueColor = ThemeColorsTools.GetColor(ListValueColor);

            return RenderList(List, keyColor, valueColor);
        }

        /// <summary>
        /// Renders a list and value.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        /// <returns>A list without the new line at the end</returns>
        public static string RenderList<TKey, TValue>(Dictionary<TKey, TValue> List, Color ListKeyColor, Color ListValueColor)
        {
            var list = new Listing()
            {
                Objects = List,
                KeyColor = ListKeyColor,
                ValueColor = ListValueColor,
            };
            return list.Render();
        }

        /// <summary>
        /// Renders a list and value.
        /// </summary>
        /// <param name="List">An enumerable that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        /// <returns>A list without the new line at the end</returns>
        public static string RenderList<T>(IEnumerable<T> List, ThemeColorType ListKeyColor = ThemeColorType.ListEntry, ThemeColorType ListValueColor = ThemeColorType.ListValue)
        {
            // Get the colors
            var keyColor = ThemeColorsTools.GetColor(ListKeyColor);
            var valueColor = ThemeColorsTools.GetColor(ListValueColor);

            return RenderList(List, keyColor, valueColor);
        }

        /// <summary>
        /// Renders a list and value.
        /// </summary>
        /// <param name="List">An enumerable that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        /// <returns>A list without the new line at the end</returns>
        public static string RenderList<T>(IEnumerable<T> List, Color ListKeyColor, Color ListValueColor)
        {
            var list = new Listing()
            {
                Objects = List,
                KeyColor = ListKeyColor,
                ValueColor = ListValueColor,
            };
            return list.Render();
        }
    }
}
