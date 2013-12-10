using System;
using System.ComponentModel;
using System.IO;
using AspiringDemo.ANN;
using AspiringDemo.ANN.Actions;
using AspiringDemo.ANN.Actions.Unit;
using AspiringDemo.ANN.War;
using AspiringDemo.Combat;
using AspiringDemo.Combat.Attacks;
using AspiringDemo.Combat.Behaviour;
using AspiringDemo.Factions;
using AspiringDemo.Factions.Custom;
using AspiringDemo.Factions.Diplomacy;
using AspiringDemo.GameActions;
using AspiringDemo.GameActions.Combat;
using AspiringDemo.GameActions.Movement;
using AspiringDemo.Gamecore;
using AspiringDemo.GameCore;
using AspiringDemo.Gamecore.Helpers;
using AspiringDemo.Gamecore.Log;
using AspiringDemo.Gamecore.Types;
using AspiringDemo.GameObjects;
using AspiringDemo.GameObjects.Squads;
using AspiringDemo.GameObjects.Units;
using AspiringDemo.Orders;
using AspiringDemo.Pathfinding;
using AspiringDemo.Procedural;
using AspiringDemo.Roleplaying;
using AspiringDemo.Roleplaying.Stats;
using AspiringDemo.Saving;
using AspiringDemo.Sites;
using AspiringDemo.Weapons;
using AspiringDemo.Zones;
using AspiringDemo.Zones.Interiors;
using Ninject;
using ProtoBuf;
using ProtoBuf.Meta;

namespace AspiringDemo
{
    public class GameFrame
    {
        private static IGame _instance;
        private static ILogger _debugLogger;
        private static Random _random;
        private static readonly XmlCreator _creator = new XmlCreator();

        public static Random Random
        {
            get { return _random ?? (_random = new Random()); }
        }

        public static ILogger Debug
        {
            get
            {
                if (_debugLogger == null)
                    _debugLogger = new DebugLog();

                return _debugLogger;
            }
        }

        public static IGame Game
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Game();
                    _instance.GameTime.TimeTicker += ReadXmlCreation;
                    Init();
                }

                return _instance;
            }
        }

        public static void SaveMe()
        {
            Init();

            var game = (Game) GameFrame.Game;

            using (var file = File.Create("game.bin"))
            {
                Serializer.Serialize<Game>(file, game);
            }
        }

        private static void Init()
        {
            //RuntimeTypeModel.Default.Add(typeof(Faction), true).Add("ObjectFactory", "ID", "IsComputer", "Name", "FactionManager", "CreateUnit", "Wealth", "Power", "StructurePoints", "Areas", "CapitalZone", "Taxes", "Army", "Strength", "Relations", "FactionUnits", "_random", "_initialized");
            //RuntimeTypeModel.Default.Add(typeof(Ability), true);
            //RuntimeTypeModel.Default.Add(typeof(AttackTypes), true);
            //RuntimeTypeModel.Default.Add(typeof(IUnitModifier), true).Add("Source");
            //RuntimeTypeModel.Default.Add(typeof(IDetection), true);
            //RuntimeTypeModel.Default.Add(typeof(Detection), true);
            //RuntimeTypeModel.Default.Add(typeof(ICombatModule), true).Add("CanFlee","CombatReluctance","Kills","CurrentFight","CurrentTarget","DetectionDistance");
            //RuntimeTypeModel.Default.Add(typeof(CombatModule), true).Add("CanFlee","CombatReluctance","Kills","CurrentFight","CurrentTarget","DetectionDistance","_unit","_nextAttack","_detection");
            //RuntimeTypeModel.Default.Add(typeof(INewFight), true).Add("Units","Finished");
            //RuntimeTypeModel.Default.Add(typeof(NewFight), true).Add("Units","Finished");
            //RuntimeTypeModel.Default.Add(typeof(IFactionUnits), true).Add("Units");
            //RuntimeTypeModel.Default.Add(typeof(GameAction), true).Add("Finished");
            //RuntimeTypeModel.Default.Add(typeof(CompositeAction), true).Add("Actions","Finished");
            //RuntimeTypeModel.Default.Add(typeof(Fighting), true).Add("Actions","Finished","_fight","_unit");
            //RuntimeTypeModel.Default.Add(typeof(ActionManager), true).Add("Faction");
            //RuntimeTypeModel.Default.Add(typeof(IManagementAction), true).Add("Faction");
            //RuntimeTypeModel.Default.Add(typeof(IBuildAction), true).Add("AreaType","PlacementDecider");
            //RuntimeTypeModel.Default.Add(typeof(BuildOutpost), true).Add("AreaType","PlacementDecider","Faction");
            //RuntimeTypeModel.Default.Add(typeof(AttackAction), true).Add("Squads","AttackTargetZone","GatherZone","AttackStarted","MemberCount","_faction","_minAttackSize");
            //RuntimeTypeModel.Default.Add(typeof(IUnitAction), true);
            //RuntimeTypeModel.Default.Add(typeof(GuardAction), true).Add("Faction","_areaToGuard","_unit");
            //RuntimeTypeModel.Default.Add(typeof(IUnitManager), true).Add("Faction","AllowedActions");
            //RuntimeTypeModel.Default.Add(typeof(RecruitUnit), true).Add("Faction");
            //RuntimeTypeModel.Default.Add(typeof(SquadFormAction), true);
            //RuntimeTypeModel.Default.Add(typeof(IBuildingManager), true).Add("Faction","AllowedActions","BuildingSettings");
            //RuntimeTypeModel.Default.Add(typeof(BuildingManager), true).Add("PlacementDecider","Faction","AllowedActions","BuildingSettings");
            //RuntimeTypeModel.Default.Add(typeof(IFactionManager), true).Add("Faction","BuildManager","RecruitmentManager","PlacementDecider","UnitManager","QueuedActions","ActionsPerTurn");
            //RuntimeTypeModel.Default.Add(typeof(FactionManager), true).Add("Faction","BuildManager","RecruitmentManager","UnitManager","PlacementDecider","ActionsPerTurn","QueuedActions");
            //RuntimeTypeModel.Default.Add(typeof(IPlacementDecider), true).Add("PreferredCapitalDistance","PreferredFactionZoneDistance","MaxDistanceFromCapital","MinDistanceFromCapital","MaxDistanceFromFactionZone","MinDistanceFromFactionZone","Faction");
            //RuntimeTypeModel.Default.Add(typeof(FactionPlacementDecider), true).Add("PreferredCapitalDistance","PreferredFactionZoneDistance","MaxDistanceFromCapital","MinDistanceFromCapital","MaxDistanceFromFactionZone","MinDistanceFromFactionZone","Faction","_factionZones","_openZones","_zones");
            //RuntimeTypeModel.Default.Add(typeof(IArmyManagement), true);
            //RuntimeTypeModel.Default.Add(typeof(IManager), true).Add("Faction","AllowedActions");
            //RuntimeTypeModel.Default.Add(typeof(IRecruitmentManager), true).Add("Faction","AllowedActions");
            //RuntimeTypeModel.Default.Add(typeof(RecruitmentManager), true).Add("Faction","AllowedActions");
            ////RuntimeTypeModel.Default.Add(typeof(IArmyManagement), true).Add("Faction","AllowedActions");
            //RuntimeTypeModel.Default.Add(typeof(UnitManager), true).Add("Faction","AllowedActions","_currentAttackActions","_warmodule");
            //RuntimeTypeModel.Default.Add(typeof(IWarmodule), true);
            //RuntimeTypeModel.Default.Add(typeof(Warmodule), true);
            //RuntimeTypeModel.Default.Add(typeof(IActionHandler), true);
            //RuntimeTypeModel.Default.Add(typeof(Fleeing), true);
            //RuntimeTypeModel.Default.Add(typeof(IArmy), true).Add("Squads","Units","AliveUnitsCount","AliveUnits");
            //RuntimeTypeModel.Default.Add(typeof(Army), true).Add("Squads","Units","AliveUnits","AliveUnitsCount");
            ////RuntimeTypeModel.Default.Add(typeof(IFaction), true).Add("Areas","FactionUnits","FactionManager","CapitalZone","Taxes","IsComputer","Name","Power","Wealth","StructurePoints","ID","Army","Strength","Relations");
            //RuntimeTypeModel.Default.Add(typeof(IFaction), true).AddSubType(1, typeof(Faction)).Add("Areas", "FactionUnits", "FactionManager", "CapitalZone", "Taxes", "IsComputer", "Name", "Wealth", "StructurePoints", "ID", "Army", "Strength", "Relations");
            //RuntimeTypeModel.Default.Add(typeof(INeutralFaction), true);
            //RuntimeTypeModel.Default.Add(typeof(NeutralFaction), true).Add("CreateUnit","Relations","Areas","FactionManager","CapitalZone","Taxes","IsComputer","Name","Wealth","StructurePoints","ID","Army","Strength","FactionUnits");
            //RuntimeTypeModel.Default.Add(typeof(IFactionRelations), true).Add("Allies");
            //RuntimeTypeModel.Default.Add(typeof(FactionRelations), true).Add("Allies","_faction","_relations");
            //RuntimeTypeModel.Default.Add(typeof(IFactionRelation), true).Add("Faction","Relation");
            //RuntimeTypeModel.Default.Add(typeof(FactionRelation), true).Add("Faction","Relation");
            //RuntimeTypeModel.Default.Add(typeof(RelationType), true);
            //RuntimeTypeModel.Default.Add(typeof(UnitCreationDelegate), true).Add("Method","Target","_target","_methodBase","_methodPtr","_methodPtrAux");
            //RuntimeTypeModel.Default.Add(typeof(StrengthMeasurement), true);
            //RuntimeTypeModel.Default.Add(typeof(IArmyUnit), true).Add("Unit","Squad");
            //RuntimeTypeModel.Default.Add(typeof(ITaxes), true).Add("LastTaxCollection","NextTaxCollection","CollectionRate","TaxPerPayer");
            //RuntimeTypeModel.Default.Add(typeof(StrengthMap), true).Add("Factions","NextStrenghtMapping","StrengthMappingInterval");
            //RuntimeTypeModel.Default.Add(typeof(Taxes), true).Add("LastTaxCollection","NextTaxCollection","CollectionRate","TaxPerPayer");
            //RuntimeTypeModel.Default.Add(typeof(IGame), true).Add("Factions","Weapons","FactionCount","IncludeMonsters","ZonesWidth","ZonesHeight","Savegame","ObjectFactory","GameTime","ZonePathfinder","TimeToTravelThroughZone","Factory","ActionProcesser");
            //RuntimeTypeModel.Default.Add(typeof(Game), true).Add("Factions","Weapons","FactionCount","IncludeMonsters","ZonesWidth","ZonesHeight","Savegame","ObjectFactory","GameTime","ZonePathfinder","TimeToTravelThroughZone","ActionProcesser","Factory","_kernel","_strengthMap","_timerStarted");
            //RuntimeTypeModel.Default.Add(typeof(EnterInterior), true).Add("Finished","_unit","_zone");
            //RuntimeTypeModel.Default.Add(typeof(Sequence), true).Add("Actions","Finished");
            //RuntimeTypeModel.Default.Add(typeof(MoveFromExteriorToInterior), true).Add("Actions","Finished","_unit","_interiorZone");
            //RuntimeTypeModel.Default.Add(typeof(MoveToPosition), true).Add("Finished","_unit","_position","_travelPath","_started","_targetNode","_startNode","_nextZoneChange","moveSpeed");
            //RuntimeTypeModel.Default.Add(typeof(Parallel), true).Add("Actions","Finished");
            //RuntimeTypeModel.Default.Add(typeof(Patrol), true).Add("Actions","Finished");
            //RuntimeTypeModel.Default.Add(typeof(ZoneMove), true).Add("Finished","_startZone","_targetZone","_travelPath","_unit","_nextZoneChange","_started");
            //RuntimeTypeModel.Default.Add(typeof(IGameTime), true).Add("Time","SecondsPerTick","GamePaused","TimeTicker");
            //RuntimeTypeModel.Default.Add(typeof(GameTime), true).Add("Time","SecondsPerTick","GamePaused","TimeTicker");
            //RuntimeTypeModel.Default.Add(typeof(Actions), true);
            //RuntimeTypeModel.Default.Add(typeof(Relations), true);
            //RuntimeTypeModel.Default.Add(typeof(Vector2), true).Add("X","Y");
            //RuntimeTypeModel.Default.Add(typeof(Vector3), true).Add("X","Y","Z");
            //RuntimeTypeModel.Default.Add(typeof(Rect), true).Add("X1","Y1","X2","Y2","Height","Width","Center");
            //RuntimeTypeModel.Default.Add(typeof(Room), true).Add("X1","Y1","X2","Y2","Height","Width","Center");
            //RuntimeTypeModel.Default.Add(typeof(Space), true).Add("X1","Y1","X2","Y2","Height","Width","Center");
            //RuntimeTypeModel.Default.Add(typeof(Corridor), true).Add("X1","Y1","X2","Y2","Height","Width","Center");
            //RuntimeTypeModel.Default.Add(typeof(CorridorPath), true).Add("Corridors","ConnectedRooms","PathType");
            //RuntimeTypeModel.Default.Add(typeof(IPathfindingNode), true).Add("Position","Neighbours","Parent","GValue","HValue","FValue","State");
            //RuntimeTypeModel.Default.Add(typeof(IZone), true).Add("Nodes","IsPlayerNearby","PopulatedAreas","Area","Type","Units","ZoneEntrances","Pathfinder");
            //RuntimeTypeModel.Default.Add(typeof(IInterior), true).Add("Rooms","Paths","Entrance","InteriorWidth","InteriorHeight");
            //RuntimeTypeModel.Default.Add(typeof(IInteriorNode), true);
            //RuntimeTypeModel.Default.Add(typeof(InteriorNode), true).Add("Position","Neighbours","Parent","GValue","HValue","FValue","State");
            //RuntimeTypeModel.Default.Add(typeof(InteriorType), true);
            //RuntimeTypeModel.Default.Add(typeof(InteriorValues), true).Add("MaxRoomSize","MinRoomSize","CorridorWidth","RoomCount");
            //RuntimeTypeModel.Default.Add(typeof(Tomb), true).Add("Nodes","Rooms","Paths","Neighbours","Parent","GValue","HValue","FValue","State","ZoneEntrances","Units","Entrance","InteriorWidth","InteriorHeight","IsPlayerNearby","PopulatedAreas","Area","Type","Pathfinder","Position","_maxRoomSize","_minRoomSize","_corridorWidth","_maxRooms");
            //RuntimeTypeModel.Default.Add(typeof(IZoneEntrance), true).Add("Position","Zone");
            //RuntimeTypeModel.Default.Add(typeof(ZoneEntrance), true).Add("Position","Zone");
            //RuntimeTypeModel.Default.Add(typeof(Zonudes), true);
            //RuntimeTypeModel.Default.Add(typeof(GameTimeTicker), true).Add("Method","Target","_target","_methodBase","_methodPtr","_methodPtrAux");
            //RuntimeTypeModel.Default.Add(typeof(ILogger), true);
            //RuntimeTypeModel.Default.Add(typeof(DebugLog), true).Add("_initalized","_writer");
            //RuntimeTypeModel.Default.Add(typeof(IObjectFactory), true);
            //RuntimeTypeModel.Default.Add(typeof(ICreatureGenerator), true).Add("Creatures");
            //RuntimeTypeModel.Default.Add(typeof(CreatureGenerator), true).Add("Creatures");
            //RuntimeTypeModel.Default.Add(typeof(Utility), true);
            //RuntimeTypeModel.Default.Add(typeof(XmlCreator), true);
            ////RuntimeTypeModel.Default.Add(typeof(GameFrame), true).Add("Random","Debug","Game");
            //RuntimeTypeModel.Default.Add(typeof(IGameObject), true).Add("Position","Zone");
            //RuntimeTypeModel.Default.Add(typeof(ObjectType), true);
            //RuntimeTypeModel.Default.Add(typeof(ISquad), true).Add("ID","IsVisible","KillCounter","Leader","Members","State");
            //RuntimeTypeModel.Default.Add(typeof(ISquadBehaviour), true);
            //RuntimeTypeModel.Default.Add(typeof(Squad), true).Add("Members","ID","State","Leader","KillCounter","IsVisible");
            //RuntimeTypeModel.Default.Add(typeof(SquadRank), true);
            //RuntimeTypeModel.Default.Add(typeof(SquadState), true);
            //RuntimeTypeModel.Default.Add(typeof(IUnit), true).Add("CombatModule","ActionProcesser","Items","Actions","Faction","Hp","ID","IsPlayer","Name","Order","Rank","State","Zone","ChangeRank","Squad","XPWorth","Stats");
            //RuntimeTypeModel.Default.Add(typeof(BaseUnit), true).Add("ChangeState","Rank","Name","IsPlayer","XPWorth","Squad","ActionProcesser","Items","Actions","Faction","CombatModule","ChangeRank","Order","Position","Zone","Stats","Interior","ID","Hp","State","ObjectDestructionTime","_hp","_rank","_state");
            //RuntimeTypeModel.Default.Add(typeof(ILeveling), true).Add("CharacterLevel");
            //RuntimeTypeModel.Default.Add(typeof(IUnitLeveling), true);
            //RuntimeTypeModel.Default.Add(typeof(RankChanged), true).Add("Method","Target","_target","_methodBase","_methodPtr","_methodPtrAux");
            //RuntimeTypeModel.Default.Add(typeof(StateChanged), true).Add("Method","Target","_target","_methodBase","_methodPtr","_methodPtrAux");
            //RuntimeTypeModel.Default.Add(typeof(Unit), true).Add("CharacterLevel","ChangeState","Rank","Name","IsPlayer","XPWorth","Squad","ActionProcesser","Items","Actions","Faction","CombatModule","ChangeRank","Order","Position","Zone","Stats","Interior","ID","Hp","State","ObjectDestructionTime","_hp","_rank","_state");
            //RuntimeTypeModel.Default.Add(typeof(UnitState), true);
            //RuntimeTypeModel.Default.Add(typeof(Zombie), true).Add("ChangeState","Rank","Name","IsPlayer","XPWorth","Squad","ActionProcesser","Items","Actions","Faction","CombatModule","ChangeRank","Order","Position","Zone","Stats","Interior","ID","Hp","State","ObjectDestructionTime","_hp","_rank","_state");
            //RuntimeTypeModel.Default.Add(typeof(TombCreatureGenerator), true).Add("Creatures","_unitCount","_faction");
            //RuntimeTypeModel.Default.Add(typeof(IItems), true).Add("CurrentWeapon","Weapons");
            //RuntimeTypeModel.Default.Add(typeof(Items), true).Add("CurrentWeapon","Weapons");
            //RuntimeTypeModel.Default.Add(typeof(IActionProcesser), true).Add("Actions");
            //RuntimeTypeModel.Default.Add(typeof(ActionProcesser), true).Add("Actions");
            //RuntimeTypeModel.Default.Add(typeof(UnitAttack), true).Add("Finished","_attacker","_target");
            //RuntimeTypeModel.Default.Add(typeof(BuildOrder), true).Add("BuildLocation","BuildType");
            //RuntimeTypeModel.Default.Add(typeof(IUnitOrder), true).Add("Unit","IsExecuting","IsDone","Finish","OrderName");
            //RuntimeTypeModel.Default.Add(typeof(GuardAreaOrder), true).Add("TargetArea","Unit","IsExecuting","IsDone","OrderName","Finish","_targetZone","_travelPath","_nextWorkTime","_startZone");
            //RuntimeTypeModel.Default.Add(typeof(UnitOrderBase), true).Add("Unit","IsExecuting","IsDone","Finish","OrderName");
            ////RuntimeTypeModel.Default.Add(typeof(ProductionFactory), true).Add("Instance","Kernel","Name","Bindings","IsDisposed");
            ////RuntimeTypeModel.Default.Add(typeof(ISerializedTypeData), true).Add("ObjectType","SerializedData");
            ////RuntimeTypeModel.Default.Add(typeof(ICustomSerializable), true);
            //RuntimeTypeModel.Default.Add(typeof(ISavegame), true);
            ////RuntimeTypeModel.Default.Add(typeof(TestSave), true).Add("Factions","Squads","Units","Fights","Zones","DatabaseName","_connstring","_databaseName","_squadsCache","_unitsCache","dbname");
            //RuntimeTypeModel.Default.Add(typeof(IPopulatedArea), true).Add("Owner","Razed","IsUnderAttack","AreaValue","BuildTime","Cost","Population","Zone");
            //RuntimeTypeModel.Default.Add(typeof(ICapital), true);
            //RuntimeTypeModel.Default.Add(typeof(PopulatedArea), true).Add("Owner","Zone","Razed","IsUnderAttack","AreaValue","BuildTime","Cost","Population");
            //RuntimeTypeModel.Default.Add(typeof(Outpost), true).Add("Owner","Zone","Razed","IsUnderAttack","AreaValue","BuildTime","Cost","Population");
            //RuntimeTypeModel.Default.Add(typeof(IWeapon), true).Add("Type","Wielding","BaseDamage","ID","WeaponName","WeaponSpeed");
            //RuntimeTypeModel.Default.Add(typeof(OrderFinished), true).Add("Method","Target","_target","_methodBase","_methodPtr","_methodPtrAux");
            //RuntimeTypeModel.Default.Add(typeof(OrderTick), true).Add("Method","Target","_target","_methodBase","_methodPtr","_methodPtrAux");
            //RuntimeTypeModel.Default.Add(typeof(TravelOrder), true).Add("TargetZone","TravelPath","OrderName","Unit","IsExecuting","IsDone","Finish","_waitOnComplete","_nextWorkTime");
            ////RuntimeTypeModel.Default.Add(typeof(IPathfinder<IZone>), true).Add("Nodes");
            //RuntimeTypeModel.Default.Add(typeof(NodeState), true);
            //RuntimeTypeModel.Default.Add(typeof(Pathfinder<IPathfindingNode>), true).Add("OpenList","ClosedList","Nodes");
            //RuntimeTypeModel.Default.Add(typeof(Pathfinder<IZone>), true).Add("OpenList", "ClosedList", "Nodes");
            ////RuntimeTypeModel.Default.Add(typeof(PriorityQueue`1), true);
            //RuntimeTypeModel.Default.Add(typeof(LevelGain), true).Add("Method","Target","_target","_methodBase","_methodPtr","_methodPtrAux");
            //RuntimeTypeModel.Default.Add(typeof(ICharacterLevel), true).Add("CurrentXP","Level","NextLevelXP","StartLevelXP","ProgressModifier","GainLevel");
            //RuntimeTypeModel.Default.Add(typeof(CharacterLevel), true).Add("XpWorth","Level","CurrentXP","NextLevelXP","StartLevelXP","ProgressModifier","GainLevel","_xpBase");
            //RuntimeTypeModel.Default.Add(typeof(ICharacterModifier), true);
            //RuntimeTypeModel.Default.Add(typeof(IUnitStats), true).Add("CurrentHp","MaxHp","Speed","Strength","BaseStrength","BaseSpeed","BaseHp","GrowthHp","GrowthStrength","GrowthSpeed","RegenRate","RegenHpAmount");
            //RuntimeTypeModel.Default.Add(typeof(UnitStats), true).Add("CurrentHp","MaxHp","Speed","Strength","BaseStrength","BaseSpeed","BaseHp","GrowthHp","GrowthStrength","GrowthSpeed","RegenRate","RegenHpAmount","_baseHp","_currentHp","_currentLevel","_growthHp","_maxHp","_nextRegen","_speed","_strength");
            //RuntimeTypeModel.Default.Add(typeof(ICharacterSkills), true);
            //RuntimeTypeModel.Default.Add(typeof(IWeaponStats), true).Add("Onehanded","Twohanded","Bows");
            //RuntimeTypeModel.Default.Add(typeof(LevelProgressModifier), true).Add("XpMultiplier");
            //RuntimeTypeModel.Default.Add(typeof(WeaponStats), true).Add("Onehanded","Twohanded","Bows");
            //RuntimeTypeModel.Default.Add(typeof(Bow), true).Add("Type","Wielding","BaseDamage","ID","WeaponName","WeaponSpeed");
            //RuntimeTypeModel.Default.Add(typeof(Muldsword), true).Add("Type","Wielding","BaseDamage","ID","WeaponName","WeaponSpeed");
            //RuntimeTypeModel.Default.Add(typeof(Smackhammer), true).Add("Type","Wielding","BaseDamage","ID","WeaponName","WeaponSpeed");
            //RuntimeTypeModel.Default.Add(typeof(Sword), true).Add("Type","Wielding","BaseDamage","ID","WeaponName","WeaponSpeed");
            //RuntimeTypeModel.Default.Add(typeof(Unarmed), true).Add("Type","Wielding","BaseDamage","ID","WeaponName","WeaponSpeed");
            //RuntimeTypeModel.Default.Add(typeof(ZoneType), true);
            //RuntimeTypeModel.Default.Add(typeof(Zone), true).Add("Area","Nodes","IsPlayerNearby","ID","Type","Neighbours","Parent","PopulatedAreas","Position","GValue","HValue","FValue","State","Units","ZoneEntrances","Pathfinder");
            //RuntimeTypeModel.Default.Add(typeof(WeaponType), true);
            //RuntimeTypeModel.Default.Add(typeof(WieldType), true);
            //RuntimeTypeModel.Default.Add(typeof(Game), true).Add("Factions", "Weapons", "FactionCount", "IncludeMonsters", "ZonesWidth", "ZonesHeight", "Savegame", "ObjectFactory", "GameTime", "ZonePathfinder", "TimeToTravelThroughZone", "Factory", "ActionProcesser");
            //RuntimeTypeModel.Default.Add(typeof (StandardKernel), true);
        }

        private static void ReadXmlCreation(float time)
        {
            _creator.ReadXml("units.xml");
        }

        /// <summary>
        ///     Used if you want to override the standard game
        /// </summary>
        /// <param name="game"></param>
        public static void SetGame(IGame game)
        {
            _instance = game;
        }
    }
}