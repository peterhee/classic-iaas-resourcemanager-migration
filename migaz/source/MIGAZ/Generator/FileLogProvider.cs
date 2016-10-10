using System;
using System.IO;

namespace MIGAZ.Generator
{
    class FileLogProvider : ILogProvider
    {
        private object lockObject = new object();

        public FileLogProvider()
        {

        }

        public void WriteLog(string function, string message)
        {
            lock (lockObject)
            {
                string logfiledir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\MIGAZ";
                if (!Directory.Exists(logfiledir)) { Directory.CreateDirectory(logfiledir); }

                string logfilepath = logfiledir + "\\MIGAZ-" + string.Format("{0:yyyyMMdd}", DateTime.Now) + ".log";
                string text = DateTime.Now.ToString() + "   " + function + "  " + message + Environment.NewLine;
                File.AppendAllText(logfilepath, text);
            }
        }
    }
}
