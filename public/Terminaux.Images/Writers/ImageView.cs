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

using ImageMagick;
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Colorimetry;
using Colorimetry.Data;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Themes.Colors;
using Terminaux.Writer.CyclicWriters;

namespace Terminaux.Images.Writers
{
    /// <summary>
    /// Image viewer renderable
    /// </summary>
    public class ImageView : GraphicalCyclicWriter
    {
        private MagickImage image = ImageProcessor.OpenImage();
        internal Color[,] imageColors;
        private int width = 0;
        private int height = 0;
        private int columnOffset = 0;
        private int rowOffset = 0;
        private Color? backgroundColor;

        /// <summary>
        /// An image to render
        /// </summary>
        public MagickImage Image
        {
            get => image;
            set
            {
                image = value;
                imageColors = ImageProcessor.GetColorsFromImage(image);
            }
        }

        /// <summary>
        /// Background color of the image, overriding any transparency
        /// </summary>
        public Color? BackgroundColor
        {
            get => backgroundColor;
            set => backgroundColor = value;
        }

        /// <summary>
        /// Left margin of the image
        /// </summary>
        public override int Width
        {
            get => width;
            set => width = value;
        }

        /// <summary>
        /// Right margin of the image
        /// </summary>
        public override int Height
        {
            get => height;
            set => height = value;
        }

        /// <summary>
        /// Specifies a zero-based row offset
        /// </summary>
        public int RowOffset
        {
            get => rowOffset;
            set => rowOffset = value;
        }

        /// <summary>
        /// Specifies a zero-based column offset
        /// </summary>
        public int ColumnOffset
        {
            get => columnOffset;
            set => columnOffset = value;
        }

        /// <summary>
        /// Whether to stretch the image or to render as is?
        /// </summary>
        public bool Fit { get; set; } = true;

        /// <summary>
        /// Whether to use positioning or not?
        /// </summary>
        public bool UsePositioning { get; set; } = true;

        /// <summary>
        /// Renders an image
        /// </summary>
        /// <returns>Rendered image that will be used by the renderer</returns>
        public override string Render()
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
            var themeBackground = ThemeColorsTools.GetColor(ThemeColorType.Background);
            string bgSeq = BackgroundColor is null ? ConsoleColoring.RenderRevertBackground() : ConsoleColoring.RenderSetConsoleColor(BackgroundColor, true);
            string bgSeqFg = BackgroundColor is null ? (!ConsoleColoring.AllowBackground ? ConsoleColoring.RenderSetConsoleColor(ConsoleColors.Black) : ConsoleColoring.RenderSetConsoleColor(themeBackground)) : ConsoleColoring.RenderSetConsoleColor(BackgroundColor);
            var imageBackground = BackgroundColor is null ? Color.Empty : BackgroundColor;
            for (double y = RowOffset; y < imageHeight && absoluteY < Height; y += Fit ? imageHeightThreshold : 2, absoluteY++)
            {
                // Some positioning
                if (UsePositioning)
                    buffer.Append(CsiSequences.GenerateCsiCursorPosition(Left + 1, Top + absoluteY + 1));

                // Determine how to process the width
                int absoluteX = 0;
                for (double x = ColumnOffset; x < imageWidth && absoluteX < Width; x += Fit ? imageWidthThreshold : 1, absoluteX++)
                {
                    // Add the appropriate color to the buffer
                    int pixelX = (int)x;
                    int pixelY = (int)y;
                    var imageColor = imageColors[pixelX, pixelY];
                    var imageColorNext = (pixelY + 1 < imageColors.GetLength(1) ? imageColors[pixelX, pixelY + 1] : BackgroundColor) ?? themeBackground;
                    bool isSame = imageColor == imageColorNext;
                    bool isSameTransparency = imageColor.RGB.A == 0 && imageColorNext.RGB.A == 0;
                    string highSequence = (imageColor.RGB == themeBackground.RGB || imageColor.RGB == imageBackground.RGB) && imageColor.RGB.A == 0 ? bgSeqFg : imageColor.VTSequenceForegroundTrueColor();
                    string lowSequence = (imageColorNext.RGB == themeBackground.RGB || imageColorNext.RGB == imageBackground.RGB) && imageColorNext.RGB.A == 0 ? bgSeq : imageColorNext.VTSequenceBackgroundTrueColor();
                    buffer.Append(
                        highSequence +
                        lowSequence +
                        (isSameTransparency ? " " : isSame ? "█" : "▀"));
                }

                // Add space if not using console positioning
                if (!UsePositioning)
                    buffer.AppendLine();
            }

            // Return the resulting buffer
            buffer.Append(ConsoleColoring.RenderRevertBackground());
            ConsoleLogger.Debug("Need to write {0} bytes to the console", buffer.Length);
            return buffer.ToString();
        }

        /// <summary>
        /// Makes a new instance of the image renderer
        /// </summary>
        public ImageView()
        {
            imageColors = ImageProcessor.GetColorsFromImage(image);
        }

        /// <summary>
        /// Makes a new instance of the image renderer
        /// </summary>
        /// <param name="image">Image to use</param>
        public ImageView(MagickImage image)
        {
            this.image = image;
            imageColors = ImageProcessor.GetColorsFromImage(image);
        }

        internal ImageView(Color[,] imagePixels)
        {
            imageColors = imagePixels;
        }
    }
}
