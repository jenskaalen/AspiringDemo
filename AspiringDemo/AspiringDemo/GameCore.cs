using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo
{
    class GameCore
    {
        //public Game Game { get; set; }

        public void CheckContestedZones()
        { 
            
        }

        public bool IsZoneContested(Zone zone)
        {
            //TODO: Doesnt take account for relations
            int factionsInZone = 0;

            foreach (Faction faction in Game.Factions)
            {
                if (faction.Squads.Where(x => x.Zone == zone).Any())
                    factionsInZone++;
            }

            if (factionsInZone > 1)
                return true;
            else
                return false;

        }

        public void StartFight(Zone zone)
        { 
            
        }

        public static void UpdatePlayerAreas()
        {

        }

        public static void PlayerChangedArea()
        {
            
        }
    }
}
