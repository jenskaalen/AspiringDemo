using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo
{
    public class SaveGame : DbContext, ISavegame
    {
        public DbSet<Faction> Factions { get; set; }
        public DbSet<Squad> Squads { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Weapon> Weapons { get; set; }
        public DbSet<Fight> Fights { get; set; }
        //public DbSet<SquadRank> SquadRanks { get; set; }
        public DbSet<Zone> Zones { get; set; }
        //public DbSet<Zone> Zones { get; set; }
        //public DbSet<Zone> Zones { get; set; }

        public DbSet<DemoSquad> DemoSquads { get; set; }

        private string dbname = "thishsouldnthappen";

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
            //foreach (Weapon wpn in Game.Weapons)
            //{
            //    //TODO: Gjør om alt dette til direkte SQL spørringer
            //    Weapon dbWeapon = Weapons.SingleOrDefault(x => x.WeaponName == wpn.WeaponName);

            //    if (dbWeapon == null)
            //    {
            //        Weapons.Add(wpn);
            //    }
            //    else
            //    {
            //        // this entity should be linked then and no changes needed...
            //    }
            //}

            //Unit unit1 = new Unit();
            //unit1.Name = "Petrus";

            ////Units.Add(unit1);

            //Faction fac = Game.Factions[0];
            ////fac.Squads[0].Members = null;
            //Squad sq = fac.Squads[0];
            ////sq.Faction = null;
            //sq.Leader = null;

            //fac.Squads = new List<Squad>();
            //fac.Squads.Add(sq);

            //Squads.Add(sq);

            Squad squad = new Squad();
            Unit unit1 = new Unit();
            squad.Members = new List<Unit>();
            squad.Members.Add(unit1);
            squad.Leader = unit1;

            //Units.Add(unit1);
            Squads.Add(squad);

            
            SaveChanges();

            //Faction faction = new Faction();
            //faction.ID = "Raggarfantin";

            //Squad squad = new Squad();
            //Squads.Add(squad);

            //faction.Squads = new HashSet<Squad>();
            //faction.Squads.Add(squad);

            //Factions.Add(faction);

            //foreach (Faction faction in Game.Factions)
            //{
            //    foreach (Squad squad in faction.Squads)
            //    {
            //        //TODO: Gjør om alt dette til direkte SQL spørringer
            //        Squad dbSquad = Squads.SingleOrDefault(x => x.ID == squad.ID);

            //        if (dbSquad == null)
            //        {
            //            Squads.Add(squad);
            //        }
            //        else
            //        {
            //            // this entity should be linked then and no changes needed...
            //        }
            //    }
            //}

            
        }

        public void Load()
        {
            Game.Factions = Factions.ToList();
            Game.Weapons = Weapons.ToList();
        }
    }
}
