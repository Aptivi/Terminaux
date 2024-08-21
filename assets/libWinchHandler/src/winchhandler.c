
// SpecProbe  Copyright (C) 2023  Aptivi
//
// This file is part of SpecProbe
//
// SpecProbe is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// SpecProbe is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

/* -------------------------------------------------------------------- */

#include <stdio.h>
#include <signal.h>
#include <string.h>
#include <stdlib.h>
#include <stdio.h>
#include <unistd.h>

/* -------------------------------------------------------------------- */

static void (*sigwinch_callback)(int);

/* -------------------------------------------------------------------- */

void
winchhandler_handle
(
    int sig,
    siginfo_t* info,
    void* context
)
/*
 * -----------------------------------------------------------------------
 * Name        : winchhandler_handle
 * Description : Handles SIGWINCH
 * -----------------------------------------------------------------------
 * Arguments   : [Out] Callback of the handler information that calls
 *                     HandleResize()
 * Returning   : 0 if successful; -1 if failure
 * -----------------------------------------------------------------------
 * Exposure    : Exposed to the Terminaux managed world
 * -----------------------------------------------------------------------
 */
{
    if (sig != SIGWINCH)
        return;
    sigwinch_callback(sig);
}

int
    winchhandler_start
    (
        void *winch_callback
    )
/*
 * -----------------------------------------------------------------------
 * Name        : winchhandler_start
 * Description : Starts the SIGWINCH signal handler
 * -----------------------------------------------------------------------
 * Arguments   : [Out] Callback of the handler information that calls
 *                     HandleResize()
 * Returning   : 0 if successful; -1 if failure
 * -----------------------------------------------------------------------
 * Exposure    : Exposed to the Terminaux managed world
 * -----------------------------------------------------------------------
 */
{
    sigwinch_callback = (void(*)(int))winch_callback;

    struct sigaction sigact = { 0 };
    sigemptyset(&sigact.sa_mask);

    sigact.sa_flags = SA_SIGINFO;
    sigact.sa_sigaction = winchhandler_handle;
    return sigaction(SIGWINCH, &sigact, NULL);
}
