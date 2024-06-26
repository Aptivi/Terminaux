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

using ImageMagick;
using System.IO;
using System.Text;
using Terminaux.Colors;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Images
{
    /// <summary>
    /// Class for processing images
    /// </summary>
    public static class ImageProcessor
    {
        /// <summary>
        /// Gets the list of colors by the number of pixels from the image
        /// </summary>
        /// <param name="imagePath">Path to the image that ImageMagick can process</param>
        /// <returns>A list of Terminaux's <see cref="Color"/> instance translated from ImageMagick's <see cref="IPixel{TQuantumType}"/> instance</returns>
        public static Color[,] GetColorsFromImage(string imagePath)
        {
            var imageStream = File.OpenRead(imagePath);
            return GetColorsFromImage(imageStream);
        }

        /// <summary>
        /// Gets the list of colors by the number of pixels from the image
        /// </summary>
        /// <param name="imageBytes">Array of bytes that contains the image data that ImageMagick can process</param>
        /// <returns>A list of Terminaux's <see cref="Color"/> instance translated from ImageMagick's <see cref="IPixel{TQuantumType}"/> instance</returns>
        public static Color[,] GetColorsFromImage(byte[] imageBytes)
        {
            var imageStream = new MemoryStream(imageBytes);
            return GetColorsFromImage(imageStream);
        }

        /// <summary>
        /// Gets the list of colors by the number of pixels from the image
        /// </summary>
        /// <param name="imageStream">Stream that contains the image data that ImageMagick can process</param>
        /// <returns>A list of Terminaux's <see cref="Color"/> instance translated from ImageMagick's <see cref="IPixel{TQuantumType}"/> instance</returns>
        public static Color[,] GetColorsFromImage(Stream imageStream)
        {
            // Open the image and get the amount of pixels to get color information
            var settings = new MagickReadSettings
            {
                BackgroundColor = MagickColors.Transparent,
            };
            var image = new MagickImage(imageStream, settings);
            var pixelCollection = image.GetPixels();
            Color[,] colors = new Color[image.Width, image.Height];

            // Iterate through each pixel
            foreach (var pixel in pixelCollection)
            {
                int pixelX = pixel.X;
                int pixelY = pixel.Y;

                // Using ImageMagick's pixel information, return the new Color instance by adding it to the colors array.
                var pixelColor = pixel.ToColor();
                var color = new Color(pixelColor.R, pixelColor.G, pixelColor.B, new(ColorTools.GlobalSettings) { Opacity = pixelColor.A });
                colors[pixelX, pixelY] = color;
            }

            // Return the array!
            return colors;
        }

        /// <summary>
        /// Renders the image to a string that you can print to the console
        /// </summary>
        /// <param name="imagePath">Path to the image that ImageMagick can process</param>
        /// <param name="width">Width of the resulting image</param>
        /// <param name="height">Height of the resulting image</param>
        /// <param name="left">Zero-based console left position to start writing the image to</param>
        /// <param name="top">Zero-based console top position to start writing the image to</param>
        /// <returns>A string that contains the resulting pixels that you can print to the console using the <see cref="TextWriterRaw.WriteRaw(string, object[])"/> function</returns>
        public static string RenderImage(string imagePath, int width, int height, int left, int top)
        {
            var imageColors = GetColorsFromImage(imagePath);
            return RenderImage(imageColors, width, height, left, top);
        }

        /// <summary>
        /// Renders the image to a string that you can print to the console
        /// </summary>
        /// <param name="imageBytes">Array of bytes that contains the image data that ImageMagick can process</param>
        /// <param name="width">Width of the resulting image</param>
        /// <param name="height">Height of the resulting image</param>
        /// <param name="left">Zero-based console left position to start writing the image to</param>
        /// <param name="top">Zero-based console top position to start writing the image to</param>
        /// <returns>A string that contains the resulting pixels that you can print to the console using the <see cref="TextWriterRaw.WriteRaw(string, object[])"/> function</returns>
        public static string RenderImage(byte[] imageBytes, int width, int height, int left, int top)
        {
            var imageColors = GetColorsFromImage(imageBytes);
            return RenderImage(imageColors, width, height, left, top);
        }

        /// <summary>
        /// Renders the image to a string that you can print to the console
        /// </summary>
        /// <param name="imageStream">Stream that contains the image data that ImageMagick can process</param>
        /// <param name="width">Width of the resulting image</param>
        /// <param name="height">Height of the resulting image</param>
        /// <param name="left">Zero-based console left position to start writing the image to</param>
        /// <param name="top">Zero-based console top position to start writing the image to</param>
        /// <returns>A string that contains the resulting pixels that you can print to the console using the <see cref="TextWriterRaw.WriteRaw(string, object[])"/> function</returns>
        public static string RenderImage(Stream imageStream, int width, int height, int left, int top)
        {
            var imageColors = GetColorsFromImage(imageStream);
            return RenderImage(imageColors, width, height, left, top);
        }

        internal static string RenderImage(Color[,] imageColors, int width, int height, int left, int top)
        {
            // Get the image width and height in pixels and get their comparison factor
            int imageWidth = imageColors.GetLength(0);
            int imageHeight = imageColors.GetLength(1);
            double imageWidthThreshold = (double)imageWidth / width;
            double imageHeightThreshold = (double)imageHeight / height;

            // Build the buffer
            StringBuilder buffer = new();
            int absoluteY = 0;

            // Process the pixels in scanlines
            for (double y = 0; y < imageHeight; y += imageHeightThreshold, absoluteY++)
            {
                // Some positioning
                buffer.Append(CsiSequences.GenerateCsiCursorPosition(left + 1, top + absoluteY + 1));

                // Determine how to process the width
                for (double x = 0; x < imageWidth; x += imageWidthThreshold)
                {
                    // Add the appropriate color to the buffer
                    int pixelX = (int)x;
                    int pixelY = (int)y;
                    var imageColor = imageColors[pixelX, pixelY];
                    buffer.Append((imageColor.RGB == ColorTools.CurrentBackgroundColor.RGB ? ColorTools.RenderRevertBackground() : imageColor.VTSequenceBackgroundTrueColor) + " ");
                }
            }

            // Return the resulting buffer
            return buffer.ToString();
        }
    }
}
