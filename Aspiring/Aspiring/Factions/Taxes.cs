using AspiringDemo.Factions;
using AspiringDemo.Sites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.Factions
{
    public class Taxes : ITaxes
    {
        public float LastTaxCollection { get; set; }

        public float NextTaxCollection { get; set; }

        public float CollectionRate { get; set; }

        public double TaxPerPayer { get; set; }

        public int CollectTaxes(List<IPopulatedArea> areas, double taxPerCitizen, float time)
        {
            if (NextTaxCollection > time)
                return 0;

            int goldCollected = 0;

            foreach (var area in areas)
            {
                goldCollected += (int)(area.Population * taxPerCitizen);
            }

            NextTaxCollection += CollectionRate;
            LastTaxCollection = time;
            return goldCollected;
        }
    }
}
