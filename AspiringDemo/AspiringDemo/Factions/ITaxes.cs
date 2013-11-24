using AspiringDemo.Sites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.Factions
{
    public interface ITaxes
    {
        long LastTaxCollection { get; set; }
        long NextTaxCollection { get; set; }
        int CollectionRate { get; set; }
        double TaxPerPayer { get; set; }
        int CollectTaxes(List<IPopulatedArea> areas, double taxPerCitizen, long time);
    }
}
