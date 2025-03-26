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

using System.Text;
using Terminaux.Base;
using Terminaux.Base.Structures;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Writer.CyclicWriters.Renderer
{
    /// <summary>
    /// Container tools for renderables
    /// </summary>
    public static class ContainerTools
    {
        /// <summary>
        /// Writes the container to the console
        /// </summary>
        /// <param name="container">Container instance to write</param>
        public static void WriteContainer(Container container) =>
            TextWriterRaw.WriteRaw(RenderContainer(container));

        /// <summary>
        /// Renders the container
        /// </summary>
        /// <param name="container">Container instance to render</param>
        /// <returns>A container representation that you can render with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderContainer(Container container)
        {
            var containerBuffer = new StringBuilder();
            var renderables = container.GetRenderableNames();
            foreach (var renderable in renderables)
            {
                var instance = container.GetRenderable(renderable);
                var pos = container.GetRenderablePosition(renderable);
                var size = container.GetRenderableSize(renderable);
                containerBuffer.Append(RendererTools.RenderRenderable(instance, pos, size));
            }
            return containerBuffer.ToString();
        }
    }
}
