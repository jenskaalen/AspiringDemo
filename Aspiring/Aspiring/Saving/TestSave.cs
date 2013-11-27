using AspiringDemo.Combat;
using AspiringDemo.Factions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.GameCore;
using AspiringDemo.Units;

namespace AspiringDemo.Saving
{
    public class TestSave : ISavegame, IObjectFactory
    {
        public List<IFaction> Factions { get; set; }
        public List<Squad> Squads { get; set; }
        public List<Unit> Units { get; set; }
        //public DbSet<IWeapon> Weapons { get; set; }
        public List<NewFight> Fights { get; set; }
        public List<Zone> Zones { get; set; }

        private List<Unit> _unitsCache = new List<Unit>();
        private List<Squad> _squadsCache = new List<Squad>();
        //private List<Unit> _squadsCache = new List<Unit>();

        //private Game _game;
        private string dbname = "thishsouldnthappen";

        public TestSave(string name)
        {
            dbname = name;
            //base.Database.Connection.ConnectionString = "Data Source=" + dbname + ".sdf;Persist Security Info=False;";
            ////base.Database.Connection.ConnectionString = "Server=localhost;Database=" + dbname + ";Trusted_Connection=true;Persist Security Info=true";//Persist Security Info=true";

            //Database.CreateIfNotExists();
        }

        public void Create(string databaseName)
        {

        }

        public string DatabaseName
        {
            get
            {
                return _databaseName;
            }
            set
            {
                _databaseName = value;
                _connstring.Replace("[Database]", _databaseName);
            }
        }

        private string _databaseName;
        private string _connstring = "";


        public void Save()
        {
            SaveChanges();
        }

        /// <summary>
        /// Makes sure all squadmembers are added to the entity framework
        /// </summary>
        /// <param name="squad"></param>
        private void InsertSquad(Squad squad)
        {
            var tempMembers = squad.Members;
            squad.Members = null;

            //squad.Leader = null;D:\Projects\AspiringDemo\AspiringDemo\SquadRank.cs
            Squads.Add(squad);
            SaveChanges();
            squad.Members = tempMembers;

            //SaveChanges();
        }

        public void Load()
        {
            throw new NotImplementedException();
            //_game.Factions = Factions.ToList();
            //_game.Weapons = Weapons.ToList();
        }

        public T GetObject<T>() where T : class, new()
        {
            T obj = new T();
            return obj;
            //var instancedType = Set<T>().Create();
            //Set<T>().Add(instancedType);

            //if (typeof(T) == typeof(Unit))
            //{
            //    SaveChanges();
            //}

            //return instancedType;
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
