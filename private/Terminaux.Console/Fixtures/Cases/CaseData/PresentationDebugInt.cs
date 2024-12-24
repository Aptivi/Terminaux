﻿//
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

using System.Linq;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Inputs.Presentation;
using Terminaux.Inputs.Presentation.Elements;
using System;
using Terminaux.Inputs.Presentation.Inputs;
using Textify.General;
using Terminaux.Inputs.Styles;

namespace Terminaux.Console.Fixtures.Cases.CaseData
{
    internal static class PresentationDebugInt
    {
        internal static string[] data1 =
        [
            "Alex",
            "Zhao",
            "Agustin",
            "Jim",
            "Sarah"
        ];

        internal static string[] data2 =
        [
            "Alex",
            "Zhao",
            "Agustin",
            "Jim",
            "Sarah",
            "Akshay",
            "Aladdin",
            "Bella",
            "Billy",
            "Blake",
            "Bobby",
            "Chandran",
            "Colin",
            "Connor",
            "Debbie",
            "Eduard",
            "David",
            "Paul",
            "Ella",
            "Elizabeth",
            "Fitz",
            "Gary",
            "Hendrick",
            "Henry",
            "Jared",
            "Jasmine",
            "Johnny",
            "Sofia",
            "Thalia",
            "Vincent"
        ];

        internal static InputInfo input1 =
            new("Test enjoyment", "Asks the user if they have enjoyed testing",
                new TextInputMethod()
                {
                    Question = $"Did you enjoy {new Color(ConsoleColors.Green1).VTSequenceForeground}testing?"
                }, true
            );

        internal static InputInfo input2 =
            new("First choice", "Asks the user to select one of the names (small)",
                new SelectionInputMethod()
                {
                    Question = "Ultricies mi eget mauris pharetra:",
                    Choices = data1.Select((data) => new InputChoiceInfo(data, data)).ToArray()
                }, true
            );

        internal static InputInfo input3 =
            new("Second choice", "Asks the user to select one of the names (larger)",
                new SelectionInputMethod()
                {
                    Question = "Ultricies mi eget mauris pharetra sapien et ligula:",
                    Choices = data2.Select((data) => new InputChoiceInfo(data, data)).ToArray()
                }, true
            );

        internal static InputInfo input4 =
            new("First multiple choice", "Asks the user to select one or more of the names (small)",
                new SelectionMultipleInputMethod()
                {
                    Question = "Ultricies mi eget mauris pharetra:",
                    Choices = data1.Select((data) => new InputChoiceInfo(data, data)).ToArray()
                }, true
            );

        internal static InputInfo input5 =
            new("Second multiple choice", "Asks the user to select one or more of the names (larger)",
                new SelectionMultipleInputMethod()
                {
                    Question = "Ultricies mi eget mauris pharetra sapien et ligula:",
                    Choices = data2.Select((data) => new InputChoiceInfo(data, data)).ToArray()
                }, true
            );

        internal static Slideshow Debug =>
            new(
                "Debugging the Presentation",
                [
                    #region First page - Debugging just text elements
                    new PresentationPage("First page - Debugging just text elements",
                        [
                            new TextElement()
                            {
                                Arguments = [
                                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt " +
                                    "ut labore et dolore magna aliqua. Risus sed vulputate odio ut enim blandit. Ac tortor vitae " +
                                    "purus faucibus. Quis eleifend quam adipiscing vitae. Enim blandit volutpat maecenas volutpat " +
                                    "blandit aliquam. Ultricies mi eget mauris pharetra. Vitae elementum curabitur vitae nunc sed " +
                                    "velit dignissim. Tempor orci dapibus ultrices in iaculis nunc sed augue lacus. Cras tincidunt " +
                                    "lobortis feugiat vivamus at. Scelerisque fermentum dui faucibus in ornare quam viverra. " +
                                    "Tincidunt nunc pulvinar sapien et ligula ullamcorper malesuada proin."
                                ]
                            },
                            new TextElement()
                            {
                                Arguments = [
                                    "Enjoying yet? {0}Color treat!",
                                    new Color(ConsoleColors.Purple4Alt).VTSequenceForeground
                                ]
                            }
                        ]
                    ),
                    #endregion

                    #region Second page - Debugging text and input elements
                    new PresentationPage("Second page - Debugging text and input elements",
                        [
                            new TextElement()
                            {
                                Arguments = [
                                    "{0}Lorem ipsum {1}dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt " +
                                    "ut labore et dolore {2}magna aliqua. {1}Risus sed vulputate odio ut enim blandit. Ac tortor vitae " +
                                    "purus faucibus. Quis eleifend quam adipiscing vitae. {2}Enim blandit {1}volutpat maecenas volutpat " +
                                    "blandit aliquam. {3}Ultricies {1}mi eget mauris pharetra. {3}Vitae {1}elementum curabitur vitae nunc sed " +
                                    "velit dignissim. {3}Tempor {1}orci dapibus ultrices in iaculis nunc sed augue lacus. Cras tincidunt " +
                                    "lobortis feugiat vivamus at. Scelerisque fermentum dui faucibus in ornare quam viverra. " +
                                    "Tincidunt nunc {2}pulvinar sapien {1}et ligula ullamcorper malesuada proin.",
                                    new Color(ConsoleColors.Green).VTSequenceForeground,
                                    new Color(ConsoleColors.White).VTSequenceForeground,
                                    new Color(ConsoleColors.Yellow).VTSequenceForeground,
                                    new Color(ConsoleColors.Red).VTSequenceForeground
                                ]
                            },
                            new TextElement()
                            {
                                Arguments = [
                                    "Happy {0}hacking!",
                                    new Color(ConsoleColors.Green1).VTSequenceForeground
                                ]
                            }
                        ],
                        [
                            input1
                        ]
                    ),
                    #endregion

                    #region Third page - Debugging dynamic text
                    new PresentationPage("Third page - Debugging dynamic text",
                        [
                            new DynamicTextElement()
                            {
                                Arguments = [
                                    () => DateTime.Now.ToString(),
                                ]
                            }
                        ]
                    ),
                    #endregion

                    #region Fourth page - Debugging overflow check
                    new PresentationPage("Fourth page - Debugging overflow check",
                        [
                            new TextElement()
                            {
                                Arguments = [
                                    "This should work like your typical modal informational box.\n\n" +
                                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt " +
                                    "ut labore et dolore magna aliqua. Risus sed vulputate odio ut enim blandit. Ac tortor vitae " +
                                    "purus faucibus. Quis eleifend quam adipiscing vitae. Enim blandit volutpat maecenas volutpat " +
                                    "blandit aliquam. Ultricies mi eget mauris pharetra. Vitae elementum curabitur vitae nunc sed " +
                                    "velit dignissim. Tempor orci dapibus ultrices in iaculis nunc sed augue lacus. Cras tincidunt " +
                                    "lobortis feugiat vivamus at. Scelerisque fermentum dui faucibus in ornare quam viverra. " +
                                    "Tincidunt nunc pulvinar sapien et ligula ullamcorper malesuada proin."
                                ]
                            },
                            new TextElement()
                            {
                                Arguments = [
                                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt " +
                                    "ut labore et dolore magna aliqua. Risus sed vulputate odio ut enim blandit. Ac tortor vitae " +
                                    "purus faucibus. Quis eleifend quam adipiscing vitae. Enim blandit volutpat maecenas volutpat " +
                                    "blandit aliquam. Ultricies mi eget mauris pharetra. Vitae elementum curabitur vitae nunc sed " +
                                    "velit dignissim. Tempor orci dapibus ultrices in iaculis nunc sed augue lacus. Cras tincidunt " +
                                    "lobortis feugiat vivamus at. Scelerisque fermentum dui faucibus in ornare quam viverra. " +
                                    "Tincidunt nunc pulvinar sapien et ligula ullamcorper malesuada proin."
                                ]
                            },
                            new TextElement()
                            {
                                Arguments = [
                                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt " +
                                    "ut labore et dolore magna aliqua. Risus sed vulputate odio ut enim blandit. Ac tortor vitae " +
                                    "purus faucibus. Quis eleifend quam adipiscing vitae. Enim blandit volutpat maecenas volutpat " +
                                    "blandit aliquam. Ultricies mi eget mauris pharetra. Vitae elementum curabitur vitae nunc sed " +
                                    "velit dignissim. Tempor orci dapibus ultrices in iaculis nunc sed augue lacus. Cras tincidunt " +
                                    "lobortis feugiat vivamus at. Scelerisque fermentum dui faucibus in ornare quam viverra. " +
                                    "Tincidunt nunc pulvinar sapien et ligula ullamcorper malesuada proin."
                                ]
                            },
                            new TextElement()
                            {
                                Arguments = [
                                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt " +
                                    "ut labore et dolore magna aliqua. Risus sed vulputate odio ut enim blandit. Ac tortor vitae " +
                                    "purus faucibus. Quis eleifend quam adipiscing vitae. Enim blandit volutpat maecenas volutpat " +
                                    "blandit aliquam. Ultricies mi eget mauris pharetra. Vitae elementum curabitur vitae nunc sed " +
                                    "velit dignissim. Tempor orci dapibus ultrices in iaculis nunc sed augue lacus. Cras tincidunt " +
                                    "lobortis feugiat vivamus at. Scelerisque fermentum dui faucibus in ornare quam viverra. " +
                                    "Tincidunt nunc pulvinar sapien et ligula ullamcorper malesuada proin."
                                ]
                            },
                            new TextElement()
                            {
                                Arguments = [
                                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt " +
                                    "ut labore et dolore magna aliqua. Risus sed vulputate odio ut enim blandit. Ac tortor vitae " +
                                    "purus faucibus. Quis eleifend quam adipiscing vitae. Enim blandit volutpat maecenas volutpat " +
                                    "blandit aliquam. Ultricies mi eget mauris pharetra. Vitae elementum curabitur vitae nunc sed " +
                                    "velit dignissim. Tempor orci dapibus ultrices in iaculis nunc sed augue lacus. Cras tincidunt " +
                                    "lobortis feugiat vivamus at. Scelerisque fermentum dui faucibus in ornare quam viverra. " +
                                    "Tincidunt nunc pulvinar sapien et ligula ullamcorper malesuada proin."
                                ]
                            }
                        ]
                    ),
                    #endregion

                    #region Fifth page - Debugging choice input
                    new PresentationPage("Fifth page - Debugging choice input",
                        [
                            new TextElement()
                            {
                                Arguments = [
                                    "Tincidunt nunc pulvinar sapien et ligula ullamcorper malesuada proin."
                                ]
                            },
                        ],
                        [
                            input2,
                            input3,
                        ]
                    ),
                    #endregion

                    #region Sixth page - Debugging multiple choice input
                    new PresentationPage("Sixth page - Debugging choice input",
                        [
                            new TextElement()
                            {
                                Arguments = [
                                    "Tempor orci dapibus ultrices in iaculis nunc sed augue lacus."
                                ]
                            },
                        ],
                        [
                            input4,
                            input5,
                        ]
                    ),
                    #endregion

                    #region Seventh page - Showcasing all inputs
                    new PresentationPage("Seventh page - Showcasing all inputs",
                        [
                            new DynamicTextElement()
                            {
                                Arguments = [
                                    new Func<string>(() =>
                                    {
                                        // Get the input methods
                                        var method1 = input1.GetInputMethod<TextInputMethod>();
                                        var method2 = input2.GetInputMethod<SelectionInputMethod>();
                                        var method3 = input3.GetInputMethod<SelectionInputMethod>();
                                        var method4 = input4.GetInputMethod<SelectionMultipleInputMethod>();
                                        var method5 = input5.GetInputMethod<SelectionMultipleInputMethod>();

                                        // Now, form the resulting string
                                        if (method2.Choices is not null && method3.Choices is not null && method4.Choices is not null && method5.Choices is not null){
                                            string results = ("Input 1: {0}\n" +
                                                              "Input 2: {1}\n" +
                                                              "Input 3: {2}\n" +
                                                              "Input 4: {3}\n" +
                                                              "Input 5: {4}")
                                            .FormatString
                                            (
                                                // Second page
                                                method1.Input,

                                                // Fifth page
                                                method2.Choices[method2.Input].ChoiceName,
                                                method3.Choices[method3.Input].ChoiceName,

                                                // Sixth page
                                                string.Join(", ", method4.Input.Select((idx) => method4.Choices[idx].ChoiceName)),
                                                string.Join(", ", method5.Input.Select((idx) => method5.Choices[idx].ChoiceName))
                                            );
                                            return results;
                                        }
                                        return "";
                                    })
                                ]
                            },
                        ]
                    ),
                    #endregion
                ]
            );
    }
}
