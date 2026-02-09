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

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terminaux.Base;
using Colorimetry;
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
        /// <param name="background">Specifies the background color, or null for default</param>
        /// <returns>A string that contains the resulting pixels that you can print to the console using the <see cref="TextWriterRaw.WriteRaw(string, object[])"/> function</returns>
        public static string RenderIcon(string iconName, int width, int height, int left, int top, IconsColor color = IconsColor.Colored, Color? background = null)
        {
            // Check for icon
            string[] iconNames = GetIconNames(color);
            ConsoleLogger.Debug("Got {0} icons from quality {1}", iconNames.Length);
            if (!iconNames.Contains(iconName))
            {
                ConsoleLogger.Error("Can't find {0} of all {1} icons", iconName, iconNames.Length);
                throw new TerminauxException(LanguageTools.GetLocalized("TII_ICONSMANAGER_RENDERICON_EXCEPTION_NOICON").FormatString(iconName));
            }

            // Now, get the fully qualified name of the icon and render it
            string iconFullyQualifiedName = BuildFullyQualifiedIconName(iconName, color);
            ConsoleLogger.Info("Got icon fully qualified name {0} from {1}, {2}, {3}", iconFullyQualifiedName, iconName, color);
            var stream = typeof(IconsManager).Assembly.GetManifestResourceStream(iconFullyQualifiedName) ??
                throw new TerminauxException(LanguageTools.GetLocalized("TII_ICONSMANAGER_RENDERICON_EXCEPTION_ICONLOADERROR").FormatString(iconName));
            ConsoleLogger.Debug("Stream length is {0} bytes", stream.Length);
            return ImageProcessor.RenderImage(stream, width, height, left, top, background);
        }

        /// <summary>
        /// Gets the icon names
        /// </summary>
        /// <param name="color">Icon color</param>
        /// <returns>An array of icon names</returns>
        public static string[] GetIconNames(IconsColor color = IconsColor.Colored)
        {
            // Unqualify the full icon names and get only the names
            List<string> names = [];
            string prefix = GetPrefix(color);
            string[] icons = GetIconGroup(color);
            ConsoleLogger.Info("Prefix is {0} from color {1}.", prefix, color);
            foreach (string icon in icons)
            {
                ConsoleLogger.Debug("Populating {0}...", icon);
                names.Add(icon.RemovePrefix(prefix).RemoveSuffix(".png"));
            }

            // Return the emoji names
            return [.. names];
        }

        private static string BuildFullyQualifiedIconName(string iconName, IconsColor color = IconsColor.Colored)
        {
            // We have the emoji name, so we need to check for the qualification before going ahead
            string prefix = GetPrefix(color);
            if (iconName.StartsWith(prefix))
                return iconName;

            // We're done here.
            ConsoleLogger.Info("Prefix is {0} from color {1}. Icon name is {2}", prefix, color, iconName);
            return prefix + iconName + ".png";
        }

        private static string GetPrefix(IconsColor color = IconsColor.Colored)
        {
            // Build the prefix according to the color and the quality of the icons requested.
            var prefixBuilder = new StringBuilder($"Terminaux.Images.Icons.Resources.Icons.{color}");

            // We're done here.
            prefixBuilder.Append(".");
            return prefixBuilder.ToString();
        }

        private static string[] GetIconGroup(IconsColor color = IconsColor.Colored)
        {
            // Get the prefix to test the icon resource names
            string prefix = GetPrefix(color);
            List<string> names = [];
            foreach (string icon in builtinIcons)
                if (icon.StartsWith(prefix))
                    names.Add(icon);

            // We're done here.
            return [.. names];
        }
    }
}
