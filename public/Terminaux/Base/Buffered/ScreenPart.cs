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
using System.Collections.Generic;
using System.Text;
using Terminaux.Colors;
using Terminaux.Sequences.Builder.Types;

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
        /// Adds the VT sequence to set the left cursor position
        /// </summary>
        /// <param name="left">Zero-based left position</param>
        [Obsolete("Use " + nameof(AddDynamicText) + " and relevant functions instead")]
        public void LeftPosition(int left) =>
            Position(left, ConsoleWrapper.CursorTop);

        /// <summary>
        /// Adds the VT sequence to set the top cursor position
        /// </summary>
        /// <param name="top">Zero-based top position</param>
        [Obsolete("Use " + nameof(AddDynamicText) + " and relevant functions instead")]
        public void TopPosition(int top) =>
            Position(ConsoleWrapper.CursorLeft, top);

        /// <summary>
        /// Adds the VT sequence to set the cursor position
        /// </summary>
        /// <param name="left">Zero-based left position</param>
        /// <param name="top">Zero-based top position</param>
        [Obsolete("Use " + nameof(AddDynamicText) + " and relevant functions instead")]
        public void Position(int left, int top)
        {
            string pos = CsiSequences.GenerateCsiCursorPosition(left + 1, top + 1);
            AddText(pos);
        }

        /// <summary>
        /// Adds the VT sequence to set the foreground color
        /// </summary>
        /// <param name="color">Color to use for foreground color</param>
        /// <param name="forceTrue">Forces the usage of the true color</param>
        [Obsolete("Use " + nameof(AddDynamicText) + " and relevant functions instead")]
        public void ForegroundColor(Color color, bool forceTrue = false)
        {
            string colorSeq = forceTrue ? color.VTSequenceForegroundTrueColor : ColorTools.RenderSetConsoleColor(color);
            AddText(colorSeq);
        }

        /// <summary>
        /// Adds the VT sequence to set the background color
        /// </summary>
        /// <param name="color">Color to use for background color</param>
        /// <param name="forceTrue">Forces the usage of the true color</param>
        [Obsolete("Use " + nameof(AddDynamicText) + " and relevant functions instead")]
        public void BackgroundColor(Color color, bool forceTrue = false)
        {
            string colorSeq = forceTrue ? color.VTSequenceBackgroundTrueColor : ColorTools.RenderSetConsoleColor(color, true);
            AddText(colorSeq);
        }

        /// <summary>
        /// Adds the VT sequence to reset the colors
        /// </summary>
        [Obsolete("Use " + nameof(AddDynamicText) + " and relevant functions instead")]
        public void ResetColors()
        {
            ResetForegroundColor();
            ResetBackgroundColor();
        }

        /// <summary>
        /// Adds the VT sequence to reset the foreground color
        /// </summary>
        [Obsolete("Use " + nameof(AddDynamicText) + " and relevant functions instead")]
        public void ResetForegroundColor() =>
            AddText($"{Convert.ToChar(0x1B)}[39m");

        /// <summary>
        /// Adds the VT sequence to reset the background color
        /// </summary>
        [Obsolete("Use " + nameof(AddDynamicText) + " and relevant functions instead")]
        public void ResetBackgroundColor() =>
            AddText($"{Convert.ToChar(0x1B)}[49m");

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
            foreach (var dynamicBuffer in dynamicBuffers)
                if (dynamicBuffer.visible)
                    finalBuffer.Append(dynamicBuffer.buffer());
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
                throw new TerminauxException($"Buffer index may not be less than zero or greater than {dynamicBuffers.Count} buffers.");
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
                throw new TerminauxException($"Buffer index may not be less than zero or greater than {dynamicBuffers.Count} buffers.");
            dynamicBuffers[buffer] = (visible, dynamicBuffers[buffer].buffer);
        }

        /// <summary>
        /// Makes a new instance of the screen part
        /// </summary>
        public ScreenPart()
        { }
    }
}
