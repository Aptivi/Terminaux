using System.Diagnostics;

namespace Generator.Commands
{
    [DebuggerDisplay("{Type,nq}:{Name,nq} = {Description, nq}")]
    public sealed class Capability
    {
        public string Name { get; set; }
        public string Variable { get; set; }
        public CapabilityType Type { get; set; }
        public string Description { get; set; }
        public int Index { get; set; }
    }
}
