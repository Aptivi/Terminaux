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
using Terminaux.Base.Extensions;
using Terminaux.Base.Structures;
using Colorimetry.Transformation;
using Terminaux.Inputs.Pointer;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Writer.CyclicWriters.Simple;

namespace Terminaux.Inputs.Modules
{
    /// <summary>
    /// Slider box module
    /// </summary>
    public class SliderBoxModule : InputModule
    {
        /// <summary>
        /// Minimum position
        /// </summary>
        public int MinPos { get; set; }

        /// <summary>
        /// Maximum position
        /// </summary>
        public int MaxPos { get; set; }

        /// <inheritdoc/>
        public override string RenderInput(int width)
        {
            // Render an input text box with selected value and blanks as underscores.
            int value = Value is int valueInt ? valueInt : MinPos;
            string valueString = $"{value} of {MinPos}/{MaxPos}";
            string[] wrappedValue = ConsoleMisc.GetWrappedSentencesByWords($"▼ {valueString}", width);
            valueString = wrappedValue[0];

            // Determine how many underscores we need to render
            int valueWidth = ConsoleChar.EstimateCellWidth(valueString);
            int diffWidth = width - valueWidth;
            string underscores = new('_', diffWidth);

            // Render the text box contents now
            string textBox =
                ConsoleColoring.RenderSetConsoleColor(Foreground) +
                ConsoleColoring.RenderSetConsoleColor(Background, true) +
                valueString +
                ConsoleColoring.RenderSetConsoleColor(BlankForeground) +
                underscores +
                ConsoleColoring.RenderRevertForeground() +
                ConsoleColoring.RenderRevertBackground();
            return textBox;
        }

        /// <inheritdoc/>
        public override void ProcessInput(Coordinate inputPopoverPos = default, Size inputPopoverSize = default)
        {
            if (inputPopoverPos == default || inputPopoverSize == default)
            {
                // Use the input info box, since the caller needs to provide info about the popover, which doesn't exist
                int value = Value is int valueInt ? valueInt : MinPos;
                int choiceIndex = InfoBoxSliderColor.WriteInfoBoxSlider(value, MaxPos, Description, new InfoBoxSettings()
                {
                    Title = Name,
                    ForegroundColor = Foreground,
                    BackgroundColor = Background,
                }, MinPos);
                Value = choiceIndex;
            }
            else
            {
                bool bail = false;
                bool cancel = false;
                int value = Value is int valueInt ? valueInt : MinPos;
                while (!bail)
                {
                    // Render the popover. A slider will appear on the input.
                    var slider = new Slider(value, MinPos, MaxPos)
                    {
                        Width = inputPopoverSize.Width - 1,
                        SliderActiveForegroundColor = Foreground,
                        SliderForegroundColor = TransformationTools.GetDarkBackground(Foreground),
                        SliderBackgroundColor = Background,
                    };
                    TextWriterRaw.WriteRaw(
                        RendererTools.RenderRenderable(slider, new(inputPopoverPos.X + 1, inputPopoverPos.Y)) +
                        TextWriterWhereColor.RenderWhereColorBack("◀", inputPopoverPos.X, inputPopoverPos.Y, Foreground, Background) +
                        TextWriterWhereColor.RenderWhereColorBack("▶", inputPopoverPos.X + inputPopoverSize.Width - 1, inputPopoverPos.Y, Foreground, Background)
                    );

                    // Handle keypress
                    InputEventInfo data = Input.ReadPointerOrKey();
                    int maxSliderWidth = inputPopoverSize.Width - 4;
                    int sliderArrowTop = inputPopoverPos.Y;
                    int sliderArrowLeft = inputPopoverPos.X;
                    int sliderArrowRight = inputPopoverPos.X + inputPopoverSize.Width - 1;
                    var mouse = data.PointerEventContext;
                    if (mouse is not null)
                    {
                        // Make hitboxes for arrow presses
                        var arrowSliderDecreaseHitbox = new PointerHitbox(new(sliderArrowLeft, sliderArrowTop), new Action<PointerEventContext>((_) => ValueGoUp(ref value, MinPos, MaxPos))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                        var arrowSliderIncreaseHitbox = new PointerHitbox(new(sliderArrowRight, sliderArrowTop), new Action<PointerEventContext>((_) => ValueGoDown(ref value, MinPos, MaxPos))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };

                        // Mouse input received.
                        switch (mouse.Button)
                        {
                            case PointerButton.WheelUp:
                                if (PointerTools.PointerWithinRange(mouse, (sliderArrowLeft, sliderArrowTop), (sliderArrowRight, sliderArrowTop)))
                                    ValueGoUp(ref value, MinPos, MaxPos);
                                break;
                            case PointerButton.WheelDown:
                                if (PointerTools.PointerWithinRange(mouse, (sliderArrowLeft, sliderArrowTop), (sliderArrowRight, sliderArrowTop)))
                                    ValueGoDown(ref value, MinPos, MaxPos);
                                break;
                            case PointerButton.Left:
                                if (mouse.ButtonPress != PointerButtonPress.Released)
                                    break;
                                if (arrowSliderIncreaseHitbox.IsPointerWithin(mouse) || arrowSliderDecreaseHitbox.IsPointerWithin(mouse))
                                {
                                    arrowSliderIncreaseHitbox.ProcessPointer(mouse, out bool done);
                                    if (!done)
                                        arrowSliderDecreaseHitbox.ProcessPointer(mouse, out done);
                                }
                                break;
                        }
                    }
                    else if (data.ConsoleKeyInfo is ConsoleKeyInfo cki && !Input.PointerActive)
                    {
                        switch (cki.Key)
                        {
                            case ConsoleKey.LeftArrow:
                                value--;
                                if (value < MinPos)
                                    value = MaxPos;
                                break;
                            case ConsoleKey.RightArrow:
                                value++;
                                if (value > MaxPos)
                                    value = MinPos;
                                break;
                            case ConsoleKey.Home:
                                value = MinPos;
                                break;
                            case ConsoleKey.End:
                                value = MaxPos;
                                break;
                            case ConsoleKey.Enter:
                                bail = true;
                                break;
                            case ConsoleKey.Escape:
                                bail = true;
                                cancel = true;
                                break;
                        }
                    }
                }
                if (!cancel)
                    Value = value;
            }
            Provided = true;
        }

        private static void ValueGoUp(ref int selected, int minPos, int maxPos)
        {
            selected--;
            if (selected < minPos)
                selected = maxPos;
        }

        private static void ValueGoDown(ref int selected, int minPos, int maxPos)
        {
            selected++;
            if (selected > maxPos)
                selected = minPos;
        }
    }
}
