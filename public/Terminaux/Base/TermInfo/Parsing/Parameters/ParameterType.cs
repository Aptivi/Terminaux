//
// Terminaux  Copyright (C) 2023-2026  Aptivi
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

namespace Terminaux.Base.TermInfo.Parsing.Parameters
{
    /// <summary>
    /// Parameter type
    /// </summary>
    public enum ParameterType
    {
        /// <summary>
        /// Unknown parameter type
        /// </summary>
        Unknown,
        /// <summary>
        /// Outputs %: <c>%%</c>
        /// </summary>
        Literal,
        /// <summary>
        /// String formatting as in printf(3): <c>%[[:]flags][width[.precision]][doxXs]</c>
        /// </summary>
        Formatting,
        /// <summary>
        /// Pops a value as a character: <c>%c</c>
        /// </summary>
        PopChar,
        /// <summary>
        /// Pops a value as a string: <c>%s</c>
        /// </summary>
        PopString,
        /// <summary>
        /// Pushes nth parameter: <c>%p[1-9]</c>
        /// </summary>
        PushParam,
        /// <summary>
        /// Sets a variable: <c>%P[a-z]</c> / <c>%P[A-Z]</c>
        /// </summary>
        SetVariable,
        /// <summary>
        /// Gets a variable: <c>%g[a-z]</c> / <c>%g[A-Z]</c>
        /// </summary>
        GetVariable,
        /// <summary>
        /// Character constant: <c>%'c'</c>
        /// </summary>
        CharConst,
        /// <summary>
        /// Character list: <c>%[abc...]</c>
        /// </summary>
        CharList,
        /// <summary>
        /// Integer constant: <c>%{nn}</c>
        /// </summary>
        IntConst,
        /// <summary>
        /// String length of a pop'd variable: <c>%l</c>
        /// </summary>
        StringLength,
        /// <summary>
        /// Arithmetic addition: <c>%+</c>
        /// </summary>
        ArithmeticAdd,
        /// <summary>
        /// Arithmetic subtraction: <c>%-</c>
        /// </summary>
        ArithmeticSub,
        /// <summary>
        /// Arithmetic multiplication: <c>%*</c>
        /// </summary>
        ArithmeticMul,
        /// <summary>
        /// Arithmetic division: <c>%/</c>
        /// </summary>
        ArithmeticDiv,
        /// <summary>
        /// Arithmetic modulus: <c>%m</c>
        /// </summary>
        ArithmeticMod,
        /// <summary>
        /// Bitwise AND operator: <c>%&amp;</c>
        /// </summary>
        BitwiseAnd,
        /// <summary>
        /// Bitwise OR operator: <c>%|</c>
        /// </summary>
        BitwiseOr,
        /// <summary>
        /// Bitwise XOR operator: <c>%^</c>
        /// </summary>
        BitwiseXOr,
        /// <summary>
        /// Logical equals operator: <c>%=</c>
        /// </summary>
        LogicalEqual,
        /// <summary>
        /// Logical greater than operator: <c>%&gt;</c>
        /// </summary>
        LogicalGreaterThan,
        /// <summary>
        /// Logical less than operator: <c>%&lt;</c>
        /// </summary>
        LogicalLessThan,
        /// <summary>
        /// Logical AND operator: <c>%A</c>
        /// </summary>
        LogicalAnd,
        /// <summary>
        /// Logical OR operator: <c>%O</c>
        /// </summary>
        LogicalOr,
        /// <summary>
        /// Unary logical complement: <c>%!</c>
        /// </summary>
        UnaryLogicalComplement,
        /// <summary>
        /// Unary bit complement: <c>%~</c>
        /// </summary>
        UnaryBitComplement,
        /// <summary>
        /// Add one to the first two parameters: <c>%i</c>
        /// </summary>
        AddOneToTwoParams,
        /// <summary>
        /// Conditional operator: <c>%? expr %t thenpart %e elsepart %;</c>
        /// </summary>
        Conditional,
    }
}
