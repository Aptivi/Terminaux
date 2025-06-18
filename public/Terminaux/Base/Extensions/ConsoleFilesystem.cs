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

using SpecProbe.Software.Platform;
using System;
using System.IO;
using System.Linq;
using Textify.General;

namespace Terminaux.Base.Extensions
{
    /// <summary>
    /// Console filesystem tools
    /// </summary>
    public static class ConsoleFilesystem
    {
        private static string currentDir = Directory.GetCurrentDirectory();

        /// <summary>
        /// Current directory (for shell)
        /// </summary>
        public static string CurrentDir
        {
            get => NeutralizePath(currentDir, "");
            set => currentDir = NeutralizePath(value);
        }

        /// <summary>
        /// Path to the Terminaux configuration folder
        /// </summary>
        public static string ConfigurationPath
        {
            get
            {
                string finalPath = GetPath();
                if (!Directory.Exists(finalPath))
                    Directory.CreateDirectory(finalPath);
                return NeutralizePath(finalPath);
            }
        }

        /// <summary>
        /// Group of paths separated by the delimiter that <see cref="Path.PathSeparator"/> specifies. It works the same as PATH.
        /// </summary>
        public static string LookupPaths { get; set; } = Environment.GetEnvironmentVariable("PATH") ?? "";

        internal static void ClearFile(string path)
        {
            FileStream clearer = new(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            clearer.SetLength(0);
            clearer.Close();
        }

        internal static string NeutralizePath(string? path) =>
            NeutralizePath(path, CurrentDir);

        internal static string NeutralizePath(string? path, string? source)
        {
            // Warning: There should be no debug statements until the strict check point.
            path ??= "";
            source ??= "";

            // Unescape the characters
            path = TextTools.Unescape(path.Replace(@"\", "/"));
            source = TextTools.Unescape(source.Replace(@"\", "/"));

            // Append current directory to path
            if (!Path.IsPathRooted(path))
                if (!source.EndsWith("/"))
                    path = $"{source}/{path}";
                else
                    path = $"{source}{path}";

            // Replace last occurrences of current directory of path with nothing.
            if (!string.IsNullOrEmpty(source))
                if (path.Contains(source) & path.AllIndexesOf(source).Count() > 1)
                    path = path.ReplaceLastOccurrence(source, "");

            // Finalize the path in case NeutralizePath didn't normalize it correctly.
            path = Path.GetFullPath(path).Replace(@"\", "/");
            return path;
        }

        internal static bool TryParseFileName(string Name)
        {
            try
            {
                return !(Name.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0);
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, "Failed to parse file name {0}: {1}", Name, ex.Message);
            }
            return false;
        }

        internal static bool TryParsePath(string Path)
        {
            try
            {
                return !(Path.IndexOfAny(GetInvalidPathChars()) >= 0);
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, "Failed to parse path {0}: {1}", Path, ex.Message);
            }
            return false;
        }

        internal static char[] GetInvalidPathChars()
        {
            var FinalInvalidPathChars = Path.GetInvalidPathChars();
            var WindowsInvalidPathChars = new[] { '"', '<', '>' };
            if (PlatformHelper.IsOnWindows())
            {
                // It's weird of .NET 6.0 to not consider the above three Windows invalid directory chars to be invalid,
                // so make them invalid as in .NET Framework.
                Array.Resize(ref FinalInvalidPathChars, 36);
                WindowsInvalidPathChars.CopyTo(FinalInvalidPathChars, FinalInvalidPathChars.Length - 3);
            }
            return FinalInvalidPathChars;
        }

        internal static bool FileExistsInPath(string FilePath, ref string Result)
        {
            string[] lookupPaths = [.. LookupPaths.Split(Path.PathSeparator)];
            string ResultingPath;
            foreach (string LookupPath in lookupPaths)
            {
                ResultingPath = NeutralizePath(FilePath, LookupPath);
                if (File.Exists(ResultingPath))
                {
                    Result = ResultingPath;
                    return true;
                }
            }
            Result = "";
            return false;
        }

        internal static string GetPath()
        {
            string path =
                PlatformHelper.IsOnWindows() ?
                $"{Environment.GetEnvironmentVariable("LOCALAPPDATA")}\\Aptivi\\Terminaux" :
                $"{Environment.GetEnvironmentVariable("HOME")}/.config/Aptivi/Terminaux";
            return path;
        }

        internal static string GetSubPath(string subPathName)
        {
            string path = $"{ConfigurationPath}/{subPathName}";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }
    }
}
