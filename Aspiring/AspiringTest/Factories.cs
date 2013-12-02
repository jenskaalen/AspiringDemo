using System.Runtime.CompilerServices;
using AspiringDemo;
using AspiringDemo.Factions;
using AspiringDemo.GameObjects;
using AspiringDemo.GameObjects.Squads;
using AspiringDemo.GameObjects.Units;
using AspiringDemo.Roleplaying;
using AspiringDemo.Roleplaying.Stats;
using AspiringDemo.Weapons;
using AspiringDemo.Zones;
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
                    IZone zone = new Zone(i * 1000, j * 1000, 999, 999);
                        //Kernel.Get<Zone>(i * 1000, j * 1000, 999, 999);

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

            int xPos = checkzone.Area.X1;
            int yPos = checkzone.Area.Y1;

            var zone1 = zones.FirstOrDefault(x => x.Area.X1 == (xPos - 1000) && x.Area.Y1 == yPos);
            var zone2 = zones.FirstOrDefault(x => x.Area.X1 == (xPos + 1000) && x.Area.Y1 == yPos);
            var zone3 = zones.FirstOrDefault(x => x.Area.Y1 == (yPos - 1000) && x.Area.X1 == xPos);
            var zone4 = zones.FirstOrDefault(x => x.Area.Y1 == (yPos + 1000) && x.Area.X1 == xPos);

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
