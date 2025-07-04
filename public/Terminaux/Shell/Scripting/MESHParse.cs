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

using System;
using System.Linq;
using System.Collections.Generic;
using Terminaux.Shell.Scripting.Conditions;
using Terminaux.Shell.Shells;
using Textify.General;
using Terminaux.Writer.CyclicWriters.Simple;
using System.IO;
using Terminaux.Base;

namespace Terminaux.Shell.Scripting
{
    /// <summary>
    /// MESH script parser
    /// </summary>
    public static class MESHParse
    {
        /// <summary>
        /// Executes the MESH script
        /// </summary>
        /// <param name="ScriptPath">Full path to script</param>
        /// <param name="ScriptArguments">Script arguments</param>
        /// <param name="justLint">If true, just lints the script and throws exception if there are parsing errors</param>
        public static void Execute(string ScriptPath, string ScriptArguments, bool justLint = false) =>
            Execute(ScriptPath, ScriptArguments, ShellManager.CurrentShellType, justLint);

        /// <summary>
        /// Executes the MESH script
        /// </summary>
        /// <param name="ScriptPath">Full path to script</param>
        /// <param name="ScriptArguments">Script arguments</param>
        /// <param name="commandType">Context in which to execute the script (command type)</param>
        /// <param name="justLint">If true, just lints the script and throws exception if there are parsing errors</param>
        public static void Execute(string ScriptPath, string ScriptArguments, string commandType, bool justLint = false)
        {
            int LineNo = 1;
            try
            {
                // Open the script file for reading
                var FileLines = File.ReadAllLines(ScriptPath);
                ConsoleLogger.Debug("Stream opened. Parsing script");

                // Look for $variables and initialize them
                for (int l = 0; l < FileLines.Length; l++)
                {
                    // Get line
                    string Line = FileLines[l];
                    ConsoleLogger.Debug("Line {0}: \"{1}\"", LineNo, Line);

                    // If $variable is found in string, initialize it
                    var SplitWords = Line.Split(' ');
                    for (int i = 0; i <= SplitWords.Length - 1; i++)
                        if (!MESHVariables.ShellVariables.ContainsKey(SplitWords[i]) & SplitWords[i].StartsWith("$"))
                            MESHVariables.InitializeVariable(SplitWords[i]);
                    LineNo++;
                }

                // Get all lines and parse comments, commands, and arguments
                string[] commandBlocks = ["if", "while", "until"];
                int commandStackNum = 0;
                bool newCommandStackRequired = false;
                bool retryLoopCondition = false;
                bool advance = true;
                List<(int, int)> whilePlaces = [];
                LineNo = 1;
                for (int l = 0; l < FileLines.Length; l++)
                {
                    // Decrement if not advancing
                    if (!advance)
                    {
                        advance = true;
                        l--;
                    }

                    // Get line
                    string Line = FileLines[l];
                    ConsoleLogger.Debug("Line {0}: \"{1}\"", LineNo, Line);

                    // First, trim the line from the left after checking the stack
                    string stackIndicator = new('|', commandStackNum);
                    if (Line.StartsWith(stackIndicator))
                    {
                        newCommandStackRequired = false;

                        // Get the actual command
                        Line = Line.Substring(commandStackNum);

                        // If it still starts with the new stack indicator, throw an error
                        if (Line.StartsWith("|"))
                            throw new TerminauxException(LanguageTools.GetLocalized("T_SHELL_BASE_SCRIPTING_EXCEPTION_NEWBLOCKBEFORECONDITION") + " {1}:{2}\n{3}", commandStackNum, ScriptPath, LineNo, GetLineHandleString(ScriptPath, LineNo, commandStackNum));
                    }
                    else if (!Line.StartsWith(stackIndicator) && newCommandStackRequired)
                        throw new TerminauxException(LanguageTools.GetLocalized("T_SHELL_BASE_SCRIPTING_EXCEPTION_INDENTINVALID") + " {1}:{2}\n{3}", commandStackNum, ScriptPath, LineNo, GetLineHandleString(ScriptPath, LineNo, commandStackNum));
                    else
                    {
                        if (retryLoopCondition && !justLint)
                        {
                            (int, int) whilePlace = whilePlaces[whilePlaces.Count - 1];
                            commandStackNum = whilePlace.Item2;
                            l = whilePlace.Item1;
                            Line = FileLines[l].Substring(commandStackNum);
                        }
                        else
                            commandStackNum = 0;
                    }

                    // See if the line contains variable, and replace every instance of it with its value
                    var SplitWords = Line.SplitEncloseDoubleQuotes();
                    if (SplitWords is not null)
                        // Iterate every word
                        for (int i = 0; i <= SplitWords.Length - 1; i++)
                            // Every word that start with the $ sign means it's a variable that should be replaced with the
                            // value from the MESH variable manager.
                            if (SplitWords[i].StartsWith("$"))
                                Line = MESHVariables.GetVariableCommand(SplitWords[i], Line, commandType);

                    // See if the line contains argument placeholder, and replace every instance of it with its value
                    var SplitArguments = ScriptArguments.SplitEncloseDoubleQuotes();
                    if (SplitArguments is not null)
                        // Iterate every script argument
                        for (int j = 0; j <= SplitArguments.Length - 1; j++)
                            // If there is a placeholder variable like so:
                            //     echo Hello, {0}
                            // ...or...
                            //     echo {0}ification
                            // ...then proceed to replace the placeholder that contains an index of argument with the
                            // actual value
                            Line = Line.Replace($"{{{j}}}", SplitArguments[j]);

                    // See if the line is a command that starts with the if statement
                    if (SplitWords is not null)
                    {
                        string Command = SplitWords[0];
                        string Arguments = Line.RemovePrefix($"{Command} ");
                        bool isBlock = commandBlocks.Contains(Command);
                        if (isBlock)
                        {
                            bool satisfied = false;
                            switch (Command)
                            {
                                case "if":
                                case "while":
                                    satisfied = justLint || MESHConditional.ConditionSatisfied(Arguments);
                                    if (Command == "while")
                                    {
                                        if (!whilePlaces.Contains((l, commandStackNum)))
                                            whilePlaces.Add((l, commandStackNum));
                                        retryLoopCondition = true;
                                    }
                                    break;
                                case "until":
                                    satisfied = justLint || !MESHConditional.ConditionSatisfied(Arguments);
                                    if (!whilePlaces.Contains((l, commandStackNum)))
                                        whilePlaces.Add((l, commandStackNum));
                                    retryLoopCondition = true;
                                    break;
                            }
                            if (satisfied)
                            {
                                // New stack required
                                newCommandStackRequired = true;
                                commandStackNum++;
                                continue;
                            }
                            else if (!justLint)
                            {
                                // Skip all the if block until we reach our stack
                                while (true)
                                {
                                    l++;
                                    if (l < FileLines.Length)
                                        Line = FileLines[l];
                                    string blockStackIndicator = new('|', commandStackNum + 1);
                                    if (!Line.StartsWith(blockStackIndicator))
                                    {
                                        int newStackNum = 0;
                                        int charNum = 0;
                                        while (Line[charNum] == '|')
                                        {
                                            newStackNum++;
                                            charNum++;
                                        }
                                        commandStackNum = newStackNum;
                                        break;
                                    }
                                    if (l >= FileLines.Length)
                                        return;
                                }
                                Line = Line.Substring(commandStackNum);
                                retryLoopCondition = false;

                                // Continue, because the script might have the if condition directly after the stack
                                advance = false;
                                continue;
                            }
                        }
                    }

                    // See if the line is a comment or command
                    if (!Line.StartsWith("#") & !Line.StartsWith(" "))
                    {
                        ConsoleLogger.Debug("Line {0} is not a comment.", Line);
                        if (!justLint)
                            ShellManager.GetLine(Line);
                    }
                    else
                        // For debugging purposes
                        ConsoleLogger.Debug("Line {0} is a comment.", Line);

                    // Increment the new line number
                    LineNo++;
                }
            }
            catch (TerminauxException ex)
            {
                ConsoleLogger.Error(ex, "Error trying to execute script {0} with arguments {1}: {2}", ScriptPath, ScriptArguments, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, "Error trying to execute script {0} with arguments {1}: {2}", ScriptPath, ScriptArguments, ex.Message);
                throw new TerminauxException(LanguageTools.GetLocalized("T_SHELL_BASE_SCRIPTING_EXCEPTION_SCRIPTMALFORMED") + "\n{1}", ex, ex.Message, GetLineHandleString(ScriptPath, LineNo, 0));
            }
        }

        internal static string GetLineHandleString(string path, int line, int column)
        {
            var lineHandle = new LineHandle(path)
            {
                Position = line,
                SourcePosition = column,
                UseColors = false
            };
            return lineHandle.Render();
        }
    }
}
