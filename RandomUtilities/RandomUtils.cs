using System;
using System.Collections;
using System.Collections.Generic;

namespace RandomUtilities
{
    public static class RandomUtils
    {
        public static Random random;

        public static void Init()
        {
            Dictionary<int,int> dic = new Dictionary<int,int>();

            random = new Random();
        }
    }
}