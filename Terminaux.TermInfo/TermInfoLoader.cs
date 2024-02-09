//
// Terminaux.TermInfo  Copyright (C) 2023-2024  Aptivi
//
// This file is part of Terminaux.TermInfo
//
// Terminaux.TermInfo is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Terminaux.TermInfo is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Terminaux.TermInfo.Extensions;
using Terminaux.TermInfo.Parsing;

namespace Terminaux.TermInfo
{
    internal static class TermInfoLoader
    {
        public static TermInfoDesc? Load(string? name = null)
        {
            if (name == null)
            {
                name = Environment.GetEnvironmentVariable("TERM");
                if (name == null)
                {
                    return null;
                }
            }

            var directories = new List<string>();

            // TERMINFO
            directories.AddIfNotNullOrEmpty(Environment.GetEnvironmentVariable("TERMINFO"));

            // ~/.terminfo
            var profile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            if (!string.IsNullOrWhiteSpace(profile))
            {
                directories.Add(Path.Combine(profile, ".terminfo"));
            }

            // TERMINFO_DIRS
            var dirs = Environment.GetEnvironmentVariable("TERMINFO_DIRS");
            if (!string.IsNullOrWhiteSpace(dirs))
            {
                var separator = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? ';' : ':';
                foreach (var path in dirs.Split(new char[] { separator }, StringSplitOptions.RemoveEmptyEntries))
                {
                    directories.Add(path);
                }
            }

            // Fallback
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                directories.Add("/etc/terminfo");
                directories.Add("/lib/terminfo");
                directories.Add("/usr/share/terminfo");
            }

            // Check all directories
            foreach (var directory in directories)
            {
                var desc = Load(directory, name);
                if (desc != null)
                {
                    return desc;
                }
            }

            return null;
        }

        public static TermInfoDesc Load(Stream stream)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            return TermInfoParser.Parse(stream);
        }

        private static TermInfoDesc? Load(string path, string name)
        {
            var files = new List<string>
            {
                Path.Combine(path, name[0].ToString(), name),
                Path.Combine(path, Convert.ToByte(name[0]).ToString("x"), name),
            };

            foreach (var file in files)
            {
                if (File.Exists(file))
                {
                    return Load(File.OpenRead(file));
                }
            }

            return null;
        }
    }
}
