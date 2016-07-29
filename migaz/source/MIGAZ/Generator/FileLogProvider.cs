using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIGAZ.Generator
{
    class FileLogProvider : ILogProvider
    {
        public FileLogProvider()
        {

        }

        public void WriteLog(string function, string message)
        {
            string logfilepath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\MIGAZ\\MIGAZ-" + string.Format("{0:yyyyMMdd}", DateTime.Now) + ".log";
            string text = DateTime.Now.ToString() + "   " + function + "  " + message + Environment.NewLine;
            File.AppendAllText(logfilepath, text);
        }
    }
}
