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
using Terminaux.Inputs.Styles.Editor;
using Textify.General;

namespace Terminaux.Console.Fixtures.Cases.Inputs
{
    internal class TestTextViewerInteractiveCjk : IFixture
    {
        public void RunFixture()
        {
            string toBeEdited =
                """
                我们已经删除了使用 mod 部件来组织多个同名的 mod，因为当 mod 只由单个源代码文件（Visual Basic 或 C#）
                组成时，它们是相关的，并且大型 mod 将使用多个源代码文件进行分组，这些源代码文件相互连接。 互相运作。
                某些版本之前，我们删除了基于源代码的 mod，并依赖 mod 开发人员制作 .DLL 版本，以确保它们获得最大的灵
                活性。 由于其刚性，此功能不会再出现。 Nitrocid KS 的所有 API v3.0 之前的版本（包括 0.0.24.x）都以 KS
                作为其根命名空间，因为当时的名称是 Kernel Simulator。 我们给这个应用程序命名为 Nitrocid KS，内核名称
                已更改为 Nitrocid Kernel。 因此，根命名空间已从 KS 更改为 Nitrocid，以便与应用程序品牌更加一致。 由于
                历史原因，一些重大变更注释仍然使用 KS 根命名空间，不应修改它们，因为它们列出了上一个版本和下一个版本之
                间发生的更改历史记录。 您必须更新导入以指向新的根命名空间。
                """;
            var editLines = toBeEdited.SplitNewLines().ToList();
            TextViewInteractive.OpenInteractive(editLines);
        }
    }
}
