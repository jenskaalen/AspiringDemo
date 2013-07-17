using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspiringDemo
{
    /// <summary>
    /// an AI fight
    /// </summary>
    public class Fight
    {
        public List<Squad> Squads { get; set; }
        public int KilledCount { get; set; }

        public void AddSquad(Squad squad)
        {
            throw new NotImplementedException();
        }

        public void RemoveSquad(Squad squad)
        {
            throw new NotImplementedException();
        }

        public Squad GetAttackingSquad()
        {
            throw new NotImplementedException();
        }

        public void SquadsFight()
        {
            throw new NotImplementedException();
        }
    }
}
