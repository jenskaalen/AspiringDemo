using AspiringDemo;
using AspiringDemo.Factions;
using AspiringDemo.GameObjects;
using AspiringDemo.GameObjects.Squads;
using AspiringDemo.GameObjects.Units;
using AspiringDemo.Roleplaying;
using AspiringDemo.Roleplaying.Stats;
using AspiringDemo.Weapons;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AspiringDemoTest
{
    /// <summary>
    /// Used for constructing test-objects
    /// </summary>
    public class Factories
    {
        public static IKernel Kernel = new StandardKernel(new NinFactory());

        /// <summary>
        /// Gets a standardized unit to be used for testing
        /// </summary>
        /// <returns></returns>
        public static Unit GetStandardUnit()
        {
            var unit = new Unit(GetStandardFaction());
            return unit;
        }

        public static IWeapon GetStandardWeaponSword()
        {
            //Unarmed weapon = new Unarmed();
            //return weapon;
            return new Sword();
        }

        //TODO: Remove
        //public static UnitStats GetStandardStats()
        //{
        //    UnitStats stats = new UnitStats();
        //    stats.Speed = 10;
        //    stats.Strength = 10;
        //    stats.Constitution = 10;

        //    return stats;
        //}

        /// <summary>
        /// Gets a standardized faction to be used for testing.
        /// </summary>
        /// <returns></returns>
        public static Faction GetStandardFaction()
        {
            Faction faction = new Faction();
            return faction;
        }

        public static Squad GetStandardSquad()
        {
            Squad squad = new Squad();

            for (int i = 0; i < 10; i++)
                squad.AddMember(GetStandardUnit());

            return squad;
        
        
        }

        public static List<IZone> GetZones(int height, int width)
        {
            List<IZone> zones = new List<IZone>();

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    IZone zone = Kernel.Get<IZone>();
                    zone.PositionXStart = (i * 1000);
                    zone.PositionXEnd = (i * 1000) + 999;
                    zone.PositionYStart = (j * 1000);
                    zone.PositionYEnd = (j * 1000) + 999;

                    zones.Add(zone);
                }
            }

            foreach (var zone in zones)
            {
                var nbors = Neighbours(zones, zone, 1000);

                zone.Neighbours = new List<IZone>();
                zone.Neighbours = nbors;
            }

            return zones;
        }

        public static List<IZone> Neighbours(List<IZone> zones, IZone checkzone, int size)
        {
            var neighbours = new List<IZone>();

            int xPos = checkzone.PositionXStart;
            int yPos = checkzone.PositionYStart;

            var zone1 = zones.FirstOrDefault(x => x.PositionXStart == (xPos - 1000) && x.PositionYStart == yPos);
            var zone2 = zones.FirstOrDefault(x => x.PositionXStart == (xPos + 1000) && x.PositionYStart == yPos);
            var zone3 = zones.FirstOrDefault(x => x.PositionYStart == (yPos - 1000) && x.PositionXStart == xPos);
            var zone4 = zones.FirstOrDefault(x => x.PositionYStart == (yPos + 1000) && x.PositionXStart == xPos);

            if (zone1 != null)
                neighbours.Add(zone1);

            if (zone2 != null)
                neighbours.Add(zone2);

            if (zone3 != null)
                neighbours.Add(zone3);

            if (zone4 != null)
                neighbours.Add(zone4);

            return neighbours;
        }
    }
}
