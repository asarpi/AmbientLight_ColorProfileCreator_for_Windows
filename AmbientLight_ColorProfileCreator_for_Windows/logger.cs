using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AmbientLight_ColorProfileCreator_for_Windows
{
    public enum LogTypes
    {
        NoType = 0,
        ColorCapturing = 1,
        ColorAvgCalculator = 2,
        CatchedException = 3,


    }
    public static class logger
    {
        private static List<string> Logger = new List<string>();
        

        public static void add(LogTypes logtype, string str)
        {
            string logline = "[" + logtype.ToString() + "]  " + str;
            Logger.Add(logline);
            //Console.WriteLine(logline);
        }

        public static void close()
        {
            Logger.Add("");
            Logger.Add("");
            Logger.Add("");
            Logger.Add("######## LOG END ##########");
            Logger.Add(DateTime.Now.ToString());
            File.WriteAllLines("log.txt", Logger);
            Console.WriteLine("log file saved to log.txt");
        }

        public static void begin()
        {
            Logger.Add("########AmbiLight log #######");
            Logger.Add(DateTime.Now.ToString());
            Logger.Add("");
            Logger.Add("");
            Logger.Add("");
            Console.WriteLine("log created");
        }

    }
}
