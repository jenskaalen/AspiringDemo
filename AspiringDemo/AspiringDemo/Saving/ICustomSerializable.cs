using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.Saving
{
    public interface ICustomSerializable
    {
        void LoadSerializedData(string data);
        string GetSerializedData();
    }
}
