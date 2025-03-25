﻿//
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
                containerBuffer.Append(RenderRenderable(instance, pos));
            }
            return containerBuffer.ToString();
        }

        /// <summary>
        /// Writes the renderable to the console
        /// </summary>
        /// <param name="renderable">Renderable instance to write</param>
        public static void WriteRenderable(CyclicWriter renderable) =>
            WriteRenderable(renderable, new(0, 0), new(ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight));

        /// <summary>
        /// Writes the renderable to the console
        /// </summary>
        /// <param name="renderable">Renderable instance to write</param>
        /// <param name="pos">Position to write to</param>
        public static void WriteRenderable(CyclicWriter renderable, Coordinate pos) =>
            WriteRenderable(renderable, pos, new(ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight));

        /// <summary>
        /// Writes the renderable to the console
        /// </summary>
        /// <param name="renderable">Renderable instance to write</param>
        /// <param name="pos">Position to write to</param>
        /// <param name="size">Size of the renderable for width and height</param>
        public static void WriteRenderable(CyclicWriter renderable, Coordinate pos, Size size) =>
            TextWriterRaw.WriteRaw(RenderRenderable(renderable, pos, size));

        /// <summary>
        /// Writes the renderable to the console
        /// </summary>
        /// <param name="renderable">Renderable instance to write</param>
        public static void WriteRenderable(SimpleCyclicWriter renderable) =>
            WriteRenderable(renderable, new(0, 0));

        /// <summary>
        /// Writes the renderable to the console
        /// </summary>
        /// <param name="renderable">Renderable instance to write</param>
        /// <param name="pos">Position to write to</param>
        public static void WriteRenderable(SimpleCyclicWriter renderable, Coordinate pos) =>
            TextWriterRaw.WriteRaw(RenderRenderable(renderable, pos));

        /// <summary>
        /// Writes the renderable to the console
        /// </summary>
        /// <param name="renderable">Renderable instance to write</param>
        public static void WriteRenderable(GraphicalCyclicWriter renderable) =>
            WriteRenderable(renderable, new(0, 0), new(ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight));

        /// <summary>
        /// Writes the renderable to the console
        /// </summary>
        /// <param name="renderable">Renderable instance to write</param>
        /// <param name="pos">Position to write to</param>
        public static void WriteRenderable(GraphicalCyclicWriter renderable, Coordinate pos) =>
            WriteRenderable(renderable, pos, new(ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight));

        /// <summary>
        /// Writes the renderable to the console
        /// </summary>
        /// <param name="renderable">Renderable instance to write</param>
        /// <param name="pos">Position to write to</param>
        /// <param name="size">Size of the renderable for width and height</param>
        public static void WriteRenderable(GraphicalCyclicWriter renderable, Coordinate pos, Size size) =>
            TextWriterRaw.WriteRaw(RenderRenderable(renderable, pos, size));

        /// <summary>
        /// Renders the renderable
        /// </summary>
        /// <param name="renderable">Renderable instance to render</param>
        /// <returns>A container representation that you can render with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderRenderable(CyclicWriter renderable) =>
            RenderRenderable(renderable, new(0, 0));

        /// <summary>
        /// Renders the renderable
        /// </summary>
        /// <param name="renderable">Renderable instance to render</param>
        /// <param name="pos">Position to write to</param>
        /// <returns>A container representation that you can render with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderRenderable(CyclicWriter renderable, Coordinate pos) =>
            TextWriterWhereColor.RenderWhere(renderable.Render(), pos.X, pos.Y);

        /// <summary>
        /// Renders the renderable
        /// </summary>
        /// <param name="renderable">Renderable instance to render</param>
        /// <param name="pos">Position to write to</param>
        /// <param name="size">Size of the renderable for width and height</param>
        /// <returns>A container representation that you can render with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderRenderable(CyclicWriter renderable, Coordinate pos, Size size)
        {
            if (renderable is SimpleCyclicWriter simpleRenderable)
                return RenderRenderable(simpleRenderable, pos);
            else if (renderable is GraphicalCyclicWriter graphicalRenderable)
                return RenderRenderable(graphicalRenderable, pos, size);
            return "";
        }

        /// <summary>
        /// Renders the renderable
        /// </summary>
        /// <param name="renderable">Renderable instance to render</param>
        /// <returns>A container representation that you can render with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderRenderable(SimpleCyclicWriter renderable) =>
            RenderRenderable(renderable, new(0, 0));

        /// <summary>
        /// Renders the renderable
        /// </summary>
        /// <param name="renderable">Renderable instance to render</param>
        /// <param name="pos">Position to write to</param>
        /// <returns>A container representation that you can render with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderRenderable(SimpleCyclicWriter renderable, Coordinate pos) =>
            TextWriterWhereColor.RenderWhere(renderable.Render(), pos.X, pos.Y);

        /// <summary>
        /// Renders the renderable
        /// </summary>
        /// <param name="renderable">Renderable instance to render</param>
        /// <returns>A container representation that you can render with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderRenderable(GraphicalCyclicWriter renderable) =>
            RenderRenderable(renderable, new(0, 0), new(ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight));

        /// <summary>
        /// Renders the renderable
        /// </summary>
        /// <param name="renderable">Renderable instance to render</param>
        /// <param name="pos">Position to write to</param>
        /// <returns>A container representation that you can render with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderRenderable(GraphicalCyclicWriter renderable, Coordinate pos) =>
            RenderRenderable(renderable, pos, new(ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight));

        /// <summary>
        /// Renders the renderable
        /// </summary>
        /// <param name="renderable">Renderable instance to render</param>
        /// <param name="pos">Position to write to</param>
        /// <param name="size">Size of the renderable for width and height</param>
        /// <returns>A container representation that you can render with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderRenderable(GraphicalCyclicWriter renderable, Coordinate pos, Size size)
        {
            renderable.Left = pos.X;
            renderable.Top = pos.Y;
            renderable.Width = size.Width;
            renderable.Height = size.Height;

            // Use RenderWhere anyways, in case the renderable doesn't respect the left and the top position set
            // by the Left and the Top properties above.
            return TextWriterWhereColor.RenderWhere(renderable.Render(), pos.X, pos.Y);
        }
    }
}
