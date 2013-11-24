using System.Threading;
using AspiringDemo;
using AspiringDemo.Roleplaying;
using AspiringDemo.Units;
using AspiringDemo.Weapons;
using AspiringImplementation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AspiringVisual
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ISquad _squad;
        private IGame _game;

        public MainWindow()
        {
            InitializeComponent();

            GameRig rig = new GameRig();
            rig.Worldsize = 12;
            rig.FactionCount = 2;
            rig.RigGame();
            RigStuff();
            _game = GameFrame.Game;
            Thread thread = new Thread(GameFrame.Game.StartTimer);
            thread.Start();

            Task.Factory.StartNew((ContiniousUpdate));
        }

        private void ContiniousUpdate()
        {
            while (true)
            {
                Thread.Sleep(1000);
                Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background,
                    new System.Action(() => 
                        squadList.UpdateLayout()
                        
                        ));

                Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background,
                    new System.Action(() =>
                        squadList.Items.Refresh()));

                Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background,
                    new System.Action(() =>
                        lbl_squadState.Content = _squad.State.ToString()
                        ));

                Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background,
                    new System.Action(() =>
                        lbl_faction1Units.Content = _game.Factions[0].Army.Units.Count(x => x.State != UnitState.Dead)
                        ));

                Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background,
                    new System.Action(() =>
                        lbl_faction2Units.Content = _game.Factions[1].Army.Units.Count(x => x.State != UnitState.Dead)
                        ));
            }
        }

        public void RigStuff()
        {
            // created a squad and appoint a leader
            var leFaction = _game.Factions[1];

            _squad = _game.Factions[1].CreateSquad();
            _squad.AddMember(leFaction.CreateUnit());
            _squad.AddMember(leFaction.CreateUnit());
            _squad.AddMember(leFaction.CreateUnit());
            _squad.AddMember(leFaction.CreateUnit());
            _squad.AddMember(leFaction.CreateUnit());
            _squad.Members[0].Name = "Dolan";
            _squad.Members[1].Name = "Goofy";
            _squad.Members[2].Name = "Scrooge";
            _squad.Members[3].Name = "Rotor";
            _squad.Members[4].Name = "Muldvarpen";
            _squad.Members[4].Rank = SquadRank.Commander;
            _squad.Members[4].Weapons.Add(new Smackhammer());

            _game.Factions[0].CreateUnit();
            _game.Factions[0].CreateUnit();
            _game.Factions[0].CreateUnit();
            _game.Factions[0].CreateUnit();
            _game.Factions[0].CreateUnit();
            _game.Factions[0].CreateUnit();
            _game.Factions[0].CreateUnit();
            _game.Factions[0].CreateUnit();
            _game.Factions[0].CreateUnit();
            _game.Factions[0].CreateUnit();
            _game.Factions[0].CreateUnit();

            squadList.ItemsSource = _squad.Members;
            squadList.UpdateLayout();
        }
    }
}
