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

using System.Text;
using Terminaux.Inputs.Styles.Editor;

namespace Terminaux.Console.Fixtures.Cases.Inputs
{
    internal class TestHexViewerFullscreenInteractive : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Input;

        public void RunFixture()
        {
            string toBeEdited =
                """
                Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut
                labore et dolore magna aliqua. Quam lacus suspendisse faucibus interdum. Fermentum odio eu
                feugiat pretium nibh ipsum consequat nisl. Purus in mollis nunc sed id semper. Ultrices sagittis
                orci a scelerisque purus. Malesuada nunc vel risus commodo viverra maecenas accumsan lacus vel.
                Nibh tortor id aliquet lectus proin. Mus mauris vitae ultricies leo integer malesuada nunc vel.
                Proin fermentum leo vel orci porta non pulvinar. Nulla posuere sollicitudin aliquam ultrices
                sagittis orci a scelerisque. Lacus vel facilisis volutpat est. Porta lorem mollis aliquam ut
                porttitor leo. Feugiat scelerisque varius morbi enim nunc faucibus a pellentesque sit. A erat
                nam at lectus. Semper risus in hendrerit gravida rutrum. Aliquet enim tortor at auctor urna nunc.
                Eget nunc scelerisque viverra mauris in. Augue lacus viverra vitae congue eu consequat ac. Non
                odio euismod lacinia at quis risus. Ullamcorper sit amet risus nullam. Malesuada fames ac turpis
                egestas. Odio ut sem nulla pharetra diam sit. Viverra ipsum nunc aliquet bibendum enim facilisis
                gravida. Urna neque viverra justo nec ultrices dui sapien eget mi. Donec massa sapien faucibus et
                molestie ac feugiat sed. Potenti nullam ac tortor vitae purus faucibus ornare suspendisse sed.
                Eleifend mi in nulla posuere sollicitudin aliquam ultrices sagittis orci. Arcu cursus vitae
                congue mauris. Odio euismod lacinia at quis risus sed vulputate odio ut. Quis enim lobortis
                scelerisque fermentum dui faucibus in ornare quam.
                """;
            var editBytes = Encoding.Default.GetBytes(toBeEdited);
            HexEditInteractive.OpenInteractive(ref editBytes, fullscreen: true, edit: false);
        }
    }
}
