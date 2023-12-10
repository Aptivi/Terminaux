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
using Textify.General;
using Textify.Sequences.Builder.Types;

namespace Terminaux.Base.Buffered
{
    /// <summary>
    /// Buffered screen part
    /// </summary>
    public class ScreenPart
    {
        private readonly List<Func<string>> dynamicBuffers = [];

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
            string colorSeq = forceTrue ? color.VTSequenceForegroundTrueColor : color.VTSequenceForeground;
            AddText(colorSeq);
        }

        /// <summary>
        /// Adds the VT sequence to set the background color
        /// </summary>
        /// <param name="color">Color to use for background color</param>
        /// <param name="forceTrue">Forces the usage of the true color</param>
        public void BackgroundColor(Color color, bool forceTrue = false)
        {
            string colorSeq = forceTrue ? color.VTSequenceBackgroundTrueColor : color.VTSequenceBackground;
            AddText(colorSeq);
        }

        /// <summary>
        /// Adds the VT sequence to reset the colors to the kernel-defined colors
        /// </summary>
        public void ResetColor()
        {
            AddText($"{Convert.ToChar(0x1B)}[39m");
            AddText($"{Convert.ToChar(0x1B)}[49m");
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
