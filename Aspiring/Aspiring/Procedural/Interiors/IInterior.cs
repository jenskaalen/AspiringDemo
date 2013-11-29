using System.Collections.Generic;

namespace AspiringDemo.Procedural.Interiors
{
    public interface IInterior
    {
        List<IInteriorNode> InteriorNodes { get; set; }
        int InteriorWidth { get; set; }
        int InteriorHeight { get; set; }
    }
}
