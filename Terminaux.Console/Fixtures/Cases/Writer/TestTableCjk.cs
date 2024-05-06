//
// Terminaux  Copyright (C) 2023-2024  Aptivi
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
using Terminaux.Writer.FancyWriters;

namespace Terminaux.Console.Fixtures.Cases.Writer
{
    internal class TestTableCjk : IFixture
    {
        public string FixtureID => "TestTableCjk";
        public void RunFixture()
        {
            var Headers = new string[] { "Ubuntu版本", "发布日期", "支持结束", "ESM支持结束" };
            var Rows = new string[,]
            {
                { "12.04（精准穿山甲） [precise]", new DateTime(2012, 4, 26).ToString(), new DateTime(2017, 4, 28).ToString(), new DateTime(2019, 4, 28).ToString() },
                { "14.04（值得信赖的塔尔） [trusty]", new DateTime(2014, 4, 17).ToString(), new DateTime(2019, 4, 25).ToString(), new DateTime(2024, 4, 25).ToString() },
                { "16.04（赛尼尔·泽鲁斯） [xenial]", new DateTime(2016, 4, 21).ToString(), new DateTime(2021, 4, 30).ToString(), new DateTime(2026, 4, 30).ToString() },
                { "18.04（仿生海狸） [bionic]", new DateTime(2018, 4, 26).ToString(), new DateTime(2023, 4, 30).ToString(), new DateTime(2028, 4, 30).ToString() },
                { "20.04（局灶窝） [focal]", new DateTime(2020, 4, 23).ToString(), new DateTime(2025, 4, 25).ToString(), new DateTime(2030, 4, 25).ToString() },
                { "22.04（果酱水母） [jammy]", new DateTime(2022, 4, 26).ToString(), new DateTime(2027, 4, 25).ToString(), new DateTime(2032, 4, 25).ToString() }
            };
            TableColor.WriteTable(Headers, Rows, 2);
        }
    }
}
