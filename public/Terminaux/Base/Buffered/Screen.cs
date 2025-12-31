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
        private bool refreshWasDone = false;
        private int cycleFrequency = 0;
        private ScreenPart? overlayPart;
        private readonly ScreenPart clearPart = new();
        private readonly Dictionary<string, ScreenPart> screenParts = [];
        private static ScreenPart? globalOverlayPart;

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
        /// Whether the screen needs refreshing
        /// </summary>
        public bool NeedsRefresh =>
            needsRefresh;

        /// <summary>
        /// If <see cref="NeedsRefresh"/> was true before the screen was rendered, this returns true.
        /// </summary>
        public bool RefreshWasDone =>
            refreshWasDone;

        /// <summary>
        /// Specifies the amount of milliseconds of cyclic screen frequency
        /// </summary>
        public int CycleFrequency
        {
            get => cycleFrequency;
            set => cycleFrequency = value;
        }

        /// <summary>
        /// The screen overlay part that renders on top of this screen instance
        /// </summary>
        public ScreenPart? OverlayPart
        {
            get => overlayPart;
            set => overlayPart = value;
        }

        /// <summary>
        /// The screen overlay part that renders on top of all screen instances
        /// </summary>
        public static ScreenPart? GlobalOverlayPart
        {
            get => globalOverlayPart;
            set => globalOverlayPart = value;
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
                throw new TerminauxException(LanguageTools.GetLocalized("T_BB_SCREEN_EXCEPTION_NOSCREENPART"));
            if (string.IsNullOrEmpty(name))
                throw new TerminauxException(LanguageTools.GetLocalized("T_BB_SCREEN_EXCEPTION_NOSCREENNAME"));
            while (screenParts.ContainsKey(name))
            {
                name += $" [{screenParts.Count}]";
                ConsoleLogger.Debug("Changed screen part name to {0} due to conflict", name);
            }

            // Now, add the buffered part
            ConsoleLogger.Info("Adding screen part {0}...", name);
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
                throw new TerminauxException(LanguageTools.GetLocalized("T_BB_SCREEN_EXCEPTION_IDXOUTOFRANGE"));
            if (part is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_BB_SCREEN_EXCEPTION_NOSCREENPART"));

            // Now, edit the buffered part
            ConsoleLogger.Debug("Getting screen part from idx {0} / {1}", idx, screenParts.Count);
            var kvp = screenParts.ElementAt(idx);
            ConsoleLogger.Info("Editing screen part {0} from idx {1}...", kvp.Key, idx);
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
                throw new TerminauxException(LanguageTools.GetLocalized("T_BB_SCREEN_EXCEPTION_NOSCREENPARTNAME"));
            if (part is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_BB_SCREEN_EXCEPTION_NOSCREENPART"));

            // Now, edit the buffered part
            ConsoleLogger.Info("Editing screen part {0} from id...", name);
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
            ConsoleLogger.Debug("Screen part GUID is {0}.", id.ToString());
            var partSource = screenParts.FirstOrDefault((part) => part.Value.Id == id);
            if (partSource.Value is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_BB_SCREEN_EXCEPTION_SCREENPARTNOTFOUND"));
            if (part is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_BB_SCREEN_EXCEPTION_NOSCREENPART"));

            // Now, edit the buffered part
            ConsoleLogger.Info("Editing screen part {0} from id...", partSource.Key);
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
                throw new TerminauxException(LanguageTools.GetLocalized("T_BB_SCREEN_EXCEPTION_IDXOUTOFRANGE"));

            // Now, remove the buffered part
            ConsoleLogger.Debug("Getting screen part from idx {0} / {1}", idx, screenParts.Count);
            var kvp = screenParts.ElementAt(idx);
            ConsoleLogger.Info("Removing screen part {0} from idx {1}...", kvp.Key, idx);
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
                throw new TerminauxException(LanguageTools.GetLocalized("T_BB_SCREEN_EXCEPTION_NOSCREENPARTNAME"));

            // Now, remove the buffered part
            ConsoleLogger.Info("Removing screen part {0} from id...", name);
            screenParts.Remove(name);
        }

        /// <summary>
        /// Removes the buffered part from the list of screen parts using the GUID
        /// </summary>
        /// <param name="id">Screen buffer part GUID</param>
        /// <exception cref="TerminauxException"></exception>
        public void RemoveBufferedPart(Guid id)
        {
            ConsoleLogger.Debug("Screen part GUID is {0}.", id.ToString());
            var part = screenParts.FirstOrDefault((part) => part.Value.Id == id);
            if (part.Value is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_BB_SCREEN_EXCEPTION_SCREENPARTNOTFOUND"));
            ConsoleLogger.Info("Removing screen part {0} from id...", part.Key);
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
                throw new TerminauxException(LanguageTools.GetLocalized("T_BB_SCREEN_EXCEPTION_IDXOUTOFRANGE"));

            // Now, get the buffered part
            ConsoleLogger.Debug("Getting screen part from idx {0} / {1}", idx, screenParts.Count);
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
                throw new TerminauxException(LanguageTools.GetLocalized("T_BB_SCREEN_EXCEPTION_NOSCREENPARTNAME"));

            // Now, get the buffered part
            ConsoleLogger.Info("Returning screen part {0}...", name);
            return part;
        }

        /// <summary>
        /// Gets the buffered part from the list of screen parts
        /// </summary>
        /// <param name="id">Screen buffer part GUID</param>
        /// <exception cref="TerminauxException"></exception>
        public ScreenPart GetBufferedPart(Guid id)
        {
            ConsoleLogger.Debug("Screen part GUID is {0}.", id.ToString());
            var part = screenParts.FirstOrDefault((part) => part.Value.Id == id);
            if (part.Value is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_BB_SCREEN_EXCEPTION_SCREENPARTNOTFOUND"));

            // Now, get the buffered part
            ConsoleLogger.Info("Returning screen part {0} from id...", part.Key);
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
            ConsoleLogger.Debug("Getting screen part from idx {0} / {1}", idx, screenParts.Count);
            var kvp = screenParts.ElementAt(idx);
            ConsoleLogger.Info("Checking screen part {0} from idx {1}...", kvp.Key, idx);
            return CheckBufferedPart(kvp.Key);
        }

        /// <summary>
        /// Checks the buffered part in the list of screen parts
        /// </summary>
        /// <param name="name">Screen buffer part name</param>
        /// <exception cref="TerminauxException"></exception>
        public bool CheckBufferedPart(string name)
        {
            ConsoleLogger.Info("Checking screen part {0} from id...", name);
            bool exists = screenParts.ContainsKey(name);
            ConsoleLogger.Debug("{0} exists: {1}", name, exists);
            return exists;
        }

        /// <summary>
        /// Checks the buffered part in the list of screen parts using the GUID
        /// </summary>
        /// <param name="id">Screen buffer part GUID</param>
        /// <exception cref="TerminauxException"></exception>
        public bool CheckBufferedPart(Guid id)
        {
            ConsoleLogger.Debug("Screen part GUID is {0}.", id.ToString());
            var part = screenParts.FirstOrDefault((part) => part.Value.Id == id);
            bool exists = part.Value is not null;
            ConsoleLogger.Debug("{0} exists: {1}", id.ToString(), exists);
            return exists;
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

            // Clear the screen
            builder.Append(clearPart.GetBuffer());

            // Add the screen parts sorted by their order
            var sortedParts = ScreenParts.OrderBy((part) => part.Order).ToList();
            ConsoleLogger.Info("Getting buffer from {0} parts...", sortedParts.Count);
            foreach (var part in sortedParts)
                builder.Append(part.GetBuffer());

            // Add a screen-specific overlay part
            ConsoleLogger.Info("Screen-specific overlay part required: {0}", OverlayPart is not null);
            if (OverlayPart is not null)
                builder.Append(OverlayPart.GetBuffer());

            // Add a global overlay part
            ConsoleLogger.Info("Global overlay part required: {0}", GlobalOverlayPart is not null);
            if (GlobalOverlayPart is not null)
                builder.Append(GlobalOverlayPart.GetBuffer());

            // Return the result
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
                refreshWasDone = false;
                if (needsRefresh || ConsoleResizeHandler.WasResized(ResetResize))
                {
                    ConsoleLogger.Info("Screen requires refresh due to refresh request ({0}) or resize.", needsRefresh);
                    needsRefresh = false;
                    refreshWasDone = true;
                    builder.Append(ConsoleClearing.GetClearWholeScreenSequence());
                }
                return builder.ToString();
            });
        }
    }
}
