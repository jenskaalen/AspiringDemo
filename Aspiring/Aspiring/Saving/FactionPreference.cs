using System;

namespace AspiringDemo.Saving
{
    public class FactionPreference : ISerializedTypeData
    {
        public Type ObjectType { get; set; }

        public string SerializedData { get; set; }
    }
}