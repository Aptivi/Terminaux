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

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Base.TermInfo;
using Colorimetry;
using Terminaux.Sequences;
using Terminaux.Writer.CyclicWriters.Renderer.Effects;
using Terminaux.Writer.CyclicWriters.Renderer.Effects.Builtins;
using Textify.General;
using Textify.General.Structures;

namespace Terminaux.Writer.CyclicWriters.Renderer.Markup
{
    /// <summary>
    /// Markup parsing and processing tools
    /// </summary>
    public static class MarkupTools
    {
        internal static IEffect[] effects =
        [
            new BoldEffect(),
            new ConcealEffect(),
            new DimEffect(),
            new InvertEffect(),
            new ItalicEffect(),
            new RapidBlinkEffect(),
            new SlowBlinkEffect(),
            new StandoutEffect(),
            new StrikethroughEffect(),
            new UnderlineEffect(),
        ];

        /// <summary>
        /// Gets a list of markup information instances
        /// </summary>
        /// <param name="mark">Markup instance</param>
        /// <returns>Array of parsed markup representations</returns>
        public static MarkupInfo[] GetMarkupInfo(Mark? mark)
        {
            if (mark is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_WRITER_CYCLICWRITERS_RENDERER_MARKUP_EXCEPTION_MARKUPUNSPECIFIED"));
            return GetMarkupInfo(mark.Markup);
        }

        /// <summary>
        /// Gets a list of markup information instances
        /// </summary>
        /// <param name="markup">Markup representation</param>
        /// <returns>Array of parsed markup representations</returns>
        public static MarkupInfo[] GetMarkupInfo(string markup) =>
            GetMarkupInfo((WideString)markup);

        /// <summary>
        /// Gets a list of markup information instances
        /// </summary>
        /// <param name="markup">Markup representation</param>
        /// <returns>Array of parsed markup representations</returns>
        public static MarkupInfo[] GetMarkupInfo(WideString markup)
        {
            // Markup is inspired by BBCode and can have escaped starting and ending sequence characters.
            var finalResult = new StringBuilder(markup);
            List<MarkupInfo> sequences = [];
            List<MarkupInfo> queuedSequences = [];
            StringBuilder appended = new();
            bool append = false;
            int nestLevel = -1;
            for (int i = 0; i < markup.Length; i++)
            {
                WideChar markupChar = markup[i];
                WideChar nextChar = i + 1 < markup.Length ? markup[i + 1] : (WideChar)'\0';

                // Check to see if we are escaping or not
                if (((markupChar == '[' && nextChar == '[') || (markupChar == ']' && nextChar == ']')) && queuedSequences.Count == 0)
                {
                    ConsoleLogger.Debug("Adding escape markup {0}, {1}", markupChar, nextChar);
                    sequences.Add(new()
                    {
                        entranceIndex = i,
                        exitIndex = i + 1,
                        nestLevel = nestLevel,
                        represent = $"{markupChar}",
                        isEscape = true,
                    });
                    i++;
                    continue;
                }

                // Now, parse the markups by checking for them while seeking characters to get properties inside them and to apply
                // the changes.
                ConsoleLogger.Debug("Processing characters {0}, {1}", markupChar, nextChar);
                if (markupChar == '[' && nextChar != '/')
                {
                    nestLevel++;
                    queuedSequences.Add(new()
                    {
                        entranceIndex = i,
                        nestLevel = nestLevel,
                    });
                    appended.Append($"{markupChar}");
                    append = true;
                    continue;
                }
                else if (markupChar == '[' && nextChar == '/' && queuedSequences.Count > 0)
                {
                    WideChar nextTwoChar = i + 2 < markup.Length ? markup[i + 2] : (WideChar)'\0';
                    if (nextTwoChar != ']')
                        throw new TerminauxException(LanguageTools.GetLocalized("T_WRITER_CYCLICWRITERS_RENDERER_MARKUP_EXCEPTION_ENDTAGINVALID"));
                    if (queuedSequences.Count > 0)
                    {
                        nestLevel--;
                        var queued = queuedSequences[queuedSequences.Count - 1];
                        queuedSequences.Remove(queued);
                        queued.exitIndex = i;
                        sequences.Add(queued);
                    }
                    else
                        throw new TerminauxException(LanguageTools.GetLocalized("T_WRITER_CYCLICWRITERS_RENDERER_MARKUP_EXCEPTION_NOQUEUEDSEQS"));
                    i += 2;
                    continue;
                }
                if (queuedSequences.Count > 0 && i == markup.Length - 1)
                {
                    ConsoleLogger.Warning("When reaching end of sequence, queued sequences: {0}", queuedSequences.Count);
                    throw new TerminauxException(LanguageTools.GetLocalized("T_WRITER_CYCLICWRITERS_RENDERER_MARKUP_EXCEPTION_MISSINGTAGS"), queuedSequences.Count);
                }

                // Add a character
                if (append)
                {
                    appended.Append((string)markupChar);
                    if (markupChar == ']' && queuedSequences.Count > 0)
                    {
                        queuedSequences[queuedSequences.Count - 1].represent = appended.ToString();
                        append = false;
                        appended.Clear();
                    }
                }
            }
            return [.. sequences.OrderBy((mi) => mi.entranceIndex)];
        }

        /// <summary>
        /// Parses the markup representation
        /// </summary>
        /// <param name="mark">Markup instance</param>
        /// <param name="foregroundColor">Foreground color of the initial markup color. Overrides the initial format argument.</param>
        /// <param name="backgroundColor">Background color of the initial markup color. Overrides the initial format argument.</param>
        /// <param name="initialFormat">Initial format. Overrides the foreground and the background color arguments.</param>
        /// <returns>A string that can be written to the console with the parsed markup representations</returns>
        public static string ParseMarkup(Mark? mark, Color? foregroundColor = null, Color? backgroundColor = null, string initialFormat = "")
        {
            if (mark is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_WRITER_CYCLICWRITERS_RENDERER_MARKUP_EXCEPTION_MARKUPUNSPECIFIED"));
            return ParseMarkup(mark.Markup, foregroundColor, backgroundColor, initialFormat);
        }

        /// <summary>
        /// Parses the markup representation
        /// </summary>
        /// <param name="markup">Markup representation</param>
        /// <param name="foregroundColor">Foreground color of the initial markup color. Overrides the initial format argument.</param>
        /// <param name="backgroundColor">Background color of the initial markup color. Overrides the initial format argument.</param>
        /// <param name="initialFormat">Initial format. Overrides the foreground and the background color arguments.</param>
        /// <returns>A string that can be written to the console with the parsed markup representations</returns>
        public static string ParseMarkup(string markup, Color? foregroundColor = null, Color? backgroundColor = null, string initialFormat = "") =>
            ParseMarkup((WideString)markup, foregroundColor, backgroundColor, initialFormat);

        /// <summary>
        /// Parses the markup representation
        /// </summary>
        /// <param name="markup">Markup representation</param>
        /// <param name="foregroundColor">Foreground color of the initial markup color. Overrides the initial format argument.</param>
        /// <param name="backgroundColor">Background color of the initial markup color. Overrides the initial format argument.</param>
        /// <param name="initialFormat">Initial format. Overrides the foreground and the background color arguments.</param>
        /// <returns>A string that can be written to the console with the parsed markup representations</returns>
        public static string ParseMarkup(WideString markup, Color? foregroundColor = null, Color? backgroundColor = null, string initialFormat = "")
        {
            // Markup is inspired by BBCode and can have escaped starting and ending sequence characters.
            var finalResult = new StringBuilder(markup);
            MarkupInfo[] sequences = GetMarkupInfo(markup);

            // Now that we've got properties, split them to process them
            Dictionary<string, IEffect> effectNames = effects.ToDictionary((effect) => effect.MarkupTag, (effect) => effect);
            bool useInitialFormat = !string.IsNullOrEmpty(initialFormat);
            if (useInitialFormat && VtSequenceTools.FilterVTSequences(initialFormat).Length != 0)
            {
                ConsoleLogger.Warning("Formatting should not have anything other than VT sequences");
                throw new TerminauxException(LanguageTools.GetLocalized("T_WRITER_CYCLICWRITERS_RENDERER_MARKUP_EXCEPTION_FORMATONLYVT"));
            }
            string finalFormat =
                useInitialFormat ? initialFormat :
                $"{(foregroundColor is not null ? ConsoleColoring.RenderSetConsoleColor(foregroundColor) : "")}" +
                $"{(backgroundColor is not null ? ConsoleColoring.RenderSetConsoleColor(backgroundColor, true) : "")}";
            string resetAll = (TermInfoDesc.Current?.ExitAttributeMode?.Value ?? "") + finalFormat;
            for (int i = finalResult.Length - 1; i >= 0; i--)
            {
                for (int seq = 0; seq < sequences.Length; seq++)
                {
                    MarkupInfo? sequence = sequences[seq];
                    bool isEntrance = i == sequence.entranceIndex;
                    bool isExit = i == sequence.exitIndex && !sequence.isEscape;
                    if (isEntrance || isExit)
                    {
                        // Check to see if this is an escape character or not
                        if (sequence.isEscape)
                        {
                            finalResult.Remove(i, 2);
                            finalResult.Insert(i, sequence.represent);
                            break;
                        }

                        // Remove the representation
                        finalResult.Remove(i, isExit ? "[/]".Length : sequence.represent.Length);

                        // Process representations and form valid sequences from them
                        string representation =
                            isExit ?
                                seq > 0 && sequence.nestLevel != sequences[seq - 1].nestLevel ?
                                sequences[seq - 1].represent :
                                resetAll :
                            sequence.represent;
                        string[] representations = representation.RemovePrefix("[").RemoveSuffix("]").ToString().Split(' ');
                        var representationBuilder = new StringBuilder(isExit ? resetAll : "");
                        if (representation != resetAll)
                        {
                            foreach (string part in representations)
                            {
                                if (string.IsNullOrWhiteSpace(part))
                                    continue;
                                ConsoleLogger.Warning("Adding part {0}", part);
                                if (effectNames.TryGetValue(part, out var effect))
                                    representationBuilder.Append(effect.EffectStart);
                                else if (ColorTools.TryParseColor(part))
                                    representationBuilder.Append(ConsoleColoring.RenderSetConsoleColor(part));
                            }
                        }

                        // Add the result
                        finalResult.Insert(i, representationBuilder.ToString());
                        break;
                    }
                    else
                        continue;
                }
            }
            finalResult.Insert(0, finalFormat);
            return finalResult.ToString();
        }

        /// <summary>
        /// Parses the markup representation
        /// </summary>
        /// <param name="mark">Markup instance</param>
        /// <param name="foregroundColor">Foreground color of the initial markup color. Overrides the initial format argument.</param>
        /// <param name="backgroundColor">Background color of the initial markup color. Overrides the initial format argument.</param>
        /// <param name="initialFormat">Initial format. Overrides the foreground and the background color arguments.</param>
        /// <returns>A string that can be written to the console with the parsed markup representations</returns>
        public static bool TryParseMarkup(Mark? mark, Color? foregroundColor = null, Color? backgroundColor = null, string initialFormat = "")
        {
            if (mark is null)
                return false;
            return TryParseMarkup(mark.Markup, foregroundColor, backgroundColor, initialFormat);
        }

        /// <summary>
        /// Parses the markup representation
        /// </summary>
        /// <param name="markup">Markup representation</param>
        /// <param name="foregroundColor">Foreground color of the initial markup color. Overrides the initial format argument.</param>
        /// <param name="backgroundColor">Background color of the initial markup color. Overrides the initial format argument.</param>
        /// <param name="initialFormat">Initial format. Overrides the foreground and the background color arguments.</param>
        /// <returns>A string that can be written to the console with the parsed markup representations</returns>
        public static bool TryParseMarkup(string markup, Color? foregroundColor = null, Color? backgroundColor = null, string initialFormat = "") =>
            TryParseMarkup((WideString)markup, foregroundColor, backgroundColor, initialFormat);

        /// <summary>
        /// Parses the markup representation
        /// </summary>
        /// <param name="markup">Markup representation</param>
        /// <param name="foregroundColor">Foreground color of the initial markup color. Overrides the initial format argument.</param>
        /// <param name="backgroundColor">Background color of the initial markup color. Overrides the initial format argument.</param>
        /// <param name="initialFormat">Initial format. Overrides the foreground and the background color arguments.</param>
        /// <returns>A string that can be written to the console with the parsed markup representations</returns>
        public static bool TryParseMarkup(WideString markup, Color? foregroundColor = null, Color? backgroundColor = null, string initialFormat = "")
        {
            try
            {
                ParseMarkup(markup, foregroundColor, backgroundColor, initialFormat);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
