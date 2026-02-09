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
using System.Diagnostics;
using System.Text;
using Terminaux.Base.Extensions;
using Colorimetry;
using Terminaux.Themes.Colors;

namespace Terminaux.Writer.CyclicWriters.Simple
{
    /// <summary>
    /// Exception text renderable
    /// </summary>
    public class TextException : SimpleCyclicWriter
    {
        private Exception exception;
        private Color foregroundColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
        private Color exceptionNameColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
        private Color exceptionMessageColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
        private Color methodMemberColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
        private Color methodColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
        private Color parameterTypeColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
        private Color parameterColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
        private Color ilOffsetColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
        private Color fileNameColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
        private Color fileLineNumberColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
        private Color fileColumnNumberColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
        private Color backgroundColor = ThemeColorsTools.GetColor(ThemeColorType.Background);
        private bool useFileInfo = true;
        private bool useColors = true;

        /// <summary>
        /// Exception to render
        /// </summary>
        public Exception Exception
        {
            get => exception;
            set => exception = value;
        }

        /// <summary>
        /// Foreground color of the exception text (for general text)
        /// </summary>
        public Color ForegroundColor
        {
            get => foregroundColor;
            set => foregroundColor = value;
        }

        /// <summary>
        /// Exception name color
        /// </summary>
        public Color ExceptionNameColor
        {
            get => exceptionNameColor;
            set => exceptionNameColor = value;
        }

        /// <summary>
        /// Exception message color
        /// </summary>
        public Color ExceptionMessageColor
        {
            get => exceptionMessageColor;
            set => exceptionMessageColor = value;
        }

        /// <summary>
        /// Method member color
        /// </summary>
        public Color MethodMemberColor
        {
            get => methodMemberColor;
            set => methodMemberColor = value;
        }
       
        /// <summary>
        /// Method color
        /// </summary>
        public Color MethodColor
        {
            get => methodColor;
            set => methodColor = value;
        }

        /// <summary>
        /// Parameter type color
        /// </summary>
        public Color ParameterTypeColor
        {
            get => parameterTypeColor;
            set => parameterTypeColor = value;
        }
       
        /// <summary>
        /// Parameter color
        /// </summary>
        public Color ParameterColor
        {
            get => parameterColor;
            set => parameterColor = value;
        }
       
        /// <summary>
        /// Intermediate Language offset color
        /// </summary>
        public Color IlOffsetColor
        {
            get => ilOffsetColor;
            set => ilOffsetColor = value;
        }
       
        /// <summary>
        /// File name color
        /// </summary>
        public Color FileNameColor
        {
            get => fileNameColor;
            set => fileNameColor = value;
        }
       
        /// <summary>
        /// File line number color
        /// </summary>
        public Color FileLineNumberColor
        {
            get => fileLineNumberColor;
            set => fileLineNumberColor = value;
        }
       
        /// <summary>
        /// File column number color
        /// </summary>
        public Color FileColumnNumberColor
        {
            get => fileColumnNumberColor;
            set => fileColumnNumberColor = value;
        }

        /// <summary>
        /// Background color of the exception text
        /// </summary>
        public Color BackgroundColor
        {
            get => backgroundColor;
            set => backgroundColor = value;
        }

        /// <summary>
        /// Whether to use file info or not
        /// </summary>
        public bool UseFileInfo
        {
            get => useFileInfo;
            set => useFileInfo = value;
        }

        /// <summary>
        /// Whether to use colors or not
        /// </summary>
        public bool UseColors
        {
            get => useColors;
            set => useColors = value;
        }

        /// <summary>
        /// Renders an exception text
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public override string Render()
        {
            // Populate the message
            var stackTraceBuilder = new StringBuilder();
            stackTraceBuilder.Append(ProcessException(0, Exception));

            // Populate inner exceptions
            int innerExceptions = 0;
            var inner = Exception.InnerException;
            while (inner is not null)
            {
                stackTraceBuilder.Append(ProcessException(++innerExceptions, inner));
                inner = inner.InnerException;
            }
            return stackTraceBuilder.ToString();
        }

        private string ProcessException(int indent, Exception ex)
        {
            // Populate the message
            var stackTraceBuilder = new StringBuilder();
            stackTraceBuilder.AppendLine(
                $"{(UseColors ? ConsoleColoring.RenderSetConsoleColor(ForegroundColor) : "")}" +
                $"{(UseColors ? ConsoleColoring.RenderSetConsoleColor(BackgroundColor, true) : "")}" +
                new string(' ', indent * 4) +
                "Error " +
                $"{(UseColors ? ConsoleColoring.RenderSetConsoleColor(ExceptionNameColor) : "")}" +
                ex.GetType().Name +
                $"{(UseColors ? ConsoleColoring.RenderSetConsoleColor(ForegroundColor) : "")}" +
                ": " +
                $"{(UseColors ? ConsoleColoring.RenderSetConsoleColor(ExceptionMessageColor) : "")}" +
                ex.Message
            );

            // Populate the stack trace
            var stackTrace = new StackTrace(ex, UseFileInfo);
            int frames = stackTrace.FrameCount;
            for (int i = 0; i < frames; i++)
            {
                // Get the frame
                stackTraceBuilder.Append(
                    $"{(UseColors ? ConsoleColoring.RenderSetConsoleColor(ForegroundColor) : "")}" +
                    new string(' ', indent * 4) +
                    "  at "
                );
                var frame = stackTrace.GetFrame(i);

                // Get the method
                if (frame.HasMethod())
                {
                    // Write the method name and class name
                    var methodInfo = frame.GetMethod();
                    string memberName = methodInfo.Name;
                    string memberClassName = methodInfo.DeclaringType.Name;
                    stackTraceBuilder.Append(
                        $"{(UseColors ? ConsoleColoring.RenderSetConsoleColor(MethodMemberColor) : "")}" +
                        $"{memberClassName}." +
                        $"{(UseColors ? ConsoleColoring.RenderSetConsoleColor(MethodColor) : "")}" +
                        $"{memberName}" +
                        $"{(UseColors ? ConsoleColoring.RenderSetConsoleColor(ForegroundColor) : "")}" +
                        "("
                    );

                    // Write the parameters
                    var parameters = methodInfo.GetParameters();
                    foreach (var parameter in parameters)
                    {
                        string parameterName = parameter.Name;
                        var parameterType = parameter.ParameterType;
                        stackTraceBuilder.Append(
                            $"{(UseColors ? ConsoleColoring.RenderSetConsoleColor(ParameterTypeColor) : "")}" +
                            $"{parameterType.Name} " +
                            $"{(UseColors ? ConsoleColoring.RenderSetConsoleColor(ParameterColor) : "")}" +
                            $"{parameterName}, "
                        );
                    }

                    // Close it
                    stackTraceBuilder.Append(
                        $"{(UseColors ? ConsoleColoring.RenderSetConsoleColor(ForegroundColor) : "")}" +
                        ") "
                    );
                }
                else
                {
                    stackTraceBuilder.Append(
                        $"{(UseColors ? ConsoleColoring.RenderSetConsoleColor(MethodColor) : "")}" +
                        "<unknown method> "
                    );
                }

                // Get the IL offset
                if (frame.HasILOffset())
                {
                    var offset = frame.GetILOffset();
                    stackTraceBuilder.Append(
                        $"{(UseColors ? ConsoleColoring.RenderSetConsoleColor(ForegroundColor) : "")}" +
                        "[" +
                        $"{(UseColors ? ConsoleColoring.RenderSetConsoleColor(IlOffsetColor) : "")}" +
                        $"0x{offset:X8}" +
                        $"{(UseColors ? ConsoleColoring.RenderSetConsoleColor(ForegroundColor) : "")}" +
                        "] "
                    );
                }
                else
                {
                    stackTraceBuilder.Append(
                        $"{(UseColors ? ConsoleColoring.RenderSetConsoleColor(IlOffsetColor) : "")}" +
                        "<unknown il offset> "
                    );
                }

                // Get the source
                if (frame.HasSource())
                {
                    var fileName = frame.GetFileName();
                    var fileLineNum = frame.GetFileLineNumber();
                    var fileColumnNum = frame.GetFileColumnNumber();
                    stackTraceBuilder.AppendLine(
                        $"{(UseColors ? ConsoleColoring.RenderSetConsoleColor(ForegroundColor) : "")}" +
                        "@ " +
                        $"{(UseColors ? ConsoleColoring.RenderSetConsoleColor(FileNameColor) : "")}" +
                        fileName +
                        $"{(UseColors ? ConsoleColoring.RenderSetConsoleColor(ForegroundColor) : "")}" +
                        ":" +
                        $"{(UseColors ? ConsoleColoring.RenderSetConsoleColor(FileLineNumberColor) : "")}" +
                        fileLineNum +
                        $"{(UseColors ? ConsoleColoring.RenderSetConsoleColor(ForegroundColor) : "")}" +
                        "," +
                        $"{(UseColors ? ConsoleColoring.RenderSetConsoleColor(FileColumnNumberColor) : "")}" +
                        fileColumnNum +
                        $"{(UseColors ? ConsoleColoring.RenderSetConsoleColor(ForegroundColor) : "")}" +
                        " "
                    );
                }
                else
                {
                    stackTraceBuilder.AppendLine(
                        $"{(UseColors ? ConsoleColoring.RenderSetConsoleColor(IlOffsetColor) : "")}" +
                        "<unknown source>"
                    );
                }
            }

            // Reset colors if needed
            stackTraceBuilder.Append(
                $"{(UseColors ? ConsoleColoring.RenderResetColors() : "")}"
            );
            return stackTraceBuilder.ToString();
        }

        /// <summary>
        /// Makes a new instance of the exception text renderer
        /// </summary>
        public TextException(Exception ex)
        {
            exception = ex;
        }
    }
}
