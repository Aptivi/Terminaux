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

using Aptivestigate.Logging;
using Aptivestigate.Serilog;
using Serilog;
using System;
using System.Linq;
using System.Text;
using Textify.General;

namespace Terminaux.Base
{
    /// <summary>
    /// Console logger class
    /// </summary>
    public static class ConsoleLogger
    {
        private static BaseLogger? abstractLogger = new SerilogLogger(new LoggerConfiguration().WriteTo.File(LogTools.GenerateLogFilePath(out _)));

        /// <summary>
        /// Whether to enable logging or not
        /// </summary>
        public static bool EnableLogging { get; set; }

        /// <summary>
        /// Sets the logger to use
        /// </summary>
        public static BaseLogger? AbstractLogger
        {
            get => EnableLogging ? abstractLogger : null;
            set => abstractLogger = value;
        }

        internal static void Debug(string message, params object?[]? args) =>
            abstractLogger?.Debug(message.FilterMessage(args), args);

        internal static void Debug(Exception ex, string message, params object?[]? args) =>
            abstractLogger?.Debug(ex, message.FilterMessage(args), args);

        internal static void Error(string message, params object?[]? args) =>
            abstractLogger?.Error(message.FilterMessage(args), args);

        internal static void Error(Exception ex, string message, params object?[]? args) =>
            abstractLogger?.Error(ex, message.FilterMessage(args), args);

        internal static void Fatal(string message, params object?[]? args) =>
            abstractLogger?.Fatal(message.FilterMessage(args), args);

        internal static void Fatal(Exception ex, string message, params object?[]? args) =>
            abstractLogger?.Fatal(ex, message.FilterMessage(args), args);

        internal static void Info(string message, params object?[]? args) =>
            abstractLogger?.Info(message.FilterMessage(args), args);

        internal static void Info(Exception ex, string message, params object?[]? args) =>
            abstractLogger?.Info(ex, message.FilterMessage(args), args);

        internal static void Warning(string message, params object?[]? args) =>
            abstractLogger?.Warning(message.FilterMessage(args), args);

        internal static void Warning(Exception ex, string message, params object?[]? args) =>
            abstractLogger?.Warning(ex, message.FilterMessage(args), args);

        private static string FilterMessage(this string message, params object?[]? args)
        {
            if (!EnableLogging)
                return message;
            StringBuilder messageBuilder = new();
            char[] controlChars = CharManager.GetAllControlChars();
            message = message.FormatString(args);
            foreach (char messageChar in message)
            {
                string charString = $"{messageChar}";
                if (controlChars.Contains(messageChar))
                    charString = $"[C:{(int)messageChar}]";
                messageBuilder.Append(charString);
            }
            return messageBuilder.ToString();
        }
    }
}
