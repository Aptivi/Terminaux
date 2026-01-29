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
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Terminaux.Reader;
using System.Collections.ObjectModel;
using Textify.General;
using Terminaux.Base;
using Terminaux.Reader.History;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Prompts;
using Terminaux.Shell.Switches;
using Terminaux.Shell.Shells.Unified;
using System.Threading;
using Terminaux.Writer.ConsoleWriters;
using Textify.Tools;
using Terminaux.Shell.Scripting;
using System.IO;
using Terminaux.Base.Extensions;
using Terminaux.Shell.Commands.ProcessExecution;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Terminaux.Base.Wrappers;
using Terminaux.Shell.Aliases;
using Textify.Tools.Placeholder;
using Textify.Data.Words.Profanity;
using Terminaux.Colors;
using SpecProbe.Software.Platform;
using Terminaux.Themes.Colors;

namespace Terminaux.Shell.Shells
{
    /// <summary>
    /// Base shell module
    /// </summary>
    public static class ShellManager
    {
        internal static List<ShellExecuteInfo> ShellStack = [];
        internal static string lastCommand = "";

        internal readonly static List<CommandInfo> unifiedCommandDict =
        [
            new CommandInfo("addprefix", /* Localizable */ "T_SHELL_UNIFIED_ADDPREFIX_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                        new CommandArgumentPart(true, "prefix", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_PREFIX_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("check", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_CHECK_DESC_PREFIX", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true)
                ], new AddPrefixCommand(), CommandFlags.Hidden),
            
            new CommandInfo("addsuffix", /* Localizable */ "T_SHELL_UNIFIED_ADDSUFFIX_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                        new CommandArgumentPart(true, "suffix", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_SUFFIX_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("check", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_CHECK_DESC_SUFFIX", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true)
                ], new AddSuffixCommand(), CommandFlags.Hidden),

            new CommandInfo("alias", /* Localizable */ "T_SHELL_UNIFIED_ALIAS_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "add", new()
                        {
                            ExactWording = ["add"],
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ALIAS_ARGUMENT_ADD_DESC"
                        }),
                        new CommandArgumentPart(true, "shell", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ALIAS_ARGUMENT_ADD_TYPE_DESC"
                        }),
                        new CommandArgumentPart(true, "alias", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ALIAS_ARGUMENT_ADD_ALIAS_DESC"
                        }),
                        new CommandArgumentPart(true, "cmd", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ALIAS_ARGUMENT_ADD_CMD_DESC"
                        }),
                    ]),
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "rem", new()
                        {
                            ExactWording = ["rem"],
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ALIAS_ARGUMENT_REM_DESC"
                        }),
                        new CommandArgumentPart(true, "shell", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ALIAS_ARGUMENT_ADD_TYPE_DESC"
                        }),
                        new CommandArgumentPart(true, "alias", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ALIAS_ARGUMENT_ADD_ALIAS_DESC"
                        }),
                    ]),
                ], new AliasCommand()),

            new CommandInfo("bgcolor", /* Localizable */ "T_SHELL_UNIFIED_BGCOLOR_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "specifier", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_SPECIFIER_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("plain", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_PLAIN_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("truevt", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_TRUEVT_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true)
                ], new BgColorCommand(), CommandFlags.Hidden),

            new CommandInfo("chkprefix", /* Localizable */ "T_SHELL_UNIFIED_CHKPREFIX_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                        new CommandArgumentPart(true, "prefix", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_PREFIX_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true)
                ], new ChkPrefixCommand(), CommandFlags.Hidden),

            new CommandInfo("chksuffix", /* Localizable */ "T_SHELL_UNIFIED_CHKSUFFIX_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                        new CommandArgumentPart(true, "suffix", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_SUFFIX_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true)
                ], new ChkSuffixCommand(), CommandFlags.Hidden),

            new CommandInfo("choice", /* Localizable */ "T_SHELL_UNIFIED_CHOICE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "answers", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_CHOICE_ARGUMENT_ANSWERS_DESC"
                        }),
                        new CommandArgumentPart(true, "input", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_CHOICE_ARGUMENT_INPUT_DESC"
                        }),
                        new CommandArgumentPart(false, "answertitle1", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_CHOICE_ARGUMENT_TITLE1_DESC"
                        }),
                        new CommandArgumentPart(false, "answertitle2", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_CHOICE_ARGUMENT_TITLE2_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("o", /* Localizable */ "T_SHELL_UNIFIED_CHOICE_SWITCH_O_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["t", "m"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("t", /* Localizable */ "T_SHELL_UNIFIED_CHOICE_SWITCH_T_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["o", "m"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("m", /* Localizable */ "T_SHELL_UNIFIED_CHOICE_SWITCH_M_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["t", "o"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("single", /* Localizable */ "T_SHELL_UNIFIED_CHOICE_SWITCH_SINGLE_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["multiple"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("multiple", /* Localizable */ "T_SHELL_UNIFIED_CHOICE_SWITCH_MULTIPLE_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["single"],
                            AcceptsValues = false
                        })
                    ], true, true)
                ], new ChoiceCommand()),

            new CommandInfo("cls", /* Localizable */ "T_SHELL_UNIFIED_CLS_DESC", new ClsCommand()),

            new CommandInfo("consoledimensions", /* Localizable */ "T_SHELL_UNIFIED_CONSOLEDIMENSIONS_DESC",
                [
                    new CommandArgumentInfo(true)
                ], new ConsoleDimensionsCommand(), CommandFlags.Hidden),

            new CommandInfo("containsallchars", /* Localizable */ "T_SHELL_UNIFIED_CONTAINSALLCHARS_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                        new CommandArgumentPart(true, "charseq", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_CHARSEQ_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true)
                ], new ContainsAllCharsCommand(), CommandFlags.Hidden),

            new CommandInfo("containsallstrs", /* Localizable */ "T_SHELL_UNIFIED_CONTAINSALLSTRS_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                        new CommandArgumentPart(true, "strseq", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_STRSEQ_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true, true)
                ], new ContainsAllStrsCommand(), CommandFlags.Hidden),

            new CommandInfo("containsanychars", /* Localizable */ "T_SHELL_UNIFIED_CONTAINSANYCHARS_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                        new CommandArgumentPart(true, "charseq", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_CHARSEQ_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true)
                ], new ContainsAnyCharsCommand(), CommandFlags.Hidden),

            new CommandInfo("containsanystrs", /* Localizable */ "T_SHELL_UNIFIED_CONTAINSANYSTRS_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                        new CommandArgumentPart(true, "strseq", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_STRSEQ_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true, true)
                ], new ContainsAnyStrsCommand(), CommandFlags.Hidden),

            new CommandInfo("contains", /* Localizable */ "T_SHELL_UNIFIED_CONTAINS_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                        new CommandArgumentPart(true, "substr", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_SUBSTR_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("ignorecase", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_IGNORECASE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ])
                ], new ContainsCommand(), CommandFlags.Hidden),

            new CommandInfo("degrees", /* Localizable */ "T_SHELL_UNIFIED_DEGREES_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "radians", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_DEGREES_ARGUMENT_RADIANS_DESC",
                            IsNumeric = true,
                        }),
                    ],
                    [
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ])
                ], new DegreesCommand()),

            new CommandInfo("decodebase64", /* Localizable */ "T_SHELL_UNIFIED_DECODEBASE64_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "encoded", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_DECODEBASE64_ARGUMENT_ENCODED_DESC"
                        })
                    ])
                ], new DecodeBase64Command(), CommandFlags.Hidden),

            new CommandInfo("dpi", /* Localizable */ "T_SHELL_UNIFIED_DPI_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "pixels", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_DPI_ARGUMENT_PIXELS_DESC",
                            IsNumeric = true,
                        }),
                        new CommandArgumentPart(true, "lengthinches", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_DPI_ARGUMENT_LENGTHINCHES_DESC",
                            IsNumeric = true,
                        })
                    ],
                    [
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ])
                ], new DpiCommand()),

            new CommandInfo("pixels", /* Localizable */ "T_SHELL_UNIFIED_PIXELS_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "width", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_PIXELS_ARGUMENT_WIDTH_DESC",
                            IsNumeric = true,
                        }),
                        new CommandArgumentPart(true, "height", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_PIXELS_ARGUMENT_HEIGHT_DESC",
                            IsNumeric = true,
                        }),
                    ],
                    [
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ])
                ], new PixelsCommand()),

            new CommandInfo("echo", /* Localizable */ "T_SHELL_UNIFIED_ECHO_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ECHO_ARGUMENT_TEXT_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("noparse", /* Localizable */ "T_SHELL_UNIFIED_ECHO_SWITCH_NOPARSE_DESC", false, false, [], 0, false)
                    ], true)
                ], new EchoCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("encodebase64", /* Localizable */ "T_SHELL_UNIFIED_ENCODEBASE64_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "string", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ENCODEBASE64_ARGUMENT_STRING_DESC"
                        })
                    ])
                ], new EncodeBase64Command(), CommandFlags.Hidden),

            new CommandInfo("endswithallchars", /* Localizable */ "T_SHELL_UNIFIED_ENDSWITHALLCHARS_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                        new CommandArgumentPart(true, "charseq", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_CHARSEQ_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true)
                ], new EndsWithAllCharsCommand(), CommandFlags.Hidden),

            new CommandInfo("endswithallstrs", /* Localizable */ "T_SHELL_UNIFIED_ENDSWITHALLSTRS_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                        new CommandArgumentPart(true, "strseq", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_STRSEQ_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true)
                ], new EndsWithAllStrsCommand(), CommandFlags.Hidden),

            new CommandInfo("endswithanychars", /* Localizable */ "T_SHELL_UNIFIED_ENDSWITHANYCHARS_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                        new CommandArgumentPart(true, "charseq", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_CHARSEQ_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true)
                ], new EndsWithAnyCharsCommand(), CommandFlags.Hidden),

            new CommandInfo("endswithanystrs", /* Localizable */ "T_SHELL_UNIFIED_ENDSWITHANYSTRS_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                        new CommandArgumentPart(true, "strseq", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_STRSEQ_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true)
                ], new EndsWithAnyStrsCommand(), CommandFlags.Hidden),

            new CommandInfo("endswith", /* Localizable */ "T_SHELL_UNIFIED_ENDSWITH_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                        new CommandArgumentPart(true, "substr", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_SUBSTR_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("ignorecase", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_IGNORECASE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ])
                ], new EndsWithCommand(), CommandFlags.Hidden),

            new CommandInfo("equals", /* Localizable */ "T_SHELL_UNIFIED_EQUALS_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                        new CommandArgumentPart(true, "targetstr", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TARGETSTR_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("ignorecase", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_IGNORECASE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ])
                ], new EqualsCommand(), CommandFlags.Hidden),

            new CommandInfo("exec", /* Localizable */ "T_SHELL_UNIFIED_EXEC_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "process", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_EXEC_ARGUMENT_PATH_DESC"
                        }),
                        new CommandArgumentPart(false, "args", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_EXEC_ARGUMENT_ARGS_DESC"
                        })
                    ],
                    [
                        new SwitchInfo("forked", /* Localizable */ "T_SHELL_UNIFIED_EXEC_SWITCH_FORKED_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new ExecUnifiedCommand()),

            new CommandInfo("exit", /* Localizable */ "T_SHELL_UNIFIED_EXIT_HELP_DESC", new ExitUnifiedCommand()),

            new CommandInfo("fgcolor", /* Localizable */ "T_SHELL_UNIFIED_FGCOLOR_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "specifier", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_SPECIFIER_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("plain", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_PLAIN_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("truevt", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_TRUEVT_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true)
                ], new FgColorCommand(), CommandFlags.Hidden),

            new CommandInfo("findcmds", /* Localizable */ "T_SHELL_UNIFIED_FINDCMDS_HELP_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "search", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_FINDCMDS_ARGUMENT_SWITCH_DESC"
                        })
                    ], false)
                ], new FindCmdsUnifiedCommand()),

            new CommandInfo("firstnames", /* Localizable */ "T_SHELL_UNIFIED_FIRSTNAMES_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "term", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_FIRSTNAMES_ARGUMENT_TERM_DESC"
                        }),
                        new CommandArgumentPart(false, "nameprefix", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_FIRSTNAMES_ARGUMENT_NAMEPREFIX_DESC"
                        }),
                        new CommandArgumentPart(false, "namesuffix", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_FIRSTNAMES_ARGUMENT_NAMESUFFIX_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("t", /* Localizable */ "T_SHELL_UNIFIED_FIRSTNAMES_SWITCH_T_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("male", /* Localizable */ "T_SHELL_UNIFIED_FIRSTNAMES_SWITCH_MALE_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["female", "both"],
                            AcceptsValues = false,
                        }),
                        new SwitchInfo("female", /* Localizable */ "T_SHELL_UNIFIED_FIRSTNAMES_SWITCH_FEMALE_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["male", "both"],
                            AcceptsValues = false,
                        }),
                        new SwitchInfo("both", /* Localizable */ "T_SHELL_UNIFIED_FIRSTNAMES_SWITCH_UNIFIED_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["female", "male"],
                            AcceptsValues = false,
                        }),
                    ], true)
                ], new FirstNamesCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable | CommandFlags.Hidden),

            new CommandInfo("genfirstnames", /* Localizable */ "T_SHELL_UNIFIED_GENFIRSTNAMES_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "firstnamescount", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_GENFIRSTNAMES_ARGUMENT_FIRSTNAMESCOUNT_DESC"
                        }),
                        new CommandArgumentPart(false, "nameprefix", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_FIRSTNAMES_ARGUMENT_NAMEPREFIX_DESC"
                        }),
                        new CommandArgumentPart(false, "namesuffix", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_FIRSTNAMES_ARGUMENT_NAMESUFFIX_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("male", /* Localizable */ "T_SHELL_UNIFIED_FIRSTNAMES_SWITCH_MALE_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["female", "both"],
                            AcceptsValues = false,
                        }),
                        new SwitchInfo("female", /* Localizable */ "T_SHELL_UNIFIED_FIRSTNAMES_SWITCH_FEMALE_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["male", "both"],
                            AcceptsValues = false,
                        }),
                        new SwitchInfo("both", /* Localizable */ "T_SHELL_UNIFIED_FIRSTNAMES_SWITCH_UNIFIED_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["female", "male"],
                            AcceptsValues = false,
                        }),
                    ], true)
                ], new GenFirstNamesCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable | CommandFlags.Hidden),

            new CommandInfo("genlastnames", /* Localizable */ "T_SHELL_UNIFIED_GENLASTNAMES_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "surnamescount", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_GENLASTNAMES_ARGUMENT_SURNAMESCOUNT_DESC"
                        }),
                        new CommandArgumentPart(false, "surnameprefix", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_LASTNAMES_ARGUMENT_SURNAMEPREFIX_DESC"
                        }),
                        new CommandArgumentPart(false, "surnamesuffix", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_LASTNAMES_ARGUMENT_SURNAMESUFFIX_DESC"
                        }),
                    ], true)
                ], new GenLastNamesCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable | CommandFlags.Hidden),

            new CommandInfo("gennames", /* Localizable */ "T_SHELL_UNIFIED_GENNAMES_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "namescount", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_GENNAMES_ARGUMENT_NAMESCOUNT_DESC"
                        }),
                        new CommandArgumentPart(false, "nameprefix", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_FIRSTNAMES_ARGUMENT_NAMEPREFIX_DESC"
                        }),
                        new CommandArgumentPart(false, "namesuffix", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_FIRSTNAMES_ARGUMENT_NAMESUFFIX_DESC"
                        }),
                        new CommandArgumentPart(false, "surnameprefix", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_LASTNAMES_ARGUMENT_SURNAMEPREFIX_DESC"
                        }),
                        new CommandArgumentPart(false, "surnamesuffix", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_LASTNAMES_ARGUMENT_SURNAMESUFFIX_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("t", /* Localizable */ "T_SHELL_UNIFIED_FIRSTNAMES_SWITCH_T_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("male", /* Localizable */ "T_SHELL_UNIFIED_FIRSTNAMES_SWITCH_MALE_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["female", "both"],
                            AcceptsValues = false,
                        }),
                        new SwitchInfo("female", /* Localizable */ "T_SHELL_UNIFIED_FIRSTNAMES_SWITCH_FEMALE_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["male", "both"],
                            AcceptsValues = false,
                        }),
                        new SwitchInfo("both", /* Localizable */ "T_SHELL_UNIFIED_FIRSTNAMES_SWITCH_UNIFIED_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["female", "male"],
                            AcceptsValues = false,
                        }),
                    ], true)
                ], new GenNamesCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable | CommandFlags.Hidden),

            new CommandInfo("fork", /* Localizable */ "T_SHELL_UNIFIED_FORK_DESC", new ForkCommand()),

            new CommandInfo("help", /* Localizable */ "T_SHELL_UNIFIED_HELP_HELP_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "command", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => CommandManager.GetCommandNames(CurrentShellType),
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_HELP_ARGUMENT_COMMAND_DESC"
                        })
                    ],
                    [
                        new SwitchInfo("general", /* Localizable */ "T_SHELL_UNIFIED_HELP_GENERAL_SWITCH_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("alias", /* Localizable */ "T_SHELL_UNIFIED_HELP_ALIAS_SWITCH_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("unified", /* Localizable */ "T_SHELL_UNIFIED_HELP_UNIFIED_SWITCH_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("extra", /* Localizable */ "T_SHELL_UNIFIED_HELP_EXTRA_SWITCH_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("all", /* Localizable */ "T_SHELL_UNIFIED_HELP_ALL_SWITCH_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("simplified", /* Localizable */ "T_SHELL_UNIFIED_HELP_SIMPLIFIED_SWITCH_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("hidden", /* Localizable */ "T_SHELL_UNIFIED_HELP_HIDDEN_SWITCH_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("count", /* Localizable */ "T_SHELL_UNIFIED_HELP_COUNT_SWITCH_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("markdown", /* Localizable */ "T_SHELL_UNIFIED_HELP_MARKDOWN_SWITCH_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], false)
                ], new HelpUnifiedCommand(), CommandFlags.Wrappable),

            new CommandInfo("history", /* Localizable */ "T_SHELL_UNIFIED_HISTORY_DESC",
                [
                    new CommandArgumentInfo(true)
                ], new HistoryCommand(), CommandFlags.Hidden),

            new CommandInfo("if", /* Localizable */ "T_SHELL_UNIFIED_IF_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "MESHExpression", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_IF_ARGUMENT_MESHEXPRESSION_DESC"
                        }),
                        new CommandArgumentPart(true, "command", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_IF_ARGUMENT_COMMAND_DESC"
                        }),
                    ])
                ], new IfCommand()),

            new CommandInfo("input", /* Localizable */ "T_SHELL_UNIFIED_INPUT_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "question", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_CHOICE_ARGUMENT_INPUT_DESC"
                        }),
                    ], true)
                ], new InputCommand()),

            new CommandInfo("inputpass", /* Localizable */ "T_SHELL_UNIFIED_INPUTPASS_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "question", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_CHOICE_ARGUMENT_INPUT_DESC"
                        }),
                    ], true)
                ], new InputPassCommand()),

            new CommandInfo("lastnames", /* Localizable */ "T_SHELL_UNIFIED_LASTNAMES_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "term", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TERM_DESC"
                        }),
                        new CommandArgumentPart(false, "surnameprefix", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_LASTNAMES_ARGUMENT_SURNAMEPREFIX_DESC"
                        }),
                        new CommandArgumentPart(false, "surnamesuffix", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_LASTNAMES_ARGUMENT_SURNAMESUFFIX_DESC"
                        }),
                    ], true)
                ], new LastNamesCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable | CommandFlags.Hidden),

            new CommandInfo("lintscript", /* Localizable */ "T_SHELL_UNIFIED_LINTSCRIPT_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "script", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_LINTSCRIPT_ARGUMENT_SCRIPT_DESC"
                        }),
                    ], true)
                ], new LintScriptCommand()),

            new CommandInfo("loadhistories", /* Localizable */ "T_SHELL_UNIFIED_LOADHISTORIES_DESC", new LoadHistoriesUnifiedCommand()),

            new CommandInfo("lowercase", /* Localizable */ "T_SHELL_UNIFIED_LOWERCASE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("whole", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_CASING_WHOLE_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["first"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("first", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_CASING_FIRST_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["whole"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ])
                ], new LowercaseCommand(), CommandFlags.Hidden),

            new CommandInfo("lsvars", /* Localizable */ "T_SHELL_UNIFIED_LSVARS_DESC", new LsVarsCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("now", /* Localizable */ "T_SHELL_UNIFIED_SHOWTD_DESC",
                [
                    new CommandArgumentInfo([
                        new SwitchInfo("date", /* Localizable */ "T_SHELL_UNIFIED_DATE_SWITCH_DATE_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["time", "full"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("time", /* Localizable */ "T_SHELL_UNIFIED_DATE_SWITCH_TIME_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["date", "full"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("full", /* Localizable */ "T_SHELL_UNIFIED_SHOWTD_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["date", "time"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("utc", /* Localizable */ "T_SHELL_UNIFIED_DATE_SWITCH_UTC_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ], true)
                ], new NowCommand(), CommandFlags.RedirectionSupported),

            new CommandInfo("pipe", /* Localizable */ "T_SHELL_UNIFIED_PIPE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "sourceCommand", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => CommandManager.GetCommandNames(CurrentShellType),
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_PIPE_ARGUMENT_SOURCE_DESC"
                        }),
                        new CommandArgumentPart(true, "targetCommand", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => CommandManager.GetCommandNames(CurrentShellType),
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_PIPE_ARGUMENT_TARGET_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("quoted", /* Localizable */ "T_SHELL_UNIFIED_PIPE_SWITCH_QUOTED_DESC")
                    ], true)
                ], new PipeUnifiedCommand()),

            new CommandInfo("presets", /* Localizable */ "T_SHELL_UNIFIED_PRESETS_HELP_DESC", new PresetsUnifiedCommand()),

            new CommandInfo("radians", /* Localizable */ "T_SHELL_UNIFIED_RADIANS_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "degrees", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_RADIANS_ARGUMENT_DEGREES_DESC",
                            IsNumeric = true,
                        }),
                    ],
                    [
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ])
                ], new RadiansCommand()),

            new CommandInfo("remprefix", /* Localizable */ "T_SHELL_UNIFIED_REMPREFIX_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                        new CommandArgumentPart(true, "prefix", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_PREFIX_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ])
                ], new RemPrefixCommand(), CommandFlags.Hidden),

            new CommandInfo("remsuffix", /* Localizable */ "T_SHELL_UNIFIED_REMSUFFIX_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                        new CommandArgumentPart(true, "suffix", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_SUFFIX_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ])
                ], new RemSuffixCommand(), CommandFlags.Hidden),

            new CommandInfo("repeat", /* Localizable */ "T_SHELL_UNIFIED_REPEAT_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "times", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_PIPE_ARGUMENT_TARGET_DESC"
                        }),
                        new CommandArgumentPart(false, "command"),
                    ])
                ], new RepeatUnifiedCommand()),

            new CommandInfo("charwidth", /* Localizable */ "T_SHELL_UNIFIED_CHARWIDTH_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "char", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_CHAR_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true)
                ], new CharWidthCommand(), CommandFlags.Hidden),

            new CommandInfo("strwidth", /* Localizable */ "T_SHELL_UNIFIED_STRWIDTH_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true)
                ], new StrWidthCommand(), CommandFlags.Hidden),

            new CommandInfo("wrapsentences", /* Localizable */ "T_SHELL_UNIFIED_WRAPSENTENCES_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                        new CommandArgumentPart(true, "width", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_WRAPSENTENCES_ARGUMENT_WIDTH_DESC",
                            IsNumeric = true,
                        }),
                    ],
                    [
                        new SwitchInfo("charwise", /* Localizable */ "T_SHELL_UNIFIED_WRAPSENTENCES_SWITCH_CHARWISE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true)
                ], new WrapSentencesCommand(), CommandFlags.Hidden),

            new CommandInfo("palindrome", /* Localizable */ "T_SHELL_UNIFIED_PALINDROME_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("ignorecase", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_IGNORECASE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true)
                ], new PalindromeCommand(), CommandFlags.Hidden),

            new CommandInfo("numeric", /* Localizable */ "T_SHELL_UNIFIED_NUMERIC_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true)
                ], new NumericCommand(), CommandFlags.Hidden),

            new CommandInfo("printf", /* Localizable */ "T_SHELL_UNIFIED_PRINTF_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                        new CommandArgumentPart(false, "parameters", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_PRINTF_ARGUMENT_PARAMETERS_DESC"
                        }),
                    ], true, true)
                ], new PrintFCommand(), CommandFlags.Hidden),

            new CommandInfo("replace", /* Localizable */ "T_SHELL_UNIFIED_REPLACE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                        new CommandArgumentPart(true, "toreplace", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_REPLACE_ARGUMENT_TOREPLACE_DESC"
                        }),
                        new CommandArgumentPart(true, "replacewith", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_REPLACEWITH_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("regex", /* Localizable */ "T_SHELL_UNIFIED_REPLACE_SWITCH_REGEX_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true)
                ], new ReplaceCommand(), CommandFlags.Hidden),

            new CommandInfo("replaceall", /* Localizable */ "T_SHELL_UNIFIED_REPLACEALL_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                        new CommandArgumentPart(true, "replacewith", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_REPLACEWITH_DESC"
                        }),
                        new CommandArgumentPart(true, "toreplace", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_REPLACEALL_ARGUMENT_TOREPLACE_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true, true)
                ], new ReplaceAllCommand(), CommandFlags.Hidden),

            new CommandInfo("replaceallrange", /* Localizable */ "T_SHELL_UNIFIED_REPLACEALLRANGE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                        new CommandArgumentPart(true, "toreplace", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_REPLACEALLRANGE_ARGUMENT_TOREPLACE_DESC"
                        }),
                        new CommandArgumentPart(true, "strings", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_STRINGS_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true, true)
                ], new ReplaceAllRangeCommand(), CommandFlags.Hidden),

            new CommandInfo("replacechar", /* Localizable */ "T_SHELL_UNIFIED_REPLACECHAR_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                        new CommandArgumentPart(true, "charIdx", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_CHARIDX_DESC",
                            IsNumeric = true,
                        }),
                        new CommandArgumentPart(true, "toreplace", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_REPLACECHAR_ARGUMENT_TOREPLACE_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true)
                ], new ReplaceCharCommand(), CommandFlags.Hidden),

            new CommandInfo("replacelastocc", /* Localizable */ "T_SHELL_UNIFIED_REPLACELASTOCC_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                        new CommandArgumentPart(true, "toreplace", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_REPLACE_ARGUMENT_TOREPLACE_DESC"
                        }),
                        new CommandArgumentPart(true, "replacewith", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_REPLACEWITH_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true)
                ], new ReplaceLastOccCommand(), CommandFlags.Hidden),

            new CommandInfo("replaceocc", /* Localizable */ "T_SHELL_UNIFIED_REPLACEOCC_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                        new CommandArgumentPart(true, "toreplace", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_REPLACE_ARGUMENT_TOREPLACE_DESC"
                        }),
                        new CommandArgumentPart(true, "replacewith", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_REPLACEWITH_DESC"
                        }),
                        new CommandArgumentPart(true, "occurrencenum", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_REPLACEOCC_ARGUMENT_OCCURRENCENUM_DESC",
                            IsNumeric = true,
                        }),
                    ],
                    [
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true)
                ], new ReplaceOccCommand(), CommandFlags.Hidden),

            new CommandInfo("reverse", /* Localizable */ "T_SHELL_UNIFIED_REVERSE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true)
                ], new ReverseCommand(), CommandFlags.Hidden),

            new CommandInfo("shift", /* Localizable */ "T_SHELL_UNIFIED_SHIFT_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                        new CommandArgumentPart(true, "shift", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_SHIFT_ARGUMENT_SHIFT_DESC",
                            IsNumeric = true,
                        }),
                    ],
                    [
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true)
                ], new ShiftCommand(), CommandFlags.Hidden),

            new CommandInfo("split", /* Localizable */ "T_SHELL_UNIFIED_SPLIT_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                        new CommandArgumentPart(false, "delimiter", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_SPLIT_ARGUMENT_DELIMITER_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true)
                ], new SplitCommand(), CommandFlags.Hidden),

            new CommandInfo("splitnewlines", /* Localizable */ "T_SHELL_UNIFIED_SPLITNEWLINES_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true)
                ], new SplitNewLinesCommand(), CommandFlags.Hidden),

            new CommandInfo("splitdq", /* Localizable */ "T_SHELL_UNIFIED_SPLITDQ_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                        new CommandArgumentPart(false, "delimiter", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_SPLIT_ARGUMENT_DELIMITER_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true)
                ], new SplitdqCommand(), CommandFlags.Hidden),

            new CommandInfo("truncate", /* Localizable */ "T_SHELL_UNIFIED_TRUNCATE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                        new CommandArgumentPart(true, "threshold", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_TRUNCATE_ARGUMENT_THRESHOLD_DESC",
                            IsNumeric = true,
                        }),
                    ],
                    [
                        new SwitchInfo("noellipsis", /* Localizable */ "T_SHELL_UNIFIED_TRUNCATE_SWITCH_NOELLIPSIS_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true)
                ], new TruncateCommand(), CommandFlags.Hidden),

            new CommandInfo("escapestr", /* Localizable */ "T_SHELL_UNIFIED_ESCAPESTR_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true)
                ], new EscapeStrCommand(), CommandFlags.Hidden),

            new CommandInfo("unescapestr", /* Localizable */ "T_SHELL_UNIFIED_UNESCAPESTR_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true)
                ], new UnescapeStrCommand(), CommandFlags.Hidden),

            new CommandInfo("tobraille", /* Localizable */ "T_SHELL_UNIFIED_TOBRAILLE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("quiet", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_QUIET_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true)
                ], new ToBrailleCommand(), CommandFlags.Hidden),

            new CommandInfo("tofiglet", /* Localizable */ "T_SHELL_UNIFIED_TOFIGLET_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                        new CommandArgumentPart(false, "font", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_TOFIGLET_ARGUMENT_FONT_DESC"
                        }),
                        new CommandArgumentPart(false, "width", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_TOFIGLET_ARGUMENT_WIDTH_DESC",
                            IsNumeric = true,
                        }),
                    ],
                    [
                        new SwitchInfo("quiet", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_QUIET_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true)
                ], new ToFigletCommand(), CommandFlags.Hidden),

            new CommandInfo("figletmetrics", /* Localizable */ "T_SHELL_UNIFIED_FIGLETMETRICS_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                        new CommandArgumentPart(false, "font", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_TOFIGLET_ARGUMENT_FONT_DESC"
                        }),
                        new CommandArgumentPart(false, "width", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_TOFIGLET_ARGUMENT_WIDTH_DESC",
                            IsNumeric = true,
                        }),
                    ],
                    [
                        new SwitchInfo("detailed", /* Localizable */ "T_SHELL_UNIFIED_FIGLETMETRICS_SWITCH_DETAILED_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true)
                ], new FigletMetricsCommand(), CommandFlags.Hidden),

            new CommandInfo("reversertl", /* Localizable */ "T_SHELL_UNIFIED_REVERSERTL_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true)
                ], new ReverseRtlCommand(), CommandFlags.Hidden),

            new CommandInfo("randomword", /* Localizable */ "T_SHELL_UNIFIED_RANDOMWORD_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new SwitchInfo("quiet", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_QUIET_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true)
                ], new RandomWordCommand(), CommandFlags.Hidden),

            new CommandInfo("randomwordcond", /* Localizable */ "T_SHELL_UNIFIED_RANDOMWORDCOND_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "maxLength", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_RANDOMWORDCOND_ARGUMENT_MAXLENGTH_DESC",
                            IsNumeric = true,
                        }),
                        new CommandArgumentPart(false, "startsWith", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_RANDOMWORDCOND_ARGUMENT_STARTSWITH_DESC"
                        }),
                        new CommandArgumentPart(false, "endsWith", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_RANDOMWORDCOND_ARGUMENT_ENDSWITH_DESC"
                        }),
                        new CommandArgumentPart(false, "exactLength", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_RANDOMWORDCOND_ARGUMENT_EXACTLENGTH_DESC",
                            IsNumeric = true,
                        }),
                    ],
                    [
                        new SwitchInfo("quiet", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_QUIET_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true)
                ], new RandomWordCondCommand(), CommandFlags.Hidden),

            new CommandInfo("findprofanity", /* Localizable */ "T_SHELL_UNIFIED_FINDPROFANITY_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                        new CommandArgumentPart(false, "profanitytype", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_FINDPROFANITY_ARGUMENT_PROFANITYTYPE_DESC",
                            AutoCompleter = (_) => Enum.GetNames(typeof(ProfanitySearchType)),
                        }),
                    ],
                    [
                        new SwitchInfo("quiet", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_QUIET_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true)
                ], new FindProfanityCommand(), CommandFlags.Hidden),

            new CommandInfo("filterprofanity", /* Localizable */ "T_SHELL_UNIFIED_FILTERPROFANITY_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                        new CommandArgumentPart(false, "profanitytype", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_FINDPROFANITY_ARGUMENT_PROFANITYTYPE_DESC",
                            AutoCompleter = (_) => Enum.GetNames(typeof(ProfanitySearchType)),
                        }),
                    ],
                    [
                        new SwitchInfo("quiet", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_QUIET_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true)
                ], new FilterProfanityCommand(), CommandFlags.Hidden),

            new CommandInfo("normalizespaces", /* Localizable */ "T_SHELL_UNIFIED_NORMALIZESPACES_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("simple", /* Localizable */ "T_SHELL_UNIFIED_NORMALIZESPACES_SWITCH_SIMPLE_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["analytical"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("analytical", /* Localizable */ "T_SHELL_UNIFIED_NORMALIZESPACES_SWITCH_ANALYTICAL_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["simple"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true)
                ], new NormalizeSpacesCommand(), CommandFlags.Hidden),

            new CommandInfo("savehistories", /* Localizable */ "T_SHELL_UNIFIED_SAVEHISTORIES_DESC", new SaveHistoriesUnifiedCommand()),

            new CommandInfo("select", /* Localizable */ "T_SHELL_UNIFIED_SELECT_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "answers", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_CHOICE_ARGUMENT_ANSWERS_DESC"
                        }),
                        new CommandArgumentPart(true, "input", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_CHOICE_ARGUMENT_INPUT_DESC"
                        }),
                        new CommandArgumentPart(false, "answertitle1", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_CHOICE_ARGUMENT_TITLE1_DESC"
                        }),
                        new CommandArgumentPart(false, "answertitle2", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_CHOICE_ARGUMENT_TITLE2_DESC"
                        }),
                    ], true, true)
                ], new SelectCommand()),

            new CommandInfo("set", /* Localizable */ "T_SHELL_UNIFIED_SET_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "value", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_SET_ARGUMENT_VALUE_DESC"
                        }),
                    ], true)
                ], new SetCommand()),

            new CommandInfo("setrange", /* Localizable */ "T_SHELL_UNIFIED_SETRANGE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "value", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_SETRANGE_ARGUMENT_VALUE1_DESC"
                        }),
                        new CommandArgumentPart(false, "value2", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_SETRANGE_ARGUMENT_VALUE2_DESC"
                        }),
                        new CommandArgumentPart(false, "value3", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_SETRANGE_ARGUMENT_VALUE3_DESC"
                        }),
                    ], true, true)
                ], new SetRangeCommand()),

            new CommandInfo("sleep", /* Localizable */ "T_SHELL_UNIFIED_SLEEP_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "ms", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_SLEEP_ARGUMENT_MS_DESC"
                        }),
                    ])
                ], new SleepCommand()),

            new CommandInfo("startswithallchars", /* Localizable */ "T_SHELL_UNIFIED_STARTSWITHALLCHARS_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                        new CommandArgumentPart(true, "charseq", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_CHARSEQ_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true)
                ], new StartsWithAllCharsCommand(), CommandFlags.Hidden),

            new CommandInfo("startswithallstrs", /* Localizable */ "T_SHELL_UNIFIED_STARTSWITHALLSTRS_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                        new CommandArgumentPart(true, "strseq", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_STRSEQ_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true)
                ], new StartsWithAllStrsCommand(), CommandFlags.Hidden),

            new CommandInfo("startswithanychars", /* Localizable */ "T_SHELL_UNIFIED_STARTSWITHANYCHARS_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                        new CommandArgumentPart(true, "charseq", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_CHARSEQ_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true)
                ], new StartsWithAnyCharsCommand(), CommandFlags.Hidden),

            new CommandInfo("startswithanystrs", /* Localizable */ "T_SHELL_UNIFIED_STARTSWITHANYSTRS_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                        new CommandArgumentPart(true, "strseq", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_STRSEQ_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], true)
                ], new StartsWithAnyStrsCommand(), CommandFlags.Hidden),

            new CommandInfo("startswith", /* Localizable */ "T_SHELL_UNIFIED_STARTSWITH_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                        new CommandArgumentPart(true, "substr", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_SUBSTR_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("ignorecase", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_IGNORECASE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ])
                ], new StartsWithCommand(), CommandFlags.Hidden),

            new CommandInfo("titlecase", /* Localizable */ "T_SHELL_UNIFIED_TITLECASE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ])
                ], new TitlecaseCommand(), CommandFlags.Hidden),

            new CommandInfo("unset", /* Localizable */ "T_SHELL_UNIFIED_UNSET_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "$variable", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_UNSET_ARGUMENT_VARIABLE_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("justwipe", /* Localizable */ "T_SHELL_UNIFIED_UNSET_SWITCH_JUSTWIPE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new UnsetCommand()),

            new CommandInfo("uppercase", /* Localizable */ "T_SHELL_UNIFIED_UPPERCASE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("whole", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_CASING_WHOLE_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["first"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("first", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_CASING_FIRST_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["whole"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ])
                ], new UppercaseCommand(), CommandFlags.Hidden),

            new CommandInfo("wrap", /* Localizable */ "T_SHELL_UNIFIED_WRAP_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "command", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => CommandExecutor.GetWrappableCommands(CurrentShellType),
                            ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_WRAP_ARGUMENT_COMMAND_DESC"
                        })
                    ])
                ], new WrapUnifiedCommand()),
        ];

        internal readonly static Dictionary<string, BaseShellInfo> availableShells = [];

        /// <summary>
        /// List of unified commands
        /// </summary>
        public static CommandInfo[] UnifiedCommands =>
            [.. unifiedCommandDict];

        /// <summary>
        /// List of available shells
        /// </summary>
        public static ReadOnlyDictionary<string, BaseShellInfo> AvailableShells =>
            new(availableShells);

        /// <summary>
        /// Current shell type
        /// </summary>
        public static string CurrentShellType =>
            ShellStack[ShellStack.Count - 1].ShellType;

        /// <summary>
        /// Last shell type
        /// </summary>
        public static string LastShellType
        {
            get
            {
                if (ShellStack.Count == 0)
                {
                    // We don't have any shell. Return Shell.
                    ConsoleLogger.Warning("Trying to call LastShellType on empty shell stack. Assuming MESH...");
                    return "Shell";
                }
                else if (ShellStack.Count == 1)
                {
                    // We only have one shell. Consider current as last.
                    ConsoleLogger.Warning("Trying to call LastShellType on shell stack containing only one shell. Assuming current...");
                    return CurrentShellType;
                }
                else
                {
                    // We have more than one shell. Return the shell type for a shell before the last one.
                    var type = ShellStack[ShellStack.Count - 2].ShellType;
                    ConsoleLogger.Debug("Returning shell type {0} for last shell from the stack...", type);
                    return type;
                }
            }
        }

        /// <summary>
        /// Whether to set the title on command execution
        /// </summary>
        public static bool SetTitle { get; set; }

        /// <summary>
        /// Whether to enable input history
        /// </summary>
        public static bool InputHistoryEnabled { get; set; } = true;

        /// <summary>
        /// Initial console title
        /// </summary>
        public static string InitialTitle { get; set; } = "";

        /// <summary>
        /// Count of shells in a stack
        /// </summary>
        public static int ShellCount =>
            ShellStack.Count;

        /// <summary>
        /// Inputs for command then parses a specified command.
        /// </summary>
        /// <remarks>All shells should use this routine to allow effective and consistent line parsing.</remarks>
        public static void GetLine() =>
            GetLine("", "", CurrentShellType, true, SetTitle);

        /// <summary>
        /// Parses a specified command.
        /// </summary>
        /// <param name="FullCommand">The full command string</param>
        /// <remarks>All shells should use this routine to allow effective and consistent line parsing.</remarks>
        public static void GetLine(string FullCommand) =>
            GetLine(FullCommand, "", CurrentShellType, true, SetTitle);

        /// <summary>
        /// Parses a specified command.
        /// </summary>
        /// <param name="FullCommand">The full command string</param>
        /// <param name="OutputPath">Optional (non-)neutralized output path</param>
        /// <remarks>All shells should use this routine to allow effective and consistent line parsing.</remarks>
        public static void GetLine(string FullCommand, string OutputPath = "") =>
            GetLine(FullCommand, OutputPath, CurrentShellType, true, SetTitle);

        /// <summary>
        /// Parses a specified command.
        /// </summary>
        /// <param name="FullCommand">The full command string</param>
        /// <param name="OutputPath">Optional (non-)neutralized output path</param>
        /// <param name="ShellType">Shell type</param>
        /// <param name="restoreDriver">Whether to restore the driver to the previous state</param>
        /// <remarks>All shells should use this routine to allow effective and consistent line parsing.</remarks>
        public static void GetLine(string FullCommand, string OutputPath = "", string ShellType = "Shell", bool restoreDriver = true) =>
            GetLine(FullCommand, OutputPath, ShellType, restoreDriver, SetTitle, InputHistoryEnabled);

        /// <summary>
        /// Parses a specified command.
        /// </summary>
        /// <param name="FullCommand">The full command string</param>
        /// <param name="OutputPath">Optional (non-)neutralized output path</param>
        /// <param name="ShellType">Shell type</param>
        /// <param name="restoreDriver">Whether to restore the driver to the previous state</param>
        /// <param name="setTitle">Whether to set the console title</param>
        /// <remarks>All shells should use this routine to allow effective and consistent line parsing.</remarks>
        public static void GetLine(string FullCommand, string OutputPath = "", string ShellType = "Shell", bool restoreDriver = true, bool setTitle = true) =>
            GetLine(FullCommand, OutputPath, ShellType, restoreDriver, setTitle, InputHistoryEnabled);

        /// <summary>
        /// Parses a specified command.
        /// </summary>
        /// <param name="FullCommand">The full command string</param>
        /// <param name="OutputPath">Optional (non-)neutralized output path</param>
        /// <param name="ShellType">Shell type</param>
        /// <param name="restoreDriver">Whether to restore the driver to the previous state</param>
        /// <param name="setTitle">Whether to set the console title</param>
        /// <param name="enableInputHistory">Whether to enable the input history</param>
        /// <remarks>All shells should use this routine to allow effective and consistent line parsing.</remarks>
        public static void GetLine(string FullCommand, string OutputPath = "", string ShellType = "Shell", bool restoreDriver = true, bool setTitle = true, bool enableInputHistory = true)
        {
            // Check for sanity
            if (string.IsNullOrEmpty(FullCommand))
                FullCommand = "";

            // Variables
            string TargetFile = "";
            string TargetFileName = "";

            // Get the shell info
            var shellInfo = GetShellInfo(ShellType);
            var ShellInstance = ShellStack[ShellStack.Count - 1];

            // Now, initialize the command autocomplete handler. This will not be invoked if we have auto completion disabled.
            HistoryTools.LoadHistories();
            var settings = new TermReaderSettings()
            {
                Suggestions = (text, index, _) => CommandAutoComplete.GetSuggestions(text, index),
                SuggestionsDelimiters = [' '],
                TreatCtrlCAsInput = true,
                HistoryName = ShellType,
                HistoryEnabled = enableInputHistory,
            };

            // Check to see if the full command string ends with the semicolon
            while (FullCommand.EndsWith(";") || string.IsNullOrEmpty(FullCommand))
            {
                // Enable cursor
                ConsoleWrapper.CursorVisible = true;

                // Tell the user to provide the command
                StringBuilder commandBuilder = new(FullCommand);

                // We need to put a synclock in the below steps, because the cancellation handlers seem to be taking their time to try to suppress the
                // thread abort error messages. If the shell tried to write to the console while these handlers were still working, the command prompt
                // would either be incomplete or not printed to the console at all.
                string prompt = "";
                lock (CancellationHandlers.GetCancelSyncLock(ShellType))
                {
                    // Print a prompt
                    var preset = PromptPresetManager.GetCurrentPresetBaseFromShell(ShellType);
                    if (!string.IsNullOrEmpty(FullCommand))
                        prompt = preset.PresetPromptCompletion;
                    else
                        prompt = preset.PresetPrompt;
                }

                // Wait for command
                ConsoleLogger.Debug("Waiting for command");
                string strcommand = TermReader.Read(prompt, "", settings, oneLineWrap: shellInfo.OneLineWrap);
                ConsoleLogger.Debug("Waited for command [{0}]", strcommand);
                if (strcommand == ";")
                    strcommand = "";

                // Add command to command builder and return the final result. The reason to add the extra space before the second command written is that
                // because if we need to provide a second command to the shell in a separate line, we usually add the semicolon at the end of the primary
                // command input.
                if (!string.IsNullOrEmpty(FullCommand) && !string.IsNullOrEmpty(strcommand))
                    commandBuilder.Append(' ');

                // There are cases when strcommand may be empty, so ignore that if it's empty.
                if (!string.IsNullOrEmpty(strcommand))
                    commandBuilder.Append(strcommand);
                FullCommand = commandBuilder.ToString();

                // There are cases when the kernel panics or reboots in the middle of the command input. If reboot is requested,
                // ensure that we're really gone.
                if (ShellInstance.interrupting)
                    return;
            }

            // Check for a type of command
            CancellationHandlers.AllowCancel();
            var SplitCommands = FullCommand.Split([" ; "], StringSplitOptions.RemoveEmptyEntries);
            var Commands = CommandManager.GetCommands(ShellType);
            for (int i = 0; i < SplitCommands.Length; i++)
            {
                string Command = SplitCommands[i];

                // Then, check to see if this shell uses the slash command
                if (shellInfo.SlashCommand)
                {
                    // Check if we need to remove the slash
                    if (!Command.StartsWith("/"))
                    {
                        // Not a slash command. Do things differently
                        ConsoleLogger.Debug("Non-slash cmd exec succeeded. Running with {0}", Command);
                        var Params = new CommandExecutorParameters(Command, shellInfo.NonSlashCommandInfo, ShellType, ShellInstance);
                        CommandExecutor.StartCommandThread(Params);
                        continue;
                    }
                    else
                    {
                        // Strip the slash
                        Command = Command.Substring(1).Trim();
                    }
                }

                // Initialize local MESH variables (if found)
                string localVarStoreMatchRegex = /* lang=regex */ @"^\((.+)\)\s+";
                var localVarStoreMatch = RegexTools.Match(Command, localVarStoreMatchRegex);
                string varStoreString = localVarStoreMatch.Groups[1].Value;
                ConsoleLogger.Debug("varStoreString is: {0}", varStoreString);
                string varStoreStringFull = localVarStoreMatch.Value;
                var varStoreVars = MESHVariables.GetVariablesFrom(varStoreString);

                // First, check to see if we already have that variable. If we do, get its old value.
                List<(string, string)> oldVarValues = [];
                foreach (string varStoreKey in varStoreVars.varStoreKeys)
                {
                    if (MESHVariables.Variables.ContainsKey(varStoreKey))
                        oldVarValues.Add((varStoreKey, MESHVariables.GetVariable(varStoreKey)));
                }
                MESHVariables.InitializeVariablesFrom(varStoreString);
                Command = Command.Substring(varStoreStringFull.Length);

                // Check to see if the command is a comment
                if (!string.IsNullOrWhiteSpace(Command) && !Command.StartsWithAnyOf([" ", "#"]))
                {
                    // Get the command name
                    var words = Command.SplitEncloseDoubleQuotes();
                    string commandName = words[0].ReleaseDoubleQuotes();

                    // Verify that we aren't tricked into processing an empty command
                    if (string.IsNullOrEmpty(commandName))
                        break;

                    // Now, split the arguments
                    string arguments = string.Join(" ", words.Skip(1));

                    // Get the target file and path
                    TargetFile = TextTools.Unescape(commandName);
                    string[] possiblePaths = PlatformHelper.GetPossiblePaths(commandName);
                    bool existsInPath = possiblePaths.Length > 0;
                    if (existsInPath)
                        TargetFile = possiblePaths[possiblePaths.Length - 1];
                    bool pathValid = PlatformHelper.TryParsePath(TargetFile);
                    if (!existsInPath || string.IsNullOrEmpty(TargetFile))
                        TargetFile = ConsoleFilesystem.NeutralizePath(commandName);
                    if (pathValid)
                        TargetFileName = Path.GetFileName(TargetFile);
                    ConsoleLogger.Debug("Finished finalCommand: {0}", commandName);
                    ConsoleLogger.Debug("Finished TargetFile: {0}", TargetFile);

                    // Reads command written by user
                    try
                    {
                        // Set title
                        if (setTitle)
                            ConsoleMisc.SetTitle($"{InitialTitle} - {Command}");

                        // Check the command
                        bool exists = Commands.Any((ci) => ci.Command == commandName || ci.Aliases.Any((ai) => ai.Alias == commandName));
                        if (exists)
                        {
                            // Execute the command
                            ConsoleLogger.Debug("Executing command");
                            var cmdInfo = Commands.Single((ci) => ci.Command == commandName || ci.Aliases.Any((ai) => ai.Alias == commandName));

                            // Check to see if the command supports redirection
                            if (cmdInfo.Flags.HasFlag(CommandFlags.RedirectionSupported))
                            {
                                ConsoleLogger.Debug("Redirection supported!");
                                Command = InitializeRedirection(Command);
                            }

                            // Check to see if the optional path is specified
                            if (!string.IsNullOrEmpty(OutputPath))
                            {
                                ConsoleLogger.Debug("Output path provided!");
                                InitializeOutputPathWriter(OutputPath);
                            }

                            if (!string.IsNullOrEmpty(commandName) || !commandName.StartsWithAnyOf([" ", "#"]))
                            {
                                // Check the command before starting
                                ConsoleLogger.Debug("Cmd exec {0} succeeded. Running with {1}", commandName, Command);
                                var Params = new CommandExecutorParameters(Command, cmdInfo, ShellType, ShellInstance);
                                CommandExecutor.StartCommandThread(Params);
                                MESHVariables.SetVariable("MESHErrorCode", $"{ShellInstance.lastErrorCode}");
                            }
                        }
                        else if (pathValid)
                        {
                            // Parse the script file or executable file
                            if (File.Exists(TargetFile))
                            {
                                ConsoleLogger.Debug("Cmd exec {0} succeeded because file is found.", commandName);
                                if (TargetFile.EndsWith(".mesh", StringComparison.OrdinalIgnoreCase))
                                {
                                    // Run the script file
                                    try
                                    {
                                        ConsoleLogger.Debug("Cmd exec {0} succeeded because it's a MESH script.", commandName);
                                        MESHParse.Execute(TargetFile, arguments, ShellType);
                                        MESHVariables.SetVariable("MESHErrorCode", "0");
                                        ShellInstance.lastErrorCode = 0;
                                    }
                                    catch (Exception ex)
                                    {
                                        TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_SHELLMANAGER_SCRIPTING_CANTEXECUTE"), true, ThemeColorType.Error, ex.Message);
                                        MESHVariables.SetVariable("MESHErrorCode", $"{ex.GetHashCode()}");
                                        ShellInstance.lastErrorCode = ex.GetHashCode();
                                    }
                                }
                                else
                                {
                                    int processExitCode = 0;
                                    try
                                    {
                                        // Create a new instance of process
                                        ConsoleLogger.Debug("Command: {0}, Arguments: {1}", TargetFile, arguments);
                                        var Params = new ExecuteProcessThreadParameters(TargetFile, arguments);
                                        ProcessExecutor.processExecutorThread = new Thread((processParams) => processExitCode = ProcessExecutor.ExecuteProcess((ExecuteProcessThreadParameters?)processParams));
                                        ProcessExecutor.processExecutorThread.Start(Params);
                                        ProcessExecutor.processExecutorThread.Join();
                                        MESHVariables.SetVariable("MESHErrorCode", $"{processExitCode}");
                                        ShellInstance.lastErrorCode = processExitCode;
                                    }
                                    catch (Exception ex)
                                    {
                                        ConsoleLogger.Error(ex, "Failed to start process: {0}", ex.Message);
                                        TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_SHELLMANAGER_CMDEXECUTEERROR"), true, ThemeColorType.Error, commandName, ex.Message);
                                        MESHVariables.SetVariable("MESHErrorCode", $"{ex.GetHashCode()}");
                                        ShellInstance.lastErrorCode = ex.GetHashCode();
                                    }
                                    finally
                                    {
                                        ProcessExecutor.processExecutorThread.Interrupt();
                                        ProcessExecutor.processExecutorThread.Join();
                                        ProcessExecutor.processExecutorThread = new Thread((processParams) => processExitCode = ProcessExecutor.ExecuteProcess((ExecuteProcessThreadParameters?)processParams));
                                    }
                                }
                            }
                            else
                            {
                                ConsoleLogger.Warning("Cmd exec {0} failed: command {0} not found parsing target file", commandName);
                                TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_SHELLMANAGER_CMDNOTFOUND"), true, ThemeColorType.Error, commandName);
                                MESHVariables.SetVariable("MESHErrorCode", "-2");
                                ShellInstance.lastErrorCode = -2;
                            }
                        }
                        else
                        {
                            ConsoleLogger.Warning("Cmd exec {0} failed: command {0} not found", commandName);
                            TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_SHELLMANAGER_CMDNOTFOUND"), true, ThemeColorType.Error, commandName);
                            MESHVariables.SetVariable("MESHErrorCode", "-1");
                            ShellInstance.lastErrorCode = -1;
                        }
                    }
                    catch (Exception ex)
                    {
                        TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_SHELLMANAGER_CMDEXECUTEERROR") + CharManager.NewLine + LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_ERRORCOMMAND2"), true, ThemeColorType.Error, ex.GetType().FullName ?? "<null>", ex.Message);
                        MESHVariables.SetVariable("MESHErrorCode", $"{ex.GetHashCode()}");
                        ShellInstance.lastErrorCode = ex.GetHashCode();
                    }
                }

                // Fire an event of PostExecuteCommand and reset all local variables
                var varStoreKeys = varStoreVars.varStoreKeys;
                foreach (string varStoreKey in varStoreKeys)
                    MESHVariables.RemoveVariable(varStoreKey);
                foreach (var varStoreKeyOld in oldVarValues)
                {
                    string key = varStoreKeyOld.Item1;
                    string value = varStoreKeyOld.Item2;
                    MESHVariables.InitializeVariable(key);
                    MESHVariables.SetVariable(key, value);
                }
            }

            // Restore console output to its original state if any
            if (restoreDriver)
            {
                if (ConsoleWrapperTools.Wrapper is FileWrite writer)
                    writer.FilterVT = false;
                if (ConsoleWrapperTools.Wrapper is FileSequence writerSeq)
                    writerSeq.FilterVT = false;
                ConsoleWrapperTools.UnsetWrapperLocal();
            }

            // Restore title and cancel possibility state
            if (setTitle)
                ConsoleMisc.SetTitle(InitialTitle);
            CancellationHandlers.InhibitCancel();
            lastCommand = FullCommand;
        }

        /// <summary>
        /// Gets the shell information instance
        /// </summary>
        /// <param name="shellType">Shell type name</param>
        public static BaseShellInfo GetShellInfo(string shellType) =>
            AvailableShells.TryGetValue(shellType, out BaseShellInfo? baseShellInfo) ? baseShellInfo : AvailableShells["Shell"];

        /// <summary>
        /// Starts the shell
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        /// <param name="ShellArgs">Arguments to pass to shell</param>
        public static void StartShell(string ShellType, params object[] ShellArgs)
        {
            int shellCount = ShellStack.Count;
            try
            {
                // Make a shell executor based on shell type to select a specific executor (if the shell type is not MESH, and if the new shell isn't a mother shell)
                // Please note that the remote debug shell is not supported because it works on its own space, so it can't be interfaced using the standard IShell.
                var ShellExecute = GetShellExecutor(ShellType) ??
                    throw new TerminauxException(LanguageTools.GetLocalized("T_SHELL_SHELLMANAGER_EXCEPTION_NOEXECUTOR"), ShellType.ToString());

                // Make a new instance of shell information
                var ShellCommandThread = RegenerateCommandThread(ShellType);
                var ShellInfo = new ShellExecuteInfo(ShellType, ShellExecute, ShellCommandThread);

                // Add a new shell to the shell stack to indicate that we have a new shell (a visitor)!
                ShellStack.Add(ShellInfo);

                // Load the placeholder for an MESH variable, unless it's already defined
                PlaceParse.RegisterCustomPlaceholder("$", MESHVariables.GetVariable);

                // Now, load the color placeholders
                PlaceParse.RegisterCustomPlaceholder("f", (c) => new Color(c).VTSequenceForeground());
                PlaceParse.RegisterCustomPlaceholder("b", (c) => new Color(c).VTSequenceBackground());
                PlaceParse.RegisterCustomPlaceholder("fgreset", (_) => ThemeColorsTools.GetColor(ThemeColorType.NeutralText).VTSequenceForeground());
                PlaceParse.RegisterCustomPlaceholder("bgreset", (_) => ThemeColorsTools.GetColor(ThemeColorType.Background).VTSequenceBackground());

                // Load the platform placeholders
                PlaceParse.RegisterCustomPlaceholder("ridgeneric", (_) => PlatformHelper.GetCurrentGenericRid());
                PlaceParse.RegisterCustomPlaceholder("termemu", (_) => PlatformHelper.GetTerminalEmulator());
                PlaceParse.RegisterCustomPlaceholder("termtype", (_) => PlatformHelper.GetTerminalType());

                // Load the aliases
                AliasManager.InitAliases(ShellType);

                // Reset title in case we're going to another shell
                ConsoleMisc.SetTitle(InitialTitle);
                ShellExecute.InitializeShell(ShellArgs);
            }
            catch (Exception ex)
            {
                // There is an exception trying to run the shell. Throw the message to the debugger and to the caller.
                ConsoleLogger.Error("Failed initializing shell!!! Type: {0}, Message: {1}", ShellType, ex.Message);
                ConsoleLogger.Error("Additional info: Args: {0} [{1}], Shell Stack: {2} shells, shellCount: {3} shells", ShellArgs.Length, string.Join(", ", ShellArgs), ShellStack.Count, shellCount);
                ConsoleLogger.Error(ex, "This shell needs to be killed in order for the shell manager to proceed. Passing exception to caller...");
                throw new TerminauxException(LanguageTools.GetLocalized("T_SHELL_SHELLMANAGER_EXCEPTION_SHELLINIT"), ex);
            }
            finally
            {
                // There is either an unknown shell error trying to be initialized or a shell has manually set Bail to true prior to exiting, like the JSON shell
                // that sets this property when it fails to open the JSON file due to syntax error or something. If we haven't added the shell to the shell stack,
                // do nothing. Else, purge that shell with KillShell(). Otherwise, we'll get another shell's commands in the wrong shell and other problems will
                // occur until the ghost shell has exited either automatically or manually, so check to see if we have added the newly created shell to the shell
                // stack and kill that faulted shell so that we can have the correct shell in the most recent shell, ^1, from the stack.
                int newShellCount = ShellStack.Count;
                ConsoleLogger.Debug("Purge: newShellCount: {0} shells, shellCount: {1} shells", newShellCount, shellCount);
                if (newShellCount > shellCount)
                    KillShell();

                // Save the aliases
                AliasManager.SaveAliases(ShellType);
            }
        }

        /// <summary>
        /// Kills the last running shell
        /// </summary>
        public static void KillShell()
        {
            if (ShellStack.Count >= 1)
            {
                var shell = ShellStack[ShellStack.Count - 1];
                var shellBase = ShellStack[ShellStack.Count - 1].ShellBase;
                if (shellBase is not null)
                {
                    shell.interrupting = true;
                    shellBase.Bail = true;
                }
                PurgeShells();
            }
        }

        /// <summary>
        /// Kills all the shells
        /// </summary>
        public static void KillAllShells()
        {
            for (int i = ShellStack.Count - 1; i >= 0; i--)
            {
                var shell = ShellStack[i];
                var shellBase = ShellStack[i].ShellBase;
                if (shellBase is not null)
                {
                    shell.interrupting = true;
                    shellBase.Bail = true;
                }
            }
            PurgeShells();
        }

        /// <summary>
        /// Cleans up the shell stack
        /// </summary>
        public static void PurgeShells() =>
            // Remove these shells from the stack
            ShellStack.RemoveAll(x => x.ShellBase?.Bail ?? true);

        /// <summary>
        /// Gets the shell executor based on the shell type
        /// </summary>
        /// <param name="ShellType">The requested shell type</param>
        public static BaseShell? GetShellExecutor(string ShellType) =>
            GetShellInfo(ShellType).ShellBase;

        /// <summary>
        /// Registers the custom shell
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        /// <param name="ShellTypeInfo">The shell type information</param>
        public static void RegisterShell(string ShellType, BaseShellInfo ShellTypeInfo)
        {
            if (!ShellTypeExists(ShellType))
            {
                // First, add the shell
                availableShells.Add(ShellType, ShellTypeInfo);

                // Then, add the default preset if the current preset is not found
                if (PromptPresetManager.CurrentPresets.ContainsKey(ShellType))
                    return;

                // Rare state.
                ConsoleLogger.Debug("Reached rare state or unconfigurable shell.");
                var presets = ShellTypeInfo.ShellPresets;
                var basePreset = new PromptPresetBase();
                if (presets is not null)
                {
                    // Add a default preset
                    if (presets.ContainsKey("Default"))
                        PromptPresetManager.CurrentPresets.Add(ShellType, "Default");
                    else if (presets.Count > 0)
                        PromptPresetManager.CurrentPresets.Add(ShellType, presets.ElementAt(0).Value.PresetName);
                    else
                        PromptPresetManager.CurrentPresets.Add(ShellType, basePreset.PresetName);
                }
                else
                {
                    // Make a base shell preset and set it as default.
                    PromptPresetManager.CurrentPresets.Add(ShellType, basePreset.PresetName);
                }
            }
        }

        /// <summary>
        /// Unregisters the custom shell
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        public static void UnregisterShell(string ShellType)
        {
            if (ShellTypeExists(ShellType))
            {
                // First, remove the shell
                availableShells.Remove(ShellType);

                // Then, remove the preset
                PromptPresetManager.CurrentPresets.Remove(ShellType);
            }
        }

        /// <summary>
        /// Does the shell exist?
        /// </summary>
        /// <param name="ShellType">Shell type to check</param>
        /// <returns>True if it exists; false otherwise.</returns>
        public static bool ShellTypeExists(string ShellType) =>
            AvailableShells.ContainsKey(ShellType);

        /// <summary>
        /// Adds the alternate shell command thread to the current shell
        /// </summary>
        public static void AddAlternateThread()
        {
            if (ShellStack.Count < 1)
                return;
            var AltThreads = ShellStack[ShellStack.Count - 1].AltCommandThreads;
            if (AltThreads.Count == 0 || AltThreads[AltThreads.Count - 1].IsAlive)
            {
                var CommandThread = new Thread((cmdThreadParams) => CommandExecutor.ExecuteCommand((CommandExecutorParameters?)cmdThreadParams));
                ShellStack[ShellStack.Count - 1].AltCommandThreads.Add(CommandThread);
            }
        }

        internal static Thread RegenerateCommandThread(string ShellType) =>
            new((cmdThreadParams) => CommandExecutor.ExecuteCommand((CommandExecutorParameters?)cmdThreadParams))
            {
                Name = $"{ShellType} Command Thread"
            };

        /// <summary>
        /// Initializes the redirection
        /// </summary>
        private static string InitializeRedirection(string Command)
        {
            // If requested command has output redirection sign after arguments, remove it from final command string and set output to that file
            string RedirectionPattern = /*lang=regex*/ @"(?:( (?:>>|>>>) )(.+?))+$";
            if (RegexTools.IsMatch(Command, RedirectionPattern))
            {
                var outputMatch = Regex.Match(Command, RedirectionPattern);
                var outputFileCaptures = outputMatch.Groups[2].Captures;
                var outputFileModeCaptures = outputMatch.Groups[1].Captures;
                List<string> captureList = [];
                List<string> modeCaptureList = [];
                foreach (Capture capture in outputFileCaptures)
                    captureList.Add(capture.Value);
                foreach (Capture capture in outputFileModeCaptures)
                    modeCaptureList.Add(capture.Value);
                string[] outputFiles = [.. captureList];
                string[] outputFileModes = [.. modeCaptureList];
                List<string> filePaths = [];
                for (int i = 0; i < outputFiles.Length; i++)
                {
                    string outputFile = outputFiles[i];
                    bool isOverwrite = outputFileModes[i] != " >>> ";
                    string OutputFilePath = ConsoleFilesystem.NeutralizePath(outputFile);
                    ConsoleLogger.Debug("Output redirection found for file {1} with overwrite mode [{0}].", isOverwrite, OutputFilePath);
                    if (isOverwrite)
                        ConsoleFilesystem.ClearFile(OutputFilePath);
                    filePaths.Add(OutputFilePath);
                }
                ConsoleWrapperTools.SetWrapperLocal(nameof(FileSequence));
                ((FileSequence)ConsoleWrapperTools.Wrapper).PathsToWrite = [.. filePaths];
                ((FileSequence)ConsoleWrapperTools.Wrapper).FilterVT = true;
                Command = Command.RemoveSuffix(outputMatch.Value);
            }
            else if (Command.EndsWith(" |SILENT|"))
            {
                ConsoleLogger.Debug("Silence found. Redirecting to null writer...");
                ConsoleWrapperTools.SetWrapperLocal(nameof(Null));
                Command = Command.RemoveSuffix(" |SILENT|");
            }

            return Command;
        }

        /// <summary>
        /// Initializes the optional file path writer
        /// </summary>
        private static void InitializeOutputPathWriter(string OutputPath)
        {
            // Checks to see if the user provided optional path
            if (!string.IsNullOrWhiteSpace(OutputPath))
            {
                ConsoleLogger.Debug("Optional output redirection found using OutputPath ({0}).", OutputPath);
                OutputPath = ConsoleFilesystem.NeutralizePath(OutputPath);
                ConsoleWrapperTools.SetWrapperLocal("FileWrite");
                ((FileWrite)ConsoleWrapperTools.Wrapper).PathToWrite = OutputPath;
            }
        }
    }
}
