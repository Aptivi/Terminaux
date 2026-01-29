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

using System.Threading;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Console.Fixtures.Cases.Writer
{
    internal class TestNonModalInfoBoxExcess : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Writer;

        public void RunFixture()
        {
            var infobox = InfoBoxNonModalColor.WriteInfoBox(
                "This info box full of information below should close automatically within 10 seconds:\n\n" +
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Facilisis volutpat est velit egestas dui id ornare arcu odio. Sollicitudin aliquam ultrices sagittis orci. Lobortis scelerisque fermentum dui faucibus in ornare quam. Pretium lectus quam id leo in vitae turpis massa sed. Tellus in hac habitasse platea dictumst vestibulum. Ac orci phasellus egestas tellus rutrum. Arcu cursus vitae congue mauris rhoncus aenean. Tellus rutrum tellus pellentesque eu tincidunt tortor aliquam nulla. Habitasse platea dictumst quisque sagittis purus sit amet volutpat consequat. Et netus et malesuada fames ac turpis egestas maecenas pharetra. Sodales ut eu sem integer vitae justo eget magna. Enim facilisis gravida neque convallis. Suspendisse potenti nullam ac tortor vitae purus faucibus ornare suspendisse. Nisl pretium fusce id velit ut tortor pretium viverra suspendisse. Elementum sagittis vitae et leo duis. Viverra ipsum nunc aliquet bibendum enim facilisis gravida. Egestas maecenas pharetra convallis posuere morbi. Et tortor at risus viverra adipiscing at. Gravida in fermentum et sollicitudin ac orci. Sapien et ligula ullamcorper malesuada proin libero nunc. Tortor at risus viverra adipiscing. Risus feugiat in ante metus dictum at tempor commodo. Tincidunt vitae semper quis lectus nulla at volutpat diam. Facilisis mauris sit amet massa vitae. A pellentesque sit amet porttitor eget. Pellentesque id nibh tortor id aliquet lectus proin. Mauris a diam maecenas sed enim. Sed faucibus turpis in eu mi bibendum neque egestas congue. Risus at ultrices mi tempus imperdiet.\n\n" +
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Facilisis volutpat est velit egestas dui id ornare arcu odio. Sollicitudin aliquam ultrices sagittis orci. Lobortis scelerisque fermentum dui faucibus in ornare quam. Pretium lectus quam id leo in vitae turpis massa sed. Tellus in hac habitasse platea dictumst vestibulum. Ac orci phasellus egestas tellus rutrum. Arcu cursus vitae congue mauris rhoncus aenean. Tellus rutrum tellus pellentesque eu tincidunt tortor aliquam nulla. Habitasse platea dictumst quisque sagittis purus sit amet volutpat consequat. Et netus et malesuada fames ac turpis egestas maecenas pharetra. Sodales ut eu sem integer vitae justo eget magna. Enim facilisis gravida neque convallis. Suspendisse potenti nullam ac tortor vitae purus faucibus ornare suspendisse. Nisl pretium fusce id velit ut tortor pretium viverra suspendisse. Elementum sagittis vitae et leo duis. Viverra ipsum nunc aliquet bibendum enim facilisis gravida. Egestas maecenas pharetra convallis posuere morbi. Et tortor at risus viverra adipiscing at. Gravida in fermentum et sollicitudin ac orci. Sapien et ligula ullamcorper malesuada proin libero nunc. Tortor at risus viverra adipiscing. Risus feugiat in ante metus dictum at tempor commodo. Tincidunt vitae semper quis lectus nulla at volutpat diam. Facilisis mauris sit amet massa vitae. A pellentesque sit amet porttitor eget. Pellentesque id nibh tortor id aliquet lectus proin. Mauris a diam maecenas sed enim. Sed faucibus turpis in eu mi bibendum neque egestas congue. Risus at ultrices mi tempus imperdiet.\n\n" +
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Facilisis volutpat est velit egestas dui id ornare arcu odio. Sollicitudin aliquam ultrices sagittis orci. Lobortis scelerisque fermentum dui faucibus in ornare quam. Pretium lectus quam id leo in vitae turpis massa sed. Tellus in hac habitasse platea dictumst vestibulum. Ac orci phasellus egestas tellus rutrum. Arcu cursus vitae congue mauris rhoncus aenean. Tellus rutrum tellus pellentesque eu tincidunt tortor aliquam nulla. Habitasse platea dictumst quisque sagittis purus sit amet volutpat consequat. Et netus et malesuada fames ac turpis egestas maecenas pharetra. Sodales ut eu sem integer vitae justo eget magna. Enim facilisis gravida neque convallis. Suspendisse potenti nullam ac tortor vitae purus faucibus ornare suspendisse. Nisl pretium fusce id velit ut tortor pretium viverra suspendisse. Elementum sagittis vitae et leo duis. Viverra ipsum nunc aliquet bibendum enim facilisis gravida. Egestas maecenas pharetra convallis posuere morbi. Et tortor at risus viverra adipiscing at. Gravida in fermentum et sollicitudin ac orci. Sapien et ligula ullamcorper malesuada proin libero nunc. Tortor at risus viverra adipiscing. Risus feugiat in ante metus dictum at tempor commodo. Tincidunt vitae semper quis lectus nulla at volutpat diam. Facilisis mauris sit amet massa vitae. A pellentesque sit amet porttitor eget. Pellentesque id nibh tortor id aliquet lectus proin. Mauris a diam maecenas sed enim. Sed faucibus turpis in eu mi bibendum neque egestas congue. Risus at ultrices mi tempus imperdiet."
                , new InfoBoxSettings()
                {
                    Title = "Excess non-modal information box"
                }
            );
            Thread.Sleep(10000);
            TextWriterRaw.WriteRaw(infobox.Erase());
        }
    }
}
