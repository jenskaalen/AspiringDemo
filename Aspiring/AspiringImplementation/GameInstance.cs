using AspiringDemo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AspiringDemo.Units;

namespace AspiringImplementation
{
    class GameInstance
    {
        static void Main() 
        {
            GameRig rig = new GameRig();
            rig.FactionCount = 2;
            rig.Worldsize = 12;

            rig.RigGame();

            Thread thread = new Thread(GameFrame.Game.StartTimer);

            thread.Start();

            while (true)
            {
                Console.WriteLine("Faction 1 units: " + GameFrame.Game.Factions[0].Army.Units.Where(x => x.State != UnitState.Dead).Count());
                Console.WriteLine("Faction 1 gold: " + GameFrame.Game.Factions[0].Wealth);
                Console.WriteLine("Faction 2 units: " + GameFrame.Game.Factions[1].Army.Units.Where(x => x.State != UnitState.Dead).Count());
                Console.WriteLine("Faction 2 gold: " + GameFrame.Game.Factions[1].Wealth);
                Console.WriteLine();

                Thread.Sleep(2000);
            }

        }
    }
}
