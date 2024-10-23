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
using System.Runtime.InteropServices;
using System.Text;

namespace Terminaux.Base.TermInfo.Parsing.Parameters
{
    internal static class ParameterProcessor
    {
        internal static string ProcessSequenceParams(TermInfoValueDesc<string> valueDesc, object?[]? args)
        {
            // Sanity checks
            args ??= [];
            string sequence = valueDesc.Value ??
                throw new TerminauxInternalException("Can't process null sequence.");
            var parameters = valueDesc.Parameters ??
                throw new TerminauxInternalException("Can't process null parameters.");

            // Evaluate the extracted parameters and replace them
            var finalValue = new StringBuilder(valueDesc.Value);
            Queue<string> popArgs = [];
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
                        finalValue.Remove(paramStrIdx, paramToken.Length);
                        finalValue.Insert(paramStrIdx, "%");
                        break;
                    case ParameterType.Formatting:
                        break;
                    case ParameterType.PopChar:
                        finalValue.Remove(paramStrIdx, paramToken.Length);
                        {
                            string poppedString = popArgs.Dequeue();
                            if (!int.TryParse(poppedString, out int charNum))
                                throw new TerminauxInternalException($"Integer constant for character is not valid. {poppedString}");
                            char character = (char)charNum;
                            finalValue.Insert(paramStrIdx, character);
                        }
                        break;
                    case ParameterType.PopString:
                        finalValue.Remove(paramStrIdx, paramToken.Length);
                        {
                            string poppedString = popArgs.Dequeue();
                            finalValue.Insert(paramStrIdx, poppedString);
                        }
                        break;
                    case ParameterType.PushParam:
                        finalValue.Remove(paramStrIdx, paramToken.Length);
                        char paramNumChar = paramToken[2];
                        if (!int.TryParse($"{paramNumChar}", out int paramNum))
                            throw new TerminauxInternalException($"Integer constant is not valid. {paramNumChar}");
                        var objectPush = args[paramNum - 1];
                        break;
                    case ParameterType.SetVariable:
                        break;
                    case ParameterType.GetVariable:
                        break;
                    case ParameterType.CharConst:
                        finalValue.Remove(paramStrIdx, paramToken.Length);
                        char characterConst = paramToken[2];
                        popArgs.Enqueue($"{characterConst}");
                        break;
                    case ParameterType.CharList:
                        break;
                    case ParameterType.IntConst:
                        finalValue.Remove(paramStrIdx, paramToken.Length);
                        string integerConstStr = paramToken.Substring(2, paramToken.Length - 3);
                        if (!int.TryParse(integerConstStr, out _))
                            throw new TerminauxInternalException($"Integer constant is not valid. {integerConstStr}");
                        popArgs.Enqueue(integerConstStr);
                        break;
                    case ParameterType.StringLength:
                        break;
                    case ParameterType.ArithmeticAdd:
                        // Attempt to convert last two enqueued arguments to numbers
                        finalValue.Remove(paramStrIdx, paramToken.Length);
                        if (popArgs.Count < 2)
                            throw new TerminauxInternalException($"This is a binary operation, but enqueued argument count is {popArgs.Count}");

                        {
                            // Remove last two arguments from the stack, and check for integers
                            string second = popArgs.Dequeue();
                            string first = popArgs.Dequeue();
                            if (!int.TryParse(second, out int secondInt))
                                throw new TerminauxInternalException($"Integer constant for second operand is not valid. {second}");
                            if (!int.TryParse(first, out int firstInt))
                                throw new TerminauxInternalException($"Integer constant for first operand is not valid. {first}");

                            // Now, add the two numbers and pass it to the stack as a string
                            int result = firstInt + secondInt;
                            string resultStr = $"{result}";
                            popArgs.Enqueue(resultStr);
                        }
                        break;
                    case ParameterType.ArithmeticSub:
                        // Attempt to convert last two enqueued arguments to numbers
                        finalValue.Remove(paramStrIdx, paramToken.Length);
                        if (popArgs.Count < 2)
                            throw new TerminauxInternalException($"This is a binary operation, but enqueued argument count is {popArgs.Count}");

                        {
                            // Remove last two arguments from the stack, and check for integers
                            string second = popArgs.Dequeue();
                            string first = popArgs.Dequeue();
                            if (!int.TryParse(second, out int secondInt))
                                throw new TerminauxInternalException($"Integer constant for second operand is not valid. {second}");
                            if (!int.TryParse(first, out int firstInt))
                                throw new TerminauxInternalException($"Integer constant for first operand is not valid. {first}");

                            // Now, subtract the two numbers and pass it to the stack as a string
                            int result = firstInt - secondInt;
                            string resultStr = $"{result}";
                            popArgs.Enqueue(resultStr);
                        }
                        break;
                    case ParameterType.ArithmeticMul:
                        // Attempt to convert last two enqueued arguments to numbers
                        finalValue.Remove(paramStrIdx, paramToken.Length);
                        if (popArgs.Count < 2)
                            throw new TerminauxInternalException($"This is a binary operation, but enqueued argument count is {popArgs.Count}");

                        {
                            // Remove last two arguments from the stack, and check for integers
                            string second = popArgs.Dequeue();
                            string first = popArgs.Dequeue();
                            if (!int.TryParse(second, out int secondInt))
                                throw new TerminauxInternalException($"Integer constant for second operand is not valid. {second}");
                            if (!int.TryParse(first, out int firstInt))
                                throw new TerminauxInternalException($"Integer constant for first operand is not valid. {first}");

                            // Now, multiply the two numbers and pass it to the stack as a string
                            int result = firstInt * secondInt;
                            string resultStr = $"{result}";
                            popArgs.Enqueue(resultStr);
                        }
                        break;
                    case ParameterType.ArithmeticDiv:
                        // Attempt to convert last two enqueued arguments to numbers
                        finalValue.Remove(paramStrIdx, paramToken.Length);
                        if (popArgs.Count < 2)
                            throw new TerminauxInternalException($"This is a binary operation, but enqueued argument count is {popArgs.Count}");

                        {
                            // Remove last two arguments from the stack, and check for integers
                            string second = popArgs.Dequeue();
                            string first = popArgs.Dequeue();
                            if (!int.TryParse(second, out int secondInt))
                                throw new TerminauxInternalException($"Integer constant for second operand is not valid. {second}");
                            if (!int.TryParse(first, out int firstInt))
                                throw new TerminauxInternalException($"Integer constant for first operand is not valid. {first}");

                            // Now, divide the two numbers and pass it to the stack as a string
                            int result = firstInt / secondInt;
                            string resultStr = $"{result}";
                            popArgs.Enqueue(resultStr);
                        }
                        break;
                    case ParameterType.ArithmeticMod:
                        // Attempt to convert last two enqueued arguments to numbers
                        finalValue.Remove(paramStrIdx, paramToken.Length);
                        if (popArgs.Count < 2)
                            throw new TerminauxInternalException($"This is a binary operation, but enqueued argument count is {popArgs.Count}");

                        {
                            // Remove last two arguments from the stack, and check for integers
                            string second = popArgs.Dequeue();
                            string first = popArgs.Dequeue();
                            if (!int.TryParse(second, out int secondInt))
                                throw new TerminauxInternalException($"Integer constant for second operand is not valid. {second}");
                            if (!int.TryParse(first, out int firstInt))
                                throw new TerminauxInternalException($"Integer constant for first operand is not valid. {first}");

                            // Now, get the modulus of the two numbers and pass it to the stack as a string
                            int result = firstInt % secondInt;
                            string resultStr = $"{result}";
                            popArgs.Enqueue(resultStr);
                        }
                        break;
                    case ParameterType.BitwiseAnd:
                        break;
                    case ParameterType.BitwiseOr:
                        break;
                    case ParameterType.BitwiseXOr:
                        break;
                    case ParameterType.LogicalEqual:
                        break;
                    case ParameterType.LogicalGreaterThan:
                        break;
                    case ParameterType.LogicalLessThan:
                        break;
                    case ParameterType.LogicalAnd:
                        break;
                    case ParameterType.LogicalOr:
                        break;
                    case ParameterType.UnaryLogicalComplement:
                        break;
                    case ParameterType.UnaryBitComplement:
                        break;
                    case ParameterType.AddOneToTwoParams:
                        break;
                    case ParameterType.Conditional:
                        break;
                }
            }
            return finalValue.ToString();
        }
    }
}
