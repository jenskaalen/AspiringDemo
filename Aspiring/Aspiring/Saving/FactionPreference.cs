using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.Saving
{
    public class FactionPreference : ISerializedTypeData
    {
        public Type ObjectType
        {
            get;
            set;
        }

        public string SerializedData
        {
            get;
            set;
        }
    }
}
