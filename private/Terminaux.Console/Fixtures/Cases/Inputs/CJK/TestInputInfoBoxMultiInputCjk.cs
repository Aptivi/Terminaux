﻿//
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

using System.Linq;
using Terminaux.Inputs;
using Terminaux.Inputs.Modules;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Console.Fixtures.Cases.Inputs.CJK
{
    internal class TestInputInfoBoxMultiInputCjk : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Input;

        public void RunFixture()
        {
            Input.EnableMouse = true;

            var modules = new InputModule[]
            {
                new TextBoxModule()
                {
                    Name = "文本框整数",
                    Description = "输入任意整数以继续",
                },
                new TextBoxModule()
                {
                    Name = "文本框字符串",
                    Description = "输入任意字符串以继续",
                },
                new MaskedTextBoxModule()
                {
                    Name = "文本框密码",
                    Description = "输入任意密码继续",
                },
                new SliderBoxModule()
                {
                    Name = "选择一个号码",
                    Description = "选择一个介于 75 和 250 之间的数字。从 100 开始。",
                    MinPos = 75,
                    MaxPos = 250,
                    Value = 100,
                },
            };
            InfoBoxMultiInputColor.WriteInfoBoxMultiInput(modules, "选择要测试的输入模块...", new InfoBoxSettings()
            {
                Title = nameof(TestInputInfoBoxMultiInputCjk)
            });
            string[] rendered = [.. modules.Select((im) => im.Value?.ToString() ?? "")];
            TextWriterWhereColor.WriteWhere("  - " + string.Join("\n  - ", rendered), 0, 0);
            Input.EnableMouse = false;
        }
    }
}
