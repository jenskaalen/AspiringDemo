using AspiringDemo;
using AspiringDemo.Factions;
using AspiringDemo.Units;
using AspiringDemo.Weapons;
using AspiringImplementation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AspiringVisual
{
    /// <summary>
    /// Interaction logic for FactionsOverview.xaml
    /// </summary>
    public partial class FactionsOverview : Window
    {
        static IGame _game;
        private IFaction _selectedFaction;
        private List<IUnit> _unitList;
        //lock object for synchronization;
        private static object _syncLock = new object();
        private ObservableCollection<IUnit> _unitObs;

        public FactionsOverview()
        {
            InitializeComponent();
            _game = GameFrame.Game;

            //var rig = new GameRig();
            //rig.Worldsize = 20;
            //rig.FactionCount = 3;
            //rig.RigGame();
            //RigStuff();

            //var thread = new Thread(_game.StartTimer);

            //thread.Start();
            lw_factions.ItemsSource = _game.Factions;
            lw_factions.SelectedIndex = 1;
            _selectedFaction = (IFaction) lw_factions.SelectedItem;

            _unitList =  _selectedFaction.Army.Units;
            lw_units.ItemsSource = _unitObs;

            Task.Factory.StartNew((ContinousUpdate));
            //Task.Factory.StartNew((UpdateCurrentFaction));
        }


        private void ContinousUpdate()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(1000);

                    _unitList = _selectedFaction.Army.Units.Where(unit => unit.State != UnitState.Dead).ToList();
                    _unitObs = _unitList.ToObservableCollection();

                    Application.Current.Dispatcher.BeginInvoke(
                        DispatcherPriority.Background,
                        new System.Action(() =>
                            lw_units.ItemsSource = _unitObs
                            ));

                    Application.Current.Dispatcher.BeginInvoke(
                        DispatcherPriority.Background,
                        new System.Action(() =>
                            lw_units.Items.Refresh()
                            ));
                }
                catch
                {
                }

                Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background,
                    new System.Action(() =>
                        lbl_factionStrength.Content = _selectedFaction.Strength
                        ));
            }
        }

        public void RigStuff()
        {
            try
            {
                _game.Factions[0].Name = "Faction 1";
                _game.Factions[1].Name = "Faction 2";
                _game.Factions[2].Name = "Faction 3";
                // created a squad and appoint a leader
                var leFaction = _game.Factions[1];
            }
            catch
            {
            }
        }


        private void Lw_factions_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedFaction = (IFaction) ((ListView) sender).SelectedItem;
            
            UpdateFaction();
        }

        //private void Lw_units_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        public void UpdateFaction()
        {
            _unitList = _selectedFaction.Army.Units.Where(unit => unit.State != UnitState.Dead).ToList();

            //TODO: Make a stance on this
            //BindingOperations.EnableCollectionSynchronization(_unitList, _syncLock);

            //lw_units.ItemsSource = _selectedFaction.Army.Units;

            lw_factions.Items.Refresh();

            _unitObs = _unitList.ToObservableCollection();
            lw_units.ItemsSource = _unitObs;
            lw_units.Items.Refresh();
            lbl_factionUnitCount.Content = "Units: " + _selectedFaction.Army.AliveUnitsCount;
            lbl_factionSquads.Content = "Squads: " + _selectedFaction.Army.Squads.Count;
            lbl_areasCount.Content = "Areas: " + _selectedFaction.Areas.Count(area => area.Razed == false);
        }
    }
    

    
    public static class Ext
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerable) {
          var col = new ObservableCollection<T>();
          foreach ( var cur in enumerable ) {
            col.Add(cur);
          }
          return col;
        }
    }
}
