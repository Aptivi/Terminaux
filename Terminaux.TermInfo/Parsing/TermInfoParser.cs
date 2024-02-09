//
// Terminaux.TermInfo  Copyright (C) 2023-2024  Aptivi
//
// This file is part of Terminaux.TermInfo
//
// Terminaux.TermInfo is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Terminaux.TermInfo is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.IO;
using System.Text;
using Terminaux.TermInfo.Extensions;

namespace Terminaux.TermInfo.Parsing
{
    internal static class TermInfoParser
    {
        private static class Ordinals
        {
            public const int Magic = 0;
            public const int NameSize = 1;
            public const int BoolCount = 2;
            public const int NumCount = 3;
            public const int StringCount = 4;
            public const int TableSize = 5;
            public const int ExtBoolCount = 0;
            public const int ExtNumCount = 1;
            public const int ExtStringCount = 2;
            public const int ExtOffsetCount = 3;
            public const int ExtTableSize = 4;
        }

        public static TermInfoDesc Parse(Stream stream)
        {
            var reader = new ByteReader(stream);
            return Parse(reader);
        }

        private static TermInfoDesc Parse(ByteReader reader)
        {
            // Read header
            var header = ReadIntegers(reader, 6, BitWidth.TwoBytes);

            // The number of bytes a num occupies is defined by the header magic
            var numWidth = GetBitWidth(header[Ordinals.Magic]);

            // Read names
            var names = ReadNames(reader, header[Ordinals.NameSize]);

            // Read boolean caps
            var booleans = ReadBools(reader, header[Ordinals.BoolCount]);

            // Read num caps
            var nums = ReadNums(reader, header[Ordinals.NumCount], numWidth);

            // Read string caps
            var strings = ReadStrings(reader, header[Ordinals.StringCount], header[Ordinals.TableSize]);

            // Got extended caps?
            var extendedCaps = default(ExtendedCapabilities);
            if (!reader.Eof())
            {
                extendedCaps = ParseExtendedCaps(reader, numWidth);
            }

            return new TermInfoDesc(names, booleans, nums, strings, extendedCaps);
        }

        private static ExtendedCapabilities ParseExtendedCaps(ByteReader reader, BitWidth numWidth)
        {
            var header = ReadIntegers(reader, 5, BitWidth.TwoBytes);

            // Read boolean caps
            var booleans = ReadBools(reader, header[Ordinals.ExtBoolCount]);

            // Read num caps
            var nums = ReadNums(reader, header[Ordinals.ExtNumCount], numWidth);

            var indices = new Span<int>(ReadIntegers(reader, header[Ordinals.ExtOffsetCount], BitWidth.TwoBytes));
            var data = new Span<char>(Encoding.ASCII.GetString(reader.ReadBytes(header[Ordinals.ExtTableSize])).ToCharArray());

            // Read string caps
            var (strings, last) = ReadStrings(indices, data, header[Ordinals.ExtStringCount]);
            indices = indices.Slice(header[Ordinals.ExtStringCount]);
            data = data.Slice(last);

            // Read bool names
            var (booleanNames, _) = ReadStrings(indices, data, header[Ordinals.ExtBoolCount]);
            indices = indices.Slice(header[Ordinals.ExtBoolCount]);

            // Read num names
            var (numNames, _) = ReadStrings(indices, data, header[Ordinals.ExtNumCount]);
            indices = indices.Slice(header[Ordinals.ExtNumCount]);

            // Read string names
            var (stringNames, _) = ReadStrings(indices, data, header[Ordinals.ExtStringCount]);

            return new ExtendedCapabilities(
                booleans, nums, strings,
                booleanNames, numNames, stringNames);
        }

        private static BitWidth GetBitWidth(int magic)
        {
            if (magic == 282)
            {
                return BitWidth.TwoBytes;
            }
            else if (magic == 542)
            {
                return BitWidth.FourBytes;
            }

            throw new InvalidOperationException("Invalid header magic");
        }

        private static string[] ReadNames(ByteReader reader, int size)
        {
            var data = Encoding.ASCII.GetString(reader.ReadBytes(size)).TrimEnd('\0');
            return data.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        }

        private static bool?[] ReadBools(ByteReader reader, int count)
        {
            var buffer = ReadIntegers(reader, count, BitWidth.OneByte);
            var result = new bool?[count];
            for (var i = 0; i < count; i++)
            {
                result[i] = buffer[i] == 1;
                if (buffer[i] == -2 || buffer[i] == -1)
                {
                    result[i] = null;
                }
            }

            return result;
        }

        private static int?[] ReadNums(ByteReader reader, int count, BitWidth width)
        {
            var buffer = ReadIntegers(reader, count, width);
            var result = new int?[count];
            for (var i = 0; i < count; i++)
            {
                result[i] = buffer[i];
                if (buffer[i] == -2 || buffer[i] == -1)
                {
                    result[i] = null;
                }
            }

            return result;
        }

        private static string?[] ReadStrings(ByteReader reader, int count, int size)
        {
            var offsets = ReadIntegers(reader, count, BitWidth.TwoBytes);
            var data = Encoding.ASCII.GetString(reader.ReadBytes(size));

            if (reader.Position % 2 != 0)
            {
                reader.ReadByte();
            }

            return ReadStrings(offsets, data, count);
        }

        private static string?[] ReadStrings(int[] indexes, string data, int count)
        {
            var strings = new string?[count];
            for (var i = 0; i < count; i++)
            {
                if (indexes[i] != -1 && indexes[i] != -2)
                {
                    strings[i] = data.ReadNullTerminatedString(indexes[i]);
                }
            }

            return strings;
        }

        private static (string[] Strings, int LastIndex) ReadStrings(Span<int> indexes, Span<char> data, int count)
        {
            var result = new string[count];

            var last = 0;
            for (var i = 0; i < count; i++)
            {
                var start = indexes[i];
                if (start < 0)
                {
                    continue;
                }

                var end = data.FindNullTerminator(start);
                if (end != -1)
                {
                    result[i] = data.Slice(start, end - start).ToString();
                    last = end + 1;
                }
                else
                {
                    throw new InvalidOperationException("Invalid string table!");
                }
            }

            return (result, last);
        }

        private static int[] ReadIntegers(ByteReader reader, int count, BitWidth width)
        {
            var bytes = (int)width / 8;
            var length = count * bytes;
            var buffer = reader.ReadBytes(length);

            if (reader.Position % 2 != 0)
            {
                reader.ReadByte();
            }

            var result = new int[count];
            for (int i = 0, j = 0; i < length; i += bytes, j++)
            {
                switch (bytes)
                {
                    case 1:
                        result[i] = buffer[i];
                        break;
                    case 2:
                        result[j] = (short)(buffer[i + 1] << 8 | buffer[i]);
                        break;
                    case 4:
                        result[j] = buffer[i + 3] << 24 | buffer[i + 2] << 16 | buffer[i + 1] << 8 | buffer[i];
                        break;
                }
            }

            return result;
        }
    }
}
