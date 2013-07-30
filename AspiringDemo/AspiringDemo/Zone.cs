using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace AspiringDemo
{
    public enum ZoneType
    {
        Plains,
        Mountains
    }

    public class Zone
    {
        public int PositionXStart { get; set; }
        public int PositionXEnd { get; set; }
        public int PositionYStart { get; set; }
        public int PositionYEnd { get; set; }
        public bool IsPlayerNearby { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public ZoneType Type { get; set; }
        public Fight Fight { get; set; }

        public void LoadZone()
        {
        }

        public void UnloadZone()
        {
        }
    }
}
