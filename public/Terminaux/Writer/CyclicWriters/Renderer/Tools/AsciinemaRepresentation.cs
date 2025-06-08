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

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;
using Terminaux.Base;

namespace Terminaux.Writer.CyclicWriters.Renderer.Tools
{
    /// <summary>
    /// Representation class for the Asciinema specification
    /// </summary>
    public class AsciinemaRepresentation
    {
        private Asciicast? asciicast;

        /// <summary>
        /// Asciicast recording instance
        /// </summary>
        public Asciicast Asciicast =>
            asciicast ?? throw new TerminauxException($"There is no Asciicast instance");

        /// <summary>
        /// Asciicast version
        /// </summary>
        [JsonIgnore]
        public int Version =>
            Asciicast.Version;

        /// <summary>
        /// Initial console width
        /// </summary>
        [JsonIgnore]
        public int Width =>
            Asciicast.Width;

        /// <summary>
        /// Initial console height
        /// </summary>
        [JsonIgnore]
        public int Height =>
            Asciicast.Height;

        /// <summary>
        /// Gets the Asciinema representation from the file
        /// </summary>
        /// <param name="fileName">File name to parse</param>
        /// <returns>A representation of the Asciinema recording</returns>
        /// <exception cref="TerminauxException"></exception>
        public static AsciinemaRepresentation GetRepresentationFromFile(string fileName)
        {
            // Check to see if we have this file
            if (!File.Exists(fileName))
                throw new TerminauxException("There is no Asciicast file" + $": {fileName}");

            // Open the file and parse its contents
            string contents = File.ReadAllText(fileName);
            return GetRepresentationFromContents(contents);
        }

        /// <summary>
        /// Gets the Asciinema representation from the contents
        /// </summary>
        /// <param name="asciiCastContents">Contents</param>
        /// <returns>A representation of the Asciinema recording</returns>
        /// <exception cref="TerminauxException"></exception>
        public static AsciinemaRepresentation GetRepresentationFromContents(string asciiCastContents)
        {
            using var contentStream = new MemoryStream(Encoding.Default.GetBytes(asciiCastContents));
            return GetRepresentationFromStream(contentStream);
        }

        /// <summary>
        /// Gets the Asciinema representation from the stream
        /// </summary>
        /// <param name="asciiCastStream">Stream containing Asciinema represetation</param>
        /// <returns>A representation of the Asciinema recording</returns>
        /// <exception cref="TerminauxException"></exception>
        public static AsciinemaRepresentation GetRepresentationFromStream(Stream asciiCastStream)
        {
            // Check to see if this is an Asciinema v2 file
            using var contentStreamReader = new StreamReader(asciiCastStream);
            using var contentReaderVerify = new JsonTextReader(contentStreamReader);
            using var contentReader = new JsonTextReader(contentStreamReader);
            contentReader.SupportMultipleContent = true;

            // Loop until end of file
            Asciicast? asciicast = null;
            var serializer = JsonSerializer.Create();
            bool addingData = false;
            while (contentReader.Read())
            {
                if (!addingData)
                {
                    // Try to serialize at least the version
                    var asciiCastToken = JToken.ReadFrom(contentReader);
                    var asciicastGeneric = asciiCastToken.ToObject<Asciicast>() ??
                        throw new TerminauxException("Can't deserialize base Asciicast representation");
                    int version = asciicastGeneric.Version;
                    ConsoleLogger.Debug("Got asciicast version {0}", version);

                    // Verify the version
                    if (version != 1 && version != 2)
                        throw new TerminauxException($"Version {version} is invalid");

                    // Now, determine whether to use the v1 or the v2 class
                    if (version == 1)
                    {
                        // We are on version 1. Use its class and deserialize it
                        var deserializedAsciicast = asciiCastToken.ToObject<AsciicastV1>() ??
                            throw new TerminauxException("Can't deserialize Asciicast v1 representation");
                        asciicast = deserializedAsciicast;
                        ConsoleLogger.Debug("Finalized asciicast v1 implementation");
                    }
                    else
                    {
                        // We are on version 2. Use its class and deserialize it
                        var deserializedAsciicast = asciiCastToken.ToObject<AsciicastV2>() ??
                            throw new TerminauxException("Can't deserialize Asciicast v2 representation");
                        asciicast = deserializedAsciicast;
                        addingData = true;
                        ConsoleLogger.Debug("Finalized asciicast v2 implementation; adding data...");
                    }
                }
                else if (asciicast is AsciicastV2 asciicastV2)
                {
                    // Deserialize into array
                    var readArray = serializer.Deserialize<object[]>(contentReader) ??
                        throw new TerminauxException("Can't obtain event information");

                    // Get items from this array and install their values
                    double delay = (double)readArray[0];
                    string eventType = (string)readArray[1];
                    string data = (string)readArray[2];
                    ConsoleLogger.Debug("Adding {0}, {1}... data is {2} bytes.", delay, eventType, data.Length);
                    asciicastV2.stdOutData.Add((delay, eventType, data));
                }
            }

            // Return the new representation
            return new()
            {
                asciicast = asciicast
            };
        }

        private AsciinemaRepresentation()
        { }
    }
}
