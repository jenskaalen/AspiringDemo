using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using AspiringDemo.Factions;
using AspiringDemo.Zones;

namespace AspiringDemo.Sites
{
    public class PopulatedArea : IPopulatedArea
    {
        public PopulatedArea(IFaction owner, IZone zone)
        {
            Owner = owner;
            Zone = zone;
        }

        //public PopulatedArea(IFaction owner)
        //{
        //    Owner = owner;
        //}

        public IFaction Owner { get; set; }

        public IZone Zone { get; set; }

        public bool Razed { get; set; }

        public bool IsUnderAttack { get; set; }

        public int AreaValue { get; set; }

        public int BuildTime { get; set; }

        public int Cost { get; set; }

        public void LoadSerializedData(string data)
        {
            Convert.FromBase64String(data);
            var values = new List<int>();

            IFormatter formatter = new BinaryFormatter();

            using (var ms = new MemoryStream(Convert.FromBase64String(data)))
            {
                try
                {
                    values = (List<int>) formatter.Deserialize(ms);
                }
                catch (Exception ex)
                {
                    throw new Exception("Not a valid datablob to deserialize", ex);
                }
            }

            AreaValue = values[0];
            BuildTime = values[1];
            Cost = values[2];

            //for
        }

        /// <summary>
        ///     Returns a string of serialized, base64 encoded data
        /// </summary>
        /// <returns></returns>
        public string GetSerializedData()
        {
            string data = "";
            var list = new List<int>();

            list.Add(AreaValue);
            list.Add(BuildTime);
            list.Add(Cost);

            IFormatter formatter = new BinaryFormatter();

            using (var ms = new MemoryStream())
            {
                formatter.Serialize(ms, list);

                data = Convert.ToBase64String(ms.GetBuffer());
                //using (TextReader reader = new StreamReader(ms, Encoding.ASCII))
                //{
                //    ms.Position = 0;
                //    data = reader.ReadToEnd();
                //}
            }

            if (String.IsNullOrEmpty(data))
                throw new Exception("Empty data supplied - data cant be empty.");

            return data;
        }


        public int Population { get; set; }
    }
}