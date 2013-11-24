using AspiringDemo.Combat;
using AspiringDemo.Factions;
using AspiringDemo.Units;
using Ninject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.Saving
{
    public class SaveGame : DbContext, ISavegame, AspiringDemo.GameCore.IObjectFactory
    {
        // must have concretions in dbset
        public DbSet<Faction> Factions { get; set; }
        public DbSet<Squad> Squads { get; set; }
        public DbSet<Unit> Units { get; set; }
        //public DbSet<IWeapon> Weapons { get; set; }
        public DbSet<Fight> Fights { get; set; }
        public DbSet<Zone> Zones { get; set; }

        private List<Unit> _unitsCache = new List<Unit>();
        private List<Squad> _squadsCache = new List<Squad>();
        private string _dbname = "";

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Squad>()
                .HasMany(x => x.Members)
                .WithMany();

            modelBuilder.Entity<Squad>()
                .HasOptional(x => x.Leader)
                .WithOptionalDependent();


            // this does not work!
            modelBuilder.Entity<Unit>()
                .HasOptional(x => x.Squad)
                .WithMany(x => x.Members.ConvertAll(o => (Unit)o) );
        }

        public SaveGame(string name)
        {
            _dbname = name;
            base.Database.Connection.ConnectionString = "Data Source=" + _dbname + ".sdf;Persist Security Info=False;";
            //base.Database.Connection.ConnectionString = "Server=localhost;Database=" + _dbname + ";Trusted_Connection=true;Persist Security Info=true";//Persist Security Info=true";

            Database.CreateIfNotExists();
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

            SaveChanges();
        }

        public void Load()
        {
            GameFrame.Game.Factions = (List<IFaction>) Factions.ToList().Cast<List<IFaction>>();
            //_game.Weapons = Weapons.ToList();
        }

        public T GetObject<T>() where T : class, new()
        {
            var instancedType = Set<T>().Create();
            Set<T>().Add(instancedType);

            if (typeof(T) == typeof(Unit))
            {
                SaveChanges();
            }

            return instancedType;
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
