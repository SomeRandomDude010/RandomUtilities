using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomUtilities
{
    public class Logger
    {
        List<LogEntry> logEntries;

        public Logger()
        {
            logEntries = new();
        }

        public void DisplayMsg(string msg, LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Information:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Gray;
                    break;
                case LogLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.BackgroundColor = ConsoleColor.Gray;
                    break;
                case LogLevel.Severe:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.BackgroundColor = ConsoleColor.Gray;
                    break;
                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.BackgroundColor= ConsoleColor.Yellow;
                    break;
                case LogLevel.Fatal:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    break;
                default:
                    break;
            }

            DateTime errorTime = DateTime.Now;

            string text = $"[{errorTime}][{level}] {msg}";

            Console.WriteLine(text);

            Console.ResetColor();

            logEntries.Add(new LogEntry(level, errorTime, msg));
        }

        public void Dump(string path)
        {
            List<string> output = new();

            foreach(var entry in logEntries)
            {
                output.Add($"[{entry.time}][{entry.level}] {entry.message}");
            }

            File.WriteAllLines(path, output);
        }
    }

    public enum LogLevel
    {
        Information,
        Warning,
        Severe,
        Error,
        Fatal
    }

    public struct LogEntry
    {
        public LogLevel level;
        public DateTime time;
        public string message;

        public LogEntry(LogLevel level, DateTime time, string message)
        {
            this.level = level;
            this.time = time;
            this.message = message;
        }
    }
}
