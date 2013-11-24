using AspiringDemo.Factions;
using AspiringDemo.Sites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.ANN.Actions
{
    public interface IBuildAction : IManagementAction
    {
        IPopulatedArea AreaType { get;  }
        IPlacementDecider PlacementDecider { get; set;}
    }
}
