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
using System.Text;
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Writer.ConsoleWriters;
using Textify.General;

namespace Terminaux.Images.Icons
{
    /// <summary>
    /// Icon management class
    /// </summary>
    public static class IconsManager
    {
        private static readonly string[] builtinIcons = typeof(IconsManager).Assembly.GetManifestResourceNames();

        /// <summary>
        /// Renders the icon to a string that you can print to the console
        /// </summary>
        /// <param name="iconName">Icon name</param>
        /// <param name="width">Width of the resulting icon</param>
        /// <param name="height">Height of the resulting icon</param>
        /// <param name="left">Zero-based console left position to start writing the icon to</param>
        /// <param name="top">Zero-based console top position to start writing the icon to</param>
        /// <param name="color">Icon color</param>
        /// <param name="quality">Icon quality</param>
        /// <param name="background">Specifies the background color, or null for default</param>
        /// <returns>A string that contains the resulting pixels that you can print to the console using the <see cref="TextWriterRaw.WriteRaw(string, object[])"/> function</returns>
        public static string RenderIcon(string iconName, int width, int height, int left, int top, IconsColor color = IconsColor.Colored, IconsQuality quality = IconsQuality.Normal, Color? background = null)
        {
            // Check for icon
            string[] iconNames = GetIconNames(color, quality);
            if (!iconNames.Contains(iconName))
                throw new TerminauxException($"Icon {iconName} doesn't exist.");

            // Now, get the fully qualified name of the icon and render it
            string iconFullyQualifiedName = BuildFullyQualifiedIconName(iconName, color, quality);
            ConsoleLogger.Info("Got icon fully qualified name {0} from {1}, {2}, {3}", iconFullyQualifiedName, iconName, color, quality);
            var stream = typeof(IconsManager).Assembly.GetManifestResourceStream(iconFullyQualifiedName) ??
                throw new TerminauxException($"Icon {iconName} exists, but failed to load.");
            return ImageProcessor.RenderImage(stream, width, height, left, top, background);
        }

        /// <summary>
        /// Gets the icon names
        /// </summary>
        /// <param name="color">Icon color</param>
        /// <param name="quality">Icon quality</param>
        /// <returns>An array of icon names</returns>
        public static string[] GetIconNames(IconsColor color = IconsColor.Colored, IconsQuality quality = IconsQuality.Normal)
        {
            // Unqualify the full icon names and get only the names
            List<string> names = [];
            string prefix = GetPrefix(color, quality);
            string[] icons = GetIconGroup(color, quality);
            string extension = quality == IconsQuality.Scalable ? ".svg" : ".png";
            ConsoleLogger.Info("Prefix is {0} from color {1} and quality {2}, which has an extension of {3}", prefix, color, quality, extension);
            foreach (string icon in icons)
                names.Add(icon.RemovePrefix(prefix).RemoveSuffix(extension));

            // Return the emoji names
            return [.. names];
        }

        private static string BuildFullyQualifiedIconName(string iconName, IconsColor color = IconsColor.Colored, IconsQuality quality = IconsQuality.Normal)
        {
            // We have the emoji name, so we need to check for the qualification before going ahead
            string prefix = GetPrefix(color, quality);
            if (iconName.StartsWith(prefix))
                return iconName;

            // We're done here.
            string extension = quality == IconsQuality.Scalable ? ".svg" : ".png";
            ConsoleLogger.Info("Prefix is {0} from color {1} and quality {2}, which has an extension of {3}. Icon name is {4}", prefix, color, quality, extension, iconName);
            return prefix + iconName + extension;
        }

        private static string GetPrefix(IconsColor color = IconsColor.Colored, IconsQuality quality = IconsQuality.Normal)
        {
            // Build the prefix according to the color and the quality of the icons requested.
            var prefixBuilder = new StringBuilder($"Terminaux.Images.Icons.Resources.Icons.{color}");
            if (quality == IconsQuality.High)
                prefixBuilder.Append("_HQ");
            else if (quality == IconsQuality.Scalable)
                prefixBuilder.Append("_SVG");

            // We're done here.
            prefixBuilder.Append(".");
            return prefixBuilder.ToString();
        }

        private static string[] GetIconGroup(IconsColor color = IconsColor.Colored, IconsQuality quality = IconsQuality.Normal)
        {
            // Get the prefix to test the icon resource names
            string prefix = GetPrefix(color, quality);
            List<string> names = [];
            foreach (string icon in builtinIcons)
                if (icon.StartsWith(prefix))
                    names.Add(icon);

            // We're done here.
            return [.. names];
        }
    }
}
