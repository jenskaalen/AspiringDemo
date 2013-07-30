﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspiringDemo
{
    public class Faction
    {
        public string ID { get; set; }
        public List<Squad> Squads { get; set; }
        public bool IsComputer { get; set; }

        internal Faction()
        { 
        }

        public Faction(bool isComputer, string id)
        {
            IsComputer = IsComputer;
            ID = id;

            Squads = new List<Squad>();
        }

        public Squad CreateSquad()
        {
            Squad newSquad = Game.ObjectFactory.GetObject<Squad>();
            newSquad.Faction = this;
            Squads.Add(newSquad);

            return newSquad;
        }
    }
}
