using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomUtilities
{
    public static class BetterDictionaries
    {
        public static void AddOrReplace<T1,T2>(this Dictionary<T1,T2> dict, T1 key, T2 value)
        { 
            if(dict.ContainsKey(key))
                dict[key] = value;
            else
                dict.Add(key, value);
        }

        public static T1[] GetKeysFromValue<T1, T2>(this Dictionary<T1,T2> dict, T2 value)
        {
            List<T1> keys = new();

            for(int i = 0; i < dict.Count; i++)
            {
                if(dict.Values.ToArray()[i].Equals(value))
                {
                    keys.Add(dict.Keys.ElementAt(i));
                }
            }

            return keys.ToArray();
        }

        public static T1 GetKeyFromValue<T1,T2>(this Dictionary<T1,T2> dict, T2 value)
        {
            if(dict.Values.Count(e => e.Equals(value)) == 1)
            {
                return dict.Keys.ElementAt(dict.Values.IndexOf(value));
            }

            throw new ArgumentOutOfRangeException();
        }
    }

    public static class BetterCollections
    {
        public static int IndexOf<T>(this IEnumerable<T> collection, T value)
        {
            for(int i = 0; i < collection.Count(); i++)
            {
                if (collection.ToArray()[i].Equals(value))
                    return i;
            }

            throw new ArgumentOutOfRangeException();
        }

        public static IEnumerable<T> GetCopy<T>(this IEnumerable<T> collection)
        {
            T[] output = new T[collection.Count()];

            collection.ToArray().CopyTo(output, 0);

            return output.AsEnumerable();
        }

        public static void ForEvery<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach(T item in collection)
            {
                action(item);
            }
        }
    }

    public static class BetterArrays
    {
        public static T[] Sort<T>(this T[] array, Comparer<T> comparer)
        {
            List<T> output = array.ToList();

            output.Sort(comparer);

            return output.ToArray();
        }
    }

    public static class BetterStrings
    {
        public static string Multiply(this string source, int multiplier)
        {
            string output = null;

            for(int i = 0; i < multiplier; i++)
            {
                output += source;
            }

            return output;
        }
    }

    
}
