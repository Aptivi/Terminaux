﻿//
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
        private readonly List<Func<string>> dynamicBuffers = [];

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
            AddDynamicText(() => $"{text}{CharManager.NewLine}");

        /// <summary>
        /// Adds a dynamic text to the buffer
        /// </summary>
        /// <param name="textFunc">Text to add to the dynamic buffer queue</param>
        public void AddDynamicText(Func<string> textFunc)
        {
            if (textFunc is null)
                return;

            dynamicBuffers.Add(textFunc);
        }

        /// <summary>
        /// Adds the VT sequence to set the left cursor position
        /// </summary>
        /// <param name="left">Zero-based left position</param>
        public void LeftPosition(int left) =>
            Position(left, ConsoleWrapper.CursorTop);

        /// <summary>
        /// Adds the VT sequence to set the top cursor position
        /// </summary>
        /// <param name="top">Zero-based top position</param>
        public void TopPosition(int top) =>
            Position(ConsoleWrapper.CursorLeft, top);

        /// <summary>
        /// Adds the VT sequence to set the cursor position
        /// </summary>
        /// <param name="left">Zero-based left position</param>
        /// <param name="top">Zero-based top position</param>
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
        public void BackgroundColor(Color color, bool forceTrue = false)
        {
            string colorSeq = forceTrue ? color.VTSequenceBackgroundTrueColor : ColorTools.RenderSetConsoleColor(color, true);
            AddText(colorSeq);
        }

        /// <summary>
        /// Adds the VT sequence to reset the colors
        /// </summary>
        public void ResetColors()
        {
            ResetForegroundColor();
            ResetBackgroundColor();
        }

        /// <summary>
        /// Adds the VT sequence to reset the foreground color
        /// </summary>
        public void ResetForegroundColor() =>
            AddText($"{Convert.ToChar(0x1B)}[39m");

        /// <summary>
        /// Adds the VT sequence to reset the background color
        /// </summary>
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
            var finalBuffer = new StringBuilder();
            foreach (var dynamicBuffer in dynamicBuffers)
                finalBuffer.Append(dynamicBuffer());
            return finalBuffer.ToString();
        }

        /// <summary>
        /// Makes a new instance of the screen part
        /// </summary>
        public ScreenPart()
        { }
    }
}
