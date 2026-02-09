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
using ImageMagick;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Colorimetry;
using Colorimetry.Data;
using Terminaux.Images.Writers;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Images
{
    /// <summary>
    /// Class for processing images
    /// </summary>
    public static class ImageProcessor
    {
        internal static Stream placeholderStream = typeof(ImageProcessor).Assembly.GetManifestResourceStream(
            DateTime.Now.Day == 16 && DateTime.Now.Month == 12 ? "Terminaux.Images.Resources.Placeholders.OfficialAppIcon-Aptivestigate-64.png" :
            DateTime.Now.Day == 1 && DateTime.Now.Month == 10 ? "Terminaux.Images.Resources.Placeholders.OfficialAppIcon-BassBoom-64.png" :
            DateTime.Now.Day == 9 && DateTime.Now.Month == 10 ? "Terminaux.Images.Resources.Placeholders.OfficialAppIcon-BskyId-64.png" :
            DateTime.Now.Day == 12 && DateTime.Now.Month == 1 ? "Terminaux.Images.Resources.Placeholders.OfficialAppIcon-Calendrier-64.png" :
            DateTime.Now.Day == 19 && DateTime.Now.Month == 8 ? "Terminaux.Images.Resources.Placeholders.OfficialAppIcon-ChemiStar-64.png" :
            DateTime.Now.Day == 11 && DateTime.Now.Month == 1 ? "Terminaux.Images.Resources.Placeholders.OfficialAppIcon-Colorimetry-64.png" :
            DateTime.Now.Day == 16 && DateTime.Now.Month == 5 ? "Terminaux.Images.Resources.Placeholders.OfficialAppIcon-Magico-64.png" :
            DateTime.Now.Day == 28 && DateTime.Now.Month == 8 ? "Terminaux.Images.Resources.Placeholders.OfficialAppIcon-Metalinker-64.png" :
            DateTime.Now.Day == 19 && DateTime.Now.Month == 12 ? "Terminaux.Images.Resources.Placeholders.OfficialAppIcon-Nettify-64.png" :
            DateTime.Now.Day == 22 && DateTime.Now.Month == 2 ? "Terminaux.Images.Resources.Placeholders.OfficialAppIcon-Nitrocid-64.png" :
            DateTime.Now.Day == 10 && DateTime.Now.Month == 1 ? "Terminaux.Images.Resources.Placeholders.OfficialAppIcon-PowerTools-64.png" :
            DateTime.Now.Day == 27 && DateTime.Now.Month == 10 ? "Terminaux.Images.Resources.Placeholders.OfficialAppIcon-ResourceLab-64.png" :
            DateTime.Now.Day == 15 && DateTime.Now.Month == 10 ? "Terminaux.Images.Resources.Placeholders.OfficialAppIcon-SpecProbe-64.png" :
            DateTime.Now.Day == 6 && DateTime.Now.Month == 8 ? "Terminaux.Images.Resources.Placeholders.OfficialAppIcon-Terminaux-64.png" :
            DateTime.Now.Day == 2 && DateTime.Now.Month == 12 ? "Terminaux.Images.Resources.Placeholders.OfficialAppIcon-Textify-64.png" :
            DateTime.Now.Day == 20 && DateTime.Now.Month == 2 ? "Terminaux.Images.Resources.Placeholders.OfficialAppIcon-VisualCard-64.png" :
            "Terminaux.Images.Resources.Placeholders.Modern-Logo-64.png"
        );

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
            var themeBackground = ThemeColorsTools.GetColor(ThemeColorType.Background);
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
                        OpacityColor = background is null ? (ConsoleColoring.AllowBackground ? themeBackground : ConsoleColors.Black) : background,
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
            // Make a new image viewer instance
            var viewer = new ImageView(imageColors)
            {
                Width = width,
                Height = height,
                Left = left,
                Top = top,
                UsePositioning = useLeftTop,
            };
            if (background is not null)
                viewer.BackgroundColor = background;

            // Render the image viewer
            return viewer.Render();
        }
    }
}
