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
using System.Collections.Generic;
using System.Text;
using Textify.General;

namespace Terminaux.Base.TermInfo.Parsing.Parameters
{
    internal static class ParameterProcessor
    {
        internal static string ProcessSequenceParams(TermInfoValueDesc<string> valueDesc, object?[]? args)
        {
            // Sanity checks
            args ??= [];
            string sequence = valueDesc.Value ??
                throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORNOSEQUENCE"));
            var parameters = valueDesc.Parameters ??
                throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORNOPARAMETERS"));
            return ProcessSequenceParams(sequence, parameters, args);
        }

        private static string ProcessSequenceParams(string sequence, ParameterInfo[] parameters, object?[]? args)
        {
            // Sanity checks
            args ??= [];
            Queue<string> popArgs = [];
            return ProcessSequenceParams(sequence, parameters, args, ref popArgs);
        }

        private static string ProcessSequenceParams(string sequence, ParameterInfo[] parameters, object?[]? args, ref Queue<string> popArgs)
        {
            // Sanity checks
            args ??= [];

            // Evaluate the extracted parameters and add them to the replacement list
            List<(int idx, string token, string val)> replacements = [];
            Dictionary<char, string> variables = [];
            bool addFirstTwo = false;
            for (int paramIdx = 0; paramIdx < parameters.Length; paramIdx++)
            {
                // Get necessary variables
                ParameterInfo parameter = parameters[paramIdx];
                string paramToken = parameter.Representation;
                int paramStrIdx = parameter.Index;
                var paramType = parameter.Type;

                // According to type, we need to change our behavior. The token starts with %.
                switch (paramType)
                {
                    case ParameterType.Literal:
                        replacements.Add((paramStrIdx, paramToken, "%"));
                        break;
                    case ParameterType.Formatting:
                        {
                            // Handle cases where optional parameters, such as flags, are specified
                            StringBuilder flagBuilder = new();
                            StringBuilder widthBuilder = new();
                            char formatMode = default;
                            for (int i = 1; i < paramToken.Length; i++)
                            {
                                char c = paramToken[i];
                                switch (c)
                                {
                                    // Flags
                                    case ':':
                                        if (paramToken[i + 1] != '-')
                                            throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORMINUSREQUIRED"));
                                        flagBuilder.Append("-");
                                        break;
                                    case '+':
                                    case '#':
                                    case ' ':
                                        flagBuilder.Append($"{c}");
                                        break;

                                    // Numeric digits
                                    case '0':
                                    case '1':
                                    case '2':
                                    case '3':
                                    case '4':
                                    case '5':
                                    case '6':
                                    case '7':
                                    case '8':
                                    case '9':
                                    case '.':
                                        widthBuilder.Append($"{c}");
                                        break;

                                    // Printing mode
                                    case 'd':
                                    case 'o':
                                    case 'x':
                                    case 'X':
                                    case 's':
                                        formatMode = c;
                                        if (i != paramToken.Length - 1)
                                            throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORGARBAGEDATA").FormatString(i, paramToken.Length - 1));
                                        break;
                                }
                            }

                            // Build necessary variables
                            string flags = flagBuilder.ToString();
                            double width =
                                widthBuilder.Length > 0 ?
                                double.Parse(widthBuilder.ToString()) :
                                1;

                            // Now, format the string as appropriate
                            string formatted = "";
                            string poppedString = popArgs.Dequeue();
                            double poppedDouble = double.Parse(poppedString);
                            string poppedDoubleString = poppedDouble.ToString();
                            int precLen = poppedDoubleString.Contains(".") ? int.Parse(poppedDoubleString.Substring(poppedDoubleString.IndexOf('.') + 1)) : 0;
                            int widthLen = (int)Math.Truncate(width);
                            bool usePadding = !flags.Contains("#");
                            switch (formatMode)
                            {
                                case 'd':
                                    {
                                        formatted =
                                            width > 1 ?
                                            poppedDouble.ToString(new string(usePadding ? '0' : '#', widthLen) + (precLen > 0 ? $".{new string(usePadding ? '0' : '#', precLen)}" : "")) :
                                            poppedDoubleString;
                                        if (width == 0 && poppedDouble == 0)
                                            formatted = "";
                                    }
                                    break;
                                case 'o':
                                    {
                                        poppedDouble = Convert.ToDouble(Convert.ToString((int)poppedDouble, 8));
                                        formatted =
                                            width > 1 ?
                                            poppedDouble.ToString(new string(usePadding ? '0' : '#', widthLen) + (precLen > 0 ? $".{new string(usePadding ? '0' : '#', precLen)}" : "")) :
                                            poppedDoubleString;
                                        if (width == 0 && poppedDouble == 0)
                                            formatted = "";
                                    }
                                    break;
                                case 'x':
                                case 'X':
                                    {
                                        string hex = Convert.ToString((int)poppedDouble, 16);
                                        hex = formatMode == 'X' ? hex.ToUpper() : hex;
                                        formatted = hex;
                                        if (width == 0 && poppedDouble == 0)
                                            formatted = "";
                                    }
                                    break;
                                case 's':
                                    {
                                        formatted = poppedString;
                                        if (precLen > 0 && precLen <= poppedString.Length)
                                            formatted = formatted.Substring(0, precLen);
                                    }
                                    break;
                            }

                            // Add the formatted string
                            replacements.Add((paramStrIdx, paramToken, formatted));
                        }
                        break;
                    case ParameterType.PopChar:
                        {
                            string poppedString = popArgs.Dequeue();
                            if (!int.TryParse(poppedString, out int charNum))
                                throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORINVALIDINTCONSTCHAR") + $" {poppedString}");
                            char character = (char)charNum;
                            replacements.Add((paramStrIdx, paramToken, $"{character}"));
                        }
                        break;
                    case ParameterType.PopString:
                        {
                            string poppedString = popArgs.Dequeue();
                            replacements.Add((paramStrIdx, paramToken, poppedString));
                        }
                        break;
                    case ParameterType.PushParam:
                        replacements.Add((paramStrIdx, paramToken, ""));
                        char paramNumChar = paramToken[2];
                        if (!int.TryParse($"{paramNumChar}", out int paramNum))
                            throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORINVALIDINTCONST") + $" {paramNumChar}");
                        var objectPush = args[paramNum - 1];
                        popArgs.Enqueue($"{objectPush}");
                        break;
                    case ParameterType.SetVariable:
                        {
                            replacements.Add((paramStrIdx, paramToken, ""));
                            char varChar = paramToken[2];
                            string pushed = popArgs.Dequeue();
                            if (!variables.ContainsKey(varChar))
                                variables.Add(varChar, pushed);
                            else
                                variables[varChar] = pushed;
                        }
                        break;
                    case ParameterType.GetVariable:
                        {
                            replacements.Add((paramStrIdx, paramToken, ""));
                            char varChar = paramToken[2];
                            if (!variables.ContainsKey(varChar))
                                throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORNOVAR"));
                            string toPush = variables[varChar];
                            popArgs.Enqueue(toPush);
                        }
                        break;
                    case ParameterType.CharConst:
                        replacements.Add((paramStrIdx, paramToken, ""));
                        char characterConst = paramToken[2];
                        popArgs.Enqueue($"{characterConst}");
                        break;
                    case ParameterType.CharList:
                        break;
                    case ParameterType.IntConst:
                        replacements.Add((paramStrIdx, paramToken, ""));
                        string integerConstStr = paramToken.Substring(2, paramToken.Length - 3);
                        if (!int.TryParse(integerConstStr, out _))
                            throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORINVALIDINTCONST") + $" {integerConstStr}");
                        popArgs.Enqueue(integerConstStr);
                        break;
                    case ParameterType.StringLength:
                        {
                            replacements.Add((paramStrIdx, paramToken, ""));
                            string pushed = popArgs.Dequeue();
                            popArgs.Enqueue($"{pushed.Length}");
                        }
                        break;
                    case ParameterType.ArithmeticAdd:
                        // Attempt to convert last two enqueued arguments to numbers
                        replacements.Add((paramStrIdx, paramToken, ""));
                        if (popArgs.Count < 2)
                            throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORBINARYMORETHANTWO").FormatString(popArgs.Count));

                        {
                            // Remove last two arguments from the stack, and check for integers
                            string first = popArgs.Dequeue();
                            string second = popArgs.Dequeue();
                            if (!int.TryParse(first, out int firstInt))
                                throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORINTFIRSTOPERANDINVALID") + $" {first}");
                            if (!int.TryParse(second, out int secondInt))
                                throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORINTSECONDOPERANDINVALID") + $" {second}");

                            // Now, add the two numbers and pass it to the stack as a string
                            int result = firstInt + secondInt;
                            string resultStr = $"{result}";
                            popArgs.Enqueue(resultStr);
                        }
                        break;
                    case ParameterType.ArithmeticSub:
                        // Attempt to convert last two enqueued arguments to numbers
                        replacements.Add((paramStrIdx, paramToken, ""));
                        if (popArgs.Count < 2)
                            throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORBINARYMORETHANTWO").FormatString(popArgs.Count));

                        {
                            // Remove last two arguments from the stack, and check for integers
                            string first = popArgs.Dequeue();
                            string second = popArgs.Dequeue();
                            if (!int.TryParse(first, out int firstInt))
                                throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORINTFIRSTOPERANDINVALID") + $" {first}");
                            if (!int.TryParse(second, out int secondInt))
                                throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORINTSECONDOPERANDINVALID") + $" {second}");

                            // Now, subtract the two numbers and pass it to the stack as a string
                            int result = firstInt - secondInt;
                            string resultStr = $"{result}";
                            popArgs.Enqueue(resultStr);
                        }
                        break;
                    case ParameterType.ArithmeticMul:
                        // Attempt to convert last two enqueued arguments to numbers
                        replacements.Add((paramStrIdx, paramToken, ""));
                        if (popArgs.Count < 2)
                            throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORBINARYMORETHANTWO").FormatString(popArgs.Count));

                        {
                            // Remove last two arguments from the stack, and check for integers
                            string first = popArgs.Dequeue();
                            string second = popArgs.Dequeue();
                            if (!int.TryParse(first, out int firstInt))
                                throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORINTFIRSTOPERANDINVALID") + $" {first}");
                            if (!int.TryParse(second, out int secondInt))
                                throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORINTSECONDOPERANDINVALID") + $" {second}");

                            // Now, multiply the two numbers and pass it to the stack as a string
                            int result = firstInt * secondInt;
                            string resultStr = $"{result}";
                            popArgs.Enqueue(resultStr);
                        }
                        break;
                    case ParameterType.ArithmeticDiv:
                        // Attempt to convert last two enqueued arguments to numbers
                        replacements.Add((paramStrIdx, paramToken, ""));
                        if (popArgs.Count < 2)
                            throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORBINARYMORETHANTWO").FormatString(popArgs.Count));

                        {
                            // Remove last two arguments from the stack, and check for integers
                            string first = popArgs.Dequeue();
                            string second = popArgs.Dequeue();
                            if (!int.TryParse(first, out int firstInt))
                                throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORINTFIRSTOPERANDINVALID") + $" {first}");
                            if (!int.TryParse(second, out int secondInt))
                                throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORINTSECONDOPERANDINVALID") + $" {second}");

                            // Now, divide the two numbers and pass it to the stack as a string
                            int result = firstInt / secondInt;
                            string resultStr = $"{result}";
                            popArgs.Enqueue(resultStr);
                        }
                        break;
                    case ParameterType.ArithmeticMod:
                        // Attempt to convert last two enqueued arguments to numbers
                        replacements.Add((paramStrIdx, paramToken, ""));
                        if (popArgs.Count < 2)
                            throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORBINARYMORETHANTWO").FormatString(popArgs.Count));

                        {
                            // Remove last two arguments from the stack, and check for integers
                            string first = popArgs.Dequeue();
                            string second = popArgs.Dequeue();
                            if (!int.TryParse(first, out int firstInt))
                                throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORINTFIRSTOPERANDINVALID") + $" {first}");
                            if (!int.TryParse(second, out int secondInt))
                                throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORINTSECONDOPERANDINVALID") + $" {second}");

                            // Now, get the modulus of the two numbers and pass it to the stack as a string
                            int result = firstInt % secondInt;
                            string resultStr = $"{result}";
                            popArgs.Enqueue(resultStr);
                        }
                        break;
                    case ParameterType.BitwiseAnd:
                        replacements.Add((paramStrIdx, paramToken, ""));
                        if (popArgs.Count < 2)
                            throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORBINARYMORETHANTWO").FormatString(popArgs.Count));

                        {
                            // Remove last two arguments from the stack, and check for integers
                            string first = popArgs.Dequeue();
                            string second = popArgs.Dequeue();
                            if (!int.TryParse(first, out int firstInt))
                                throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORINTFIRSTOPERANDINVALID") + $" {first}");
                            if (!int.TryParse(second, out int secondInt))
                                throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORINTSECONDOPERANDINVALID") + $" {second}");

                            // Now, perform a bitwise AND
                            int result = firstInt & secondInt;
                            string resultStr = $"{result}";
                            popArgs.Enqueue(resultStr);
                        }
                        break;
                    case ParameterType.BitwiseOr:
                        replacements.Add((paramStrIdx, paramToken, ""));
                        if (popArgs.Count < 2)
                            throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORBINARYMORETHANTWO").FormatString(popArgs.Count));

                        {
                            // Remove last two arguments from the stack, and check for integers
                            string first = popArgs.Dequeue();
                            string second = popArgs.Dequeue();
                            if (!int.TryParse(first, out int firstInt))
                                throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORINTFIRSTOPERANDINVALID") + $" {first}");
                            if (!int.TryParse(second, out int secondInt))
                                throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORINTSECONDOPERANDINVALID") + $" {second}");

                            // Now, perform a bitwise OR
                            int result = firstInt | secondInt;
                            string resultStr = $"{result}";
                            popArgs.Enqueue(resultStr);
                        }
                        break;
                    case ParameterType.BitwiseXOr:
                        replacements.Add((paramStrIdx, paramToken, ""));
                        if (popArgs.Count < 2)
                            throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORBINARYMORETHANTWO").FormatString(popArgs.Count));

                        {
                            // Remove last two arguments from the stack, and check for integers
                            string first = popArgs.Dequeue();
                            string second = popArgs.Dequeue();
                            if (!int.TryParse(first, out int firstInt))
                                throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORINTFIRSTOPERANDINVALID") + $" {first}");
                            if (!int.TryParse(second, out int secondInt))
                                throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORINTSECONDOPERANDINVALID") + $" {second}");

                            // Now, perform a bitwise XOR
                            int result = firstInt ^ secondInt;
                            string resultStr = $"{result}";
                            popArgs.Enqueue(resultStr);
                        }
                        break;
                    case ParameterType.LogicalEqual:
                        replacements.Add((paramStrIdx, paramToken, ""));
                        if (popArgs.Count < 2)
                            throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORBINARYMORETHANTWO").FormatString(popArgs.Count));

                        {
                            // Remove last two arguments from the stack, and check for integers
                            string first = popArgs.Dequeue();
                            string second = popArgs.Dequeue();
                            if (!int.TryParse(first, out int firstInt))
                                throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORINTFIRSTOPERANDINVALID") + $" {first}");
                            if (!int.TryParse(second, out int secondInt))
                                throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORINTSECONDOPERANDINVALID") + $" {second}");

                            // Now, test for equality
                            int result = firstInt == secondInt ? 1 : 0;
                            string resultStr = $"{result}";
                            popArgs.Enqueue(resultStr);
                        }
                        break;
                    case ParameterType.LogicalGreaterThan:
                        replacements.Add((paramStrIdx, paramToken, ""));
                        if (popArgs.Count < 2)
                            throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORBINARYMORETHANTWO").FormatString(popArgs.Count));

                        {
                            // Remove last two arguments from the stack, and check for integers
                            string first = popArgs.Dequeue();
                            string second = popArgs.Dequeue();
                            if (!int.TryParse(first, out int firstInt))
                                throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORINTFIRSTOPERANDINVALID") + $" {first}");
                            if (!int.TryParse(second, out int secondInt))
                                throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORINTSECONDOPERANDINVALID") + $" {second}");

                            // Now, test for equality
                            int result = firstInt > secondInt ? 1 : 0;
                            string resultStr = $"{result}";
                            popArgs.Enqueue(resultStr);
                        }
                        break;
                    case ParameterType.LogicalLessThan:
                        replacements.Add((paramStrIdx, paramToken, ""));
                        if (popArgs.Count < 2)
                            throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORBINARYMORETHANTWO").FormatString(popArgs.Count));

                        {
                            // Remove last two arguments from the stack, and check for integers
                            string first = popArgs.Dequeue();
                            string second = popArgs.Dequeue();
                            if (!int.TryParse(first, out int firstInt))
                                throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORINTFIRSTOPERANDINVALID") + $" {first}");
                            if (!int.TryParse(second, out int secondInt))
                                throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORINTSECONDOPERANDINVALID") + $" {second}");

                            // Now, test for equality
                            int result = firstInt < secondInt ? 1 : 0;
                            string resultStr = $"{result}";
                            popArgs.Enqueue(resultStr);
                        }
                        break;
                    case ParameterType.LogicalAnd:
                        replacements.Add((paramStrIdx, paramToken, ""));
                        if (popArgs.Count < 2)
                            throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORBINARYMORETHANTWO").FormatString(popArgs.Count));

                        {
                            // Remove last two arguments from the stack, and check for integers
                            string first = popArgs.Dequeue();
                            string second = popArgs.Dequeue();
                            if (!int.TryParse(first, out int firstInt))
                                throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORINTFIRSTOPERANDINVALID") + $" {first}");
                            if (!int.TryParse(second, out int secondInt))
                                throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORINTSECONDOPERANDINVALID") + $" {second}");

                            // Now, test for equality
                            int result = (firstInt != 0) && (secondInt != 0) ? 1 : 0;
                            string resultStr = $"{result}";
                            popArgs.Enqueue(resultStr);
                        }
                        break;
                    case ParameterType.LogicalOr:
                        replacements.Add((paramStrIdx, paramToken, ""));
                        if (popArgs.Count < 2)
                            throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORBINARYMORETHANTWO").FormatString(popArgs.Count));

                        {
                            // Remove last two arguments from the stack, and check for integers
                            string first = popArgs.Dequeue();
                            string second = popArgs.Dequeue();
                            if (!int.TryParse(first, out int firstInt))
                                throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORINTFIRSTOPERANDINVALID") + $" {first}");
                            if (!int.TryParse(second, out int secondInt))
                                throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORINTSECONDOPERANDINVALID") + $" {second}");

                            // Now, test for equality
                            int result = (firstInt != 0) || (secondInt != 0) ? 1 : 0;
                            string resultStr = $"{result}";
                            popArgs.Enqueue(resultStr);
                        }
                        break;
                    case ParameterType.UnaryLogicalComplement:
                        replacements.Add((paramStrIdx, paramToken, ""));
                        if (popArgs.Count < 1)
                            throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORUNARYMORETHANONE").FormatString(popArgs.Count));

                        {
                            // Remove last argument from the stack, and check for integers
                            string lastValue = popArgs.Dequeue();
                            if (!int.TryParse(lastValue, out int unaryValue))
                                throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORINVALIDINTCONST") + $" {lastValue}");

                            // Now, test for logical complement
                            int result = unaryValue == 0 ? 1 : 0;
                            string resultStr = $"{result}";
                            popArgs.Enqueue(resultStr);
                        }
                        break;
                    case ParameterType.UnaryBitComplement:
                        replacements.Add((paramStrIdx, paramToken, ""));
                        if (popArgs.Count < 1)
                            throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORUNARYMORETHANONE").FormatString(popArgs.Count));

                        {
                            // Remove last argument from the stack, and check for integers
                            string lastValue = popArgs.Dequeue();
                            if (!int.TryParse(lastValue, out int unaryValue))
                                throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORINVALIDINTCONST") + $" {lastValue}");

                            // Now, test for bit complement
                            int result = ~unaryValue;
                            string resultStr = $"{result}";
                            popArgs.Enqueue(resultStr);
                        }
                        break;
                    case ParameterType.AddOneToTwoParams:
                        replacements.Add((paramStrIdx, paramToken, ""));
                        addFirstTwo = true;
                        break;
                    case ParameterType.Conditional:
                        var syntaxes = GetConditionSyntaxes(paramToken);
                        string conditionResult = "";
                        bool skip = false;
                        bool bail = false;
                        foreach ((string syntax, ParameterConditionSyntaxType syntaxType) in syntaxes)
                        {
                            // Check if we need to skip an expression or bail
                            if (bail)
                                break;
                            if (skip)
                            {
                                skip = false;
                                continue;
                            }

                            // Get the syntax value
                            var syntaxParams = ParameterExtractor.ExtractParameters(syntax);
                            string syntaxValue = ProcessSequenceParams(syntax, syntaxParams, args, ref popArgs);

                            // Test the syntax or add it to the result
                            switch (syntaxType)
                            {
                                case ParameterConditionSyntaxType.Expression:
                                    conditionResult = syntaxValue;
                                    bail = true;
                                    break;
                                case ParameterConditionSyntaxType.Condition:
                                    string conditionResultStr = popArgs.Dequeue();
                                    skip = (int.TryParse(conditionResultStr, out int num) && num == 0) || conditionResultStr.Length == 0;
                                    break;
                            }
                        }
                        replacements.Add((paramStrIdx, paramToken, conditionResult));
                        break;
                }
            }

            // Find out where we would add one to the first two numeric arguments
            int integersProcessed = 0;
            for (int i = 0; i < replacements.Count; i++)
            {
                (int idx, string token, string val) = replacements[i];
                if (!int.TryParse(val, out int result))
                    continue;

                // Process this number
                if (integersProcessed < 2 && addFirstTwo)
                    result++;
                replacements[i] = (idx, token, $"{result}");
                integersProcessed++;
            }

            // Now, actually process replacements
            var finalValue = new StringBuilder(sequence);
            for (int i = replacements.Count - 1; i >= 0; i--)
            {
                (int idx, string token, string val) = replacements[i];
                finalValue.Remove(idx, token.Length);
                finalValue.Insert(idx, val);
            }
            return finalValue.ToString();
        }

        private static (string syntax, ParameterConditionSyntaxType syntaxType)[] GetConditionSyntaxes(string condition)
        {
            List<(string, ParameterConditionSyntaxType)> syntaxes = [];
            if (!condition.StartsWith("%?") && !condition.EndsWith("%;"))
                throw new TerminauxInternalException(LanguageTools.GetLocalized("T_CT_PARSING_PARAMETERS_EXCEPTION_PROCESSORCONDITIONALINVALID").FormatString(condition));

            // Remove the condition beginning and ending
            string representation = condition.RemovePrefix("%?").RemoveSuffix("%;");

            // Now, determine how we'll distribute the syntaxes. The else part might either be a value or a conditional
            // similar to "else if" in C#. We'll need to distinguish between %t (Then) and %e (Else or Else If).
            var syntaxBuilder = new StringBuilder();
            ParameterConditionSyntaxType type = ParameterConditionSyntaxType.Condition;
            for (int i = 0; i < representation.Length; i++)
            {
                char current = representation[i];
                char next = i + 1 >= representation.Length ? '\0' : representation[i + 1];
                if (current == '%' && (next == 't' || next == 'e'))
                {
                    syntaxes.Add((syntaxBuilder.ToString(), type));
                    type = next == 'e' ? ParameterConditionSyntaxType.Condition : ParameterConditionSyntaxType.Expression;
                    syntaxBuilder.Clear();
                    i++;
                }
                else
                    syntaxBuilder.Append(current);
            }
            if (syntaxBuilder.Length > 0)
                syntaxes.Add((syntaxBuilder.ToString(), ParameterConditionSyntaxType.Expression));

            // Return the list of syntaxes
            return [.. syntaxes];
        }
    }
}
