using System.Collections.Generic;
using System.Linq;

namespace Generator.Commands
{
    public sealed class Capabilities
    {
        private readonly Dictionary<CapabilityType, List<Capability>> _capabilities;

        public IEnumerable<Capability> Booleans => _capabilities[CapabilityType.Bool];
        public IEnumerable<Capability> Nums => _capabilities[CapabilityType.Num];
        public IEnumerable<Capability> Strings => _capabilities[CapabilityType.String];

        public Capabilities(IEnumerable<Capability> capabilities)
        {
            _capabilities = new Dictionary<CapabilityType, List<Capability>>();

            _capabilities[CapabilityType.Bool] = capabilities.Where(x => x.Type == CapabilityType.Bool).ToList();
            _capabilities[CapabilityType.Num] = capabilities.Where(x => x.Type == CapabilityType.Num).ToList();
            _capabilities[CapabilityType.String] = capabilities.Where(x => x.Type == CapabilityType.String).ToList();
        }
    }
}
