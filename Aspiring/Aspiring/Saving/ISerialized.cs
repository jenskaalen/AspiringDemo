using System;

namespace AspiringDemo.Saving
{
    public interface ISerializedTypeData
    {
        Type ObjectType { get; set; }
        string SerializedData { get; set; }
    }
}