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
using System.Linq;
using System.Text;
using Terminaux.Base.Extensions;
using Terminaux.Colors;

namespace Terminaux.Base.Buffered
{
    /// <summary>
    /// A screen instance to store your buffered screen parts
    /// </summary>
    public class Screen
    {
        private bool needsRefresh = true;
        private bool resetResize = true;
        private readonly ScreenPart clearPart = new();
        private readonly Dictionary<string, ScreenPart> screenParts = [];

        /// <summary>
        /// Buffered screen parts list to render one by one while buffering the console
        /// </summary>
        public ScreenPart[] ScreenParts =>
            [.. screenParts.Values];

        /// <summary>
        /// Whether to reset the resize state or not
        /// </summary>
        public bool ResetResize
        {
            get => resetResize;
            set => resetResize = value;
        }

        /// <summary>
        /// Adds the buffered part to the list of screen parts
        /// </summary>
        /// <param name="name">Screen buffer part name</param>
        /// <param name="part">Buffered screen part to add to the screen part list for buffering</param>
        /// <exception cref="TerminauxException"></exception>
        public void AddBufferedPart(string name, ScreenPart part)
        {
            if (part is null)
                throw new TerminauxException("You must specify the screen part.");
            if (string.IsNullOrEmpty(name))
                throw new TerminauxException("You must specify the screen name.");
            while (screenParts.ContainsKey(name))
                name += $" [{screenParts.Count}]";

            // Now, add the buffered part
            screenParts.Add(name, part);
        }

        /// <summary>
        /// Edits the buffered part in the list of screen parts
        /// </summary>
        /// <param name="idx">Part index</param>
        /// <param name="part">Buffered screen part to add to the screen part list for buffering</param>
        /// <exception cref="TerminauxException"></exception>
        public void EditBufferedPart(int idx, ScreenPart part)
        {
            if (idx < 0 || idx >= screenParts.Count)
                throw new TerminauxException("The specified part index is out of range.");
            if (part is null)
                throw new TerminauxException("You must specify the screen part.");

            // Now, edit the buffered part
            var kvp = screenParts.ElementAt(idx);
            EditBufferedPart(kvp.Key, part);
        }

        /// <summary>
        /// Edits the buffered part in the list of screen parts
        /// </summary>
        /// <param name="name">Screen buffer part name</param>
        /// <param name="part">Buffered screen part to add to the screen part list for buffering</param>
        /// <exception cref="TerminauxException"></exception>
        public void EditBufferedPart(string name, ScreenPart part)
        {
            if (!screenParts.ContainsKey(name))
                throw new TerminauxException("The specified part name is not found.");
            if (part is null)
                throw new TerminauxException("You must specify the screen part.");

            // Now, edit the buffered part
            screenParts[name] = part;
        }

        /// <summary>
        /// Edits the buffered part in the list of screen parts
        /// </summary>
        /// <param name="id">Screen buffer part GUID</param>
        /// <param name="part">Buffered screen part to add to the screen part list for buffering</param>
        /// <exception cref="TerminauxException"></exception>
        public void EditBufferedPart(Guid id, ScreenPart part)
        {
            var partSource = screenParts.FirstOrDefault((part) => part.Value.Id == id);
            if (partSource.Value is null)
                throw new TerminauxException("The specified part is not found.");
            if (part is null)
                throw new TerminauxException("You must specify the screen part.");

            // Now, edit the buffered part
            screenParts[partSource.Key] = part;
        }

        /// <summary>
        /// Removes the buffered part from the list of screen parts
        /// </summary>
        /// <param name="idx">Part index</param>
        /// <exception cref="TerminauxException"></exception>
        public void RemoveBufferedPart(int idx)
        {
            if (idx < 0 || idx >= screenParts.Count)
                throw new TerminauxException("The specified part index is out of range.");

            // Now, remove the buffered part
            var kvp = screenParts.ElementAt(idx);
            RemoveBufferedPart(kvp.Key);
        }

        /// <summary>
        /// Removes the buffered part from the list of screen parts
        /// </summary>
        /// <param name="name">Screen buffer part name</param>
        /// <exception cref="TerminauxException"></exception>
        public void RemoveBufferedPart(string name)
        {
            if (!screenParts.ContainsKey(name))
                throw new TerminauxException("The specified part name is not found.");

            // Now, remove the buffered part
            screenParts.Remove(name);
        }

        /// <summary>
        /// Removes the buffered part from the list of screen parts using the GUID
        /// </summary>
        /// <param name="id">Screen buffer part GUID</param>
        /// <exception cref="TerminauxException"></exception>
        public void RemoveBufferedPart(Guid id)
        {
            var part = screenParts.FirstOrDefault((part) => part.Value.Id == id);
            if (part.Value is null)
                throw new TerminauxException("The specified part is not found.");
            RemoveBufferedPart(part.Key);
        }

        /// <summary>
        /// Gets the buffered part from the list of screen parts
        /// </summary>
        /// <param name="idx">Part index</param>
        /// <exception cref="TerminauxException"></exception>
        public ScreenPart GetBufferedPart(int idx)
        {
            if (idx < 0 || idx >= screenParts.Count)
                throw new TerminauxException("The specified part index is out of range.");

            // Now, get the buffered part
            var kvp = screenParts.ElementAt(idx);
            return GetBufferedPart(kvp.Key);
        }

        /// <summary>
        /// Gets the buffered part from the list of screen parts
        /// </summary>
        /// <param name="name">Screen buffer part name</param>
        /// <exception cref="TerminauxException"></exception>
        public ScreenPart GetBufferedPart(string name)
        {
            if (!screenParts.TryGetValue(name, out ScreenPart part))
                throw new TerminauxException("The specified part name is not found.");

            // Now, get the buffered part
            return part;
        }

        /// <summary>
        /// Gets the buffered part from the list of screen parts
        /// </summary>
        /// <param name="id">Screen buffer part GUID</param>
        /// <exception cref="TerminauxException"></exception>
        public ScreenPart GetBufferedPart(Guid id)
        {
            var part = screenParts.FirstOrDefault((part) => part.Value.Id == id);
            if (part.Value is null)
                throw new TerminauxException("The specified part is not found.");

            // Now, get the buffered part
            return part.Value;
        }

        /// <summary>
        /// Checks the buffered part in the list of screen parts
        /// </summary>
        /// <param name="idx">Part index</param>
        /// <exception cref="TerminauxException"></exception>
        public bool CheckBufferedPart(int idx)
        {
            if (idx < 0 || idx >= screenParts.Count)
                return false;

            // Now, check the buffered part
            var kvp = screenParts.ElementAt(idx);
            return CheckBufferedPart(kvp.Key);
        }

        /// <summary>
        /// Checks the buffered part in the list of screen parts
        /// </summary>
        /// <param name="name">Screen buffer part name</param>
        /// <exception cref="TerminauxException"></exception>
        public bool CheckBufferedPart(string name)
        {
            if (!screenParts.ContainsKey(name))
                return false;
            return true;
        }

        /// <summary>
        /// Checks the buffered part in the list of screen parts using the GUID
        /// </summary>
        /// <param name="id">Screen buffer part GUID</param>
        /// <exception cref="TerminauxException"></exception>
        public bool CheckBufferedPart(Guid id)
        {
            var part = screenParts.FirstOrDefault((part) => part.Value.Id == id);
            if (part.Value is null)
                return false;
            return true;
        }

        /// <summary>
        /// Removes all the buffered parts from the list of screen parts
        /// </summary>
        public void RemoveBufferedParts() =>
            screenParts.Clear();

        /// <summary>
        /// Gets a buffer from all the buffered screen parts
        /// </summary>
        /// <returns>A buffer that is to be written to the console</returns>
        public string GetBuffer()
        {
            var builder = new StringBuilder();
            builder.Append(clearPart.GetBuffer());
            var sortedParts = ScreenParts.OrderBy((part) => part.Order).ToList();
            foreach (var part in sortedParts)
                builder.Append(part.GetBuffer());
            return builder.ToString();
        }

        /// <summary>
        /// Tells the clear screen part that the refresh is required
        /// </summary>
        public void RequireRefresh() =>
            needsRefresh = true;

        /// <summary>
        /// Makes a new instance of the screen
        /// </summary>
        public Screen() =>
            InitializeClearScreenManager();

        internal void InitializeClearScreenManager()
        {
            needsRefresh = true;
            clearPart.Clear();
            clearPart.AddDynamicText(() =>
            {
                ConsoleWrapper.CursorVisible = false;
                var builder = new StringBuilder();
                builder.Append(ColorTools.RenderRevertBackground());
                if (needsRefresh || ConsoleResizeHandler.WasResized(ResetResize))
                {
                    needsRefresh = false;
                    builder.Append(ConsoleClearing.GetClearWholeScreenSequence());
                }
                return builder.ToString();
            });
        }
    }
}
