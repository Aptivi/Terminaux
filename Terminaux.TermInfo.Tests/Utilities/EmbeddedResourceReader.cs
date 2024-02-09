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
using System.IO;
using System.Reflection;

namespace Terminaux.TermInfo.Tests.Utilities
{
    public static class EmbeddedResourceReader
    {
        public static Stream LoadResourceStream(string resourceName)
        {
            if (resourceName is null)
            {
                throw new ArgumentNullException(nameof(resourceName));
            }

            var assembly = Assembly.GetCallingAssembly();
            resourceName = resourceName.Replace("/", ".");

            return assembly.GetManifestResourceStream(resourceName);
        }

        public static Stream LoadResourceStream(Assembly assembly, string resourceName)
        {
            if (assembly is null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            if (resourceName is null)
            {
                throw new ArgumentNullException(nameof(resourceName));
            }

            resourceName = resourceName.Replace("/", ".");
            return assembly.GetManifestResourceStream(resourceName);
        }
    }
}
