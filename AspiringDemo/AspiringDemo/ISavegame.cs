﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo
{
    public interface ISavegame
    {
        void Create(string databaseName);
        void Save();
        void Load();
    }
}