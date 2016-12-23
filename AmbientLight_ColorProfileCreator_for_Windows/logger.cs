using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace AmbientLight_ColorProfileCreator_for_Windows
{
    public enum LogTypes
    {
        NoType = 0,
        ColorCapturing = 1,
        ColorAvgCalculator = 2,
        CatchedException = 3,
        ColorBox2LEDPixelConverter = 4,
        SerialPort = 5,


    }
    public static class logger
    {
        private static List<string> Logger = new List<string>();
        private static CultureInfo culture = new CultureInfo("en-GB");
        

        public static void add(LogTypes logtype, string str)
        {

            string logline = DateTime.Now.Minute.ToString("00") + ":" + DateTime.Now.Second.ToString("00") + ":" + DateTime.Now.Millisecond.ToString("000");
            logline += " [" + logtype.ToString() + "]  " + str;
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

        public static string[] getLogs()
        {
            string[] entries = new string[Logger.Count()];
            int entry_id = 0;
            foreach(string log_entry in Logger)
            {
                entries[entry_id] = log_entry;
                entry_id++;
            }
            return entries;
        }

    }
}
