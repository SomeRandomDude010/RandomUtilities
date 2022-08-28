using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace RandomUtilities
{
    public static class RandomUtils
    {
        public static string appdataPath;

        public static Logger defaultLogger;

        public static Random random
        {
            get
            {
                return new Random();
            }

            private set
            {
                random = value;
            }
        }

        public static void Init(string appdataPath)
        {
            Dictionary<int,int> dic = new Dictionary<int,int>();
            RandomUtils.appdataPath = appdataPath;
            defaultLogger = new();
        }
    }
}