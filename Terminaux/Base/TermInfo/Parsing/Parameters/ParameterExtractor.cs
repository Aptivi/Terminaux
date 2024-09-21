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
using System.Text;

namespace Terminaux.Base.TermInfo.Parsing.Parameters
{
    /// <summary>
    /// Parameter extractor class (does not process parameters)
    /// </summary>
    public static class ParameterExtractor
    {
        /// <summary>
        /// Extracts the required parameters from the string value with validation
        /// </summary>
        /// <param name="value">String value</param>
        /// <returns>Extracted parameters</returns>
        public static ParameterInfo[] ExtractParameters(string? value)
        {
            List<ParameterInfo> parameters = [];
            if (value is null || string.IsNullOrEmpty(value))
                return [];

            StringBuilder parameterBuilder = new();
            bool isParam = false;
            int conditionNest = 0;
            int index = 0;
            for (int i = 0; i < value.Length; i++)
            {
                char c = value[i];

                // Determine if we need to put the parameter to the list
                if (parameterBuilder.Length > 0 && !isParam)
                {
                    parameters.Add(new(parameterBuilder.ToString(), index));
                    parameterBuilder.Clear();
                }

                // Check to see if we have a parameter designator
                if (!isParam && c == '%')
                {
                    isParam = true;
                    parameterBuilder.Append(c);
                    index = i;
                    continue;
                }
                if (!isParam && conditionNest == 0)
                {
                    index = 0;
                    continue;
                }
                else if (conditionNest > 0)
                {
                    parameterBuilder.Append(c);
                    if (c == '%')
                    {
                        isParam = true;
                        if (i + 1 >= value.Length)
                            throw new TerminauxException("This designator may not be located at the end of the string without specifying one parameter");
                        char parameter = value[i + 1];
                        if (parameter == ';')
                        {
                            conditionNest--;
                            if (conditionNest == 0)
                                isParam = false;
                            parameterBuilder.Append(parameter);
                            i++;
                        }
                        else if (parameter == '?')
                            conditionNest++;
                    }
                    continue;
                }

                // Now, parse everything after the designator, starting from the one-letter-only parameters
                switch (c)
                {
                    case '%':
                    case 'c':
                    case 's':
                    case 'l':
                    case '+':
                    case '-':
                    case '*':
                    case '/':
                    case 'm':
                    case '&':
                    case '|':
                    case '^':
                    case '=':
                    case '<':
                    case '>':
                    case 'A':
                    case 'O':
                    case '!':
                    case '~':
                    case 'i':
                        parameterBuilder.Append(c);
                        isParam = false;
                        continue;
                    case '?':
                        parameterBuilder.Append(c);
                        conditionNest++;
                        index = i - 1;
                        continue;
                }

                // Parse all designators that require two characters (action and parameter)
                switch (c)
                {
                    case 'p':
                        if (i + 1 >= value.Length)
                        {
                            parameterBuilder.Clear();
                            isParam = false;
                            continue;
                        }
                        {
                            char arg = value[i + 1];
                            if (!int.TryParse($"{arg}", out int num))
                                throw new TerminauxException("This designator may only take a number as a parameter.");
                            if (num < 1 || num > 9)
                                throw new TerminauxException("This designator may only take a number from 1 to 9.");
                            parameterBuilder.Append(c);
                            parameterBuilder.Append(arg);
                        }
                        i++;
                        isParam = false;
                        continue;
                    case 'P':
                    case 'g':
                        if (i + 1 >= value.Length)
                        {
                            parameterBuilder.Clear();
                            isParam = false;
                            continue;
                        }
                        {
                            char arg = value[i + 1];
                            if (arg >= 'A' || arg <= 'Z' || arg >= 'a' || arg <= 'z')
                            {   
                                parameterBuilder.Append(c);
                                parameterBuilder.Append(arg);
                            }
                            else
                                throw new TerminauxException("This designator may only take a letter as a parameter.");
                        }
                        i++;
                        isParam = false;
                        continue;
                }

                // Deal with char and int constants
                switch (c)
                {
                    case '{':
                        // Integer constant
                        StringBuilder integerBuilder = new();
                        if (i + 1 >= value.Length)
                        {
                            parameterBuilder.Clear();
                            isParam = false;
                            continue;
                        }
                        {
                            int offset = 1;
                            char arg = value[i + offset];
                            if (arg == '}')
                                throw new TerminauxException("This designator needs an integer constant");
                            while (arg != '}')
                            {
                                integerBuilder.Append(arg);
                                offset++;
                                arg = value[i + offset];
                            }
                            i += offset;
                            if (!int.TryParse(integerBuilder.ToString(), out _))
                                throw new TerminauxException("This designator contains invalid integer constant");
                        }
                        parameterBuilder.Append($"{{{integerBuilder}}}");
                        isParam = false;
                        continue;
                    case '\'':
                        // Character constant
                        if (i + 1 >= value.Length || i + 2 >= value.Length)
                            throw new TerminauxException("This designator may not be located at the end of the string without specifying a character constant");
                        char character = value[i + 1];
                        char ending = value[i + 2];
                        if (ending != '\'')
                            throw new TerminauxException("You can't append more than one character");
                        i += 2;
                        parameterBuilder.Append($"{ending}{character}{ending}");
                        isParam = false;
                        continue;
                }

                // Deal with a character collection
                if (c == '[')
                {
                    StringBuilder collectionBuilder = new();
                    if (i + 1 >= value.Length)
                    {
                        parameterBuilder.Clear();
                        isParam = false;
                        continue;
                    }
                    {
                        int offset = 1;
                        char arg = value[i + offset];
                        if (i + 1 == ']')
                            throw new TerminauxException("This designator needs a character collection");
                        while (arg != ']')
                        {
                            collectionBuilder.Append(arg);
                            offset++;
                            arg = value[i + offset];
                        }
                        i += offset;
                    }
                    parameterBuilder.Append($"[{collectionBuilder}]");
                    isParam = false;
                    continue;
                }

                // Deal with printf(3) string formatting
                switch (c)
                {
                    case 'd':
                    case 'o':
                    case 'X':
                    case 'x':
                    case 's':
                        parameterBuilder.Append(c);
                        isParam = false;
                        continue;
                    case '\u001b':
                    case '\r':
                    case '\f':
                        parameterBuilder.Clear();
                        isParam = false;
                        continue;
                    case ':':
                    case '+':
                    case '#':
                    case ' ':
                        if (i + 1 >= value.Length)
                        {
                            parameterBuilder.Clear();
                            isParam = false;
                            continue;
                        }
                        {
                            char arg = value[i + 1];
                            switch (arg)
                            {
                                case 'd':
                                case 'o':
                                case 'X':
                                case 'x':
                                case 's':
                                    parameterBuilder.Append(c);
                                    parameterBuilder.Append(arg);
                                    isParam = false;
                                    i += 2;
                                    continue;
                                default:
                                    // Look for width or precision designators
                                    if (!int.TryParse($"{arg}", out _) && arg != '.' && arg != '-')
                                        throw new TerminauxException("Width is not valid");
                                    {                                   
                                        StringBuilder widthBuilder = new();
                                        int offset = 1;
                                        char target = value[i + offset];
                                        while (int.TryParse($"{target}", out _) || target == '.' || target == '-')
                                        {
                                            widthBuilder.Append(target);
                                            offset++;
                                            target = value[i + offset];
                                        }
                                        if (widthBuilder[0] == '.')
                                            widthBuilder.Insert(0, 0);
                                        i += offset;
                                        if (i >= value.Length)
                                            throw new TerminauxException("This designator may not be located at the end of the string without specifying a type");
                                        char finalType = value[i];
                                        if (finalType != 'd' && finalType != 'o' && finalType != 'X' && finalType != 'x' && finalType != 's')
                                            throw new TerminauxException("Invalid type");
                                        parameterBuilder.Append(c);
                                        parameterBuilder.Append(widthBuilder);
                                        parameterBuilder.Append(finalType);
                                    }
                                    isParam = false;
                                    continue;
                            }
                        }
                    default:
                        // Look for width or precision designators
                        if (!int.TryParse($"{c}", out _) && c != '.' && c != '-')
                        {
                            parameterBuilder.Clear();
                            isParam = false;
                            continue;
                        }
                        {
                            StringBuilder widthBuilder = new();
                            int offset = 0;
                            char target = value[i + offset];
                            while (int.TryParse($"{target}", out _) || target == '.' || target == '-')
                            {
                                widthBuilder.Append(target);
                                offset++;
                                target = value[i + offset];
                            }
                            if (widthBuilder[0] == '.')
                                widthBuilder.Insert(0, 0);
                            i += offset;
                            if (i >= value.Length)
                                throw new TerminauxException("This designator may not be located at the end of the string without specifying a type");
                            char finalType = value[i];
                            if (finalType != 'd' && finalType != 'o' && finalType != 'X' && finalType != 'x' && finalType != 's')
                                throw new TerminauxException("Invalid type");
                            parameterBuilder.Append(widthBuilder);
                            parameterBuilder.Append(finalType);
                        }
                        isParam = false;
                        continue;
                }
            }
            if (parameterBuilder.Length > 0)
                parameters.Add(new(parameterBuilder.ToString(), index));
            return [.. parameters];
        }
    }
}
