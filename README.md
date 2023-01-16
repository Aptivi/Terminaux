# TermRead

TermRead is a next-generation console reader that allows you to flexibly write your input to the console without any limitations.

## Shortcut Guide

_Note: Some keys conflict with terminal emulator keybindings._

| Keybinding                         | Action                                               |
|:-----------------------------------|:-----------------------------------------------------|
| `Ctrl`+`A` / `HOME`                | Beginning of line                                    |
| `Ctrl`+`E` / `END`                 | End of line                                          |
| `Ctrl`+`B` / `←`                   | Backward one character                               |
| `Ctrl`+`F` / `→`                   | Forward one character                                |
| `BACKSPACE`                        | Remove one character from the left                   |
| `UP ARROW`                         | Get the older input                                  |
| `DOWN ARROW`                       | Get the newer input                                  |
| `DELETE`                           | Remove one character in current position             |
| `Alt`+`B`                          | One word backwards                                   |
| `Alt`+`F`                          | One word forwards                                    |
| `TAB`                              | Next auto-completion entry                           |
| `SHIFT`+`TAB`                      | Previous auto-completion entry                       |
| `CTRL`+`U`                         | Cut to the start of the line                         |
| `CTRL`+`K`                         | Cut to the end of the line                           |
| `CTRL`+`W`                         | Cut to the end of the previous word                  |
| `ALT`+`D`                          | Cut to the end of the next word                      |
| `CTRL`+`Y`                         | Yank the cut content                                 |

## Installation

Available on [NuGet](https://www.nuget.org/packages/TermRead/)

Visual Studio:

```powershell
PM> Install-Package TermRead
```

.NET:

```bash
dotnet add package TermRead
```

## Usage

This section shows you how to use this library to read the lines and manage history. Just add this to the top of the source file you want to use TermRead on:

```csharp
using TermRead;
```

### Input

_Note: The `(prompt>)` is optional_

#### Read input

```csharp
string input = TermReader.Read("(prompt)> ");
```

## License

```
MIT License

Copyright (c) 2022-2023 Aptivi

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```
