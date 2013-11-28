using System.Collections.Generic;
using AspiringDemo.Sites;

namespace AspiringDemo.Factions
{
    public interface ITaxes
    {
        float LastTaxCollection { get; set; }
        float NextTaxCollection { get; set; }
        float CollectionRate { get; set; }
        double TaxPerPayer { get; set; }
        int CollectTaxes(List<IPopulatedArea> areas, double taxPerCitizen, float time);
    }
}