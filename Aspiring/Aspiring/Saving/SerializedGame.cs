//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Runtime.Serialization.Formatters.Binary;
//using System.Text;
//using System.Threading.Tasks;
//using AspiringDemo.Factions;

//namespace AspiringDemo.Saving
//{
//    public class SerializedGame
//    {
//        public void SerializeFaction(string filename, IFaction faction)
//        {
//            using (Stream writer = File.Open(filename, FileMode.Create))
//            {
//                BinaryFormatter formatter = new BinaryFormatter();

//                formatter.Serialize(writer, faction);
//            }
//        }
//    }
//}
