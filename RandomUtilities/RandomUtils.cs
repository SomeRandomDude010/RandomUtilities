using System;
using System.Collections.Generic;

namespace RandomUtilities
{
    public static class RandomUtils
    {
        public static string appdataPath;

        public static Logger defaultLogger;

        public static Random Random
        {
            get
            {
                return new Random();
            }

            private set
            {
                Random = value;
            }
        }

        public static void Init(string appdataPath)
        {
            RandomUtils.appdataPath = appdataPath;
            defaultLogger = new();
        }
    }
}