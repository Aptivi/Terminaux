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
using System.Linq;

namespace Terminaux.TermInfoGen
{
    public sealed class Capabilities
    {
        private readonly Dictionary<CapabilityType, List<Capability>> _capabilities;

        public IEnumerable<Capability> Booleans => _capabilities[CapabilityType.Bool];
        public IEnumerable<Capability> Nums => _capabilities[CapabilityType.Num];
        public IEnumerable<Capability> Strings => _capabilities[CapabilityType.String];

        public Capabilities(IEnumerable<Capability> capabilities)
        {
            _capabilities = new Dictionary<CapabilityType, List<Capability>>
            {
                [CapabilityType.Bool] = capabilities.Where(x => x.Type == CapabilityType.Bool).ToList(),
                [CapabilityType.Num] = capabilities.Where(x => x.Type == CapabilityType.Num).ToList(),
                [CapabilityType.String] = capabilities.Where(x => x.Type == CapabilityType.String).ToList()
            };
        }
    }
}
