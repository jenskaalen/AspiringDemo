using System;
using System.Collections.Generic;
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
using AspiringDemo;
using AspiringDemo.Factions;
using AspiringDemo.GameActions.Movement;
using AspiringDemo.GameObjects.Units;
using AspiringDemo.Pathfinding;
using AspiringDemo.Zones;
using AspiringDemo.Zones.Interiors;

namespace AspiringVisual
{
    /// <summary>
    /// Interaction logic for Tombfinder.xaml
    /// </summary>
    public partial class Tombfinder : Page
    {
        private int _worldSize = 256;

        public Tombfinder()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        { 
            var testunit = new Unit(new Faction());
            InteriorValues vals = new InteriorValues(10, 20, 10, 4);
            Tomb tomb = new Tomb(_worldSize, _worldSize, vals);
            testunit.EnterZone(tomb);


            var targetRoom = tomb.Rooms[2];

            var unitNode = testunit.Zone.Pathfinder.GetClosestNode(testunit.Position);
            var targetNode = testunit.Zone.Pathfinder.GetClosestNode(targetRoom.Center);

            var travelPath = testunit.Zone.Pathfinder.GetPath(unitNode, targetNode);


            try
            {
             //   testunit.Actions.Add(new MoveToPosition(testunit, targetRoom.Center));
            }
            catch 
            {
            }

            int zoneSize = (int)(tombcanvas.Height / _worldSize - 1);

            for (int i = 0; i < tomb.Nodes.Count; i++)
            {
                //IZone zone = GameFrame.Game.ZonePathfinder.Nodes[i];
                var node = tomb.Nodes[i];
                var brush = new SolidColorBrush();

                int row = i/_worldSize;
                int col = i - (row*_worldSize);

                brush.Color = Color.FromArgb(255, 128, 128, 128);

                var rect = new Rectangle()
                {
                    Stroke = Brushes.LightBlue,
                    StrokeThickness = 1,
                    Fill = brush
                };
                rect.Width = zoneSize;
                rect.Height = zoneSize;

                tombcanvas.Children.Add(rect);
                //Canvas.SetTop(rect, row * zoneSize);
                //Canvas.SetLeft(rect, col * zoneSize);
                Canvas.SetTop(rect, node.Position.Y * zoneSize);
                Canvas.SetLeft(rect, node.Position.X * zoneSize);
            }

            for (int i = 0; i < travelPath.Count; i++)
            {
                var node = travelPath[i];
                var brush = new SolidColorBrush();

                int row = i / _worldSize;
                int col = i - (row * _worldSize);

                brush.Color = Color.FromArgb(255, 128, 128, 128);

                var rect = new Rectangle()
                {
                    Stroke = Brushes.Red,
                    StrokeThickness = 1,
                    Fill = brush
                };
                rect.Width = zoneSize;
                rect.Height = zoneSize;

                tombcanvas.Children.Add(rect);
                //Canvas.SetTop(rect, row * zoneSize);
                //Canvas.SetLeft(rect, col * zoneSize);
                Canvas.SetTop(rect, node.Position.Y * zoneSize);
                Canvas.SetLeft(rect, node.Position.X * zoneSize);
            }
        }
    }
}
