using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspiringDemo
{
    public static class Game
    {
        public static List<Faction> Factions { get; set; }
        public static int FactionCount { get; set; }
        public static bool IncludeMonsters { get; set; }
        public static int WorldSize { get; set; }
    }
}
