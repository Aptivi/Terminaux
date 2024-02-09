namespace Terminaux.TermInfo
{
    public sealed partial class TermInfoDesc
    {
        /// <summary>
        /// The auto_left_margin [auto_left_margin, bw] bool capability indicates cub 1 wraps from column 0 to last column.
        /// </summary>
        public bool? AutoLeftMargin => GetBoolean(TermInfoCaps.Boolean.AutoLeftMargin);

        /// <summary>
        /// The auto_right_margin [auto_right_margin, am] bool capability indicates terminal has automatic margins.
        /// </summary>
        public bool? AutoRightMargin => GetBoolean(TermInfoCaps.Boolean.AutoRightMargin);

        /// <summary>
        /// The no_esc_ctlc [no_esc_ctlc, xsb] bool capability indicates beehive f 1 escape f 2 ctrl c.
        /// </summary>
        public bool? NoEscCtlc => GetBoolean(TermInfoCaps.Boolean.NoEscCtlc);

        /// <summary>
        /// The ceol_standout_glitch [ceol_standout_glitch, xhp] bool capability indicates standout not erased by overwriting hp.
        /// </summary>
        public bool? CeolStandoutGlitch => GetBoolean(TermInfoCaps.Boolean.CeolStandoutGlitch);

        /// <summary>
        /// The eat_newline_glitch [eat_newline_glitch, xenl] bool capability indicates newline ignored after 80 cols concept.
        /// </summary>
        public bool? EatNewlineGlitch => GetBoolean(TermInfoCaps.Boolean.EatNewlineGlitch);

        /// <summary>
        /// The erase_overstrike [erase_overstrike, eo] bool capability indicates can erase overstrikes with a blank.
        /// </summary>
        public bool? EraseOverstrike => GetBoolean(TermInfoCaps.Boolean.EraseOverstrike);

        /// <summary>
        /// The generic_type [generic_type, gn] bool capability indicates generic line type.
        /// </summary>
        public bool? GenericType => GetBoolean(TermInfoCaps.Boolean.GenericType);

        /// <summary>
        /// The hard_copy [hard_copy, hc] bool capability indicates hardcopy terminal.
        /// </summary>
        public bool? HardCopy => GetBoolean(TermInfoCaps.Boolean.HardCopy);

        /// <summary>
        /// The has_meta_key [has_meta_key, km] bool capability indicates has a meta key (i.e., sets 8th bit).
        /// </summary>
        public bool? HasMetaKey => GetBoolean(TermInfoCaps.Boolean.HasMetaKey);

        /// <summary>
        /// The has_status_line [has_status_line, hs] bool capability indicates has extra status line.
        /// </summary>
        public bool? HasStatusLine => GetBoolean(TermInfoCaps.Boolean.HasStatusLine);

        /// <summary>
        /// The insert_null_glitch [insert_null_glitch, in] bool capability indicates insert mode distinguishes nulls.
        /// </summary>
        public bool? InsertNullGlitch => GetBoolean(TermInfoCaps.Boolean.InsertNullGlitch);

        /// <summary>
        /// The memory_above [memory_above, da] bool capability indicates display may be retained above the screen.
        /// </summary>
        public bool? MemoryAbove => GetBoolean(TermInfoCaps.Boolean.MemoryAbove);

        /// <summary>
        /// The memory_below [memory_below, db] bool capability indicates display may be retained below the screen.
        /// </summary>
        public bool? MemoryBelow => GetBoolean(TermInfoCaps.Boolean.MemoryBelow);

        /// <summary>
        /// The move_insert_mode [move_insert_mode, mir] bool capability indicates safe to move while in insert mode.
        /// </summary>
        public bool? MoveInsertMode => GetBoolean(TermInfoCaps.Boolean.MoveInsertMode);

        /// <summary>
        /// The move_standout_mode [move_standout_mode, msgr] bool capability indicates safe to move while in standout mode.
        /// </summary>
        public bool? MoveStandoutMode => GetBoolean(TermInfoCaps.Boolean.MoveStandoutMode);

        /// <summary>
        /// The over_strike [over_strike, os] bool capability indicates terminal can overstrike.
        /// </summary>
        public bool? OverStrike => GetBoolean(TermInfoCaps.Boolean.OverStrike);

        /// <summary>
        /// The status_line_esc_ok [status_line_esc_ok, eslok] bool capability indicates escape can be used on the status line.
        /// </summary>
        public bool? StatusLineEscOk => GetBoolean(TermInfoCaps.Boolean.StatusLineEscOk);

        /// <summary>
        /// The dest_tabs_magic_smso [dest_tabs_magic_smso, xt] bool capability indicates tabs destructive magic so char t 1061.
        /// </summary>
        public bool? DestTabsMagicSmso => GetBoolean(TermInfoCaps.Boolean.DestTabsMagicSmso);

        /// <summary>
        /// The tilde_glitch [tilde_glitch, hz] bool capability indicates cannot print s hazeltine.
        /// </summary>
        public bool? TildeGlitch => GetBoolean(TermInfoCaps.Boolean.TildeGlitch);

        /// <summary>
        /// The transparent_underline [transparent_underline, ul] bool capability indicates underline character overstrikes.
        /// </summary>
        public bool? TransparentUnderline => GetBoolean(TermInfoCaps.Boolean.TransparentUnderline);

        /// <summary>
        /// The xon_xoff [xon_xoff, xon] bool capability indicates terminal uses xon xoff handshaking.
        /// </summary>
        public bool? XonXoff => GetBoolean(TermInfoCaps.Boolean.XonXoff);

        /// <summary>
        /// The needs_xon_xoff [needs_xon_xoff, nxon] bool capability indicates padding will not work xon xoff required.
        /// </summary>
        public bool? NeedsXonXoff => GetBoolean(TermInfoCaps.Boolean.NeedsXonXoff);

        /// <summary>
        /// The prtr_silent [prtr_silent, mc5i] bool capability indicates printer will not echo on screen.
        /// </summary>
        public bool? PrtrSilent => GetBoolean(TermInfoCaps.Boolean.PrtrSilent);

        /// <summary>
        /// The hard_cursor [hard_cursor, chts] bool capability indicates cursor is hard to see.
        /// </summary>
        public bool? HardCursor => GetBoolean(TermInfoCaps.Boolean.HardCursor);

        /// <summary>
        /// The non_rev_rmcup [non_rev_rmcup, nrrmc] bool capability indicates smcup does not reverse rmcup.
        /// </summary>
        public bool? NonRevRmcup => GetBoolean(TermInfoCaps.Boolean.NonRevRmcup);

        /// <summary>
        /// The no_pad_char [no_pad_char, npc] bool capability indicates pad character does not exist.
        /// </summary>
        public bool? NoPadChar => GetBoolean(TermInfoCaps.Boolean.NoPadChar);

        /// <summary>
        /// The non_dest_scroll_region [non_dest_scroll_region, ndscr] bool capability indicates scrolling region is non destructive.
        /// </summary>
        public bool? NonDestScrollRegion => GetBoolean(TermInfoCaps.Boolean.NonDestScrollRegion);

        /// <summary>
        /// The can_change [can_change, ccc] bool capability indicates terminal can re define existing colors.
        /// </summary>
        public bool? CanChange => GetBoolean(TermInfoCaps.Boolean.CanChange);

        /// <summary>
        /// The back_color_erase [back_color_erase, bce] bool capability indicates screen erased with background color.
        /// </summary>
        public bool? BackColorErase => GetBoolean(TermInfoCaps.Boolean.BackColorErase);

        /// <summary>
        /// The hue_lightness_saturation [hue_lightness_saturation, hls] bool capability indicates terminal uses only hls color notation tektronix.
        /// </summary>
        public bool? HueLightnessSaturation => GetBoolean(TermInfoCaps.Boolean.HueLightnessSaturation);

        /// <summary>
        /// The col_addr_glitch [col_addr_glitch, xhpa] bool capability indicates only positive motion for hpa mhpa caps.
        /// </summary>
        public bool? ColAddrGlitch => GetBoolean(TermInfoCaps.Boolean.ColAddrGlitch);

        /// <summary>
        /// The cr_cancels_micro_mode [cr_cancels_micro_mode, crxm] bool capability indicates using cr turns off micro mode.
        /// </summary>
        public bool? CrCancelsMicroMode => GetBoolean(TermInfoCaps.Boolean.CrCancelsMicroMode);

        /// <summary>
        /// The has_print_wheel [has_print_wheel, daisy] bool capability indicates printer needs operator to change character set.
        /// </summary>
        public bool? HasPrintWheel => GetBoolean(TermInfoCaps.Boolean.HasPrintWheel);

        /// <summary>
        /// The row_addr_glitch [row_addr_glitch, xvpa] bool capability indicates only positive motion for vpa mvpa caps.
        /// </summary>
        public bool? RowAddrGlitch => GetBoolean(TermInfoCaps.Boolean.RowAddrGlitch);

        /// <summary>
        /// The semi_auto_right_margin [semi_auto_right_margin, sam] bool capability indicates printing in last column causes cr.
        /// </summary>
        public bool? SemiAutoRightMargin => GetBoolean(TermInfoCaps.Boolean.SemiAutoRightMargin);

        /// <summary>
        /// The cpi_changes_res [cpi_changes_res, cpix] bool capability indicates changing character pitch changes resolution.
        /// </summary>
        public bool? CpiChangesRes => GetBoolean(TermInfoCaps.Boolean.CpiChangesRes);

        /// <summary>
        /// The lpi_changes_res [lpi_changes_res, lpix] bool capability indicates changing line pitch changes resolution.
        /// </summary>
        public bool? LpiChangesRes => GetBoolean(TermInfoCaps.Boolean.LpiChangesRes);

        /// <summary>
        /// The backspaces_with_bs [backspaces_with_bs, OTbs] bool capability indicates uses h to move left.
        /// </summary>
        public bool? BackspacesWithBs => GetBoolean(TermInfoCaps.Boolean.BackspacesWithBs);

        /// <summary>
        /// The crt_no_scrolling [crt_no_scrolling, OTns] bool capability indicates crt cannot scroll.
        /// </summary>
        public bool? CrtNoScrolling => GetBoolean(TermInfoCaps.Boolean.CrtNoScrolling);

        /// <summary>
        /// The no_correctly_working_cr [no_correctly_working_cr, OTnc] bool capability indicates no way to go to start of line.
        /// </summary>
        public bool? NoCorrectlyWorkingCr => GetBoolean(TermInfoCaps.Boolean.NoCorrectlyWorkingCr);

        /// <summary>
        /// The gnu_has_meta_key [gnu_has_meta_key, OTMT] bool capability indicates has meta key.
        /// </summary>
        public bool? GnuHasMetaKey => GetBoolean(TermInfoCaps.Boolean.GnuHasMetaKey);

        /// <summary>
        /// The linefeed_is_newline [linefeed_is_newline, OTNL] bool capability indicates move down with n.
        /// </summary>
        public bool? LinefeedIsNewline => GetBoolean(TermInfoCaps.Boolean.LinefeedIsNewline);

        /// <summary>
        /// The has_hardware_tabs [has_hardware_tabs, OTpt] bool capability indicates has 8 char tabs invoked with ^i.
        /// </summary>
        public bool? HasHardwareTabs => GetBoolean(TermInfoCaps.Boolean.HasHardwareTabs);

        /// <summary>
        /// The return_does_clr_eol [return_does_clr_eol, OTxr] bool capability indicates return clears the line.
        /// </summary>
        public bool? ReturnDoesClrEol => GetBoolean(TermInfoCaps.Boolean.ReturnDoesClrEol);

        /// <summary>
        /// The columns [columns, cols] num capability is number of columns in a line.
        /// </summary>
        public int? Columns => GetNum(TermInfoCaps.Num.Columns);

        /// <summary>
        /// The init_tabs [init_tabs, it] num capability is tabs initially every spaces.
        /// </summary>
        public int? InitTabs => GetNum(TermInfoCaps.Num.InitTabs);

        /// <summary>
        /// The lines [lines, lines] num capability is number of lines on screen or page.
        /// </summary>
        public int? Lines => GetNum(TermInfoCaps.Num.Lines);

        /// <summary>
        /// The lines_of_memory [lines_of_memory, lm] num capability is lines of memory if line 0 means varies.
        /// </summary>
        public int? LinesOfMemory => GetNum(TermInfoCaps.Num.LinesOfMemory);

        /// <summary>
        /// The magic_cookie_glitch [magic_cookie_glitch, xmc] num capability is number of blank characters left by smso or rmso.
        /// </summary>
        public int? MagicCookieGlitch => GetNum(TermInfoCaps.Num.MagicCookieGlitch);

        /// <summary>
        /// The padding_baud_rate [padding_baud_rate, pb] num capability is lowest baud rate where padding needed.
        /// </summary>
        public int? PaddingBaudRate => GetNum(TermInfoCaps.Num.PaddingBaudRate);

        /// <summary>
        /// The virtual_terminal [virtual_terminal, vt] num capability is virtual terminal number cb unix.
        /// </summary>
        public int? VirtualTerminal => GetNum(TermInfoCaps.Num.VirtualTerminal);

        /// <summary>
        /// The width_status_line [width_status_line, wsl] num capability is number of columns in status line.
        /// </summary>
        public int? WidthStatusLine => GetNum(TermInfoCaps.Num.WidthStatusLine);

        /// <summary>
        /// The num_labels [num_labels, nlab] num capability is number of labels on screen.
        /// </summary>
        public int? NumLabels => GetNum(TermInfoCaps.Num.NumLabels);

        /// <summary>
        /// The label_height [label_height, lh] num capability is rows in each label.
        /// </summary>
        public int? LabelHeight => GetNum(TermInfoCaps.Num.LabelHeight);

        /// <summary>
        /// The label_width [label_width, lw] num capability is columns in each label.
        /// </summary>
        public int? LabelWidth => GetNum(TermInfoCaps.Num.LabelWidth);

        /// <summary>
        /// The max_attributes [max_attributes, ma] num capability is maximum combined attributes terminal can handle.
        /// </summary>
        public int? MaxAttributes => GetNum(TermInfoCaps.Num.MaxAttributes);

        /// <summary>
        /// The maximum_windows [maximum_windows, wnum] num capability is maximum number of definable windows.
        /// </summary>
        public int? MaximumWindows => GetNum(TermInfoCaps.Num.MaximumWindows);

        /// <summary>
        /// The max_colors [max_colors, colors] num capability is maximum number of colors on screen.
        /// </summary>
        public int? MaxColors => GetNum(TermInfoCaps.Num.MaxColors);

        /// <summary>
        /// The max_pairs [max_pairs, pairs] num capability is maximum number of color pairs on the screen.
        /// </summary>
        public int? MaxPairs => GetNum(TermInfoCaps.Num.MaxPairs);

        /// <summary>
        /// The no_color_video [no_color_video, ncv] num capability is video attributes that cannot be used with colors.
        /// </summary>
        public int? NoColorVideo => GetNum(TermInfoCaps.Num.NoColorVideo);

        /// <summary>
        /// The buffer_capacity [buffer_capacity, bufsz] num capability is numbers of bytes buffered before printing.
        /// </summary>
        public int? BufferCapacity => GetNum(TermInfoCaps.Num.BufferCapacity);

        /// <summary>
        /// The dot_vert_spacing [dot_vert_spacing, spinv] num capability is spacing of pins vertically in pins per inch.
        /// </summary>
        public int? DotVertSpacing => GetNum(TermInfoCaps.Num.DotVertSpacing);

        /// <summary>
        /// The dot_horz_spacing [dot_horz_spacing, spinh] num capability is spacing of dots horizontally in dots per inch.
        /// </summary>
        public int? DotHorzSpacing => GetNum(TermInfoCaps.Num.DotHorzSpacing);

        /// <summary>
        /// The max_micro_address [max_micro_address, maddr] num capability is maximum value in micro ... address.
        /// </summary>
        public int? MaxMicroAddress => GetNum(TermInfoCaps.Num.MaxMicroAddress);

        /// <summary>
        /// The max_micro_jump [max_micro_jump, mjump] num capability is maximum value in parm ... micro.
        /// </summary>
        public int? MaxMicroJump => GetNum(TermInfoCaps.Num.MaxMicroJump);

        /// <summary>
        /// The micro_col_size [micro_col_size, mcs] num capability is character step size when in micro mode.
        /// </summary>
        public int? MicroColSize => GetNum(TermInfoCaps.Num.MicroColSize);

        /// <summary>
        /// The micro_line_size [micro_line_size, mls] num capability is line step size when in micro mode.
        /// </summary>
        public int? MicroLineSize => GetNum(TermInfoCaps.Num.MicroLineSize);

        /// <summary>
        /// The number_of_pins [number_of_pins, npins] num capability is numbers of pins in print head.
        /// </summary>
        public int? NumberOfPins => GetNum(TermInfoCaps.Num.NumberOfPins);

        /// <summary>
        /// The output_res_char [output_res_char, orc] num capability is horizontal resolution in units per line.
        /// </summary>
        public int? OutputResChar => GetNum(TermInfoCaps.Num.OutputResChar);

        /// <summary>
        /// The output_res_line [output_res_line, orl] num capability is vertical resolution in units per line.
        /// </summary>
        public int? OutputResLine => GetNum(TermInfoCaps.Num.OutputResLine);

        /// <summary>
        /// The output_res_horz_inch [output_res_horz_inch, orhi] num capability is horizontal resolution in units per inch.
        /// </summary>
        public int? OutputResHorzInch => GetNum(TermInfoCaps.Num.OutputResHorzInch);

        /// <summary>
        /// The output_res_vert_inch [output_res_vert_inch, orvi] num capability is vertical resolution in units per inch.
        /// </summary>
        public int? OutputResVertInch => GetNum(TermInfoCaps.Num.OutputResVertInch);

        /// <summary>
        /// The print_rate [print_rate, cps] num capability is print rate in characters per second.
        /// </summary>
        public int? PrintRate => GetNum(TermInfoCaps.Num.PrintRate);

        /// <summary>
        /// The wide_char_size [wide_char_size, widcs] num capability is character step size when in double wide mode.
        /// </summary>
        public int? WideCharSize => GetNum(TermInfoCaps.Num.WideCharSize);

        /// <summary>
        /// The buttons [buttons, btns] num capability is number of buttons on mouse.
        /// </summary>
        public int? Buttons => GetNum(TermInfoCaps.Num.Buttons);

        /// <summary>
        /// The bit_image_entwining [bit_image_entwining, bitwin] num capability is number of passes for each bit image row.
        /// </summary>
        public int? BitImageEntwining => GetNum(TermInfoCaps.Num.BitImageEntwining);

        /// <summary>
        /// The bit_image_type [bit_image_type, bitype] num capability is type of bit image device.
        /// </summary>
        public int? BitImageType => GetNum(TermInfoCaps.Num.BitImageType);

        /// <summary>
        /// The magic_cookie_glitch_ul [magic_cookie_glitch_ul, OTug] num capability is number of blanks left by ul.
        /// </summary>
        public int? MagicCookieGlitchUl => GetNum(TermInfoCaps.Num.MagicCookieGlitchUl);

        /// <summary>
        /// The carriage_return_delay [carriage_return_delay, OTdC] num capability is pad needed for cr.
        /// </summary>
        public int? CarriageReturnDelay => GetNum(TermInfoCaps.Num.CarriageReturnDelay);

        /// <summary>
        /// The new_line_delay [new_line_delay, OTdN] num capability is pad needed for lf.
        /// </summary>
        public int? NewLineDelay => GetNum(TermInfoCaps.Num.NewLineDelay);

        /// <summary>
        /// The backspace_delay [backspace_delay, OTdB] num capability is padding required for h.
        /// </summary>
        public int? BackspaceDelay => GetNum(TermInfoCaps.Num.BackspaceDelay);

        /// <summary>
        /// The horizontal_tab_delay [horizontal_tab_delay, OTdT] num capability is padding required for i.
        /// </summary>
        public int? HorizontalTabDelay => GetNum(TermInfoCaps.Num.HorizontalTabDelay);

        /// <summary>
        /// The number_of_function_keys [number_of_function_keys, OTkn] num capability is count of function keys.
        /// </summary>
        public int? NumberOfFunctionKeys => GetNum(TermInfoCaps.Num.NumberOfFunctionKeys);

        /// <summary>
        /// The back_tab [back_tab, cbt] string capability is the back tab p.
        /// </summary>
        public string BackTab => GetString(TermInfoCaps.String.BackTab);

        /// <summary>
        /// The bell [bell, bel] string capability is the audible signal bell p.
        /// </summary>
        public string Bell => GetString(TermInfoCaps.String.Bell);

        /// <summary>
        /// The carriage_return [carriage_return, cr] string capability is the carriage return p p.
        /// </summary>
        public string CarriageReturn => GetString(TermInfoCaps.String.CarriageReturn);

        /// <summary>
        /// The change_scroll_region [change_scroll_region, csr] string capability is the change region to line 1 to line 2 p.
        /// </summary>
        public string ChangeScrollRegion => GetString(TermInfoCaps.String.ChangeScrollRegion);

        /// <summary>
        /// The clear_all_tabs [clear_all_tabs, tbc] string capability is the clear all tab stops p.
        /// </summary>
        public string ClearAllTabs => GetString(TermInfoCaps.String.ClearAllTabs);

        /// <summary>
        /// The clear_screen [clear_screen, clear] string capability is the clear screen and home cursor p.
        /// </summary>
        public string ClearScreen => GetString(TermInfoCaps.String.ClearScreen);

        /// <summary>
        /// The clr_eol [clr_eol, el] string capability is the clear to end of line p.
        /// </summary>
        public string ClrEol => GetString(TermInfoCaps.String.ClrEol);

        /// <summary>
        /// The clr_eos [clr_eos, ed] string capability is the clear to end of screen p.
        /// </summary>
        public string ClrEos => GetString(TermInfoCaps.String.ClrEos);

        /// <summary>
        /// The column_address [column_address, hpa] string capability is the horizontal position 1 absolute p.
        /// </summary>
        public string ColumnAddress => GetString(TermInfoCaps.String.ColumnAddress);

        /// <summary>
        /// The command_character [command_character, cmdch] string capability is the terminal settable cmd character in prototype.
        /// </summary>
        public string CommandCharacter => GetString(TermInfoCaps.String.CommandCharacter);

        /// <summary>
        /// The cursor_address [cursor_address, cup] string capability is the move to row 1 columns 2.
        /// </summary>
        public string CursorAddress => GetString(TermInfoCaps.String.CursorAddress);

        /// <summary>
        /// The cursor_down [cursor_down, cud1] string capability is the down one line.
        /// </summary>
        public string CursorDown => GetString(TermInfoCaps.String.CursorDown);

        /// <summary>
        /// The cursor_home [cursor_home, home] string capability is the home cursor if no cup.
        /// </summary>
        public string CursorHome => GetString(TermInfoCaps.String.CursorHome);

        /// <summary>
        /// The cursor_invisible [cursor_invisible, civis] string capability is the make cursor invisible.
        /// </summary>
        public string CursorInvisible => GetString(TermInfoCaps.String.CursorInvisible);

        /// <summary>
        /// The cursor_left [cursor_left, cub1] string capability is the move left one space.
        /// </summary>
        public string CursorLeft => GetString(TermInfoCaps.String.CursorLeft);

        /// <summary>
        /// The cursor_mem_address [cursor_mem_address, mrcup] string capability is the memory relative cursor addressing move to row 1 columns 2.
        /// </summary>
        public string CursorMemAddress => GetString(TermInfoCaps.String.CursorMemAddress);

        /// <summary>
        /// The cursor_normal [cursor_normal, cnorm] string capability is the make cursor appear normal undo civis cvvis.
        /// </summary>
        public string CursorNormal => GetString(TermInfoCaps.String.CursorNormal);

        /// <summary>
        /// The cursor_right [cursor_right, cuf1] string capability is the non destructive space (move right one space).
        /// </summary>
        public string CursorRight => GetString(TermInfoCaps.String.CursorRight);

        /// <summary>
        /// The cursor_to_ll [cursor_to_ll, ll] string capability is the last line first column if no cup.
        /// </summary>
        public string CursorToLl => GetString(TermInfoCaps.String.CursorToLl);

        /// <summary>
        /// The cursor_up [cursor_up, cuu1] string capability is the up one line.
        /// </summary>
        public string CursorUp => GetString(TermInfoCaps.String.CursorUp);

        /// <summary>
        /// The cursor_visible [cursor_visible, cvvis] string capability is the make cursor very visible.
        /// </summary>
        public string CursorVisible => GetString(TermInfoCaps.String.CursorVisible);

        /// <summary>
        /// The delete_character [delete_character, dch1] string capability is the delete character p.
        /// </summary>
        public string DeleteCharacter => GetString(TermInfoCaps.String.DeleteCharacter);

        /// <summary>
        /// The delete_line [delete_line, dl1] string capability is the delete line p.
        /// </summary>
        public string DeleteLine => GetString(TermInfoCaps.String.DeleteLine);

        /// <summary>
        /// The dis_status_line [dis_status_line, dsl] string capability is the disable status line.
        /// </summary>
        public string DisStatusLine => GetString(TermInfoCaps.String.DisStatusLine);

        /// <summary>
        /// The down_half_line [down_half_line, hd] string capability is the half a line down.
        /// </summary>
        public string DownHalfLine => GetString(TermInfoCaps.String.DownHalfLine);

        /// <summary>
        /// The enter_alt_charset_mode [enter_alt_charset_mode, smacs] string capability is the start alternate character set p.
        /// </summary>
        public string EnterAltCharsetMode => GetString(TermInfoCaps.String.EnterAltCharsetMode);

        /// <summary>
        /// The enter_blink_mode [enter_blink_mode, blink] string capability is the turn on blinking.
        /// </summary>
        public string EnterBlinkMode => GetString(TermInfoCaps.String.EnterBlinkMode);

        /// <summary>
        /// The enter_bold_mode [enter_bold_mode, bold] string capability is the turn on bold extra bright mode.
        /// </summary>
        public string EnterBoldMode => GetString(TermInfoCaps.String.EnterBoldMode);

        /// <summary>
        /// The enter_ca_mode [enter_ca_mode, smcup] string capability is the string to start programs using cup.
        /// </summary>
        public string EnterCaMode => GetString(TermInfoCaps.String.EnterCaMode);

        /// <summary>
        /// The enter_delete_mode [enter_delete_mode, smdc] string capability is the enter delete mode.
        /// </summary>
        public string EnterDeleteMode => GetString(TermInfoCaps.String.EnterDeleteMode);

        /// <summary>
        /// The enter_dim_mode [enter_dim_mode, dim] string capability is the turn on half bright mode.
        /// </summary>
        public string EnterDimMode => GetString(TermInfoCaps.String.EnterDimMode);

        /// <summary>
        /// The enter_insert_mode [enter_insert_mode, smir] string capability is the enter insert mode.
        /// </summary>
        public string EnterInsertMode => GetString(TermInfoCaps.String.EnterInsertMode);

        /// <summary>
        /// The enter_secure_mode [enter_secure_mode, invis] string capability is the turn on blank mode characters invisible.
        /// </summary>
        public string EnterSecureMode => GetString(TermInfoCaps.String.EnterSecureMode);

        /// <summary>
        /// The enter_protected_mode [enter_protected_mode, prot] string capability is the turn on protected mode.
        /// </summary>
        public string EnterProtectedMode => GetString(TermInfoCaps.String.EnterProtectedMode);

        /// <summary>
        /// The enter_reverse_mode [enter_reverse_mode, rev] string capability is the turn on reverse video mode.
        /// </summary>
        public string EnterReverseMode => GetString(TermInfoCaps.String.EnterReverseMode);

        /// <summary>
        /// The enter_standout_mode [enter_standout_mode, smso] string capability is the begin standout mode.
        /// </summary>
        public string EnterStandoutMode => GetString(TermInfoCaps.String.EnterStandoutMode);

        /// <summary>
        /// The enter_underline_mode [enter_underline_mode, smul] string capability is the begin underline mode.
        /// </summary>
        public string EnterUnderlineMode => GetString(TermInfoCaps.String.EnterUnderlineMode);

        /// <summary>
        /// The erase_chars [erase_chars, ech] string capability is the erase 1 characters p.
        /// </summary>
        public string EraseChars => GetString(TermInfoCaps.String.EraseChars);

        /// <summary>
        /// The exit_alt_charset_mode [exit_alt_charset_mode, rmacs] string capability is the end alternate character set p.
        /// </summary>
        public string ExitAltCharsetMode => GetString(TermInfoCaps.String.ExitAltCharsetMode);

        /// <summary>
        /// The exit_attribute_mode [exit_attribute_mode, sgr0] string capability is the turn off all attributes.
        /// </summary>
        public string ExitAttributeMode => GetString(TermInfoCaps.String.ExitAttributeMode);

        /// <summary>
        /// The exit_ca_mode [exit_ca_mode, rmcup] string capability is the strings to end programs using cup.
        /// </summary>
        public string ExitCaMode => GetString(TermInfoCaps.String.ExitCaMode);

        /// <summary>
        /// The exit_delete_mode [exit_delete_mode, rmdc] string capability is the end delete mode.
        /// </summary>
        public string ExitDeleteMode => GetString(TermInfoCaps.String.ExitDeleteMode);

        /// <summary>
        /// The exit_insert_mode [exit_insert_mode, rmir] string capability is the exit insert mode.
        /// </summary>
        public string ExitInsertMode => GetString(TermInfoCaps.String.ExitInsertMode);

        /// <summary>
        /// The exit_standout_mode [exit_standout_mode, rmso] string capability is the exit standout mode.
        /// </summary>
        public string ExitStandoutMode => GetString(TermInfoCaps.String.ExitStandoutMode);

        /// <summary>
        /// The exit_underline_mode [exit_underline_mode, rmul] string capability is the exit underline mode.
        /// </summary>
        public string ExitUnderlineMode => GetString(TermInfoCaps.String.ExitUnderlineMode);

        /// <summary>
        /// The flash_screen [flash_screen, flash] string capability is the visible bell may not move cursor.
        /// </summary>
        public string FlashScreen => GetString(TermInfoCaps.String.FlashScreen);

        /// <summary>
        /// The form_feed [form_feed, ff] string capability is the hardcopy terminal page eject p.
        /// </summary>
        public string FormFeed => GetString(TermInfoCaps.String.FormFeed);

        /// <summary>
        /// The from_status_line [from_status_line, fsl] string capability is the return from status line.
        /// </summary>
        public string FromStatusLine => GetString(TermInfoCaps.String.FromStatusLine);

        /// <summary>
        /// The init_1string [init_1string, is1] string capability is the initialization string.
        /// </summary>
        public string Init1string => GetString(TermInfoCaps.String.Init1string);

        /// <summary>
        /// The init_2string [init_2string, is2] string capability is the initialization string.
        /// </summary>
        public string Init2string => GetString(TermInfoCaps.String.Init2string);

        /// <summary>
        /// The init_3string [init_3string, is3] string capability is the initialization string.
        /// </summary>
        public string Init3string => GetString(TermInfoCaps.String.Init3string);

        /// <summary>
        /// The init_file [init_file, if] string capability is the name of initialization file.
        /// </summary>
        public string InitFile => GetString(TermInfoCaps.String.InitFile);

        /// <summary>
        /// The insert_character [insert_character, ich1] string capability is the insert character p.
        /// </summary>
        public string InsertCharacter => GetString(TermInfoCaps.String.InsertCharacter);

        /// <summary>
        /// The insert_line [insert_line, il1] string capability is the insert line p.
        /// </summary>
        public string InsertLine => GetString(TermInfoCaps.String.InsertLine);

        /// <summary>
        /// The insert_padding [insert_padding, ip] string capability is the insert padding after inserted character.
        /// </summary>
        public string InsertPadding => GetString(TermInfoCaps.String.InsertPadding);

        /// <summary>
        /// The key_backspace [key_backspace, kbs] string capability is the backspace key.
        /// </summary>
        public string KeyBackspace => GetString(TermInfoCaps.String.KeyBackspace);

        /// <summary>
        /// The key_catab [key_catab, ktbc] string capability is the clear all tabs key.
        /// </summary>
        public string KeyCatab => GetString(TermInfoCaps.String.KeyCatab);

        /// <summary>
        /// The key_clear [key_clear, kclr] string capability is the clear screen or erase key.
        /// </summary>
        public string KeyClear => GetString(TermInfoCaps.String.KeyClear);

        /// <summary>
        /// The key_ctab [key_ctab, kctab] string capability is the clear tab key.
        /// </summary>
        public string KeyCtab => GetString(TermInfoCaps.String.KeyCtab);

        /// <summary>
        /// The key_dc [key_dc, kdch1] string capability is the delete character key.
        /// </summary>
        public string KeyDc => GetString(TermInfoCaps.String.KeyDc);

        /// <summary>
        /// The key_dl [key_dl, kdl1] string capability is the delete line key.
        /// </summary>
        public string KeyDl => GetString(TermInfoCaps.String.KeyDl);

        /// <summary>
        /// The key_down [key_down, kcud1] string capability is the down arrow key.
        /// </summary>
        public string KeyDown => GetString(TermInfoCaps.String.KeyDown);

        /// <summary>
        /// The key_eic [key_eic, krmir] string capability is the sent by rmir or smir in insert mode.
        /// </summary>
        public string KeyEic => GetString(TermInfoCaps.String.KeyEic);

        /// <summary>
        /// The key_eol [key_eol, kel] string capability is the clear to end of line key.
        /// </summary>
        public string KeyEol => GetString(TermInfoCaps.String.KeyEol);

        /// <summary>
        /// The key_eos [key_eos, ked] string capability is the clear to end of screen key.
        /// </summary>
        public string KeyEos => GetString(TermInfoCaps.String.KeyEos);

        /// <summary>
        /// The key_f0 [key_f0, kf0] string capability is the f 0 function key.
        /// </summary>
        public string KeyF0 => GetString(TermInfoCaps.String.KeyF0);

        /// <summary>
        /// The key_f1 [key_f1, kf1] string capability is the f 1 function key.
        /// </summary>
        public string KeyF1 => GetString(TermInfoCaps.String.KeyF1);

        /// <summary>
        /// The key_f10 [key_f10, kf10] string capability is the f 10 function key.
        /// </summary>
        public string KeyF10 => GetString(TermInfoCaps.String.KeyF10);

        /// <summary>
        /// The key_f2 [key_f2, kf2] string capability is the f 2 function key.
        /// </summary>
        public string KeyF2 => GetString(TermInfoCaps.String.KeyF2);

        /// <summary>
        /// The key_f3 [key_f3, kf3] string capability is the f 3 function key.
        /// </summary>
        public string KeyF3 => GetString(TermInfoCaps.String.KeyF3);

        /// <summary>
        /// The key_f4 [key_f4, kf4] string capability is the f 4 function key.
        /// </summary>
        public string KeyF4 => GetString(TermInfoCaps.String.KeyF4);

        /// <summary>
        /// The key_f5 [key_f5, kf5] string capability is the f 5 function key.
        /// </summary>
        public string KeyF5 => GetString(TermInfoCaps.String.KeyF5);

        /// <summary>
        /// The key_f6 [key_f6, kf6] string capability is the f 6 function key.
        /// </summary>
        public string KeyF6 => GetString(TermInfoCaps.String.KeyF6);

        /// <summary>
        /// The key_f7 [key_f7, kf7] string capability is the f 7 function key.
        /// </summary>
        public string KeyF7 => GetString(TermInfoCaps.String.KeyF7);

        /// <summary>
        /// The key_f8 [key_f8, kf8] string capability is the f 8 function key.
        /// </summary>
        public string KeyF8 => GetString(TermInfoCaps.String.KeyF8);

        /// <summary>
        /// The key_f9 [key_f9, kf9] string capability is the f 9 function key.
        /// </summary>
        public string KeyF9 => GetString(TermInfoCaps.String.KeyF9);

        /// <summary>
        /// The key_home [key_home, khome] string capability is the home key.
        /// </summary>
        public string KeyHome => GetString(TermInfoCaps.String.KeyHome);

        /// <summary>
        /// The key_ic [key_ic, kich1] string capability is the insert character key.
        /// </summary>
        public string KeyIc => GetString(TermInfoCaps.String.KeyIc);

        /// <summary>
        /// The key_il [key_il, kil1] string capability is the insert line key.
        /// </summary>
        public string KeyIl => GetString(TermInfoCaps.String.KeyIl);

        /// <summary>
        /// The key_left [key_left, kcub1] string capability is the left arrow key.
        /// </summary>
        public string KeyLeft => GetString(TermInfoCaps.String.KeyLeft);

        /// <summary>
        /// The key_ll [key_ll, kll] string capability is the lower left key (home down).
        /// </summary>
        public string KeyLl => GetString(TermInfoCaps.String.KeyLl);

        /// <summary>
        /// The key_npage [key_npage, knp] string capability is the next page key.
        /// </summary>
        public string KeyNpage => GetString(TermInfoCaps.String.KeyNpage);

        /// <summary>
        /// The key_ppage [key_ppage, kpp] string capability is the previous page key.
        /// </summary>
        public string KeyPpage => GetString(TermInfoCaps.String.KeyPpage);

        /// <summary>
        /// The key_right [key_right, kcuf1] string capability is the right arrow key.
        /// </summary>
        public string KeyRight => GetString(TermInfoCaps.String.KeyRight);

        /// <summary>
        /// The key_sf [key_sf, kind] string capability is the scroll forward key.
        /// </summary>
        public string KeySf => GetString(TermInfoCaps.String.KeySf);

        /// <summary>
        /// The key_sr [key_sr, kri] string capability is the scroll backward key.
        /// </summary>
        public string KeySr => GetString(TermInfoCaps.String.KeySr);

        /// <summary>
        /// The key_stab [key_stab, khts] string capability is the set tab key.
        /// </summary>
        public string KeyStab => GetString(TermInfoCaps.String.KeyStab);

        /// <summary>
        /// The key_up [key_up, kcuu1] string capability is the up arrow key.
        /// </summary>
        public string KeyUp => GetString(TermInfoCaps.String.KeyUp);

        /// <summary>
        /// The keypad_local [keypad_local, rmkx] string capability is the leave 'keyboard transmit' mode.
        /// </summary>
        public string KeypadLocal => GetString(TermInfoCaps.String.KeypadLocal);

        /// <summary>
        /// The keypad_xmit [keypad_xmit, smkx] string capability is the enter 'keyboard transmit' mode.
        /// </summary>
        public string KeypadXmit => GetString(TermInfoCaps.String.KeypadXmit);

        /// <summary>
        /// The lab_f0 [lab_f0, lf0] string capability is the label on function key f 0 if not f 0.
        /// </summary>
        public string LabF0 => GetString(TermInfoCaps.String.LabF0);

        /// <summary>
        /// The lab_f1 [lab_f1, lf1] string capability is the label on function key f 1 if not f 1.
        /// </summary>
        public string LabF1 => GetString(TermInfoCaps.String.LabF1);

        /// <summary>
        /// The lab_f10 [lab_f10, lf10] string capability is the label on function key f 10 if not f 10.
        /// </summary>
        public string LabF10 => GetString(TermInfoCaps.String.LabF10);

        /// <summary>
        /// The lab_f2 [lab_f2, lf2] string capability is the label on function key f 2 if not f 2.
        /// </summary>
        public string LabF2 => GetString(TermInfoCaps.String.LabF2);

        /// <summary>
        /// The lab_f3 [lab_f3, lf3] string capability is the label on function key f 3 if not f 3.
        /// </summary>
        public string LabF3 => GetString(TermInfoCaps.String.LabF3);

        /// <summary>
        /// The lab_f4 [lab_f4, lf4] string capability is the label on function key f 4 if not f 4.
        /// </summary>
        public string LabF4 => GetString(TermInfoCaps.String.LabF4);

        /// <summary>
        /// The lab_f5 [lab_f5, lf5] string capability is the label on function key f 5 if not f 5.
        /// </summary>
        public string LabF5 => GetString(TermInfoCaps.String.LabF5);

        /// <summary>
        /// The lab_f6 [lab_f6, lf6] string capability is the label on function key f 6 if not f 6.
        /// </summary>
        public string LabF6 => GetString(TermInfoCaps.String.LabF6);

        /// <summary>
        /// The lab_f7 [lab_f7, lf7] string capability is the label on function key f 7 if not f 7.
        /// </summary>
        public string LabF7 => GetString(TermInfoCaps.String.LabF7);

        /// <summary>
        /// The lab_f8 [lab_f8, lf8] string capability is the label on function key f 8 if not f 8.
        /// </summary>
        public string LabF8 => GetString(TermInfoCaps.String.LabF8);

        /// <summary>
        /// The lab_f9 [lab_f9, lf9] string capability is the label on function key f 9 if not f 9.
        /// </summary>
        public string LabF9 => GetString(TermInfoCaps.String.LabF9);

        /// <summary>
        /// The meta_off [meta_off, rmm] string capability is the turn off meta mode.
        /// </summary>
        public string MetaOff => GetString(TermInfoCaps.String.MetaOff);

        /// <summary>
        /// The meta_on [meta_on, smm] string capability is the turn on meta mode (8th bit on).
        /// </summary>
        public string MetaOn => GetString(TermInfoCaps.String.MetaOn);

        /// <summary>
        /// The newline [newline, nel] string capability is the newline behave like cr followed by lf.
        /// </summary>
        public string Newline => GetString(TermInfoCaps.String.Newline);

        /// <summary>
        /// The pad_char [pad_char, pad] string capability is the padding char instead of null.
        /// </summary>
        public string PadChar => GetString(TermInfoCaps.String.PadChar);

        /// <summary>
        /// The parm_dch [parm_dch, dch] string capability is the delete 1 characters p.
        /// </summary>
        public string ParmDch => GetString(TermInfoCaps.String.ParmDch);

        /// <summary>
        /// The parm_delete_line [parm_delete_line, dl] string capability is the delete 1 lines p.
        /// </summary>
        public string ParmDeleteLine => GetString(TermInfoCaps.String.ParmDeleteLine);

        /// <summary>
        /// The parm_down_cursor [parm_down_cursor, cud] string capability is the down 1 lines p.
        /// </summary>
        public string ParmDownCursor => GetString(TermInfoCaps.String.ParmDownCursor);

        /// <summary>
        /// The parm_ich [parm_ich, ich] string capability is the insert 1 characters p.
        /// </summary>
        public string ParmIch => GetString(TermInfoCaps.String.ParmIch);

        /// <summary>
        /// The parm_index [parm_index, indn] string capability is the scroll forward 1 lines p.
        /// </summary>
        public string ParmIndex => GetString(TermInfoCaps.String.ParmIndex);

        /// <summary>
        /// The parm_insert_line [parm_insert_line, il] string capability is the insert 1 lines p.
        /// </summary>
        public string ParmInsertLine => GetString(TermInfoCaps.String.ParmInsertLine);

        /// <summary>
        /// The parm_left_cursor [parm_left_cursor, cub] string capability is the move 1 characters to the left p.
        /// </summary>
        public string ParmLeftCursor => GetString(TermInfoCaps.String.ParmLeftCursor);

        /// <summary>
        /// The parm_right_cursor [parm_right_cursor, cuf] string capability is the move 1 characters to the right p.
        /// </summary>
        public string ParmRightCursor => GetString(TermInfoCaps.String.ParmRightCursor);

        /// <summary>
        /// The parm_rindex [parm_rindex, rin] string capability is the scroll back 1 lines p.
        /// </summary>
        public string ParmRindex => GetString(TermInfoCaps.String.ParmRindex);

        /// <summary>
        /// The parm_up_cursor [parm_up_cursor, cuu] string capability is the up 1 lines p.
        /// </summary>
        public string ParmUpCursor => GetString(TermInfoCaps.String.ParmUpCursor);

        /// <summary>
        /// The pkey_key [pkey_key, pfkey] string capability is the program function key 1 to type string 2.
        /// </summary>
        public string PkeyKey => GetString(TermInfoCaps.String.PkeyKey);

        /// <summary>
        /// The pkey_local [pkey_local, pfloc] string capability is the program function key 1 to execute string 2.
        /// </summary>
        public string PkeyLocal => GetString(TermInfoCaps.String.PkeyLocal);

        /// <summary>
        /// The pkey_xmit [pkey_xmit, pfx] string capability is the program function key 1 to transmit string 2.
        /// </summary>
        public string PkeyXmit => GetString(TermInfoCaps.String.PkeyXmit);

        /// <summary>
        /// The print_screen [print_screen, mc0] string capability is the print contents of screen.
        /// </summary>
        public string PrintScreen => GetString(TermInfoCaps.String.PrintScreen);

        /// <summary>
        /// The prtr_off [prtr_off, mc4] string capability is the turn off printer.
        /// </summary>
        public string PrtrOff => GetString(TermInfoCaps.String.PrtrOff);

        /// <summary>
        /// The prtr_on [prtr_on, mc5] string capability is the turn on printer.
        /// </summary>
        public string PrtrOn => GetString(TermInfoCaps.String.PrtrOn);

        /// <summary>
        /// The repeat_char [repeat_char, rep] string capability is the repeat char 1 2 times p.
        /// </summary>
        public string RepeatChar => GetString(TermInfoCaps.String.RepeatChar);

        /// <summary>
        /// The reset_1string [reset_1string, rs1] string capability is the reset string.
        /// </summary>
        public string Reset1string => GetString(TermInfoCaps.String.Reset1string);

        /// <summary>
        /// The reset_2string [reset_2string, rs2] string capability is the reset string.
        /// </summary>
        public string Reset2string => GetString(TermInfoCaps.String.Reset2string);

        /// <summary>
        /// The reset_3string [reset_3string, rs3] string capability is the reset string.
        /// </summary>
        public string Reset3string => GetString(TermInfoCaps.String.Reset3string);

        /// <summary>
        /// The reset_file [reset_file, rf] string capability is the name of reset file.
        /// </summary>
        public string ResetFile => GetString(TermInfoCaps.String.ResetFile);

        /// <summary>
        /// The restore_cursor [restore_cursor, rc] string capability is the restore cursor to position of last save cursor.
        /// </summary>
        public string RestoreCursor => GetString(TermInfoCaps.String.RestoreCursor);

        /// <summary>
        /// The row_address [row_address, vpa] string capability is the vertical position 1 absolute p.
        /// </summary>
        public string RowAddress => GetString(TermInfoCaps.String.RowAddress);

        /// <summary>
        /// The save_cursor [save_cursor, sc] string capability is the save current cursor position p.
        /// </summary>
        public string SaveCursor => GetString(TermInfoCaps.String.SaveCursor);

        /// <summary>
        /// The scroll_forward [scroll_forward, ind] string capability is the scroll text up p.
        /// </summary>
        public string ScrollForward => GetString(TermInfoCaps.String.ScrollForward);

        /// <summary>
        /// The scroll_reverse [scroll_reverse, ri] string capability is the scroll text down p.
        /// </summary>
        public string ScrollReverse => GetString(TermInfoCaps.String.ScrollReverse);

        /// <summary>
        /// The set_attributes [set_attributes, sgr] string capability is the define video attributes #1 #9 (pg9).
        /// </summary>
        public string SetAttributes => GetString(TermInfoCaps.String.SetAttributes);

        /// <summary>
        /// The set_tab [set_tab, hts] string capability is the set a tab in every row current columns.
        /// </summary>
        public string SetTab => GetString(TermInfoCaps.String.SetTab);

        /// <summary>
        /// The set_window [set_window, wind] string capability is the current window is lines #1 #2 cols #3 #4.
        /// </summary>
        public string SetWindow => GetString(TermInfoCaps.String.SetWindow);

        /// <summary>
        /// The tab [tab, ht] string capability is the tab to next 8 space hardware tab stop.
        /// </summary>
        public string Tab => GetString(TermInfoCaps.String.Tab);

        /// <summary>
        /// The to_status_line [to_status_line, tsl] string capability is the move to status line column 1.
        /// </summary>
        public string ToStatusLine => GetString(TermInfoCaps.String.ToStatusLine);

        /// <summary>
        /// The underline_char [underline_char, uc] string capability is the underline char and move past it.
        /// </summary>
        public string UnderlineChar => GetString(TermInfoCaps.String.UnderlineChar);

        /// <summary>
        /// The up_half_line [up_half_line, hu] string capability is the half a line up.
        /// </summary>
        public string UpHalfLine => GetString(TermInfoCaps.String.UpHalfLine);

        /// <summary>
        /// The init_prog [init_prog, iprog] string capability is the path name of program for initialization.
        /// </summary>
        public string InitProg => GetString(TermInfoCaps.String.InitProg);

        /// <summary>
        /// The key_a1 [key_a1, ka1] string capability is the upper left of keypad.
        /// </summary>
        public string KeyA1 => GetString(TermInfoCaps.String.KeyA1);

        /// <summary>
        /// The key_a3 [key_a3, ka3] string capability is the upper right of keypad.
        /// </summary>
        public string KeyA3 => GetString(TermInfoCaps.String.KeyA3);

        /// <summary>
        /// The key_b2 [key_b2, kb2] string capability is the center of keypad.
        /// </summary>
        public string KeyB2 => GetString(TermInfoCaps.String.KeyB2);

        /// <summary>
        /// The key_c1 [key_c1, kc1] string capability is the lower left of keypad.
        /// </summary>
        public string KeyC1 => GetString(TermInfoCaps.String.KeyC1);

        /// <summary>
        /// The key_c3 [key_c3, kc3] string capability is the lower right of keypad.
        /// </summary>
        public string KeyC3 => GetString(TermInfoCaps.String.KeyC3);

        /// <summary>
        /// The prtr_non [prtr_non, mc5p] string capability is the turn on printer for 1 bytes.
        /// </summary>
        public string PrtrNon => GetString(TermInfoCaps.String.PrtrNon);

        /// <summary>
        /// The char_padding [char_padding, rmp] string capability is the like ip but when in insert mode.
        /// </summary>
        public string CharPadding => GetString(TermInfoCaps.String.CharPadding);

        /// <summary>
        /// The acs_chars [acs_chars, acsc] string capability is the graphics charset pairs based on vt 100.
        /// </summary>
        public string AcsChars => GetString(TermInfoCaps.String.AcsChars);

        /// <summary>
        /// The plab_norm [plab_norm, pln] string capability is the program label 1 to show string 2.
        /// </summary>
        public string PlabNorm => GetString(TermInfoCaps.String.PlabNorm);

        /// <summary>
        /// The key_btab [key_btab, kcbt] string capability is the back tab key.
        /// </summary>
        public string KeyBtab => GetString(TermInfoCaps.String.KeyBtab);

        /// <summary>
        /// The enter_xon_mode [enter_xon_mode, smxon] string capability is the turn on xon xoff handshaking.
        /// </summary>
        public string EnterXonMode => GetString(TermInfoCaps.String.EnterXonMode);

        /// <summary>
        /// The exit_xon_mode [exit_xon_mode, rmxon] string capability is the turn off xon xoff handshaking.
        /// </summary>
        public string ExitXonMode => GetString(TermInfoCaps.String.ExitXonMode);

        /// <summary>
        /// The enter_am_mode [enter_am_mode, smam] string capability is the turn on automatic margins.
        /// </summary>
        public string EnterAmMode => GetString(TermInfoCaps.String.EnterAmMode);

        /// <summary>
        /// The exit_am_mode [exit_am_mode, rmam] string capability is the turn off automatic margins.
        /// </summary>
        public string ExitAmMode => GetString(TermInfoCaps.String.ExitAmMode);

        /// <summary>
        /// The xon_character [xon_character, xonc] string capability is the xon character.
        /// </summary>
        public string XonCharacter => GetString(TermInfoCaps.String.XonCharacter);

        /// <summary>
        /// The xoff_character [xoff_character, xoffc] string capability is the xoff character.
        /// </summary>
        public string XoffCharacter => GetString(TermInfoCaps.String.XoffCharacter);

        /// <summary>
        /// The ena_acs [ena_acs, enacs] string capability is the enable alternate char set.
        /// </summary>
        public string EnaAcs => GetString(TermInfoCaps.String.EnaAcs);

        /// <summary>
        /// The label_on [label_on, smln] string capability is the turn on soft labels.
        /// </summary>
        public string LabelOn => GetString(TermInfoCaps.String.LabelOn);

        /// <summary>
        /// The label_off [label_off, rmln] string capability is the turn off soft labels.
        /// </summary>
        public string LabelOff => GetString(TermInfoCaps.String.LabelOff);

        /// <summary>
        /// The key_beg [key_beg, kbeg] string capability is the begin key.
        /// </summary>
        public string KeyBeg => GetString(TermInfoCaps.String.KeyBeg);

        /// <summary>
        /// The key_cancel [key_cancel, kcan] string capability is the cancel key.
        /// </summary>
        public string KeyCancel => GetString(TermInfoCaps.String.KeyCancel);

        /// <summary>
        /// The key_close [key_close, kclo] string capability is the close key.
        /// </summary>
        public string KeyClose => GetString(TermInfoCaps.String.KeyClose);

        /// <summary>
        /// The key_command [key_command, kcmd] string capability is the command key.
        /// </summary>
        public string KeyCommand => GetString(TermInfoCaps.String.KeyCommand);

        /// <summary>
        /// The key_copy [key_copy, kcpy] string capability is the copy key.
        /// </summary>
        public string KeyCopy => GetString(TermInfoCaps.String.KeyCopy);

        /// <summary>
        /// The key_create [key_create, kcrt] string capability is the create key.
        /// </summary>
        public string KeyCreate => GetString(TermInfoCaps.String.KeyCreate);

        /// <summary>
        /// The key_end [key_end, kend] string capability is the end key.
        /// </summary>
        public string KeyEnd => GetString(TermInfoCaps.String.KeyEnd);

        /// <summary>
        /// The key_enter [key_enter, kent] string capability is the enter send key.
        /// </summary>
        public string KeyEnter => GetString(TermInfoCaps.String.KeyEnter);

        /// <summary>
        /// The key_exit [key_exit, kext] string capability is the exit key.
        /// </summary>
        public string KeyExit => GetString(TermInfoCaps.String.KeyExit);

        /// <summary>
        /// The key_find [key_find, kfnd] string capability is the find key.
        /// </summary>
        public string KeyFind => GetString(TermInfoCaps.String.KeyFind);

        /// <summary>
        /// The key_help [key_help, khlp] string capability is the help key.
        /// </summary>
        public string KeyHelp => GetString(TermInfoCaps.String.KeyHelp);

        /// <summary>
        /// The key_mark [key_mark, kmrk] string capability is the mark key.
        /// </summary>
        public string KeyMark => GetString(TermInfoCaps.String.KeyMark);

        /// <summary>
        /// The key_message [key_message, kmsg] string capability is the message key.
        /// </summary>
        public string KeyMessage => GetString(TermInfoCaps.String.KeyMessage);

        /// <summary>
        /// The key_move [key_move, kmov] string capability is the move key.
        /// </summary>
        public string KeyMove => GetString(TermInfoCaps.String.KeyMove);

        /// <summary>
        /// The key_next [key_next, knxt] string capability is the next key.
        /// </summary>
        public string KeyNext => GetString(TermInfoCaps.String.KeyNext);

        /// <summary>
        /// The key_open [key_open, kopn] string capability is the open key.
        /// </summary>
        public string KeyOpen => GetString(TermInfoCaps.String.KeyOpen);

        /// <summary>
        /// The key_options [key_options, kopt] string capability is the options key.
        /// </summary>
        public string KeyOptions => GetString(TermInfoCaps.String.KeyOptions);

        /// <summary>
        /// The key_previous [key_previous, kprv] string capability is the previous key.
        /// </summary>
        public string KeyPrevious => GetString(TermInfoCaps.String.KeyPrevious);

        /// <summary>
        /// The key_print [key_print, kprt] string capability is the print key.
        /// </summary>
        public string KeyPrint => GetString(TermInfoCaps.String.KeyPrint);

        /// <summary>
        /// The key_redo [key_redo, krdo] string capability is the redo key.
        /// </summary>
        public string KeyRedo => GetString(TermInfoCaps.String.KeyRedo);

        /// <summary>
        /// The key_reference [key_reference, kref] string capability is the reference key.
        /// </summary>
        public string KeyReference => GetString(TermInfoCaps.String.KeyReference);

        /// <summary>
        /// The key_refresh [key_refresh, krfr] string capability is the refresh key.
        /// </summary>
        public string KeyRefresh => GetString(TermInfoCaps.String.KeyRefresh);

        /// <summary>
        /// The key_replace [key_replace, krpl] string capability is the replace key.
        /// </summary>
        public string KeyReplace => GetString(TermInfoCaps.String.KeyReplace);

        /// <summary>
        /// The key_restart [key_restart, krst] string capability is the restart key.
        /// </summary>
        public string KeyRestart => GetString(TermInfoCaps.String.KeyRestart);

        /// <summary>
        /// The key_resume [key_resume, kres] string capability is the resume key.
        /// </summary>
        public string KeyResume => GetString(TermInfoCaps.String.KeyResume);

        /// <summary>
        /// The key_save [key_save, ksav] string capability is the save key.
        /// </summary>
        public string KeySave => GetString(TermInfoCaps.String.KeySave);

        /// <summary>
        /// The key_suspend [key_suspend, kspd] string capability is the suspend key.
        /// </summary>
        public string KeySuspend => GetString(TermInfoCaps.String.KeySuspend);

        /// <summary>
        /// The key_undo [key_undo, kund] string capability is the undo key.
        /// </summary>
        public string KeyUndo => GetString(TermInfoCaps.String.KeyUndo);

        /// <summary>
        /// The key_sbeg [key_sbeg, kBEG] string capability is the shifted begin key.
        /// </summary>
        public string KeySbeg => GetString(TermInfoCaps.String.KeySbeg);

        /// <summary>
        /// The key_scancel [key_scancel, kCAN] string capability is the shifted cancel key.
        /// </summary>
        public string KeyScancel => GetString(TermInfoCaps.String.KeyScancel);

        /// <summary>
        /// The key_scommand [key_scommand, kCMD] string capability is the shifted command key.
        /// </summary>
        public string KeyScommand => GetString(TermInfoCaps.String.KeyScommand);

        /// <summary>
        /// The key_scopy [key_scopy, kCPY] string capability is the shifted copy key.
        /// </summary>
        public string KeyScopy => GetString(TermInfoCaps.String.KeyScopy);

        /// <summary>
        /// The key_screate [key_screate, kCRT] string capability is the shifted create key.
        /// </summary>
        public string KeyScreate => GetString(TermInfoCaps.String.KeyScreate);

        /// <summary>
        /// The key_sdc [key_sdc, kDC] string capability is the shifted delete character key.
        /// </summary>
        public string KeySdc => GetString(TermInfoCaps.String.KeySdc);

        /// <summary>
        /// The key_sdl [key_sdl, kDL] string capability is the shifted delete line key.
        /// </summary>
        public string KeySdl => GetString(TermInfoCaps.String.KeySdl);

        /// <summary>
        /// The key_select [key_select, kslt] string capability is the select key.
        /// </summary>
        public string KeySelect => GetString(TermInfoCaps.String.KeySelect);

        /// <summary>
        /// The key_send [key_send, kEND] string capability is the shifted end key.
        /// </summary>
        public string KeySend => GetString(TermInfoCaps.String.KeySend);

        /// <summary>
        /// The key_seol [key_seol, kEOL] string capability is the shifted clear to end of line key.
        /// </summary>
        public string KeySeol => GetString(TermInfoCaps.String.KeySeol);

        /// <summary>
        /// The key_sexit [key_sexit, kEXT] string capability is the shifted exit key.
        /// </summary>
        public string KeySexit => GetString(TermInfoCaps.String.KeySexit);

        /// <summary>
        /// The key_sfind [key_sfind, kFND] string capability is the shifted find key.
        /// </summary>
        public string KeySfind => GetString(TermInfoCaps.String.KeySfind);

        /// <summary>
        /// The key_shelp [key_shelp, kHLP] string capability is the shifted help key.
        /// </summary>
        public string KeyShelp => GetString(TermInfoCaps.String.KeyShelp);

        /// <summary>
        /// The key_shome [key_shome, kHOM] string capability is the shifted home key.
        /// </summary>
        public string KeyShome => GetString(TermInfoCaps.String.KeyShome);

        /// <summary>
        /// The key_sic [key_sic, kIC] string capability is the shifted insert character key.
        /// </summary>
        public string KeySic => GetString(TermInfoCaps.String.KeySic);

        /// <summary>
        /// The key_sleft [key_sleft, kLFT] string capability is the shifted left arrow key.
        /// </summary>
        public string KeySleft => GetString(TermInfoCaps.String.KeySleft);

        /// <summary>
        /// The key_smessage [key_smessage, kMSG] string capability is the shifted message key.
        /// </summary>
        public string KeySmessage => GetString(TermInfoCaps.String.KeySmessage);

        /// <summary>
        /// The key_smove [key_smove, kMOV] string capability is the shifted move key.
        /// </summary>
        public string KeySmove => GetString(TermInfoCaps.String.KeySmove);

        /// <summary>
        /// The key_snext [key_snext, kNXT] string capability is the shifted next key.
        /// </summary>
        public string KeySnext => GetString(TermInfoCaps.String.KeySnext);

        /// <summary>
        /// The key_soptions [key_soptions, kOPT] string capability is the shifted options key.
        /// </summary>
        public string KeySoptions => GetString(TermInfoCaps.String.KeySoptions);

        /// <summary>
        /// The key_sprevious [key_sprevious, kPRV] string capability is the shifted previous key.
        /// </summary>
        public string KeySprevious => GetString(TermInfoCaps.String.KeySprevious);

        /// <summary>
        /// The key_sprint [key_sprint, kPRT] string capability is the shifted print key.
        /// </summary>
        public string KeySprint => GetString(TermInfoCaps.String.KeySprint);

        /// <summary>
        /// The key_sredo [key_sredo, kRDO] string capability is the shifted redo key.
        /// </summary>
        public string KeySredo => GetString(TermInfoCaps.String.KeySredo);

        /// <summary>
        /// The key_sreplace [key_sreplace, kRPL] string capability is the shifted replace key.
        /// </summary>
        public string KeySreplace => GetString(TermInfoCaps.String.KeySreplace);

        /// <summary>
        /// The key_sright [key_sright, kRIT] string capability is the shifted right arrow key.
        /// </summary>
        public string KeySright => GetString(TermInfoCaps.String.KeySright);

        /// <summary>
        /// The key_srsume [key_srsume, kRES] string capability is the shifted resume key.
        /// </summary>
        public string KeySrsume => GetString(TermInfoCaps.String.KeySrsume);

        /// <summary>
        /// The key_ssave [key_ssave, kSAV] string capability is the shifted save key.
        /// </summary>
        public string KeySsave => GetString(TermInfoCaps.String.KeySsave);

        /// <summary>
        /// The key_ssuspend [key_ssuspend, kSPD] string capability is the shifted suspend key.
        /// </summary>
        public string KeySsuspend => GetString(TermInfoCaps.String.KeySsuspend);

        /// <summary>
        /// The key_sundo [key_sundo, kUND] string capability is the shifted undo key.
        /// </summary>
        public string KeySundo => GetString(TermInfoCaps.String.KeySundo);

        /// <summary>
        /// The req_for_input [req_for_input, rfi] string capability is the send next input char for ptys.
        /// </summary>
        public string ReqForInput => GetString(TermInfoCaps.String.ReqForInput);

        /// <summary>
        /// The key_f11 [key_f11, kf11] string capability is the f 11 function key.
        /// </summary>
        public string KeyF11 => GetString(TermInfoCaps.String.KeyF11);

        /// <summary>
        /// The key_f12 [key_f12, kf12] string capability is the f 12 function key.
        /// </summary>
        public string KeyF12 => GetString(TermInfoCaps.String.KeyF12);

        /// <summary>
        /// The key_f13 [key_f13, kf13] string capability is the f 13 function key.
        /// </summary>
        public string KeyF13 => GetString(TermInfoCaps.String.KeyF13);

        /// <summary>
        /// The key_f14 [key_f14, kf14] string capability is the f 14 function key.
        /// </summary>
        public string KeyF14 => GetString(TermInfoCaps.String.KeyF14);

        /// <summary>
        /// The key_f15 [key_f15, kf15] string capability is the f 15 function key.
        /// </summary>
        public string KeyF15 => GetString(TermInfoCaps.String.KeyF15);

        /// <summary>
        /// The key_f16 [key_f16, kf16] string capability is the f 16 function key.
        /// </summary>
        public string KeyF16 => GetString(TermInfoCaps.String.KeyF16);

        /// <summary>
        /// The key_f17 [key_f17, kf17] string capability is the f 17 function key.
        /// </summary>
        public string KeyF17 => GetString(TermInfoCaps.String.KeyF17);

        /// <summary>
        /// The key_f18 [key_f18, kf18] string capability is the f 18 function key.
        /// </summary>
        public string KeyF18 => GetString(TermInfoCaps.String.KeyF18);

        /// <summary>
        /// The key_f19 [key_f19, kf19] string capability is the f 19 function key.
        /// </summary>
        public string KeyF19 => GetString(TermInfoCaps.String.KeyF19);

        /// <summary>
        /// The key_f20 [key_f20, kf20] string capability is the f 20 function key.
        /// </summary>
        public string KeyF20 => GetString(TermInfoCaps.String.KeyF20);

        /// <summary>
        /// The key_f21 [key_f21, kf21] string capability is the f 21 function key.
        /// </summary>
        public string KeyF21 => GetString(TermInfoCaps.String.KeyF21);

        /// <summary>
        /// The key_f22 [key_f22, kf22] string capability is the f 22 function key.
        /// </summary>
        public string KeyF22 => GetString(TermInfoCaps.String.KeyF22);

        /// <summary>
        /// The key_f23 [key_f23, kf23] string capability is the f 23 function key.
        /// </summary>
        public string KeyF23 => GetString(TermInfoCaps.String.KeyF23);

        /// <summary>
        /// The key_f24 [key_f24, kf24] string capability is the f 24 function key.
        /// </summary>
        public string KeyF24 => GetString(TermInfoCaps.String.KeyF24);

        /// <summary>
        /// The key_f25 [key_f25, kf25] string capability is the f 25 function key.
        /// </summary>
        public string KeyF25 => GetString(TermInfoCaps.String.KeyF25);

        /// <summary>
        /// The key_f26 [key_f26, kf26] string capability is the f 26 function key.
        /// </summary>
        public string KeyF26 => GetString(TermInfoCaps.String.KeyF26);

        /// <summary>
        /// The key_f27 [key_f27, kf27] string capability is the f 27 function key.
        /// </summary>
        public string KeyF27 => GetString(TermInfoCaps.String.KeyF27);

        /// <summary>
        /// The key_f28 [key_f28, kf28] string capability is the f 28 function key.
        /// </summary>
        public string KeyF28 => GetString(TermInfoCaps.String.KeyF28);

        /// <summary>
        /// The key_f29 [key_f29, kf29] string capability is the f 29 function key.
        /// </summary>
        public string KeyF29 => GetString(TermInfoCaps.String.KeyF29);

        /// <summary>
        /// The key_f30 [key_f30, kf30] string capability is the f 30 function key.
        /// </summary>
        public string KeyF30 => GetString(TermInfoCaps.String.KeyF30);

        /// <summary>
        /// The key_f31 [key_f31, kf31] string capability is the f 31 function key.
        /// </summary>
        public string KeyF31 => GetString(TermInfoCaps.String.KeyF31);

        /// <summary>
        /// The key_f32 [key_f32, kf32] string capability is the f 32 function key.
        /// </summary>
        public string KeyF32 => GetString(TermInfoCaps.String.KeyF32);

        /// <summary>
        /// The key_f33 [key_f33, kf33] string capability is the f 33 function key.
        /// </summary>
        public string KeyF33 => GetString(TermInfoCaps.String.KeyF33);

        /// <summary>
        /// The key_f34 [key_f34, kf34] string capability is the f 34 function key.
        /// </summary>
        public string KeyF34 => GetString(TermInfoCaps.String.KeyF34);

        /// <summary>
        /// The key_f35 [key_f35, kf35] string capability is the f 35 function key.
        /// </summary>
        public string KeyF35 => GetString(TermInfoCaps.String.KeyF35);

        /// <summary>
        /// The key_f36 [key_f36, kf36] string capability is the f 36 function key.
        /// </summary>
        public string KeyF36 => GetString(TermInfoCaps.String.KeyF36);

        /// <summary>
        /// The key_f37 [key_f37, kf37] string capability is the f 37 function key.
        /// </summary>
        public string KeyF37 => GetString(TermInfoCaps.String.KeyF37);

        /// <summary>
        /// The key_f38 [key_f38, kf38] string capability is the f 38 function key.
        /// </summary>
        public string KeyF38 => GetString(TermInfoCaps.String.KeyF38);

        /// <summary>
        /// The key_f39 [key_f39, kf39] string capability is the f 39 function key.
        /// </summary>
        public string KeyF39 => GetString(TermInfoCaps.String.KeyF39);

        /// <summary>
        /// The key_f40 [key_f40, kf40] string capability is the f 40 function key.
        /// </summary>
        public string KeyF40 => GetString(TermInfoCaps.String.KeyF40);

        /// <summary>
        /// The key_f41 [key_f41, kf41] string capability is the f 41 function key.
        /// </summary>
        public string KeyF41 => GetString(TermInfoCaps.String.KeyF41);

        /// <summary>
        /// The key_f42 [key_f42, kf42] string capability is the f 42 function key.
        /// </summary>
        public string KeyF42 => GetString(TermInfoCaps.String.KeyF42);

        /// <summary>
        /// The key_f43 [key_f43, kf43] string capability is the f 43 function key.
        /// </summary>
        public string KeyF43 => GetString(TermInfoCaps.String.KeyF43);

        /// <summary>
        /// The key_f44 [key_f44, kf44] string capability is the f 44 function key.
        /// </summary>
        public string KeyF44 => GetString(TermInfoCaps.String.KeyF44);

        /// <summary>
        /// The key_f45 [key_f45, kf45] string capability is the f 45 function key.
        /// </summary>
        public string KeyF45 => GetString(TermInfoCaps.String.KeyF45);

        /// <summary>
        /// The key_f46 [key_f46, kf46] string capability is the f 46 function key.
        /// </summary>
        public string KeyF46 => GetString(TermInfoCaps.String.KeyF46);

        /// <summary>
        /// The key_f47 [key_f47, kf47] string capability is the f 47 function key.
        /// </summary>
        public string KeyF47 => GetString(TermInfoCaps.String.KeyF47);

        /// <summary>
        /// The key_f48 [key_f48, kf48] string capability is the f 48 function key.
        /// </summary>
        public string KeyF48 => GetString(TermInfoCaps.String.KeyF48);

        /// <summary>
        /// The key_f49 [key_f49, kf49] string capability is the f 49 function key.
        /// </summary>
        public string KeyF49 => GetString(TermInfoCaps.String.KeyF49);

        /// <summary>
        /// The key_f50 [key_f50, kf50] string capability is the f 50 function key.
        /// </summary>
        public string KeyF50 => GetString(TermInfoCaps.String.KeyF50);

        /// <summary>
        /// The key_f51 [key_f51, kf51] string capability is the f 51 function key.
        /// </summary>
        public string KeyF51 => GetString(TermInfoCaps.String.KeyF51);

        /// <summary>
        /// The key_f52 [key_f52, kf52] string capability is the f 52 function key.
        /// </summary>
        public string KeyF52 => GetString(TermInfoCaps.String.KeyF52);

        /// <summary>
        /// The key_f53 [key_f53, kf53] string capability is the f 53 function key.
        /// </summary>
        public string KeyF53 => GetString(TermInfoCaps.String.KeyF53);

        /// <summary>
        /// The key_f54 [key_f54, kf54] string capability is the f 54 function key.
        /// </summary>
        public string KeyF54 => GetString(TermInfoCaps.String.KeyF54);

        /// <summary>
        /// The key_f55 [key_f55, kf55] string capability is the f 55 function key.
        /// </summary>
        public string KeyF55 => GetString(TermInfoCaps.String.KeyF55);

        /// <summary>
        /// The key_f56 [key_f56, kf56] string capability is the f 56 function key.
        /// </summary>
        public string KeyF56 => GetString(TermInfoCaps.String.KeyF56);

        /// <summary>
        /// The key_f57 [key_f57, kf57] string capability is the f 57 function key.
        /// </summary>
        public string KeyF57 => GetString(TermInfoCaps.String.KeyF57);

        /// <summary>
        /// The key_f58 [key_f58, kf58] string capability is the f 58 function key.
        /// </summary>
        public string KeyF58 => GetString(TermInfoCaps.String.KeyF58);

        /// <summary>
        /// The key_f59 [key_f59, kf59] string capability is the f 59 function key.
        /// </summary>
        public string KeyF59 => GetString(TermInfoCaps.String.KeyF59);

        /// <summary>
        /// The key_f60 [key_f60, kf60] string capability is the f 60 function key.
        /// </summary>
        public string KeyF60 => GetString(TermInfoCaps.String.KeyF60);

        /// <summary>
        /// The key_f61 [key_f61, kf61] string capability is the f 61 function key.
        /// </summary>
        public string KeyF61 => GetString(TermInfoCaps.String.KeyF61);

        /// <summary>
        /// The key_f62 [key_f62, kf62] string capability is the f 62 function key.
        /// </summary>
        public string KeyF62 => GetString(TermInfoCaps.String.KeyF62);

        /// <summary>
        /// The key_f63 [key_f63, kf63] string capability is the f 63 function key.
        /// </summary>
        public string KeyF63 => GetString(TermInfoCaps.String.KeyF63);

        /// <summary>
        /// The clr_bol [clr_bol, el1] string capability is the clear to beginning of line.
        /// </summary>
        public string ClrBol => GetString(TermInfoCaps.String.ClrBol);

        /// <summary>
        /// The clear_margins [clear_margins, mgc] string capability is the clear right and left soft margins.
        /// </summary>
        public string ClearMargins => GetString(TermInfoCaps.String.ClearMargins);

        /// <summary>
        /// The set_left_margin [set_left_margin, smgl] string capability is the set left soft margin at current column.
        /// </summary>
        public string SetLeftMargin => GetString(TermInfoCaps.String.SetLeftMargin);

        /// <summary>
        /// The set_right_margin [set_right_margin, smgr] string capability is the set right soft margin at current column.
        /// </summary>
        public string SetRightMargin => GetString(TermInfoCaps.String.SetRightMargin);

        /// <summary>
        /// The label_format [label_format, fln] string capability is the label format.
        /// </summary>
        public string LabelFormat => GetString(TermInfoCaps.String.LabelFormat);

        /// <summary>
        /// The set_clock [set_clock, sclk] string capability is the set clock 1 hrs 2 mins 3 secs.
        /// </summary>
        public string SetClock => GetString(TermInfoCaps.String.SetClock);

        /// <summary>
        /// The display_clock [display_clock, dclk] string capability is the display clock.
        /// </summary>
        public string DisplayClock => GetString(TermInfoCaps.String.DisplayClock);

        /// <summary>
        /// The remove_clock [remove_clock, rmclk] string capability is the remove clock.
        /// </summary>
        public string RemoveClock => GetString(TermInfoCaps.String.RemoveClock);

        /// <summary>
        /// The create_window [create_window, cwin] string capability is the define a window 1 from 2 3 to 4 5.
        /// </summary>
        public string CreateWindow => GetString(TermInfoCaps.String.CreateWindow);

        /// <summary>
        /// The goto_window [goto_window, wingo] string capability is the go to window 1.
        /// </summary>
        public string GotoWindow => GetString(TermInfoCaps.String.GotoWindow);

        /// <summary>
        /// The hangup [hangup, hup] string capability is the hang up phone.
        /// </summary>
        public string Hangup => GetString(TermInfoCaps.String.Hangup);

        /// <summary>
        /// The dial_phone [dial_phone, dial] string capability is the dial number 1.
        /// </summary>
        public string DialPhone => GetString(TermInfoCaps.String.DialPhone);

        /// <summary>
        /// The quick_dial [quick_dial, qdial] string capability is the dial number 1 without checking.
        /// </summary>
        public string QuickDial => GetString(TermInfoCaps.String.QuickDial);

        /// <summary>
        /// The tone [tone, tone] string capability is the select touch tone dialing.
        /// </summary>
        public string Tone => GetString(TermInfoCaps.String.Tone);

        /// <summary>
        /// The pulse [pulse, pulse] string capability is the select pulse dialing.
        /// </summary>
        public string Pulse => GetString(TermInfoCaps.String.Pulse);

        /// <summary>
        /// The flash_hook [flash_hook, hook] string capability is the flash switch hook.
        /// </summary>
        public string FlashHook => GetString(TermInfoCaps.String.FlashHook);

        /// <summary>
        /// The fixed_pause [fixed_pause, pause] string capability is the pause for 2 3 seconds.
        /// </summary>
        public string FixedPause => GetString(TermInfoCaps.String.FixedPause);

        /// <summary>
        /// The wait_tone [wait_tone, wait] string capability is the wait for dial tone.
        /// </summary>
        public string WaitTone => GetString(TermInfoCaps.String.WaitTone);

        /// <summary>
        /// The user0 [user0, u0] string capability is the user string 0.
        /// </summary>
        public string User0 => GetString(TermInfoCaps.String.User0);

        /// <summary>
        /// The user1 [user1, u1] string capability is the user string 1.
        /// </summary>
        public string User1 => GetString(TermInfoCaps.String.User1);

        /// <summary>
        /// The user2 [user2, u2] string capability is the user string 2.
        /// </summary>
        public string User2 => GetString(TermInfoCaps.String.User2);

        /// <summary>
        /// The user3 [user3, u3] string capability is the user string 3.
        /// </summary>
        public string User3 => GetString(TermInfoCaps.String.User3);

        /// <summary>
        /// The user4 [user4, u4] string capability is the user string 4.
        /// </summary>
        public string User4 => GetString(TermInfoCaps.String.User4);

        /// <summary>
        /// The user5 [user5, u5] string capability is the user string 5.
        /// </summary>
        public string User5 => GetString(TermInfoCaps.String.User5);

        /// <summary>
        /// The user6 [user6, u6] string capability is the user string 6.
        /// </summary>
        public string User6 => GetString(TermInfoCaps.String.User6);

        /// <summary>
        /// The user7 [user7, u7] string capability is the user string 7.
        /// </summary>
        public string User7 => GetString(TermInfoCaps.String.User7);

        /// <summary>
        /// The user8 [user8, u8] string capability is the user string 8.
        /// </summary>
        public string User8 => GetString(TermInfoCaps.String.User8);

        /// <summary>
        /// The user9 [user9, u9] string capability is the user string 9.
        /// </summary>
        public string User9 => GetString(TermInfoCaps.String.User9);

        /// <summary>
        /// The orig_pair [orig_pair, op] string capability is the set default pair to its original value.
        /// </summary>
        public string OrigPair => GetString(TermInfoCaps.String.OrigPair);

        /// <summary>
        /// The orig_colors [orig_colors, oc] string capability is the set all color pairs to the original ones.
        /// </summary>
        public string OrigColors => GetString(TermInfoCaps.String.OrigColors);

        /// <summary>
        /// The initialize_color [initialize_color, initc] string capability is the initialize color 1 to 2 3 4.
        /// </summary>
        public string InitializeColor => GetString(TermInfoCaps.String.InitializeColor);

        /// <summary>
        /// The initialize_pair [initialize_pair, initp] string capability is the initialize color pair 1 to fg 2 3 4 bg 5 6 7.
        /// </summary>
        public string InitializePair => GetString(TermInfoCaps.String.InitializePair);

        /// <summary>
        /// The set_color_pair [set_color_pair, scp] string capability is the set current color pair to 1.
        /// </summary>
        public string SetColorPair => GetString(TermInfoCaps.String.SetColorPair);

        /// <summary>
        /// The set_foreground [set_foreground, setf] string capability is the set foreground color 1.
        /// </summary>
        public string SetForeground => GetString(TermInfoCaps.String.SetForeground);

        /// <summary>
        /// The set_background [set_background, setb] string capability is the set background color 1.
        /// </summary>
        public string SetBackground => GetString(TermInfoCaps.String.SetBackground);

        /// <summary>
        /// The change_char_pitch [change_char_pitch, cpi] string capability is the change number of characters per inch to 1.
        /// </summary>
        public string ChangeCharPitch => GetString(TermInfoCaps.String.ChangeCharPitch);

        /// <summary>
        /// The change_line_pitch [change_line_pitch, lpi] string capability is the change number of lines per inch to 1.
        /// </summary>
        public string ChangeLinePitch => GetString(TermInfoCaps.String.ChangeLinePitch);

        /// <summary>
        /// The change_res_horz [change_res_horz, chr] string capability is the change horizontal resolution to 1.
        /// </summary>
        public string ChangeResHorz => GetString(TermInfoCaps.String.ChangeResHorz);

        /// <summary>
        /// The change_res_vert [change_res_vert, cvr] string capability is the change vertical resolution to 1.
        /// </summary>
        public string ChangeResVert => GetString(TermInfoCaps.String.ChangeResVert);

        /// <summary>
        /// The define_char [define_char, defc] string capability is the define a character 1 2 dots wide descender 3.
        /// </summary>
        public string DefineChar => GetString(TermInfoCaps.String.DefineChar);

        /// <summary>
        /// The enter_doublewide_mode [enter_doublewide_mode, swidm] string capability is the enter double wide mode.
        /// </summary>
        public string EnterDoublewideMode => GetString(TermInfoCaps.String.EnterDoublewideMode);

        /// <summary>
        /// The enter_draft_quality [enter_draft_quality, sdrfq] string capability is the enter draft quality mode.
        /// </summary>
        public string EnterDraftQuality => GetString(TermInfoCaps.String.EnterDraftQuality);

        /// <summary>
        /// The enter_italics_mode [enter_italics_mode, sitm] string capability is the enter italic mode.
        /// </summary>
        public string EnterItalicsMode => GetString(TermInfoCaps.String.EnterItalicsMode);

        /// <summary>
        /// The enter_leftward_mode [enter_leftward_mode, slm] string capability is the start leftward carriage motion.
        /// </summary>
        public string EnterLeftwardMode => GetString(TermInfoCaps.String.EnterLeftwardMode);

        /// <summary>
        /// The enter_micro_mode [enter_micro_mode, smicm] string capability is the start micro motion mode.
        /// </summary>
        public string EnterMicroMode => GetString(TermInfoCaps.String.EnterMicroMode);

        /// <summary>
        /// The enter_near_letter_quality [enter_near_letter_quality, snlq] string capability is the enter nlq mode.
        /// </summary>
        public string EnterNearLetterQuality => GetString(TermInfoCaps.String.EnterNearLetterQuality);

        /// <summary>
        /// The enter_normal_quality [enter_normal_quality, snrmq] string capability is the enter normal quality mode.
        /// </summary>
        public string EnterNormalQuality => GetString(TermInfoCaps.String.EnterNormalQuality);

        /// <summary>
        /// The enter_shadow_mode [enter_shadow_mode, sshm] string capability is the enter shadow print mode.
        /// </summary>
        public string EnterShadowMode => GetString(TermInfoCaps.String.EnterShadowMode);

        /// <summary>
        /// The enter_subscript_mode [enter_subscript_mode, ssubm] string capability is the enter subscript mode.
        /// </summary>
        public string EnterSubscriptMode => GetString(TermInfoCaps.String.EnterSubscriptMode);

        /// <summary>
        /// The enter_superscript_mode [enter_superscript_mode, ssupm] string capability is the enter superscript mode.
        /// </summary>
        public string EnterSuperscriptMode => GetString(TermInfoCaps.String.EnterSuperscriptMode);

        /// <summary>
        /// The enter_upward_mode [enter_upward_mode, sum] string capability is the start upward carriage motion.
        /// </summary>
        public string EnterUpwardMode => GetString(TermInfoCaps.String.EnterUpwardMode);

        /// <summary>
        /// The exit_doublewide_mode [exit_doublewide_mode, rwidm] string capability is the end double wide mode.
        /// </summary>
        public string ExitDoublewideMode => GetString(TermInfoCaps.String.ExitDoublewideMode);

        /// <summary>
        /// The exit_italics_mode [exit_italics_mode, ritm] string capability is the end italic mode.
        /// </summary>
        public string ExitItalicsMode => GetString(TermInfoCaps.String.ExitItalicsMode);

        /// <summary>
        /// The exit_leftward_mode [exit_leftward_mode, rlm] string capability is the end left motion mode.
        /// </summary>
        public string ExitLeftwardMode => GetString(TermInfoCaps.String.ExitLeftwardMode);

        /// <summary>
        /// The exit_micro_mode [exit_micro_mode, rmicm] string capability is the end micro motion mode.
        /// </summary>
        public string ExitMicroMode => GetString(TermInfoCaps.String.ExitMicroMode);

        /// <summary>
        /// The exit_shadow_mode [exit_shadow_mode, rshm] string capability is the end shadow print mode.
        /// </summary>
        public string ExitShadowMode => GetString(TermInfoCaps.String.ExitShadowMode);

        /// <summary>
        /// The exit_subscript_mode [exit_subscript_mode, rsubm] string capability is the end subscript mode.
        /// </summary>
        public string ExitSubscriptMode => GetString(TermInfoCaps.String.ExitSubscriptMode);

        /// <summary>
        /// The exit_superscript_mode [exit_superscript_mode, rsupm] string capability is the end superscript mode.
        /// </summary>
        public string ExitSuperscriptMode => GetString(TermInfoCaps.String.ExitSuperscriptMode);

        /// <summary>
        /// The exit_upward_mode [exit_upward_mode, rum] string capability is the end reverse character motion.
        /// </summary>
        public string ExitUpwardMode => GetString(TermInfoCaps.String.ExitUpwardMode);

        /// <summary>
        /// The micro_column_address [micro_column_address, mhpa] string capability is the like column address in micro mode.
        /// </summary>
        public string MicroColumnAddress => GetString(TermInfoCaps.String.MicroColumnAddress);

        /// <summary>
        /// The micro_down [micro_down, mcud1] string capability is the like cursor down in micro mode.
        /// </summary>
        public string MicroDown => GetString(TermInfoCaps.String.MicroDown);

        /// <summary>
        /// The micro_left [micro_left, mcub1] string capability is the like cursor left in micro mode.
        /// </summary>
        public string MicroLeft => GetString(TermInfoCaps.String.MicroLeft);

        /// <summary>
        /// The micro_right [micro_right, mcuf1] string capability is the like cursor right in micro mode.
        /// </summary>
        public string MicroRight => GetString(TermInfoCaps.String.MicroRight);

        /// <summary>
        /// The micro_row_address [micro_row_address, mvpa] string capability is the like row address #1 in micro mode.
        /// </summary>
        public string MicroRowAddress => GetString(TermInfoCaps.String.MicroRowAddress);

        /// <summary>
        /// The micro_up [micro_up, mcuu1] string capability is the like cursor up in micro mode.
        /// </summary>
        public string MicroUp => GetString(TermInfoCaps.String.MicroUp);

        /// <summary>
        /// The order_of_pins [order_of_pins, porder] string capability is the match software bits to print head pins.
        /// </summary>
        public string OrderOfPins => GetString(TermInfoCaps.String.OrderOfPins);

        /// <summary>
        /// The parm_down_micro [parm_down_micro, mcud] string capability is the like parm down cursor in micro mode.
        /// </summary>
        public string ParmDownMicro => GetString(TermInfoCaps.String.ParmDownMicro);

        /// <summary>
        /// The parm_left_micro [parm_left_micro, mcub] string capability is the like parm left cursor in micro mode.
        /// </summary>
        public string ParmLeftMicro => GetString(TermInfoCaps.String.ParmLeftMicro);

        /// <summary>
        /// The parm_right_micro [parm_right_micro, mcuf] string capability is the like parm right cursor in micro mode.
        /// </summary>
        public string ParmRightMicro => GetString(TermInfoCaps.String.ParmRightMicro);

        /// <summary>
        /// The parm_up_micro [parm_up_micro, mcuu] string capability is the like parm up cursor in micro mode.
        /// </summary>
        public string ParmUpMicro => GetString(TermInfoCaps.String.ParmUpMicro);

        /// <summary>
        /// The select_char_set [select_char_set, scs] string capability is the select character set 1.
        /// </summary>
        public string SelectCharSet => GetString(TermInfoCaps.String.SelectCharSet);

        /// <summary>
        /// The set_bottom_margin [set_bottom_margin, smgb] string capability is the set bottom margin at current line.
        /// </summary>
        public string SetBottomMargin => GetString(TermInfoCaps.String.SetBottomMargin);

        /// <summary>
        /// The set_bottom_margin_parm [set_bottom_margin_parm, smgbp] string capability is the set bottom margin at line 1 or if smgtp is not given 2 lines from bottom.
        /// </summary>
        public string SetBottomMarginParm => GetString(TermInfoCaps.String.SetBottomMarginParm);

        /// <summary>
        /// The set_left_margin_parm [set_left_margin_parm, smglp] string capability is the set left right margin at column 1.
        /// </summary>
        public string SetLeftMarginParm => GetString(TermInfoCaps.String.SetLeftMarginParm);

        /// <summary>
        /// The set_right_margin_parm [set_right_margin_parm, smgrp] string capability is the set right margin at column 1.
        /// </summary>
        public string SetRightMarginParm => GetString(TermInfoCaps.String.SetRightMarginParm);

        /// <summary>
        /// The set_top_margin [set_top_margin, smgt] string capability is the set top margin at current line.
        /// </summary>
        public string SetTopMargin => GetString(TermInfoCaps.String.SetTopMargin);

        /// <summary>
        /// The set_top_margin_parm [set_top_margin_parm, smgtp] string capability is the set top bottom margin at row 1.
        /// </summary>
        public string SetTopMarginParm => GetString(TermInfoCaps.String.SetTopMarginParm);

        /// <summary>
        /// The start_bit_image [start_bit_image, sbim] string capability is the start printing bit image graphics.
        /// </summary>
        public string StartBitImage => GetString(TermInfoCaps.String.StartBitImage);

        /// <summary>
        /// The start_char_set_def [start_char_set_def, scsd] string capability is the start character set definition 1 with 2 characters in the set.
        /// </summary>
        public string StartCharSetDef => GetString(TermInfoCaps.String.StartCharSetDef);

        /// <summary>
        /// The stop_bit_image [stop_bit_image, rbim] string capability is the stop printing bit image graphics.
        /// </summary>
        public string StopBitImage => GetString(TermInfoCaps.String.StopBitImage);

        /// <summary>
        /// The stop_char_set_def [stop_char_set_def, rcsd] string capability is the end definition of character set 1.
        /// </summary>
        public string StopCharSetDef => GetString(TermInfoCaps.String.StopCharSetDef);

        /// <summary>
        /// The subscript_characters [subscript_characters, subcs] string capability is the list of subscriptable characters.
        /// </summary>
        public string SubscriptCharacters => GetString(TermInfoCaps.String.SubscriptCharacters);

        /// <summary>
        /// The superscript_characters [superscript_characters, supcs] string capability is the list of superscriptable characters.
        /// </summary>
        public string SuperscriptCharacters => GetString(TermInfoCaps.String.SuperscriptCharacters);

        /// <summary>
        /// The these_cause_cr [these_cause_cr, docr] string capability is the printing any of these characters causes cr.
        /// </summary>
        public string TheseCauseCr => GetString(TermInfoCaps.String.TheseCauseCr);

        /// <summary>
        /// The zero_motion [zero_motion, zerom] string capability is the no motion for subsequent character.
        /// </summary>
        public string ZeroMotion => GetString(TermInfoCaps.String.ZeroMotion);

        /// <summary>
        /// The char_set_names [char_set_names, csnm] string capability is the produce 1 th item from list of character set names.
        /// </summary>
        public string CharSetNames => GetString(TermInfoCaps.String.CharSetNames);

        /// <summary>
        /// The key_mouse [key_mouse, kmous] string capability is the mouse event has occurred.
        /// </summary>
        public string KeyMouse => GetString(TermInfoCaps.String.KeyMouse);

        /// <summary>
        /// The mouse_info [mouse_info, minfo] string capability is the mouse status information.
        /// </summary>
        public string MouseInfo => GetString(TermInfoCaps.String.MouseInfo);

        /// <summary>
        /// The req_mouse_pos [req_mouse_pos, reqmp] string capability is the request mouse position.
        /// </summary>
        public string ReqMousePos => GetString(TermInfoCaps.String.ReqMousePos);

        /// <summary>
        /// The get_mouse [get_mouse, getm] string capability is the curses should get button events parameter 1 not documented.
        /// </summary>
        public string GetMouse => GetString(TermInfoCaps.String.GetMouse);

        /// <summary>
        /// The set_a_foreground [set_a_foreground, setaf] string capability is the set foreground color to 1 using ansi escape.
        /// </summary>
        public string SetAForeground => GetString(TermInfoCaps.String.SetAForeground);

        /// <summary>
        /// The set_a_background [set_a_background, setab] string capability is the set background color to 1 using ansi escape.
        /// </summary>
        public string SetABackground => GetString(TermInfoCaps.String.SetABackground);

        /// <summary>
        /// The pkey_plab [pkey_plab, pfxl] string capability is the program function key 1 to type string 2 and show string 3.
        /// </summary>
        public string PkeyPlab => GetString(TermInfoCaps.String.PkeyPlab);

        /// <summary>
        /// The device_type [device_type, devt] string capability is the indicate language codeset support.
        /// </summary>
        public string DeviceType => GetString(TermInfoCaps.String.DeviceType);

        /// <summary>
        /// The code_set_init [code_set_init, csin] string capability is the init sequence for multiple codesets.
        /// </summary>
        public string CodeSetInit => GetString(TermInfoCaps.String.CodeSetInit);

        /// <summary>
        /// The set0_des_seq [set0_des_seq, s0ds] string capability is the shift to codeset 0 euc set 0 ascii.
        /// </summary>
        public string Set0DesSeq => GetString(TermInfoCaps.String.Set0DesSeq);

        /// <summary>
        /// The set1_des_seq [set1_des_seq, s1ds] string capability is the shift to codeset 1.
        /// </summary>
        public string Set1DesSeq => GetString(TermInfoCaps.String.Set1DesSeq);

        /// <summary>
        /// The set2_des_seq [set2_des_seq, s2ds] string capability is the shift to codeset 2.
        /// </summary>
        public string Set2DesSeq => GetString(TermInfoCaps.String.Set2DesSeq);

        /// <summary>
        /// The set3_des_seq [set3_des_seq, s3ds] string capability is the shift to codeset 3.
        /// </summary>
        public string Set3DesSeq => GetString(TermInfoCaps.String.Set3DesSeq);

        /// <summary>
        /// The set_lr_margin [set_lr_margin, smglr] string capability is the set both left and right margins to 1 2 ml is not in bsd termcap.
        /// </summary>
        public string SetLrMargin => GetString(TermInfoCaps.String.SetLrMargin);

        /// <summary>
        /// The set_tb_margin [set_tb_margin, smgtb] string capability is the sets both top and bottom margins to 1 2.
        /// </summary>
        public string SetTbMargin => GetString(TermInfoCaps.String.SetTbMargin);

        /// <summary>
        /// The bit_image_repeat [bit_image_repeat, birep] string capability is the repeat bit image cell 1 2 times.
        /// </summary>
        public string BitImageRepeat => GetString(TermInfoCaps.String.BitImageRepeat);

        /// <summary>
        /// The bit_image_newline [bit_image_newline, binel] string capability is the move to next row of the bit image.
        /// </summary>
        public string BitImageNewline => GetString(TermInfoCaps.String.BitImageNewline);

        /// <summary>
        /// The bit_image_carriage_return [bit_image_carriage_return, bicr] string capability is the move to beginning of same row.
        /// </summary>
        public string BitImageCarriageReturn => GetString(TermInfoCaps.String.BitImageCarriageReturn);

        /// <summary>
        /// The color_names [color_names, colornm] string capability is the give name for color 1.
        /// </summary>
        public string ColorNames => GetString(TermInfoCaps.String.ColorNames);

        /// <summary>
        /// The define_bit_image_region [define_bit_image_region, defbi] string capability is the define rectangular bit image region.
        /// </summary>
        public string DefineBitImageRegion => GetString(TermInfoCaps.String.DefineBitImageRegion);

        /// <summary>
        /// The end_bit_image_region [end_bit_image_region, endbi] string capability is the end a bit image region.
        /// </summary>
        public string EndBitImageRegion => GetString(TermInfoCaps.String.EndBitImageRegion);

        /// <summary>
        /// The set_color_band [set_color_band, setcolor] string capability is the change to ribbon color 1.
        /// </summary>
        public string SetColorBand => GetString(TermInfoCaps.String.SetColorBand);

        /// <summary>
        /// The set_page_length [set_page_length, slines] string capability is the set page length to 1 lines.
        /// </summary>
        public string SetPageLength => GetString(TermInfoCaps.String.SetPageLength);

        /// <summary>
        /// The display_pc_char [display_pc_char, dispc] string capability is the display pc character 1.
        /// </summary>
        public string DisplayPcChar => GetString(TermInfoCaps.String.DisplayPcChar);

        /// <summary>
        /// The enter_pc_charset_mode [enter_pc_charset_mode, smpch] string capability is the enter pc character display mode.
        /// </summary>
        public string EnterPcCharsetMode => GetString(TermInfoCaps.String.EnterPcCharsetMode);

        /// <summary>
        /// The exit_pc_charset_mode [exit_pc_charset_mode, rmpch] string capability is the exit pc character display mode.
        /// </summary>
        public string ExitPcCharsetMode => GetString(TermInfoCaps.String.ExitPcCharsetMode);

        /// <summary>
        /// The enter_scancode_mode [enter_scancode_mode, smsc] string capability is the enter pc scancode mode.
        /// </summary>
        public string EnterScancodeMode => GetString(TermInfoCaps.String.EnterScancodeMode);

        /// <summary>
        /// The exit_scancode_mode [exit_scancode_mode, rmsc] string capability is the exit pc scancode mode.
        /// </summary>
        public string ExitScancodeMode => GetString(TermInfoCaps.String.ExitScancodeMode);

        /// <summary>
        /// The pc_term_options [pc_term_options, pctrm] string capability is the pc terminal options.
        /// </summary>
        public string PcTermOptions => GetString(TermInfoCaps.String.PcTermOptions);

        /// <summary>
        /// The scancode_escape [scancode_escape, scesc] string capability is the escape for scancode emulation.
        /// </summary>
        public string ScancodeEscape => GetString(TermInfoCaps.String.ScancodeEscape);

        /// <summary>
        /// The alt_scancode_esc [alt_scancode_esc, scesa] string capability is the alternate escape for scancode emulation.
        /// </summary>
        public string AltScancodeEsc => GetString(TermInfoCaps.String.AltScancodeEsc);

        /// <summary>
        /// The enter_horizontal_hl_mode [enter_horizontal_hl_mode, ehhlm] string capability is the enter horizontal highlight mode.
        /// </summary>
        public string EnterHorizontalHlMode => GetString(TermInfoCaps.String.EnterHorizontalHlMode);

        /// <summary>
        /// The enter_left_hl_mode [enter_left_hl_mode, elhlm] string capability is the enter left highlight mode.
        /// </summary>
        public string EnterLeftHlMode => GetString(TermInfoCaps.String.EnterLeftHlMode);

        /// <summary>
        /// The enter_low_hl_mode [enter_low_hl_mode, elohlm] string capability is the enter low highlight mode.
        /// </summary>
        public string EnterLowHlMode => GetString(TermInfoCaps.String.EnterLowHlMode);

        /// <summary>
        /// The enter_right_hl_mode [enter_right_hl_mode, erhlm] string capability is the enter right highlight mode.
        /// </summary>
        public string EnterRightHlMode => GetString(TermInfoCaps.String.EnterRightHlMode);

        /// <summary>
        /// The enter_top_hl_mode [enter_top_hl_mode, ethlm] string capability is the enter top highlight mode.
        /// </summary>
        public string EnterTopHlMode => GetString(TermInfoCaps.String.EnterTopHlMode);

        /// <summary>
        /// The enter_vertical_hl_mode [enter_vertical_hl_mode, evhlm] string capability is the enter vertical highlight mode.
        /// </summary>
        public string EnterVerticalHlMode => GetString(TermInfoCaps.String.EnterVerticalHlMode);

        /// <summary>
        /// The set_a_attributes [set_a_attributes, sgr1] string capability is the define second set of video attributes #1 #6.
        /// </summary>
        public string SetAAttributes => GetString(TermInfoCaps.String.SetAAttributes);

        /// <summary>
        /// The set_pglen_inch [set_pglen_inch, slength] string capability is the set page length to 1 hundredth of an inch some implementations use s l for termcap.
        /// </summary>
        public string SetPglenInch => GetString(TermInfoCaps.String.SetPglenInch);

        /// <summary>
        /// The termcap_init2 [termcap_init2, OTi2] string capability is the secondary initialization string.
        /// </summary>
        public string TermcapInit2 => GetString(TermInfoCaps.String.TermcapInit2);

        /// <summary>
        /// The termcap_reset [termcap_reset, OTrs] string capability is the terminal reset string.
        /// </summary>
        public string TermcapReset => GetString(TermInfoCaps.String.TermcapReset);

        /// <summary>
        /// The linefeed_if_not_lf [linefeed_if_not_lf, OTnl] string capability is the use to move down.
        /// </summary>
        public string LinefeedIfNotLf => GetString(TermInfoCaps.String.LinefeedIfNotLf);

        /// <summary>
        /// The backspace_if_not_bs [backspace_if_not_bs, OTbc] string capability is the move left if not h.
        /// </summary>
        public string BackspaceIfNotBs => GetString(TermInfoCaps.String.BackspaceIfNotBs);

        /// <summary>
        /// The other_non_function_keys [other_non_function_keys, OTko] string capability is the list of self mapped keycaps.
        /// </summary>
        public string OtherNonFunctionKeys => GetString(TermInfoCaps.String.OtherNonFunctionKeys);

        /// <summary>
        /// The arrow_key_map [arrow_key_map, OTma] string capability is the map motion keys for vi version 2.
        /// </summary>
        public string ArrowKeyMap => GetString(TermInfoCaps.String.ArrowKeyMap);

        /// <summary>
        /// The acs_ulcorner [acs_ulcorner, OTG2] string capability is the single upper left.
        /// </summary>
        public string AcsUlcorner => GetString(TermInfoCaps.String.AcsUlcorner);

        /// <summary>
        /// The acs_llcorner [acs_llcorner, OTG3] string capability is the single lower left.
        /// </summary>
        public string AcsLlcorner => GetString(TermInfoCaps.String.AcsLlcorner);

        /// <summary>
        /// The acs_urcorner [acs_urcorner, OTG1] string capability is the single upper right.
        /// </summary>
        public string AcsUrcorner => GetString(TermInfoCaps.String.AcsUrcorner);

        /// <summary>
        /// The acs_lrcorner [acs_lrcorner, OTG4] string capability is the single lower right.
        /// </summary>
        public string AcsLrcorner => GetString(TermInfoCaps.String.AcsLrcorner);

        /// <summary>
        /// The acs_ltee [acs_ltee, OTGR] string capability is the tee pointing right.
        /// </summary>
        public string AcsLtee => GetString(TermInfoCaps.String.AcsLtee);

        /// <summary>
        /// The acs_rtee [acs_rtee, OTGL] string capability is the tee pointing left.
        /// </summary>
        public string AcsRtee => GetString(TermInfoCaps.String.AcsRtee);

        /// <summary>
        /// The acs_btee [acs_btee, OTGU] string capability is the tee pointing up.
        /// </summary>
        public string AcsBtee => GetString(TermInfoCaps.String.AcsBtee);

        /// <summary>
        /// The acs_ttee [acs_ttee, OTGD] string capability is the tee pointing down.
        /// </summary>
        public string AcsTtee => GetString(TermInfoCaps.String.AcsTtee);

        /// <summary>
        /// The acs_hline [acs_hline, OTGH] string capability is the single horizontal line.
        /// </summary>
        public string AcsHline => GetString(TermInfoCaps.String.AcsHline);

        /// <summary>
        /// The acs_vline [acs_vline, OTGV] string capability is the single vertical line.
        /// </summary>
        public string AcsVline => GetString(TermInfoCaps.String.AcsVline);

        /// <summary>
        /// The acs_plus [acs_plus, OTGC] string capability is the single intersection.
        /// </summary>
        public string AcsPlus => GetString(TermInfoCaps.String.AcsPlus);

        /// <summary>
        /// The memory_lock [memory_lock, meml] string capability is the lock memory above cursor.
        /// </summary>
        public string MemoryLock => GetString(TermInfoCaps.String.MemoryLock);

        /// <summary>
        /// The memory_unlock [memory_unlock, memu] string capability is the unlock memory.
        /// </summary>
        public string MemoryUnlock => GetString(TermInfoCaps.String.MemoryUnlock);

        /// <summary>
        /// The box_chars_1 [box_chars_1, box1] string capability is the box characters primary set.
        /// </summary>
        public string BoxChars1 => GetString(TermInfoCaps.String.BoxChars1);
    }
}
