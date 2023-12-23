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

using System.Collections.Generic;
using System.Linq;
using Terminaux.Reader.Bindings.BaseBindings;

namespace Terminaux.Reader.Bindings
{
    internal static class BindingsList
    {
        internal static List<BaseBinding> baseBindings =
        [
            new GoRight(),
            new GoLeft(),
            new Home(),
            new End(),
            new Rubout(),
            new Return(),
            new ReturnNothing(),
            new PreviousHistory(),
            new NextHistory(),
            new Delete(),
            new BackwardOneWord(),
            new ForwardOneWord(),
            new NextSuggestion(),
            new PreviousSuggestion(),
            new CutToStart(),
            new CutToEnd(),
            new CutBackwardOneWord(),
            new CutForwardOneWord(),
            new Yank(),
            new UppercaseOneWord(),
            new LowercaseOneWord(),
            new UpAndForwardOneWord(),
            new LoAndForwardOneWord(),
            new ShowSuggestions(),
            new Refresh(),
            new InsertMode(),

#if DEBUG
            new DebugPos()
#endif
        ];

        internal static List<BaseBinding> customBindings = [];

        internal static List<BaseBinding> AllBindings =>
            baseBindings.Concat(customBindings).ToList();

        internal static BaseBinding fallbackBinding = new InsertSelf();
    }
}
