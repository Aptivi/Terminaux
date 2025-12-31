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

using System.IO;
using System.Text;
using ImageMagick;
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Colors.Data;
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
                throw new TerminauxException(LanguageTools.GetLocalized("TI_IMAGEPROCESSOR_OPENIMAGE_EXCEPTION_NULLPATH"));

            ConsoleLogger.Info("Opening image file {0}...", imagePath);
            var imageStream = File.OpenRead(imagePath);
            ConsoleLogger.Debug("Image file length is {0} bytes", imageStream.Length);
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
                throw new TerminauxException(LanguageTools.GetLocalized("TI_IMAGEPROCESSOR_OPENIMAGE_EXCEPTION_NULLDATA"));

            var imageStream = new MemoryStream(imageBytes);
            ConsoleLogger.Debug("Image stream length is {0} bytes", imageStream.Length);
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
                throw new TerminauxException(LanguageTools.GetLocalized("TI_IMAGEPROCESSOR_OPENIMAGE_EXCEPTION_NULLSTREAM"));

            // Open the image
            var settings = new MagickReadSettings
            {
                BackgroundColor = MagickColors.Transparent,
            };
            ConsoleLogger.Debug("Created Magick read settings, can seek: {0}", imageStream.CanSeek);
            if (imageStream.CanSeek)
                imageStream.Seek(0, SeekOrigin.Begin);
            var image = new MagickImage(imageStream, settings);
            ConsoleLogger.Debug("Returning valid Magick image of format {0}...", image.Format);
            return image;
        }

        /// <summary>
        /// Gets the list of colors by the number of pixels from the default image that Terminaux provides (that is, the Aptivi branding)
        /// </summary>
        /// <returns>A list of Terminaux's <see cref="Color"/> instance translated from ImageMagick's <see cref="IPixel{TQuantumType}"/> instance</returns>
        public static Color[,] GetColorsFromImage(int width = 0, int height = 0, Color? background = null) =>
            GetColorsFromImage(placeholderStream, width, height, background);

        /// <summary>
        /// Gets the list of colors by the number of pixels from the image
        /// </summary>
        /// <param name="imagePath">Path to the image that ImageMagick can process</param>
        /// <param name="width">Target width. Set to 0 to prevent resize.</param>
        /// <param name="height">Target height. Set to 0 to prevent resize.</param>
        /// <param name="background">Specifies the background color, or null for default</param>
        /// <returns>A list of Terminaux's <see cref="Color"/> instance translated from ImageMagick's <see cref="IPixel{TQuantumType}"/> instance</returns>
        public static Color[,] GetColorsFromImage(string imagePath, int width = 0, int height = 0, Color? background = null)
        {
            // Check for null
            if (string.IsNullOrWhiteSpace(imagePath))
                throw new TerminauxException(LanguageTools.GetLocalized("TI_IMAGEPROCESSOR_OPENIMAGE_EXCEPTION_NULLPATH"));

            ConsoleLogger.Info("Opening image file {0}...", imagePath);
            var imageStream = File.OpenRead(imagePath);
            ConsoleLogger.Debug("Image file length is {0} bytes", imageStream.Length);
            return GetColorsFromImage(imageStream, width, height, background);
        }

        /// <summary>
        /// Gets the list of colors by the number of pixels from the image
        /// </summary>
        /// <param name="imageBytes">Array of bytes that contains the image data that ImageMagick can process</param>
        /// <param name="width">Target width. Set to 0 to prevent resize.</param>
        /// <param name="height">Target height. Set to 0 to prevent resize.</param>
        /// <param name="background">Specifies the background color, or null for default</param>
        /// <returns>A list of Terminaux's <see cref="Color"/> instance translated from ImageMagick's <see cref="IPixel{TQuantumType}"/> instance</returns>
        public static Color[,] GetColorsFromImage(byte[] imageBytes, int width = 0, int height = 0, Color? background = null)
        {
            // Check for null
            if (imageBytes is null || imageBytes.Length == 0)
                throw new TerminauxException(LanguageTools.GetLocalized("TI_IMAGEPROCESSOR_OPENIMAGE_EXCEPTION_NULLDATA"));

            var imageStream = new MemoryStream(imageBytes);
            ConsoleLogger.Debug("Image stream length is {0} bytes", imageStream.Length);
            return GetColorsFromImage(imageStream, width, height, background);
        }

        /// <summary>
        /// Gets the list of colors by the number of pixels from the image
        /// </summary>
        /// <param name="imageStream">Stream that contains the image data that ImageMagick can process</param>
        /// <param name="width">Target width. Set to 0 to prevent resize.</param>
        /// <param name="height">Target height. Set to 0 to prevent resize.</param>
        /// <param name="background">Specifies the background color, or null for default</param>
        /// <returns>A list of Terminaux's <see cref="Color"/> instance translated from ImageMagick's <see cref="IPixel{TQuantumType}"/> instance</returns>
        public static Color[,] GetColorsFromImage(Stream imageStream, int width = 0, int height = 0, Color? background = null)
        {
            // Check for null
            if (imageStream is null)
                throw new TerminauxException(LanguageTools.GetLocalized("TI_IMAGEPROCESSOR_OPENIMAGE_EXCEPTION_NULLSTREAM"));

            // Open the image
            var image = OpenImage(imageStream);
            ConsoleLogger.Debug("Returning valid Magick image of format {0}...", image.Format);
            return GetColorsFromImage(image, width, height, background);
        }

        /// <summary>
        /// Gets the list of colors by the number of pixels from the image
        /// </summary>
        /// <param name="image">Image data that ImageMagick can process</param>
        /// <param name="width">Target width. Set to 0 to prevent resize.</param>
        /// <param name="height">Target height. Set to 0 to prevent resize.</param>
        /// <param name="background">Specifies the background color, or null for default</param>
        /// <returns>A list of Terminaux's <see cref="Color"/> instance translated from ImageMagick's <see cref="IPixel{TQuantumType}"/> instance</returns>
        public static Color[,] GetColorsFromImage(MagickImage image, int width = 0, int height = 0, Color? background = null)
        {
            // Check for null
            if (image is null)
                throw new TerminauxException(LanguageTools.GetLocalized("TI_IMAGEPROCESSOR_GETCOLORSFROMIMAGE_EXCEPTION_NULLIMAGE"));

            // Check if resizing is needed
            if (width > 0 && height > 0)
                image.Resize((uint)width, (uint)height);

            // Get the amount of pixels to get color information
            var pixelCollection = image.GetPixels();
            ConsoleLogger.Debug("Width: {0}, height: {1}", image.Width, image.Height);
            Color[,] colors = new Color[image.Width, image.Height];

            // Iterate through each pixel
            var currentBackground = ColorTools.CurrentBackgroundColor;
            foreach (var pixel in pixelCollection)
            {
                int pixelX = pixel.X;
                int pixelY = pixel.Y;

                // Using ImageMagick's pixel information, return the new Color instance by adding it to the colors array.
                var pixelColor = pixel.ToColor();
                if (pixelColor is null)
                    continue;
                ConsoleLogger.Debug("[{0}, {1}] RGBA: {2}, {3}, {4}, {5}", pixelX, pixelY, pixelColor.R, pixelColor.G, pixelColor.B, pixelColor.A);
                var color = new Color(
                    pixelColor.A == 0 ? 0 : pixelColor.R,
                    pixelColor.A == 0 ? 0 : pixelColor.G,
                    pixelColor.A == 0 ? 0 : pixelColor.B,
                    new(ColorTools.GlobalSettings)
                    {
                        Opacity = pixelColor.A,
                        OpacityColor = background is null ? (ColorTools.AllowBackground ? currentBackground : ConsoleColors.Black) : background,
                    });
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
        /// <param name="resize">Whether to perform a resize or not</param>
        /// <returns>A string that contains the resulting pixels that you can print to the console using the <see cref="TextWriterRaw.WriteRaw(string, object[])"/> function</returns>
        public static string RenderImage(int width, int height, int left, int top, Color? background = null, bool resize = true) =>
            RenderImage(placeholderStream, width, height, left, top, background, resize);

        /// <summary>
        /// Renders the image to a string that you can print to the console
        /// </summary>
        /// <param name="imagePath">Path to the image that ImageMagick can process</param>
        /// <param name="width">Width of the resulting image</param>
        /// <param name="height">Height of the resulting image</param>
        /// <param name="left">Zero-based console left position to start writing the image to</param>
        /// <param name="top">Zero-based console top position to start writing the image to</param>
        /// <param name="background">Specifies the background color, or null for default</param>
        /// <param name="resize">Whether to perform a resize or not</param>
        /// <returns>A string that contains the resulting pixels that you can print to the console using the <see cref="TextWriterRaw.WriteRaw(string, object[])"/> function</returns>
        public static string RenderImage(string imagePath, int width, int height, int left, int top, Color? background = null, bool resize = true)
        {
            var imageColors = resize ? GetColorsFromImage(imagePath, width, height * 2) : GetColorsFromImage(imagePath);
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
        /// <param name="resize">Whether to perform a resize or not</param>
        /// <returns>A string that contains the resulting pixels that you can print to the console using the <see cref="TextWriterRaw.WriteRaw(string, object[])"/> function</returns>
        public static string RenderImage(byte[] imageBytes, int width, int height, int left, int top, Color? background = null, bool resize = true)
        {
            var imageColors = resize ? GetColorsFromImage(imageBytes, width, height * 2) : GetColorsFromImage(imageBytes);
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
        /// <param name="resize">Whether to perform a resize or not</param>
        /// <returns>A string that contains the resulting pixels that you can print to the console using the <see cref="TextWriterRaw.WriteRaw(string, object[])"/> function</returns>
        public static string RenderImage(Stream imageStream, int width, int height, int left, int top, Color? background = null, bool resize = true)
        {
            var imageColors = resize ? GetColorsFromImage(imageStream, width, height * 2) : GetColorsFromImage(imageStream);
            ConsoleLogger.Debug("Can seek: {0}", imageStream.CanSeek);
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
        /// <param name="resize">Whether to perform a resize or not</param>
        /// <returns>A string that contains the resulting pixels that you can print to the console using the <see cref="TextWriterRaw.WriteRaw(string, object[])"/> function</returns>
        public static string RenderImage(MagickImage image, int width, int height, int left, int top, Color? background = null, bool resize = true)
        {
            var imageColors = resize ? GetColorsFromImage(image, width, height * 2) : GetColorsFromImage(image);
            return RenderImage(imageColors, width, height, left, top, background, true);
        }

        /// <summary>
        /// Renders the placeholder image (that is, the Aptivi branding) to a string that you can print to the console
        /// </summary>
        /// <param name="width">Width of the resulting image</param>
        /// <param name="height">Height of the resulting image</param>
        /// <param name="background">Specifies the background color, or null for default</param>
        /// <param name="resize">Whether to perform a resize or not</param>
        /// <returns>A string that contains the resulting pixels that you can print to the console using the <see cref="TextWriterRaw.WriteRaw(string, object[])"/> function</returns>
        public static string RenderImage(int width, int height, Color? background = null, bool resize = true) =>
            RenderImage(placeholderStream, width, height, background, resize);

        /// <summary>
        /// Renders the image to a string that you can print to the console
        /// </summary>
        /// <param name="imagePath">Path to the image that ImageMagick can process</param>
        /// <param name="width">Width of the resulting image</param>
        /// <param name="height">Height of the resulting image</param>
        /// <param name="background">Specifies the background color, or null for default</param>
        /// <param name="resize">Whether to perform a resize or not</param>
        /// <returns>A string that contains the resulting pixels that you can print to the console using the <see cref="TextWriterRaw.WriteRaw(string, object[])"/> function</returns>
        public static string RenderImage(string imagePath, int width, int height, Color? background = null, bool resize = true)
        {
            var imageColors = resize ? GetColorsFromImage(imagePath, width, height * 2) : GetColorsFromImage(imagePath);
            return RenderImage(imageColors, width, height, 0, 0, background, false);
        }

        /// <summary>
        /// Renders the image to a string that you can print to the console
        /// </summary>
        /// <param name="imageBytes">Array of bytes that contains the image data that ImageMagick can process</param>
        /// <param name="width">Width of the resulting image</param>
        /// <param name="height">Height of the resulting image</param>
        /// <param name="background">Specifies the background color, or null for default</param>
        /// <param name="resize">Whether to perform a resize or not</param>
        /// <returns>A string that contains the resulting pixels that you can print to the console using the <see cref="TextWriterRaw.WriteRaw(string, object[])"/> function</returns>
        public static string RenderImage(byte[] imageBytes, int width, int height, Color? background = null, bool resize = true)
        {
            var imageColors = resize ? GetColorsFromImage(imageBytes, width, height * 2) : GetColorsFromImage(imageBytes);
            return RenderImage(imageColors, width, height, 0, 0, background, false);
        }

        /// <summary>
        /// Renders the image to a string that you can print to the console
        /// </summary>
        /// <param name="imageStream">Stream that contains the image data that ImageMagick can process</param>
        /// <param name="width">Width of the resulting image</param>
        /// <param name="height">Height of the resulting image</param>
        /// <param name="background">Specifies the background color, or null for default</param>
        /// <param name="resize">Whether to perform a resize or not</param>
        /// <returns>A string that contains the resulting pixels that you can print to the console using the <see cref="TextWriterRaw.WriteRaw(string, object[])"/> function</returns>
        public static string RenderImage(Stream imageStream, int width, int height, Color? background = null, bool resize = true)
        {
            var imageColors = resize ? GetColorsFromImage(imageStream, width, height * 2) : GetColorsFromImage(imageStream);
            ConsoleLogger.Debug("Can seek: {0}", imageStream.CanSeek);
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
        /// <param name="resize">Whether to perform a resize or not</param>
        /// <returns>A string that contains the resulting pixels that you can print to the console using the <see cref="TextWriterRaw.WriteRaw(string, object[])"/> function</returns>
        public static string RenderImage(MagickImage image, int width, int height, Color? background = null, bool resize = true)
        {
            var imageColors = resize ? GetColorsFromImage(image, width, height * 2) : GetColorsFromImage(image);
            return RenderImage(imageColors, width, height, 0, 0, background, false);
        }

        internal static string RenderImage(Color[,] imageColors, int width, int height, int left, int top, Color? background, bool useLeftTop)
        {
            // Figure out how to render the image, but we need to get info about the width and the height.
            int imageWidth = imageColors.GetLength(0);
            int imageHeight = imageColors.GetLength(1);
            double imageWidthThreshold = (double)imageWidth / width;
            double imageHeightThreshold = (double)imageHeight / height;
            ConsoleLogger.Debug("Width: {0} [T: {1}], height: {2} [T: {3}]", imageWidth, imageWidthThreshold, imageHeight, imageHeightThreshold);

            // Build the buffer
            StringBuilder buffer = new();
            int absoluteY = 0;

            // Process the pixels in scanlines
            var themeBackground = ColorTools.CurrentBackgroundColor;
            string bgSeq = background is null ? ColorTools.RenderRevertBackground() : ColorTools.RenderSetConsoleColor(background, true);
            string bgSeqFg = background is null ? (!ColorTools.AllowBackground ? ColorTools.RenderSetConsoleColor(ConsoleColors.Black) : ColorTools.RenderSetConsoleColor(themeBackground)) : ColorTools.RenderSetConsoleColor(background);
            var imageBackground = background is null ? Color.Empty : background;
            for (double y = 0; y < imageHeight && absoluteY < height; y += 2, absoluteY++)
            {
                // Some positioning
                if (useLeftTop)
                    buffer.Append(CsiSequences.GenerateCsiCursorPosition(left + 1, top + absoluteY + 1));

                // Determine how to process the width
                int absoluteX = 0;
                for (double x = 0; x < imageWidth && absoluteX < width; x += 1, absoluteX++)
                {
                    // Add the appropriate color to the buffer
                    int pixelX = (int)x;
                    int pixelY = (int)y;
                    var imageColor = imageColors[pixelX, pixelY];
                    var imageColorNext = (pixelY + 1 < imageColors.GetLength(1) ? imageColors[pixelX, pixelY + 1] : background) ?? themeBackground;
                    bool isSame = imageColor == imageColorNext;
                    bool isSameTransparency = imageColor.RGB.A == 0 && imageColorNext.RGB.A == 0;
                    string highSequence = (imageColor.RGB == themeBackground.RGB || imageColor.RGB == imageBackground.RGB) && imageColor.RGB.A == 0 ? bgSeqFg : imageColor.VTSequenceForegroundTrueColor;
                    string lowSequence = (imageColorNext.RGB == themeBackground.RGB || imageColorNext.RGB == imageBackground.RGB) && imageColorNext.RGB.A == 0 ? bgSeq : imageColorNext.VTSequenceBackgroundTrueColor;
                    buffer.Append(
                        highSequence +
                        lowSequence +
                        (isSameTransparency ? " " : isSame ? "█" : "▀"));
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
