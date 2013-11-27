using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.Gamecore;
using AspiringDemo.Gamecore.Log;

namespace AspiringDemo
{
    public class GameFrame
    {
        private static IGame _instance;
        private static ILogger _debugLogger;
        private static Random _random;
        private static XmlCreator _creator = new XmlCreator();

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
                }

                return _instance;
            }
        }

        private static void ReadXmlCreation(float time)
        {
            _creator.ReadXml("units.xml");
        }

        /// <summary>
        /// Used if you want to override the standard game
        /// </summary>
        /// <param name="game"></param>
        public static void SetGame(IGame game)
        {
            _instance = game;
        }
    }
}
