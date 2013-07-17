using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspiringDemo
{
    public class Faction
    {
        public string ID { get; private set; }
        public List<Squad> Squads { get; private set; }
        public bool IsComputer { get; private set; }

        public Faction(bool isComputer, string id)
        {
            IsComputer = IsComputer;
            ID = id;
        }
    }
}
