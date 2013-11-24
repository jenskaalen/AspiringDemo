using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.Saving
{
    public interface ISavegame
    {
        void Create(string databaseName);
        void Save();
        void Load();
    }
}
