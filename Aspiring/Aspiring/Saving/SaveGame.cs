using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AspiringDemo.Combat;
using AspiringDemo.Factions;
using AspiringDemo.GameCore;
using AspiringDemo.GameObjects.Squads;
using AspiringDemo.GameObjects.Units;

namespace AspiringDemo.Saving
{
    public class SaveGame : DbContext, ISavegame, IObjectFactory
    {
        // must have concretions in dbset
        private string _connstring = "";
        private string _databaseName;
        private string _dbname = "";
        private List<Squad> _squadsCache = new List<Squad>();
        private List<Unit> _unitsCache = new List<Unit>();

        public SaveGame(string name)
        {
            _dbname = name;
            base.Database.Connection.ConnectionString = "Data Source=" + _dbname + ".sdf;Persist Security Info=False;";
            //base.Database.Connection.ConnectionString = "Server=localhost;Database=" + _dbname + ";Trusted_Connection=true;Persist Security Info=true";//Persist Security Info=true";

            Database.CreateIfNotExists();
        }

        public DbSet<Faction> Factions { get; set; }
        public DbSet<Squad> Squads { get; set; }
        public DbSet<Unit> Units { get; set; }
        //public DbSet<IWeapon> Weapons { get; set; }
        public DbSet<NewFight> Fights { get; set; }
        public DbSet<Zone> Zones { get; set; }

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
            T instancedType = Set<T>().Create();
            Set<T>().Add(instancedType);

            if (typeof (T) == typeof (Unit))
            {
                SaveChanges();
            }

            return instancedType;
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
            GameFrame.Game.Factions = (List<IFaction>) Factions.ToList().Cast<List<IFaction>>();
            //_game.Weapons = Weapons.ToList();
        }

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
                .WithMany(x => x.Members.ConvertAll(o => (Unit) o));
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

            SaveChanges();
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