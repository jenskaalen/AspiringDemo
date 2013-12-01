using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using AspiringDemo;
using AspiringDemo.Factions;
using AspiringDemo.GameObjects;
using AspiringDemo.GameObjects.Units;
using AspiringDemo.Zones;
using AspiringImplementation;
using System.Threading;

namespace AspiringVisual
{
    /// <summary>
    /// Interaction logic for ZonesVisualized.xaml
    /// </summary>
    public partial class ZonesVisualized : Page
    {
        private int _worldSize = 50;
        private Dictionary<IFaction, Color> _colorTable;
        private Color _fightColor;
        private System.Action m_thread;

        public ZonesVisualized()
        {
            InitializeComponent();


            var rig = new GameRig();
            rig.Worldsize = _worldSize;
            rig.FactionCount = 3;
            rig.RigGame();
            var thread = new Thread(GameFrame.Game.StartTimer);

            GameFrame.Game.Factions[0].Name = "Faction 1";
            GameFrame.Game.Factions[1].Name = "Faction 2";
            GameFrame.Game.Factions[2].Name = "Faction 3";

            thread.Start();


            _colorTable = new Dictionary<IFaction, Color>();
            _colorTable.Add(GameFrame.Game.Factions[0], Color.FromArgb(255, 100, 255, 0));
            _colorTable.Add(GameFrame.Game.Factions[1], Color.FromArgb(255,0,139, 0));
            _colorTable.Add(GameFrame.Game.Factions[2], Color.FromArgb(255, 133, 0, 133));
            _colorTable.Add(GameFrame.Game.Factions[3], Color.FromArgb(255, 244, 255, 0));

            _fightColor = Color.FromArgb(255, 255, 0, 0);
            Task.Factory.StartNew((RepeatedUpdate));

            FactionsOverview fo = new FactionsOverview();
            fo.Show();
        }

        private void RepeatedUpdate()
        {
            m_thread = StartThread;
            m_thread.BeginInvoke(EndThread, null);

            while (true)
            {
                m_thread = StartThread;
                m_thread.BeginInvoke(EndThread, null);

                System.Threading.Thread.Sleep(300);
            }
        }

        private void StartThread()
        {
            //m_thread = StartThread;
            //m_thread.BeginInvoke(EndThread, null);  // Kick-off a new thread  
        }

        private void EndThread(IAsyncResult result)
        {
            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
               (System.Action)(() =>
               {
                   LegendPanel.Children.Clear();
                   ZoneCanvas.Children.Clear();

                   foreach (var faction in GameFrame.Game.Factions)
                   {
                       var brush = new SolidColorBrush();
                       brush.Color = _colorTable[faction];

                       LegendPanel.Children.Add(
                           new TextBox()
                           {
                               Background = brush,
                               Text = faction.Name,
                               FontSize = 16
                           }
                           );
                   }

                   int zoneSize = (int)(800 / _worldSize - 1);

                   for (int i = 0; i < GameFrame.Game.ZonePathfinder.Nodes.Count; i++)
                   {
                       IZone zone = GameFrame.Game.ZonePathfinder.Nodes[i];
                       var brush = new SolidColorBrush();

                       int row = i / _worldSize;
                       int col = i - (row * _worldSize);

                       if (zone.Units.All(unit => unit.State == UnitState.Dead))
                           brush.Color = Color.FromArgb(255, 128, 128, 128);
                       else if (zone.Units.Where(unit => unit.State != UnitState.Dead).Select(unit => unit.Faction).Distinct().Count() == 1)
                       {
                           var firstOrDefault = zone.Units.FirstOrDefault(unit => unit.State != UnitState.Dead);
                           if (firstOrDefault != null)
                           {
                               var faction = firstOrDefault.Faction;

                               brush.Color = _colorTable[faction];
                           }
                       }
                       else
                           brush.Color = _fightColor;


                       var rect = new Rectangle()
                       {
                           Stroke = Brushes.LightBlue,
                           StrokeThickness = 1,
                           Fill = brush
                       };
                       rect.Width = zoneSize;
                       rect.Height = zoneSize;

                       ZoneCanvas.Children.Add(rect);
                       Canvas.SetTop(rect, row * zoneSize);
                       Canvas.SetLeft(rect, col * zoneSize);
                   }
               }));
        }

        private void RigStuff()
        {
                int zoneSize = (int)(ZoneCanvas.Height / _worldSize - 1);

                for (int i = 0; i < GameFrame.Game.ZonePathfinder.Nodes.Count; i++)
                {
                    IZone zone = GameFrame.Game.ZonePathfinder.Nodes[i];
                    var brush = new SolidColorBrush();

                    int row = i/_worldSize;
                    int col = i - (row*_worldSize);

                    if (!zone.Units.Any())
                        brush.Color = Color.FromArgb(255, 128, 128, 128);
                    else if (zone.Units.Select(unit => unit.Faction).Distinct().Count() == 1)
                    {
                        var faction = zone.Units.FirstOrDefault().Faction;

                        brush.Color = _colorTable[faction];
                    }
                    else
                        brush.Color = _fightColor;


                    var rect = new Rectangle()
                    {
                        Stroke = Brushes.LightBlue,
                        StrokeThickness = 1,
                        Fill = brush
                    };
                    rect.Width = zoneSize;
                    rect.Height = zoneSize;

                    ZoneCanvas.Children.Add(rect);
                    Canvas.SetTop(rect, row * zoneSize);
                    Canvas.SetLeft(rect, col * zoneSize);
                }

                //for (int i = 0; i < _worldSize; i++)
                //{
                //    for (int j = 0; j < _worldSize; j++)
                //    {
                //        var rect = new Rectangle()
                //        {
                //            Stroke = Brushes.LightBlue,
                //            StrokeThickness = 1,
                //            Fill = brush
                //        };
                //        rect.InteriorWidth = zoneSize;
                //        rect.InteriorHeight = zoneSize;

                //        ZoneCanvas.Children.Add(rect);
                //        Canvas.SetTop(rect, j * zoneSize);
                //        Canvas.SetLeft(rect, i * zoneSize);
                //    }
                //}
        }
    }
}
