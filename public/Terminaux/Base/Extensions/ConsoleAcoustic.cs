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

using Newtonsoft.Json;
using System.Threading;
using Terminaux.Base.Extensions.Data;
using Textify.General;

namespace Terminaux.Base.Extensions
{
    /// <summary>
    /// Sound-related extensions for console
    /// </summary>
    public static class ConsoleAcoustic
    {
        private static object synthLock = new();

        /// <summary>
        /// Gets synth information from the synth JSON representation
        /// </summary>
        /// <param name="synthJson">Synth JSON representation</param>
        /// <returns>An output <see cref="SynthInfo"/> class that contains the necessary information</returns>
        /// <exception cref="TerminauxException"></exception>
        public static SynthInfo GetSynthInfo(string synthJson)
        {
            var synthInfo = JsonConvert.DeserializeObject<SynthInfo>(synthJson) ??
                throw new TerminauxException(LanguageTools.GetLocalized("T_CE_ACOUSTIC_EXCEPTION_NOSYNTHINFO"));
            return synthInfo;
        }

        /// <summary>
        /// Plays the beep synth
        /// </summary>
        /// <param name="synthInfo">Synth info. You can obtain this from <see cref="GetSynthInfo(string)"/>.</param>
        /// <exception cref="TerminauxException"></exception>
        public static void PlaySynth(SynthInfo synthInfo)
        {
            lock (synthLock)
            {
                for (int i = 0; i < synthInfo.Chapters.Length; i++)
                {
                    SynthInfo.Chapter chapter = synthInfo.Chapters[i];
                    for (int j = 0; j < chapter.Synths.Length; j++)
                    {
                        string synth = chapter.Synths[j];
                        var split = synth.Split(' ');
                        if (split.Length != 2)
                            throw new TerminauxException(LanguageTools.GetLocalized("T_CE_ACOUSTIC_EXCEPTION_INVALIDSYNTH").FormatString(i + 1, j + 1));
                        if (!int.TryParse(split[0], out int freq))
                            throw new TerminauxException($LanguageTools.GetLocalized("T_CE_ACOUSTIC_EXCEPTION_INVALIDFREQUENCY").FormatString(i + 1, j + 1));
                        if (!int.TryParse(split[1], out int ms))
                            throw new TerminauxException($LanguageTools.GetLocalized("T_CE_ACOUSTIC_EXCEPTION_INVALIDDURATION").FormatString(i + 1, j + 1));
                        if (freq == 0)
                            Thread.Sleep(ms);
                        else
                            ConsoleWrapper.Beep(freq, ms);
                    }
                }
            }
        }
    }
}
