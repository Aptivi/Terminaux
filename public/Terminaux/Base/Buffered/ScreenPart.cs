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
using System.Collections.Generic;
using System.Text;
using Textify.General;

namespace Terminaux.Base.Buffered
{
    /// <summary>
    /// Buffered screen part
    /// </summary>
    public class ScreenPart
    {
        private Guid id = Guid.NewGuid();
        private int order = 0;
        private bool visible = true;
        private readonly List<(bool visible, Func<string> buffer)> dynamicBuffers = [];

        /// <summary>
        /// Order to use while buffering the screen.
        /// </summary>
        /// <remarks>
        /// This is compared in an ascending way so that the screen management system sorts the parts from the least important to
        /// the most important. For example, parts that are in order number 0 will get buffered before parts that have their
        /// order number of 1, and so on.
        /// </remarks>
        public int Order
        {
            get => order;
            set => order = value;
        }

        /// <summary>
        /// Whether this screen part is visible or not. To specify specific buffer visibility, use GetBufferVisibility() and SetBufferVisibility().
        /// </summary>
        public bool Visible
        {
            get => visible;
            set => visible = value;
        }

        /// <summary>
        /// Screen part identification
        /// </summary>
        public Guid Id =>
            id;

        /// <summary>
        /// Adds a text to the buffer
        /// </summary>
        /// <param name="text">Text to write to the buffer builder</param>
        public void AddText(string text) =>
            AddDynamicText(() => text);

        /// <summary>
        /// Adds a text to the buffer with a new line
        /// </summary>
        /// <param name="text">Text to write to the buffer builder</param>
        public void AddTextLine(string text) =>
            AddDynamicText(() => $"{text}\n");

        /// <summary>
        /// Adds a dynamic text to the buffer
        /// </summary>
        /// <param name="textFunc">Text to add to the dynamic buffer queue</param>
        public void AddDynamicText(Func<string> textFunc)
        {
            if (textFunc is null)
                return;

            dynamicBuffers.Add((true, textFunc));
        }

        /// <summary>
        /// Clears the buffer
        /// </summary>
        public void Clear() =>
            dynamicBuffers.Clear();

        /// <summary>
        /// Gets the resulting buffer
        /// </summary>
        /// <returns>The resulting buffer</returns>
        public string GetBuffer()
        {
            if (!visible)
                return "";
            var finalBuffer = new StringBuilder();
            for (int i = 0; i < dynamicBuffers.Count; i++)
            {
                (bool visible, Func<string> buffer) dynamicBuffer = dynamicBuffers[i];
                ConsoleLogger.Debug("Dynamic buffer {0} visibility ({1}).", i, dynamicBuffer.visible);
                if (dynamicBuffer.visible)
                    finalBuffer.Append(dynamicBuffer.buffer());
            }
            return finalBuffer.ToString();
        }

        /// <summary>
        /// Gets the buffer visibility status
        /// </summary>
        /// <param name="buffer">Buffer index</param>
        /// <returns>True if visible; false otherwise.</returns>
        /// <exception cref="TerminauxException">If the buffer index is less than zero or greater than or equal to the dynamic buffer count</exception>
        public bool GetBufferVisibility(int buffer)
        {
            if (buffer < 0 || buffer >= dynamicBuffers.Count)
                throw new TerminauxException(LanguageTools.GetLocalized("T_BB_SCREENPART_EXCEPTION_IDXOUTOFRANGE").FormatString(dynamicBuffers));
            return dynamicBuffers[buffer].visible;
        }

        /// <summary>
        /// Sets the buffer visibility status
        /// </summary>
        /// <param name="buffer">Buffer index</param>
        /// <param name="visible">Buffer visibility status</param>
        /// <exception cref="TerminauxException">If the buffer index is less than zero or greater than or equal to the dynamic buffer count</exception>
        public void SetBufferVisibility(int buffer, bool visible)
        {
            if (buffer < 0 || buffer >= dynamicBuffers.Count)
                throw new TerminauxException(LanguageTools.GetLocalized("T_BB_SCREENPART_EXCEPTION_IDXOUTOFRANGE").FormatString(dynamicBuffers));
            dynamicBuffers[buffer] = (visible, dynamicBuffers[buffer].buffer);
        }

        /// <summary>
        /// Makes a new instance of the screen part
        /// </summary>
        public ScreenPart()
        { }
    }
}
