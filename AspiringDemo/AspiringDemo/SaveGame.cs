using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo
{
    public class SaveGame : DbContext, ISavegame, IObjectFactory
    {
        public DbSet<Faction> Factions { get; set; }
        public DbSet<Squad> Squads { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Weapon> Weapons { get; set; }
        public DbSet<Fight> Fights { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<DemoSquad> DemoSquads { get; set; }

        private string dbname = "thishsouldnthappen";
        private DbContext _tempContext;

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Squad>()
            //    .HasMany(x => x.Members)
            //    .WithRequired()
            //    .HasForeignKey(x => x.ID);


            //modelBuilder.Entity<Squad>()
            //    .HasOptional(x => x.Leader);
        }

        public SaveGame(string name)
        {
            dbname = name;
            base.Database.Connection.ConnectionString = "Data Source=" + dbname + ".sdf;Persist Security Info=False;";

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
            bool addedMembers = false;

            var tempMembers = squad.Members;
            squad.Members = null;

            squad.Leader = null;
            Squads.Add(squad);
            SaveChanges();
            squad.Members = tempMembers;

            SaveChanges();
        }

        public void Load()
        {
            Game.Factions = Factions.ToList();
            Game.Weapons = Weapons.ToList();
        }

        public T GetObject<T>() where T : class
        {
            var instancedType = Set<T>().Create();
            Set<T>().Add(instancedType);

            SaveChanges();

            return instancedType;
        }
    }
}
