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

using System.Reflection;
using Terminaux.Images;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Console.Fixtures.Cases.Images
{
    internal class RenderImageSvg : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Image;

        public void RunFixture()
        {
            var asm = Assembly.GetExecutingAssembly();
            var pictureNames = asm.GetManifestResourceNames();
            foreach (string pictureName in pictureNames)
            {
                if (!pictureName.StartsWith("Terminaux.Console.Assets.vectors."))
                    continue;
                var stream = asm.GetManifestResourceStream(pictureName);
                if (stream is null)
                    return;
                string rendered = ImageProcessor.RenderImage(stream, 40, 20, 4, 2);
                TextWriterRaw.WriteRaw(rendered);
            }
        }
    }
}
