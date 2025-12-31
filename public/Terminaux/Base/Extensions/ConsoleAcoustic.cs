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

using BassBoom.Basolia.Independent;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Terminaux.Base.Extensions.Data;
using Terminaux.Inputs;
using Terminaux.Reader;
using Textify.General;

namespace Terminaux.Base.Extensions
{
    /// <summary>
    /// Sound-related extensions for console
    /// </summary>
    public static class ConsoleAcoustic
    {
        internal static bool cueSupported = true;
        private static readonly object synthLock = new();

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
            ConsoleLogger.Debug("Synth: {0} (JSON {1} bytes)", synthInfo.Name, synthJson.Length);
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
                    ConsoleLogger.Debug("Chapter index {0}: {1}", i, chapter.Name);
                    for (int j = 0; j < chapter.Synths.Length; j++)
                    {
                        string synth = chapter.Synths[j];
                        var split = synth.Split(' ');
                        ConsoleLogger.Debug("Synth split to {0} elements", split.Length);
                        if (split.Length != 2)
                            throw new TerminauxException(LanguageTools.GetLocalized("T_CE_ACOUSTIC_EXCEPTION_INVALIDSYNTH").FormatString(i + 1, j + 1));
                        ConsoleLogger.Debug("Frequency is {0} (unparsed)", split[0]);
                        if (!int.TryParse(split[0], out int freq))
                            throw new TerminauxException(LanguageTools.GetLocalized("T_CE_ACOUSTIC_EXCEPTION_INVALIDFREQUENCY").FormatString(i + 1, j + 1));
                        ConsoleLogger.Debug("Delay is {0} (unparsed)", split[1]);
                        if (!int.TryParse(split[1], out int ms))
                            throw new TerminauxException(LanguageTools.GetLocalized("T_CE_ACOUSTIC_EXCEPTION_INVALIDDURATION").FormatString(i + 1, j + 1));
                        if (freq == 0)
                        {
                            ConsoleLogger.Debug("Sleeping for {0} ms...", ms);
                            Thread.Sleep(ms);
                        }
                        else
                        {
                            ConsoleLogger.Debug("Playing at {0} Hz for {1} ms...", freq, ms);
                            ConsoleWrapper.Beep(freq, ms);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Plays the keyboard audio cue (requires BassBoom)
        /// </summary>
        /// <param name="settings">Reader settings that has cue configuration. If <see langword="null"/>, the global input cue settings is used.</param>
        /// <param name="audioCue">Keyboard audio cue.</param>
        public static void PlayKeyAudioCue(TermReaderSettings? settings, ConsoleKeyAudioCue audioCue)
        {
            if (Input.KeyboardCues && cueSupported)
            {
                try
                {
                    // Choose the cue stream
                    var cueStream =
                        audioCue == ConsoleKeyAudioCue.Enter ? (settings is not null ? settings.CueEnter : Input.CueEnter) :
                        audioCue == ConsoleKeyAudioCue.Backspace ? (settings is not null ? settings.CueRubout : Input.CueRubout) :
                        audioCue == ConsoleKeyAudioCue.Type ? (settings is not null ? settings.CueWrite : Input.CueWrite) : null;
                    if (cueStream is not null)
                    {
                        // Fetch the settings
                        double volume = settings is not null ? settings.CueVolume : Input.CueVolume;
                        bool volumeBoost = settings is not null ? settings.CueVolumeBoost : Input.CueVolumeBoost;

                        // Copy the stream prior to playing
                        ConsoleLogger.Debug("Calling BassBoom to play cue stream of {0} bytes (vol: {1}, boost: {2}, libpath: {3})...", cueStream.Length, volume, volumeBoost, Input.BassBoomLibraryPath);
                        var copiedStream = new MemoryStream();
                        var cueSettings = new PlayForgetSettings(volume, volumeBoost, Input.BassBoomLibraryPath);
                        cueStream.CopyTo(copiedStream);
                        copiedStream.Seek(0, SeekOrigin.Begin);
                        Task.Run(() => PlayForget.PlayStream(copiedStream, cueSettings));
                    }
                }
                catch (Exception ex)
                {
                    ConsoleLogger.Error(ex, $"Handling cue failed. {ex.Message}");
                    cueSupported = false;
                }
            }
        }
    }
}
