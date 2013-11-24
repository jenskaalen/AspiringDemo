using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.Gamecore.Log
{
    public class DebugLog : ILogger
    {
        private bool _initalized = false;
        private StreamWriter _writer;

        public void Log(string text)
        {
            if (!_initalized)
                Initialize();

            _writer.WriteLine(text);
            _writer.Flush();
        }

        private void Initialize()
        {
            _initalized = true;
            _writer = new StreamWriter("Log.txt", true);
            _writer.WriteLine("###################");
            _writer.WriteLine("#### NEW GAME #####");
            _writer.WriteLine("###################");
            _writer.Flush();
        }

        ~DebugLog()
        {
            _writer.Dispose();
        }
    }
}
