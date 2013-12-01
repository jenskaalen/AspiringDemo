using System;
using System.Collections.Generic;
using AspiringDemo.Combat;
using AspiringDemo.Factions;
using AspiringDemo.GameCore;
using AspiringDemo.GameObjects.Squads;
using AspiringDemo.GameObjects.Units;
using AspiringDemo.Zones;

namespace AspiringDemo.Saving
{
    public class TestSave : ISavegame, IObjectFactory
    {
        private string _connstring = "";
        private string _databaseName;
        private List<Squad> _squadsCache = new List<Squad>();
        private List<Unit> _unitsCache = new List<Unit>();
        private string dbname = "thishsouldnthappen";

        public TestSave(string name)
        {
            dbname = name;
            //base.Database.Connection.ConnectionString = "Data Source=" + dbname + ".sdf;Persist Security Info=False;";
            ////base.Database.Connection.ConnectionString = "Server=localhost;Database=" + dbname + ";Trusted_Connection=true;Persist Security Info=true";//Persist Security Info=true";

            //Database.CreateIfNotExists();
        }

        public List<IFaction> Factions { get; set; }
        public List<Squad> Squads { get; set; }
        public List<Unit> Units { get; set; }
        //public DbSet<IWeapon> Weapons { get; set; }
        public List<NewFight> Fights { get; set; }
        public List<Zone> Zones { get; set; }

        public string DatabaseName
        {
            get { return _databaseName; }
            set
            {
                _databaseName = value;
                _connstring.Replace("[Database]", _databaseName);
            }
        }

        public T GetObject<T>() where T : class, new()
        {
            var obj = new T();
            return obj;
            //var instancedType = Set<T>().Create();
            //Set<T>().Add(instancedType);

            //if (typeof(T) == typeof(Unit))
            //{
            //    SaveChanges();
            //}

            //return instancedType;
        }

        public void Create(string databaseName)
        {
        }


        public void Save()
        {
            SaveChanges();
        }

        public void Load()
        {
            throw new NotImplementedException();
            //_game.Factions = Factions.ToList();
            //_game.Weapons = Weapons.ToList();
        }

        /// <summary>
        ///     Makes sure all squadmembers are added to the entity framework
        /// </summary>
        /// <param name="squad"></param>
        private void InsertSquad(Squad squad)
        {
            List<IUnit> tempMembers = squad.Members;
            squad.Members = null;

            //squad.Leader = null;D:\Projects\AspiringDemo\AspiringDemo\SquadRank.cs
            Squads.Add(squad);
            SaveChanges();
            squad.Members = tempMembers;

            //SaveChanges();
        }

        public void SaveChanges()
        {
            // lol, trixs
        }

        //public T GetObject<T>() where T : Unit
        //{
        //    var instancedType = Set<T>().Create();
        //    Set<T>().Add(instancedType);

        //    SaveChanges();

        //    return instancedType;
        //}
    }
}