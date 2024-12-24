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

using System.Collections.Generic;
using Terminaux.Inputs.Interactive;

namespace Terminaux.Console.Fixtures.Cases.CaseData
{
    internal class CliInfoPaneExcessTestData : BaseInteractiveTui<string>, IInteractiveTui<string>
    {
        internal static Dictionary<string, string> strings = new()
        {
            {"Lorem Ipsum 1", "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Dolor magna eget est lorem ipsum dolor sit amet consectetur. Aliquet nibh praesent tristique magna sit amet purus gravida quis. Diam donec adipiscing tristique risus nec feugiat in fermentum posuere. Fames ac turpis egestas maecenas pharetra convallis posuere. Massa placerat duis ultricies lacus sed turpis tincidunt id. Quam adipiscing vitae proin sagittis nisl. Sit amet consectetur adipiscing elit ut aliquam. Viverra maecenas accumsan lacus vel facilisis volutpat. Vestibulum sed arcu non odio euismod lacinia at quis. Lacus viverra vitae congue eu consequat. Viverra tellus in hac habitasse. Nec ullamcorper sit amet risus. In nulla posuere sollicitudin aliquam ultrices sagittis orci. Malesuada pellentesque elit eget gravida. Lorem ipsum dolor sit amet consectetur adipiscing elit. Et tortor consequat id porta nibh venenatis cras. Euismod in pellentesque massa placerat duis ultricies lacus sed turpis. Purus sit amet volutpat consequat mauris nunc. Commodo viverra maecenas accumsan lacus vel. Sapien pellentesque habitant morbi tristique senectus et. Tincidunt ornare massa eget egestas. Egestas sed tempus urna et pharetra pharetra massa. Hendrerit gravida rutrum quisque non. Nullam vehicula ipsum a arcu cursus vitae congue. Et magnis dis parturient montes nascetur ridiculus mus mauris. Nulla porttitor massa id neque. Tempor id eu nisl nunc mi ipsum faucibus vitae aliquet. Nunc eget lorem dolor sed viverra. Et netus et malesuada fames ac turpis egestas sed."},
            {"Lorem Ipsum 2", "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Quam lacus suspendisse faucibus interdum. Fermentum odio eu feugiat pretium nibh ipsum consequat nisl. Purus in mollis nunc sed id semper. Ultrices sagittis orci a scelerisque purus. Malesuada nunc vel risus commodo viverra maecenas accumsan lacus vel. Nibh tortor id aliquet lectus proin. Mus mauris vitae ultricies leo integer malesuada nunc vel. Proin fermentum leo vel orci porta non pulvinar. Nulla posuere sollicitudin aliquam ultrices sagittis orci a scelerisque. Lacus vel facilisis volutpat est. Porta lorem mollis aliquam ut porttitor leo. Feugiat scelerisque varius morbi enim nunc faucibus a pellentesque sit. A erat nam at lectus. Semper risus in hendrerit gravida rutrum. Aliquet enim tortor at auctor urna nunc. Eget nunc scelerisque viverra mauris in. Augue lacus viverra vitae congue eu consequat ac. Non odio euismod lacinia at quis risus. Ullamcorper sit amet risus nullam. Malesuada fames ac turpis egestas. Odio ut sem nulla pharetra diam sit. Viverra ipsum nunc aliquet bibendum enim facilisis gravida. Urna neque viverra justo nec ultrices dui sapien eget mi. Donec massa sapien faucibus et molestie ac feugiat sed. Potenti nullam ac tortor vitae purus faucibus ornare suspendisse sed. Eleifend mi in nulla posuere sollicitudin aliquam ultrices sagittis orci. Arcu cursus vitae congue mauris. Odio euismod lacinia at quis risus sed vulputate odio ut. Quis enim lobortis scelerisque fermentum dui faucibus in ornare quam."},
            {"Lorem Ipsum 3", "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Facilisis volutpat est velit egestas dui id ornare arcu odio. Sollicitudin aliquam ultrices sagittis orci. Lobortis scelerisque fermentum dui faucibus in ornare quam. Pretium lectus quam id leo in vitae turpis massa sed. Tellus in hac habitasse platea dictumst vestibulum. Ac orci phasellus egestas tellus rutrum. Arcu cursus vitae congue mauris rhoncus aenean. Tellus rutrum tellus pellentesque eu tincidunt tortor aliquam nulla. Habitasse platea dictumst quisque sagittis purus sit amet volutpat consequat. Et netus et malesuada fames ac turpis egestas maecenas pharetra. Sodales ut eu sem integer vitae justo eget magna. Enim facilisis gravida neque convallis. Suspendisse potenti nullam ac tortor vitae purus faucibus ornare suspendisse. Nisl pretium fusce id velit ut tortor pretium viverra suspendisse. Elementum sagittis vitae et leo duis. Viverra ipsum nunc aliquet bibendum enim facilisis gravida. Egestas maecenas pharetra convallis posuere morbi. Et tortor at risus viverra adipiscing at. Gravida in fermentum et sollicitudin ac orci. Sapien et ligula ullamcorper malesuada proin libero nunc. Tortor at risus viverra adipiscing. Risus feugiat in ante metus dictum at tempor commodo. Tincidunt vitae semper quis lectus nulla at volutpat diam. Facilisis mauris sit amet massa vitae. A pellentesque sit amet porttitor eget. Pellentesque id nibh tortor id aliquet lectus proin. Mauris a diam maecenas sed enim. Sed faucibus turpis in eu mi bibendum neque egestas congue. Risus at ultrices mi tempus imperdiet."},
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
