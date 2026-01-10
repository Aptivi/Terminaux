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

using System.Linq;
using Terminaux.Inputs;
using Terminaux.Inputs.Modules;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Console.Fixtures.Cases.Inputs.CJK
{
    internal class TestInputInfoBoxMultiInputSelectCjk : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Input;

        public void RunFixture()
        {
            Input.EnableMouse = true;

            var modules = new InputModule[]
            {
                new ComboBoxModule()
                {
                    Name = "EDM音乐流派",
                    Description = "选择一种 EDM 音乐类型继续",
                    Choices =
                    [
                        new("Selection infobox", [new("Available options",
                        [
                            new("酸浩室", ""),
                            new("大房间", ""),
                            new("深宅", ""),
                            new("鼓打贝斯", ""),
                            new("房子", ""),
                            new("科技", ""),
                            new("迷幻音乐", ""),
                        ])])
                    ],
                    Value = 6,
                },
                new ComboBoxModule()
                {
                    Name = "首选操作系统",
                    Description = "选择您喜欢的操作系统以继续",
                    Choices =
                    [
                        new("Selection infobox", [new("Available options",
                        [
                            new("Windows", ""),
                            new("macOS", ""),
                            new("Linux", ""),
                        ])])
                    ],
                },
                new MultiComboBoxModule()
                {
                    Name = "最喜欢的颜色",
                    Description = "选择您喜欢的颜色",
                    Choices =
                    [
                        new("Selection infobox", [new("Available options",
                        [
                            new("绿色的", ""),
                            new("蓝色的", ""),
                            new("红色的", ""),
                            new("紫红色", ""),
                            new("水蓝色", ""),
                            new("橙子", ""),
                            new("黑色的", ""),
                            new("白色的", ""),
                        ])])
                    ],
                },
            };
            InfoBoxMultiInputColor.WriteInfoBoxMultiInput(modules, "选择要测试的输入模块...", new InfoBoxSettings()
            {
                Title = nameof(TestInputInfoBoxMultiInputSelectCjk)
            });
            string[] rendered = [.. modules.Select((im) => im.Value is int[] indexes ? string.Join(", ", indexes.Select((idx) => ((MultiComboBoxModule)modules[2]).Choices[0].Groups[0].Choices[idx].ChoiceName)) : im.Value?.ToString() ?? "")];
            TextWriterWhereColor.WriteWhere("  - " + string.Join("\n  - ", rendered), 0, 0);
            Input.EnableMouse = false;
        }
    }
}
