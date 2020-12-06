using System;
using System.IO;

namespace ProjectLogging
{
    /*
     * Created by Cole Lamers 
     * Date: 2020-12-06
     * 
     * == Purpose ==
     * This code sets up the debug logging library to be used  
     * 
     * Changes: (date, comment)
     * 
     */

    /*
     * == Global Task List ==
     * 
     */
    public class DebugLogging
    {
        static string LogFilePath { get; set; }
        static string LogFileName { get; set; }
        static string LogFilePathAndName { get; set; }

        /* Commented out constructor because I'm calling the CreateDebugLogger() instead to initialize it
        public DebugLogging()
        {
            CreateDebugLogger();
        }
        */

    public static void CreateDebugLogger()
        {
            LogFilePath = @"..\Debug\Logs\";
            LogFileName = $"{DateTime.Now:yyyyMMdd}_Log.txt";
            LogFilePathAndName = Path.Combine(LogFilePath, LogFileName);

            if (!Directory.Exists(LogFilePath))
            {//TODO: --2-- need to fix this so that it doesn't check if a directory exist but it just makes it instead.
                Directory.CreateDirectory(LogFilePath);
            }//creates the log directory if it doesn't exist

            if (!File.Exists(Path.GetFullPath(LogFilePathAndName)))
            {
                using (StreamWriter sw = File.CreateText(LogFilePathAndName)) { }//just creates and closes the file if it does not exist
            }//creates the debug file for specific day the program is run
        }

        public static void LogActivity(string status)
        {
            using (StreamWriter streamWriter = File.AppendText(Path.GetFullPath(LogFilePathAndName)))
            {
                streamWriter.WriteLine($"{DateTime.Now:yyyy:MM:dd HH:mm:ss.ffff}, {status}");
            }
        }//writes to the debug logging file
    }
}
