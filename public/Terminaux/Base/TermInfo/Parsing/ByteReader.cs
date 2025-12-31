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

using System;
using System.IO;

namespace Terminaux.Base.TermInfo.Parsing
{
    internal sealed class ByteReader
    {
        internal readonly BinaryReader _reader;

        public int Position { get; set; }

        public bool Eof() =>
            _reader.PeekChar() == -1;

        public int ReadByte()
        {
            var result = _reader.Read();
            if (result != -1)
                Position++;

            return result;
        }

        public byte[] ReadBytes(int count)
        {
            var buffer = new byte[count];
            var read = Read(buffer, count);
            if (read != count)
                throw new InvalidOperationException(LanguageTools.GetLocalized("T_CT_PARSING_EXCEPTION_BYTESREAD"));

            return buffer;
        }

        public int Read(byte[] buffer, int count)
        {
            var result = _reader.Read(buffer, 0, count);
            Position += result;
            return result;
        }

        public ByteReader(Stream stream)
        {
            _reader = new BinaryReader(stream);
        }
    }
}
