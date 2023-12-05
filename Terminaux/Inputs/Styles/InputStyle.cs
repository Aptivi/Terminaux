//
// Terminaux  Copyright (C) 2023  Aptivi
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

using Terminaux.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Inputs.Styles
{
    /// <summary>
    /// Input style module
    /// </summary>
    public static class InputStyle
    {

        /// <summary>
        /// Prompts user for input (answer the question with your own answers)
        /// </summary>
        public static string PromptInput(string Question)
        {
            while (true)
            {
                // Variables
                string Answer;
                Color questionColor = new(ConsoleColors.Yellow);
                Color inputColor = new(ConsoleColors.White);
                Color textColor = new(ConsoleColors.White);

                // Ask a question
                TextWriterColor.WriteColor(Question, false, questionColor);
                ColorTools.SetConsoleColor(inputColor);

                // Wait for an answer
                Answer = Input.ReadLine();
                ColorTools.SetConsoleColor(textColor);

                return Answer;
            }
        }

        /// <summary>
        /// Prompts user for password (answer the question with your own answers)
        /// </summary>
        public static string PromptInputPassword(string Question)
        {
            while (true)
            {
                // Variables
                string Answer;
                Color questionColor = new(ConsoleColors.Yellow);
                Color inputColor = new(ConsoleColors.White);
                Color textColor = new(ConsoleColors.White);

                // Ask a question
                TextWriterColor.WriteColor(Question, false, questionColor);
                ColorTools.SetConsoleColor(inputColor);

                // Wait for an answer
                Answer = Input.ReadLineNoInput();
                ColorTools.SetConsoleColor(textColor);

                return Answer;
            }
        }

    }
}
