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

using LocaleStation.Tools;
using System;
using System.Reflection;
using Terminaux.Images;
using Terminaux.Images.Interactives;

namespace Terminaux.Console.Fixtures.Cases.Images
{
    internal class TestImageViewInteractive : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Input;

        public void RunFixture()
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Terminaux.Console.Assets.pictures.aptivi-logo-ios.png") ??
                throw new Exception("Resource doesn't exist: aptivi-logo-ios.png");
            LanguageCommon.Language = "ger";
            var image = ImageProcessor.OpenImage(stream);
            ImageViewInteractive.OpenInteractive(image);
        }
    }
}
