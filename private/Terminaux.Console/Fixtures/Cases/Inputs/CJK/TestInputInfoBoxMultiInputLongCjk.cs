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

using System.Linq;
using Terminaux.Inputs;
using Terminaux.Inputs.Modules;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Console.Fixtures.Cases.Inputs.CJK
{
    internal class TestInputInfoBoxMultiInputLongCjk : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Input;

        public void RunFixture()
        {
            Input.EnableMouse = true;

            var modules = new InputModule[]
            {
                new TextBoxModule()
                {
                    Name = "姓名",
                    Description = "写下这个人的名字",
                },
                new TextBoxModule()
                {
                    Name = "电子邮件",
                    Description = "写下此人的电子邮件地址",
                },
                new TextBoxModule()
                {
                    Name = "电子邮件办公地址",
                    Description = "写下此人公司的电子邮件地址",
                },
                new TextBoxModule()
                {
                    Name = "性别",
                    Description = "这个人是男性、女性还是其他性别？",
                },
                new TextBoxModule()
                {
                    Name = "出生日期",
                    Description = "这个人的生日是什么时候？",
                },
                new TextBoxModule()
                {
                    Name = "电话号码",
                    Description = "您使用什么电话号码联系他们？",
                },
                new TextBoxModule()
                {
                    Name = "地点",
                    Description = "它们目前位于什么位置？",
                },
                new TextBoxModule()
                {
                    Name = "商业",
                    Description = "写下他们目前正在经营的业务的名称",
                },
                new TextBoxModule()
                {
                    Name = "网站",
                    Description = "写出指向个人博客或网站的网站 URL",
                },
                new TextBoxModule()
                {
                    Name = "兴趣",
                    Description = "写下此人的兴趣",
                },
                new TextBoxModule()
                {
                    Name = "爱好",
                    Description = "写下这个人的爱好",
                },
                new TextBoxModule()
                {
                    Name = "体验",
                    Description = "写下这个人的经历",
                },
            };
            InfoBoxMultiInputColor.WriteInfoBoxMultiInput(modules, nameof(TestInputInfoBoxMultiInputLong), "请填写此人的详细信息。请提供正确的信息，以便我们处理此人并将其添加到已批准人员列表中。任何不正确的信息都可能导致您的请求被拒绝。");
            string[] rendered = [.. modules.Select((im) => im.Value?.ToString() ?? "")];
            TextWriterWhereColor.WriteWhere("  - " + string.Join("\n  - ", rendered), 0, 0);
            Input.EnableMouse = false;
        }
    }
}
