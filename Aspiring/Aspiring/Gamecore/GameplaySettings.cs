using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.Gamecore
{
    //TODO: delete?
    public class GameplaySettings
    {
        public int TaxCollectionRate = 0;

        private static GameplaySettings instance;

        private GameplaySettings() {  }

        public static GameplaySettings Instance
        {
            get 
            {
                if (instance == null)
                {
                instance = new GameplaySettings();
                }
                return instance;
            }
        }
    }
}
