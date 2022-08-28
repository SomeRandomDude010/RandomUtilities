using System;
using System.Collections.Generic;

namespace RandomUtilities
{
    public static class EventSystem
    {
        public readonly static Dictionary<string, EventHandler> events = new();

        public static void InvokeEvent(string name, object sender, EventArgs e)
        {
            events[name]?.Invoke(sender, e);
        }
    }
}
