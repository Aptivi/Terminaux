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

using ImageMagick;
using System.IO;
using System.Text;
using Terminaux.Base;
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
        internal static Stream placeholderStream = typeof(ImageProcessor).Assembly.GetManifestResourceStream("Terminaux.Images.Resources.Placeholders.aptivi-logo-transparent-ios-opaque.png");

        /// <summary>
        /// Gets the list of colors by the number of pixels from the default image that Terminaux provides (that is, the Aptivi branding)
        /// </summary>
        /// <returns>A list of Terminaux's <see cref="Color"/> instance translated from ImageMagick's <see cref="IPixel{TQuantumType}"/> instance</returns>
        public static MagickImage OpenImage() =>
            OpenImage(placeholderStream);

        /// <summary>
        /// Gets the list of colors by the number of pixels from the image
        /// </summary>
        /// <param name="imagePath">Path to the image that ImageMagick can process</param>
        /// <returns>A list of Terminaux's <see cref="Color"/> instance translated from ImageMagick's <see cref="IPixel{TQuantumType}"/> instance</returns>
        public static MagickImage OpenImage(string imagePath)
        {
            // Check for null
            if (string.IsNullOrWhiteSpace(imagePath))
                throw new TerminauxException("Image path is not provided.");

            var imageStream = File.OpenRead(imagePath);
            return OpenImage(imageStream);
        }

        /// <summary>
        /// Gets the list of colors by the number of pixels from the image
        /// </summary>
        /// <param name="imageBytes">Array of bytes that contains the image data that ImageMagick can process</param>
        /// <returns>A list of Terminaux's <see cref="Color"/> instance translated from ImageMagick's <see cref="IPixel{TQuantumType}"/> instance</returns>
        public static MagickImage OpenImage(byte[] imageBytes)
        {
            // Check for null
            if (imageBytes is null || imageBytes.Length == 0)
                throw new TerminauxException("Image data is not provided.");

            var imageStream = new MemoryStream(imageBytes);
            return OpenImage(imageStream);
        }

        /// <summary>
        /// Gets the list of colors by the number of pixels from the image
        /// </summary>
        /// <param name="imageStream">Stream that contains the image data that ImageMagick can process</param>
        /// <returns>A list of Terminaux's <see cref="Color"/> instance translated from ImageMagick's <see cref="IPixel{TQuantumType}"/> instance</returns>
        public static MagickImage OpenImage(Stream imageStream)
        {
            // Check for null
            if (imageStream is null)
                throw new TerminauxException("Image stream is not provided.");

            // Open the image
            var settings = new MagickReadSettings
            {
                BackgroundColor = MagickColors.Transparent,
            };
            if (imageStream.CanSeek)
                imageStream.Seek(0, SeekOrigin.Begin);
            var image = new MagickImage(imageStream, settings);
            return image;
        }

        /// <summary>
        /// Gets the list of colors by the number of pixels from the default image that Terminaux provides (that is, the Aptivi branding)
        /// </summary>
        /// <returns>A list of Terminaux's <see cref="Color"/> instance translated from ImageMagick's <see cref="IPixel{TQuantumType}"/> instance</returns>
        public static Color[,] GetColorsFromImage() =>
            GetColorsFromImage(placeholderStream);

        /// <summary>
        /// Gets the list of colors by the number of pixels from the image
        /// </summary>
        /// <param name="imagePath">Path to the image that ImageMagick can process</param>
        /// <returns>A list of Terminaux's <see cref="Color"/> instance translated from ImageMagick's <see cref="IPixel{TQuantumType}"/> instance</returns>
        public static Color[,] GetColorsFromImage(string imagePath)
        {
            // Check for null
            if (string.IsNullOrWhiteSpace(imagePath))
                throw new TerminauxException("Image path is not provided.");

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
            // Check for null
            if (imageBytes is null || imageBytes.Length == 0)
                throw new TerminauxException("Image data is not provided.");

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
            // Check for null
            if (imageStream is null)
                throw new TerminauxException("Image stream is not provided.");

            // Open the image
            var image = OpenImage(imageStream);
            return GetColorsFromImage(image);
        }

        /// <summary>
        /// Gets the list of colors by the number of pixels from the image
        /// </summary>
        /// <param name="image">Image data that ImageMagick can process</param>
        /// <returns>A list of Terminaux's <see cref="Color"/> instance translated from ImageMagick's <see cref="IPixel{TQuantumType}"/> instance</returns>
        public static Color[,] GetColorsFromImage(MagickImage image)
        {
            // Check for null
            if (image is null)
                throw new TerminauxException("Image is not provided.");

            // Get the amount of pixels to get color information
            var pixelCollection = image.GetPixels();
            Color[,] colors = new Color[image.Width, image.Height];

            // Iterate through each pixel
            foreach (var pixel in pixelCollection)
            {
                int pixelX = pixel.X;
                int pixelY = pixel.Y;

                // Using ImageMagick's pixel information, return the new Color instance by adding it to the colors array.
                var pixelColor = pixel.ToColor();
                if (pixelColor is null)
                    continue;
                var color = new Color(pixelColor.R, pixelColor.G, pixelColor.B, new(ColorTools.GlobalSettings) { Opacity = pixelColor.A });
                colors[pixelX, pixelY] = color;
            }

            // Return the array!
            return colors;
        }

        /// <summary>
        /// Renders the placeholder image (that is, the Aptivi branding) to a string that you can print to the console
        /// </summary>
        /// <param name="width">Width of the resulting image</param>
        /// <param name="height">Height of the resulting image</param>
        /// <param name="left">Zero-based console left position to start writing the image to</param>
        /// <param name="top">Zero-based console top position to start writing the image to</param>
        /// <param name="background">Specifies the background color, or null for default</param>
        /// <returns>A string that contains the resulting pixels that you can print to the console using the <see cref="TextWriterRaw.WriteRaw(string, object[])"/> function</returns>
        public static string RenderImage(int width, int height, int left, int top, Color? background = null) =>
            RenderImage(placeholderStream, width, height, left, top, background);

        /// <summary>
        /// Renders the image to a string that you can print to the console
        /// </summary>
        /// <param name="imagePath">Path to the image that ImageMagick can process</param>
        /// <param name="width">Width of the resulting image</param>
        /// <param name="height">Height of the resulting image</param>
        /// <param name="left">Zero-based console left position to start writing the image to</param>
        /// <param name="top">Zero-based console top position to start writing the image to</param>
        /// <param name="background">Specifies the background color, or null for default</param>
        /// <returns>A string that contains the resulting pixels that you can print to the console using the <see cref="TextWriterRaw.WriteRaw(string, object[])"/> function</returns>
        public static string RenderImage(string imagePath, int width, int height, int left, int top, Color? background = null)
        {
            var imageColors = GetColorsFromImage(imagePath);
            return RenderImage(imageColors, width, height, left, top, background, true);
        }

        /// <summary>
        /// Renders the image to a string that you can print to the console
        /// </summary>
        /// <param name="imageBytes">Array of bytes that contains the image data that ImageMagick can process</param>
        /// <param name="width">Width of the resulting image</param>
        /// <param name="height">Height of the resulting image</param>
        /// <param name="left">Zero-based console left position to start writing the image to</param>
        /// <param name="top">Zero-based console top position to start writing the image to</param>
        /// <param name="background">Specifies the background color, or null for default</param>
        /// <returns>A string that contains the resulting pixels that you can print to the console using the <see cref="TextWriterRaw.WriteRaw(string, object[])"/> function</returns>
        public static string RenderImage(byte[] imageBytes, int width, int height, int left, int top, Color? background = null)
        {
            var imageColors = GetColorsFromImage(imageBytes);
            return RenderImage(imageColors, width, height, left, top, background, true);
        }

        /// <summary>
        /// Renders the image to a string that you can print to the console
        /// </summary>
        /// <param name="imageStream">Stream that contains the image data that ImageMagick can process</param>
        /// <param name="width">Width of the resulting image</param>
        /// <param name="height">Height of the resulting image</param>
        /// <param name="left">Zero-based console left position to start writing the image to</param>
        /// <param name="top">Zero-based console top position to start writing the image to</param>
        /// <param name="background">Specifies the background color, or null for default</param>
        /// <returns>A string that contains the resulting pixels that you can print to the console using the <see cref="TextWriterRaw.WriteRaw(string, object[])"/> function</returns>
        public static string RenderImage(Stream imageStream, int width, int height, int left, int top, Color? background = null)
        {
            var imageColors = GetColorsFromImage(imageStream);
            if (imageStream.CanSeek)
                imageStream.Seek(0, SeekOrigin.Begin);
            return RenderImage(imageColors, width, height, left, top, background, true);
        }

        /// <summary>
        /// Renders the image to a string that you can print to the console
        /// </summary>
        /// <param name="image">Image data that ImageMagick can process</param>
        /// <param name="width">Width of the resulting image</param>
        /// <param name="height">Height of the resulting image</param>
        /// <param name="left">Zero-based console left position to start writing the image to</param>
        /// <param name="top">Zero-based console top position to start writing the image to</param>
        /// <param name="background">Specifies the background color, or null for default</param>
        /// <returns>A string that contains the resulting pixels that you can print to the console using the <see cref="TextWriterRaw.WriteRaw(string, object[])"/> function</returns>
        public static string RenderImage(MagickImage image, int width, int height, int left, int top, Color? background = null)
        {
            var imageColors = GetColorsFromImage(image);
            return RenderImage(imageColors, width, height, left, top, background, true);
        }

        /// <summary>
        /// Renders the placeholder image (that is, the Aptivi branding) to a string that you can print to the console
        /// </summary>
        /// <param name="width">Width of the resulting image</param>
        /// <param name="height">Height of the resulting image</param>
        /// <param name="background">Specifies the background color, or null for default</param>
        /// <returns>A string that contains the resulting pixels that you can print to the console using the <see cref="TextWriterRaw.WriteRaw(string, object[])"/> function</returns>
        public static string RenderImage(int width, int height, Color? background = null) =>
            RenderImage(placeholderStream, width, height, background);

        /// <summary>
        /// Renders the image to a string that you can print to the console
        /// </summary>
        /// <param name="imagePath">Path to the image that ImageMagick can process</param>
        /// <param name="width">Width of the resulting image</param>
        /// <param name="height">Height of the resulting image</param>
        /// <param name="background">Specifies the background color, or null for default</param>
        /// <returns>A string that contains the resulting pixels that you can print to the console using the <see cref="TextWriterRaw.WriteRaw(string, object[])"/> function</returns>
        public static string RenderImage(string imagePath, int width, int height, Color? background = null)
        {
            var imageColors = GetColorsFromImage(imagePath);
            return RenderImage(imageColors, width, height, 0, 0, background, false);
        }

        /// <summary>
        /// Renders the image to a string that you can print to the console
        /// </summary>
        /// <param name="imageBytes">Array of bytes that contains the image data that ImageMagick can process</param>
        /// <param name="width">Width of the resulting image</param>
        /// <param name="height">Height of the resulting image</param>
        /// <param name="background">Specifies the background color, or null for default</param>
        /// <returns>A string that contains the resulting pixels that you can print to the console using the <see cref="TextWriterRaw.WriteRaw(string, object[])"/> function</returns>
        public static string RenderImage(byte[] imageBytes, int width, int height, Color? background = null)
        {
            var imageColors = GetColorsFromImage(imageBytes);
            return RenderImage(imageColors, width, height, 0, 0, background, false);
        }

        /// <summary>
        /// Renders the image to a string that you can print to the console
        /// </summary>
        /// <param name="imageStream">Stream that contains the image data that ImageMagick can process</param>
        /// <param name="width">Width of the resulting image</param>
        /// <param name="height">Height of the resulting image</param>
        /// <param name="background">Specifies the background color, or null for default</param>
        /// <returns>A string that contains the resulting pixels that you can print to the console using the <see cref="TextWriterRaw.WriteRaw(string, object[])"/> function</returns>
        public static string RenderImage(Stream imageStream, int width, int height, Color? background = null)
        {
            var imageColors = GetColorsFromImage(imageStream);
            if (imageStream.CanSeek)
                imageStream.Seek(0, SeekOrigin.Begin);
            return RenderImage(imageColors, width, height, 0, 0, background, false);
        }

        /// <summary>
        /// Renders the image to a string that you can print to the console
        /// </summary>
        /// <param name="image">Image data that ImageMagick can process</param>
        /// <param name="width">Width of the resulting image</param>
        /// <param name="height">Height of the resulting image</param>
        /// <param name="background">Specifies the background color, or null for default</param>
        /// <returns>A string that contains the resulting pixels that you can print to the console using the <see cref="TextWriterRaw.WriteRaw(string, object[])"/> function</returns>
        public static string RenderImage(MagickImage image, int width, int height, Color? background = null)
        {
            var imageColors = GetColorsFromImage(image);
            return RenderImage(imageColors, width, height, 0, 0, background, false);
        }

        internal static string RenderImage(Color[,] imageColors, int width, int height, int left, int top, Color? background, bool useLeftTop)
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
            string bgSeq = background is null ? ColorTools.RenderRevertBackground() : ColorTools.RenderSetConsoleColor(background, true);
            for (double y = 0; y < imageHeight; y += imageHeightThreshold, absoluteY++)
            {
                // Some positioning
                if (useLeftTop)
                    buffer.Append(CsiSequences.GenerateCsiCursorPosition(left + 1, top + absoluteY + 1));

                // Determine how to process the width
                for (double x = 0; x < imageWidth; x += imageWidthThreshold)
                {
                    // Add the appropriate color to the buffer
                    int pixelX = (int)x;
                    int pixelY = (int)y;
                    var imageColor = imageColors[pixelX, pixelY];
                    buffer.Append((imageColor.RGB == ColorTools.CurrentBackgroundColor.RGB && imageColor.RGB.A == 0 ? bgSeq : imageColor.VTSequenceBackgroundTrueColor) + " ");
                }

                // Add space if not using console positioning
                if (!useLeftTop)
                    buffer.AppendLine();
            }

            // Return the resulting buffer
            buffer.Append(ColorTools.RenderRevertBackground());
            return buffer.ToString();
        }
    }
}
