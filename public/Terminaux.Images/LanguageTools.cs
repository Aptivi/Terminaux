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

using MainLangTools = Terminaux.Base.LanguageTools;

namespace Terminaux.Images
{
    internal static class LanguageTools
    {
        internal static string GetLocalized(string id)
        {
            if (!MainLangTools.resourceManagers.ContainsKey("Terminaux.Images"))
                MainLangTools.resourceManagers.Add("Terminaux.Images", new("Terminaux.Images.Resources.Languages.Output.Localizations", typeof(LanguageTools).Assembly));
            return MainLangTools.GetLocalized(id);
        }
    }
}
