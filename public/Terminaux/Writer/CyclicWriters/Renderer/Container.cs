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

using System.Collections.Generic;
using System.Linq;
using Terminaux.Base;
using Terminaux.Base.Structures;

namespace Terminaux.Writer.CyclicWriters.Renderer
{
    /// <summary>
    /// Container for all the renderables
    /// </summary>
    public class Container
    {
        private readonly Dictionary<string, (CyclicWriter, Coordinate, Size)> renderables = [];

        /// <summary>
        /// Whether this renderable is registered or not
        /// </summary>
        /// <param name="name">Name of the renderable</param>
        /// <returns>True if registered; false otherwise</returns>
        public bool IsRegistered(string name) =>
            renderables.ContainsKey(name);

        /// <summary>
        /// Adds a renderable to the list
        /// </summary>
        /// <param name="name">Renderable name</param>
        /// <param name="renderable">Renderable instance</param>
        /// <exception cref="TerminauxException"></exception>
        public void AddRenderable(string name, CyclicWriter renderable)
        {
            if (IsRegistered(name))
                throw new TerminauxException(LanguageTools.GetLocalized("T_WRITER_CYCLICWRITERS_TOOLS_CONTAINER_EXCEPTION_HASRENDERABLE"));
            renderables.Add(name, (renderable, new(), new()));
        }

        /// <summary>
        /// Removes a renderable from the list
        /// </summary>
        /// <param name="name">Renderable name</param>
        /// <exception cref="TerminauxException"></exception>
        public void RemoveRenderable(string name)
        {
            if (!IsRegistered(name))
                throw new TerminauxException(LanguageTools.GetLocalized("T_WRITER_CYCLICWRITERS_TOOLS_CONTAINER_EXCEPTION_HASNORENDERABLE"));
            renderables.Remove(name);
        }

        /// <summary>
        /// Gets a renderable
        /// </summary>
        /// <param name="name">Renderable name</param>
        /// <returns>Renderable instance</returns>
        /// <exception cref="TerminauxException"></exception>
        public CyclicWriter GetRenderable(string name)
        {
            if (!renderables.TryGetValue(name, out var renderable))
                throw new TerminauxException(LanguageTools.GetLocalized("T_WRITER_CYCLICWRITERS_TOOLS_CONTAINER_EXCEPTION_HASNORENDERABLE"));
            return renderable.Item1;
        }

        /// <summary>
        /// Gets the position of a renderable
        /// </summary>
        /// <param name="name">Renderable name</param>
        /// <returns>Coordinates of a renderable</returns>
        /// <exception cref="TerminauxException"></exception>
        public Coordinate GetRenderablePosition(string name)
        {
            if (!renderables.TryGetValue(name, out var renderable))
                throw new TerminauxException(LanguageTools.GetLocalized("T_WRITER_CYCLICWRITERS_TOOLS_CONTAINER_EXCEPTION_HASNORENDERABLE"));
            return renderable.Item1 is GraphicalCyclicWriter graphical ? new(graphical.Left, graphical.Top) : renderable.Item2;
        }

        /// <summary>
        /// Sets the position of a renderable
        /// </summary>
        /// <param name="name">Renderable name</param>
        /// <param name="position">Position to set</param>
        /// <exception cref="TerminauxException"></exception>
        public void SetRenderablePosition(string name, Coordinate position)
        {
            if (!IsRegistered(name))
                throw new TerminauxException(LanguageTools.GetLocalized("T_WRITER_CYCLICWRITERS_TOOLS_CONTAINER_EXCEPTION_HASNORENDERABLE"));
            renderables[name] = (renderables[name].Item1, position, renderables[name].Item3);
        }

        /// <summary>
        /// Gets the size of a renderable
        /// </summary>
        /// <param name="name">Renderable name</param>
        /// <returns>Size of a renderable</returns>
        /// <exception cref="TerminauxException"></exception>
        public Size GetRenderableSize(string name)
        {
            if (!renderables.TryGetValue(name, out var renderable))
                throw new TerminauxException(LanguageTools.GetLocalized("T_WRITER_CYCLICWRITERS_TOOLS_CONTAINER_EXCEPTION_HASNORENDERABLE"));
            return renderable.Item1 is GraphicalCyclicWriter graphical ? new(graphical.Width, graphical.Height) : renderable.Item3;
        }

        /// <summary>
        /// Sets the size of a renderable
        /// </summary>
        /// <param name="name">Renderable name</param>
        /// <param name="size">Size to set</param>
        /// <exception cref="TerminauxException"></exception>
        public void SetRenderableSize(string name, Size size)
        {
            if (!IsRegistered(name))
                throw new TerminauxException(LanguageTools.GetLocalized("T_WRITER_CYCLICWRITERS_TOOLS_CONTAINER_EXCEPTION_HASNORENDERABLE"));
            renderables[name] = (renderables[name].Item1, renderables[name].Item2, size);
        }

        /// <summary>
        /// Gets the renderable names
        /// </summary>
        /// <returns></returns>
        public string[] GetRenderableNames() =>
            renderables.Select((kvp) => kvp.Key).ToArray();
    }
}
