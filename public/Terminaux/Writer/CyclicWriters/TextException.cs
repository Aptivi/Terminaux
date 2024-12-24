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

using System;
using System.Diagnostics;
using System.Text;
using Terminaux.Colors;

namespace Terminaux.Writer.CyclicWriters
{
    /// <summary>
    /// Exception text renderable
    /// </summary>
    public class TextException : IStaticRenderable
    {
        private Exception exception;
        private Color foregroundColor = ColorTools.CurrentForegroundColor;
        private Color exceptionNameColor = ColorTools.CurrentForegroundColor;
        private Color exceptionMessageColor = ColorTools.CurrentForegroundColor;
        private Color methodMemberColor = ColorTools.CurrentForegroundColor;
        private Color methodColor = ColorTools.CurrentForegroundColor;
        private Color parameterTypeColor = ColorTools.CurrentForegroundColor;
        private Color parameterColor = ColorTools.CurrentForegroundColor;
        private Color ilOffsetColor = ColorTools.CurrentForegroundColor;
        private Color fileNameColor = ColorTools.CurrentForegroundColor;
        private Color fileLineNumberColor = ColorTools.CurrentForegroundColor;
        private Color fileColumnNumberColor = ColorTools.CurrentForegroundColor;
        private Color backgroundColor = ColorTools.CurrentBackgroundColor;
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
        public string Render()
        {
            // Populate the message
            var stackTraceBuilder = new StringBuilder();
            int innerExceptions = 0;
            var inner = Exception.InnerException;
            while (inner is not null)
            {
                stackTraceBuilder.Append(ProcessException(++innerExceptions, inner));
                inner = inner.InnerException;
            }
            stackTraceBuilder.Append(ProcessException(0, Exception));
            return stackTraceBuilder.ToString();
        }

        private string ProcessException(int indent, Exception ex)
        {
            // Populate the message
            var stackTraceBuilder = new StringBuilder();
            stackTraceBuilder.AppendLine(
                $"{(UseColors ? ColorTools.RenderSetConsoleColor(ForegroundColor) : "")}" +
                $"{(UseColors ? ColorTools.RenderSetConsoleColor(BackgroundColor, true) : "")}" +
                new string(' ', indent * 4) +
                "Error " +
                $"{(UseColors ? ColorTools.RenderSetConsoleColor(ExceptionNameColor) : "")}" +
                ex.GetType().Name +
                $"{(UseColors ? ColorTools.RenderSetConsoleColor(ForegroundColor) : "")}" +
                ": " +
                $"{(UseColors ? ColorTools.RenderSetConsoleColor(ExceptionMessageColor) : "")}" +
                ex.Message
            );

            // Populate the stack trace
            var stackTrace = new StackTrace(ex, UseFileInfo);
            int frames = stackTrace.FrameCount;
            for (int i = 0; i < frames; i++)
            {
                // Get the frame
                stackTraceBuilder.Append(
                    $"{(UseColors ? ColorTools.RenderSetConsoleColor(ForegroundColor) : "")}" +
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
                        $"{(UseColors ? ColorTools.RenderSetConsoleColor(MethodMemberColor) : "")}" +
                        $"{memberClassName}." +
                        $"{(UseColors ? ColorTools.RenderSetConsoleColor(MethodColor) : "")}" +
                        $"{memberName}" +
                        $"{(UseColors ? ColorTools.RenderSetConsoleColor(ForegroundColor) : "")}" +
                        "("
                    );

                    // Write the parameters
                    var parameters = methodInfo.GetParameters();
                    foreach (var parameter in parameters)
                    {
                        string parameterName = parameter.Name;
                        var parameterType = parameter.ParameterType;
                        stackTraceBuilder.Append(
                            $"{(UseColors ? ColorTools.RenderSetConsoleColor(ParameterTypeColor) : "")}" +
                            $"{parameterType.Name} " +
                            $"{(UseColors ? ColorTools.RenderSetConsoleColor(ParameterColor) : "")}" +
                            $"{parameterName}, "
                        );
                    }

                    // Close it
                    stackTraceBuilder.Append(
                        $"{(UseColors ? ColorTools.RenderSetConsoleColor(ForegroundColor) : "")}" +
                        ") "
                    );
                }
                else
                {
                    stackTraceBuilder.Append(
                        $"{(UseColors ? ColorTools.RenderSetConsoleColor(MethodColor) : "")}" +
                        "<unknown method> "
                    );
                }

                // Get the IL offset
                if (frame.HasILOffset())
                {
                    var offset = frame.GetILOffset();
                    stackTraceBuilder.Append(
                        $"{(UseColors ? ColorTools.RenderSetConsoleColor(ForegroundColor) : "")}" +
                        "[" +
                        $"{(UseColors ? ColorTools.RenderSetConsoleColor(IlOffsetColor) : "")}" +
                        $"0x{offset:X8}" +
                        $"{(UseColors ? ColorTools.RenderSetConsoleColor(ForegroundColor) : "")}" +
                        "] "
                    );
                }
                else
                {
                    stackTraceBuilder.Append(
                        $"{(UseColors ? ColorTools.RenderSetConsoleColor(IlOffsetColor) : "")}" +
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
                        $"{(UseColors ? ColorTools.RenderSetConsoleColor(ForegroundColor) : "")}" +
                        "@ " +
                        $"{(UseColors ? ColorTools.RenderSetConsoleColor(FileNameColor) : "")}" +
                        fileName +
                        $"{(UseColors ? ColorTools.RenderSetConsoleColor(ForegroundColor) : "")}" +
                        ":" +
                        $"{(UseColors ? ColorTools.RenderSetConsoleColor(FileLineNumberColor) : "")}" +
                        fileLineNum +
                        $"{(UseColors ? ColorTools.RenderSetConsoleColor(ForegroundColor) : "")}" +
                        "," +
                        $"{(UseColors ? ColorTools.RenderSetConsoleColor(FileColumnNumberColor) : "")}" +
                        fileColumnNum +
                        $"{(UseColors ? ColorTools.RenderSetConsoleColor(ForegroundColor) : "")}" +
                        " "
                    );
                }
                else
                {
                    stackTraceBuilder.AppendLine(
                        $"{(UseColors ? ColorTools.RenderSetConsoleColor(IlOffsetColor) : "")}" +
                        "<unknown source>"
                    );
                }
            }

            // Reset colors if needed
            stackTraceBuilder.Append(
                $"{(UseColors ? ColorTools.RenderResetColors() : "")}"
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
