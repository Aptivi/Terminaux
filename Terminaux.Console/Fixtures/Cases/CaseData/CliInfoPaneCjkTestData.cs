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

using System.Collections.Generic;
using Terminaux.Inputs.Interactive;

namespace Terminaux.Console.Fixtures.Cases.CaseData
{
    internal class CliInfoPaneCjkTestData : BaseInteractiveTui<string>, IInteractiveTui<string>
    {
        internal static Dictionary<string, string> strings = new()
        {
            {"第一篇文字", "Nitrocid KS（当时的内核模拟器）于 2018 年推出，版本为 0.0.1，当时仍处于早期访问阶段（alpha）。 它具有一个非常基本的模拟器，只关注手头的事情：内核、登录机制和 shell 应用程序。 从那时起，我们进行了各种改进，最终形成了内核的“beta”阶段，即版本0.1.0。 该内核不仅可以独立运行，还可以使用 GRILO 启动它，使其成为一个成熟的计算机模拟器。 GRILO 是一个引导加载程序模拟器，可让您使 C# 和 Visual Basic 应用程序可从模拟引导加载程序引导。"},
            {"第二段文字", "将 Nitrocid KS 0.0.24.x 升级到 0.1.0 后，运行它可能会产生首次运行向导，认为您的所有配置都会丢失。 但是，它们并没有丢失，因为旧的配置样式不会被 0.1.0 删除。 不幸的是，这两种配置样式彼此不兼容，这意味着您必须手动复制使用 0.0.24.x 时所做的配置。 与手动解析每个配置键并将其安装到适当变量的旧方法相比，这是故意这样做的，因为使用了序列化技术，使配置读取器和写入器比以前更快。 新的读取器和写入器还因其实现方式而提供了更大的灵活性，使您的模组比以前更具可配置性。"},
            {"第三条文字", "我们已经删除了使用 mod 部件来组织多个同名的 mod，因为当 mod 只由单个源代码文件（Visual Basic 或 C#）组成时，它们是相关的，并且大型 mod 将使用多个源代码文件进行分组，这些源代码文件相互连接。 互相运作。 某些版本之前，我们删除了基于源代码的 mod，并依赖 mod 开发人员制作 .DLL 版本，以确保它们获得最大的灵活性。 由于其刚性，此功能不会再出现。 Nitrocid KS 的所有 API v3.0 之前的版本（包括 0.0.24.x）都以 KS 作为其根命名空间，因为当时的名称是 Kernel Simulator。 我们给这个应用程序命名为 Nitrocid KS，内核名称已更改为 Nitrocid Kernel。 因此，根命名空间已从 KS 更改为 Nitrocid，以便与应用程序品牌更加一致。 由于历史原因，一些重大变更注释仍然使用 KS 根命名空间，不应修改它们，因为它们列出了上一个版本和下一个版本之间发生的更改历史记录。 您必须更新导入以指向新的根命名空间。"},
        };

        /// <inheritdoc/>
        public override IEnumerable<string> PrimaryDataSource =>
            strings.Keys;

        /// <inheritdoc/>
        public override string GetInfoFromItem(string item) =>
            strings[item];

        /// <inheritdoc/>
        public override string GetEntryFromItem(string item) =>
            item;
    }
}
