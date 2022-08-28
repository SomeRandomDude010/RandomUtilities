using System;
using System.Linq;
using System.Threading;

namespace RandomUtilities
{
    public class RandomConsole
    {
        public enum DurationType { Character, TotalTime}

        public static void Write(int x, int y, string text, bool reset = false)
        {
            (int, int) previous = (0,0);

            if(reset)
                previous = Console.GetCursorPosition();

            Console.SetCursorPosition(x, y);
            Console.Write(text);

            if (reset)
                Console.SetCursorPosition(previous.Item1, previous.Item2);
        }

        public static void Write(int x, int y, string text, int duration, DurationType type = DurationType.Character, bool reset = false)
        {
            (int, int) previous = (0, 0);

            if (reset)
                previous = Console.GetCursorPosition();

            Console.SetCursorPosition(x, y);

            if (type == DurationType.Character)
            {
                for(int i = 0; i < text.Length; i++)
                {
                    Console.Write(text[i]);
                    Thread.Sleep(duration);
                }
            }

            else
            {
                for (int i = 0; i < text.Length; i++)
                {
                    Console.Write(text[i]);
                    Thread.Sleep(duration / text.Length);
                }
            }

            if (reset)
                Console.SetCursorPosition(previous.Item1, previous.Item2);
        }

        public static void FlushInput()
        {
            while(Console.KeyAvailable)
                Console.ReadKey(true);
        }

        public static ConsoleKey ForceKeyInput(ConsoleKey[] inputOptions, bool intercept = true)
        {
            while (true)
            {
                ConsoleKey key = Console.ReadKey(intercept).Key;

                if(inputOptions.Contains(key))
                    return key;
            }
        }

        public static char ForceKeyInput(string inputOptions, bool intercept = true)
        {
            while (true)
            {
                char key = Console.ReadKey(intercept).KeyChar;

                if (inputOptions.Contains(key))
                    return key;
            }
        }
    }
}