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

namespace Terminaux.Inputs.Interactive
{
    /// <summary>
    /// Help page information for interactive TUI to provide users with more help
    /// </summary>
    public class InteractiveTuiHelpPage
    {
        /// <summary>
        /// Help page title
        /// </summary>
        public string HelpTitle { get; set; } = /* Localizable */ "T_INPUT_INTERACTIVE_TUIHELPTITLE";
        /// <summary>
        /// Help page description
        /// </summary>
        public string HelpDescription { get; set; } = /* Localizable */ "T_INPUT_INTERACTIVE_TUIHELPDESC";
        /// <summary>
        /// Help page body
        /// </summary>
        public string HelpBody { get; set; } = /* Localizable */ "T_INPUT_INTERACTIVE_TUIHELPBODY";

        /// <summary>
        /// Constructor of the help page
        /// </summary>
        public InteractiveTuiHelpPage()
        { }
    }
}
